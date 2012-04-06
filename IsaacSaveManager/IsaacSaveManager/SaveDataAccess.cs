using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace IsaacSaveManager
{
    public static class SaveDataAccess
    {
        private static string _isaacDirectory = null;
        private static string _saveLocation = null;
        private static string _archives = Path.Combine(IsaacDirectory, "ArchivedSaves");
        private static string _archiveText = Path.Combine(Archives, "archive.txt");

        public static string IsaacDirectory
        {
            get { return FindIsaacInstallPath(); }
        }

        public static string IsaacSave
        {
            get { return FindIsaacSaveFile(); }
        }

        public static string Archives
        {
            get { return _archives; }
        }

        public static string ArchiveText
        {
            get { return _archiveText; }
        }

        public static ObservableCollection<IsaacArchive> GetArchiveList()
        {
            if (Directory.Exists(Archives))
            {
                string[] folders = Directory.GetDirectories(Archives, "*");
                if (folders.Length == 0)
                {
                    //Console.WriteLine("No Items");
                    return null;
                }

                List<IsaacArchive> archives = new List<IsaacArchive>();
                foreach (string folder in folders)
                {
                    archives.Add(ReadArchive(folder));
                }

                archives.Sort();

                ObservableCollection<IsaacArchive> observedArchives = new ObservableCollection<IsaacArchive>(archives);
                return observedArchives;
            }
            else
                return null;
        }

        public static IsaacArchive ReadArchive(string archivePath)
        {
            DateTime archiveTime = Directory.GetCreationTime(archivePath);
            string name = Path.GetFileName(archivePath);
            //Console.WriteLine(name);
            return new IsaacArchive(name, archiveTime, archivePath);
        }

        public static void DeleteArchive(IsaacArchive archive)
        {
            Directory.Delete(archive.Location, true);
        }

        public static void RestoreArchivedSave(IsaacArchive archive)
        {
            //Console.WriteLine("Restoring archived save " + archive.Name);
            string serial = Path.Combine(IsaacDirectory, "serial.txt");

            if (File.Exists(IsaacSave))
                File.Delete(IsaacSave);
            if (File.Exists(serial))
                File.Delete(serial);

            File.Copy(Path.Combine(archive.Location, "so.sol"), IsaacSave);
            File.Copy(Path.Combine(archive.Location, "serial.txt"), serial);

            if (File.Exists(ArchiveText))
                File.Delete(ArchiveText);
            File.WriteAllText(ArchiveText, archive.Name);
        }

        public static void ArchiveCurrentSave(string archiveName)
        {
            if (!Directory.Exists(Archives))
                Directory.CreateDirectory(Archives);

            DateTime saveTime = File.GetLastWriteTime(IsaacSave);
            if (archiveName == "")
                archiveName = File.GetLastWriteTime(IsaacSave).ToString("MMdd-hhmm.ss");

            string archiveDirectory = Path.Combine(Archives, archiveName);
            if (Directory.Exists(archiveDirectory))
            {
                Directory.Delete(archiveDirectory, true);
            }
            Directory.CreateDirectory(archiveDirectory);

            File.Copy(IsaacSave, Path.Combine(archiveDirectory, "so.sol"));
            File.Copy(Path.Combine(IsaacDirectory, "serial.txt"), Path.Combine(archiveDirectory, "serial.txt"));

            if (File.Exists(ArchiveText))
                File.Delete(ArchiveText);
            File.WriteAllText(ArchiveText, archiveName);

            //Console.WriteLine("Archive " + archiveName + " has been saved!");
        }

        public static void CheckCurrentSave()
        {
            DateTime saveTime = File.GetLastWriteTime(IsaacSave);
            ObservableCollection<IsaacArchive> archives = GetArchiveList();
            if (archives == null)
            {
                ArchiveCurrentSave(saveTime.ToString("yyyyMMdd-hhmmss"));
                return;
            }

            IsaacArchive lastArchive = archives[0];

            if (saveTime > lastArchive.SaveDate)
                ArchiveCurrentSave(saveTime.ToString("yyyyMMdd-hhmmss"));
            //else
            //Console.WriteLine("Archives current.");
        }

        private static string FindIsaacInstallPath()
        {
            if (_isaacDirectory == null)
            {
                string steamKey = "HKEY_LOCAL_MACHINE\\Software\\Valve\\Steam";
                string steam = (string)Registry.GetValue(steamKey, "InstallPath", null);
                _isaacDirectory = Path.Combine(steam, "steamapps", "common", "the binding of isaac");
            }
            return _isaacDirectory;
        }

        private static string FindIsaacSaveFile()
        {
            if (_saveLocation == null)
            {
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string sharedObjects = Path.Combine(appdata, "Macromedia", "Flash Player", "#SharedObjects");
                _saveLocation = Path.Combine(sharedObjects, Directory.GetDirectories(sharedObjects)[0], "localhost", "so.sol");
            }
            return _saveLocation;
        }

        public static string CheckLastUsedArchive()
        {
            string archiveText = Path.Combine(Archives, "archive.txt");
            if (File.Exists(archiveText))
                return File.ReadAllText(archiveText);
            else
                return null;
        }

        public static void RenameArchive(IsaacArchive archive, string name)
        {
            Directory.Move(archive.Location, Path.Combine(Archives, name));
        }
    }
}
