using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsaacSaveManager;

namespace IsaacManagerUI
{
    class TextEntryWindowViewModel : ViewModelBase
    {
        public string Name
        {
            get;
            set;
        }

        private IsaacArchive Archive
        {
            get;
            set;
        }

        public TextEntryWindowViewModel(IsaacArchive _archive)
        {
            Archive = _archive;
        }

        public void WriteSave()
        {
            if (Archive == null)
                SaveDataAccess.ArchiveCurrentSave(Name);
            else
                SaveDataAccess.RenameArchive(Archive, Name);
        }
    }
}
