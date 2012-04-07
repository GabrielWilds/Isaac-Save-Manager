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
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to overwrite this archived save?", "Confirm overwrite", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                SaveDataAccess.ArchiveCurrentSave(Archive.Name);
                Archives = SaveDataAccess.GetArchiveList();
                RaisePropertyChanged("Archives");
                RaisePropertyChanged("LastArchive");
            }
        }

        public void SaveNewSlot()
        {
            TextEntryWindow nameSlot = new TextEntryWindow();
            nameSlot.ShowDialog();
            string name = ((TextEntryWindowViewModel)nameSlot.DataContext).Name;
            if (name != null)
            {
                SaveDataAccess.ArchiveCurrentSave(name);
                Archives = SaveDataAccess.GetArchiveList();
                RaisePropertyChanged("Archives");
                RaisePropertyChanged("LastArchive");
            }
        }

        public void RestoreSlot()
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to overwrite your current save?", "Confirm overwrite", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                SaveDataAccess.RestoreArchivedSave(Archive);
                Archives = SaveDataAccess.GetArchiveList();
                RaisePropertyChanged("LastArchive");
            }
        }

        public void DeleteSlot()
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to delete this slot?", "Confirm deletion", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                SaveDataAccess.DeleteArchive(Archive);
                Archives = SaveDataAccess.GetArchiveList();
                RaisePropertyChanged("Archives");
            }
        }

        public void RenameSlot()
        {
            TextEntryWindow rename = new TextEntryWindow();
            rename.ShowDialog();
            string name = ((TextEntryWindowViewModel)rename.DataContext).Name;
            if (name != null)
            {
                SaveDataAccess.RenameArchive(Archive, name);
                Archives = SaveDataAccess.GetArchiveList();
                RaisePropertyChanged("Archives");
                RaisePropertyChanged("LastArchive");
            }
        }
    }
}
