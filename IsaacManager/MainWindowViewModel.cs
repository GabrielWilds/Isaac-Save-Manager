using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using IsaacSaveManager;

namespace IsaacManagerUI
{
    class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<IsaacArchive> _archives;

        public ObservableCollection<IsaacArchive> Archives
        {
            get { return _archives; }
            set { _archives = value; RaisePropertyChanged("Archives"); }
        }

        public IsaacArchive Archive
        {
            get;
            set;
        }

        public string LastArchive
        {
            get { return ParseLastUsedArchive(); }
        }

        public MainWindowViewModel()
        {
            Archives = (ObservableCollection<IsaacArchive>)SaveDataAccess.GetArchiveList();
            if (Archives != null && Archives.Count > 0)
                Archive = Archives[0];
            RaisePropertyChanged("Archive");
            RaisePropertyChanged("Archives");
        }

        public string ParseLastUsedArchive()
        {
            string lastArchive = SaveDataAccess.CheckLastUsedArchive();
            if (lastArchive == null)
                return "You should archive your save.";
            else
                return "The most recently used slot is " + lastArchive;
        }

        public void RunIsaac()
        {
            Process.Start("steam://run/113200/");
        }

        public void SaveToSlot()
        {
            SaveDataAccess.ArchiveCurrentSave(Archive.Name);
            Archives = SaveDataAccess.GetArchiveList();
            RaisePropertyChanged("Archives");
            RaisePropertyChanged("LastArchive");
        }

        public void SaveNewSlot()
        {
            TextEntryWindow nameSlot = new TextEntryWindow(null);
            nameSlot.ShowDialog();
            RaisePropertyChanged("Archives");
            RaisePropertyChanged("LastArchive");
        }

        public void RestoreSlot()
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to overwrite your current save?", "Confirm overwrite", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
                SaveDataAccess.RestoreArchivedSave(Archive);
            RaisePropertyChanged("LastArchive");
        }

        public void DeleteSlot()
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to delete this slot?", "Confirm deletion", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                SaveDataAccess.DeleteArchive(Archive);
                RaisePropertyChanged("Archives");
            }
        }

        public void RenameSlot()
        {
            TextEntryWindow rename = new TextEntryWindow(Archive);
            rename.ShowDialog();
            RaisePropertyChanged("Archives");
            RaisePropertyChanged("LastArchive");
        }
    }
}
