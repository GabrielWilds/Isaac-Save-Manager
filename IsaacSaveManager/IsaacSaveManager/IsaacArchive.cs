using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsaacSaveManager
{
    public class IsaacArchive : IComparable
    {
        public string Name
        {
            get;
            private set;
        }

        public DateTime SaveDate
        {
            get;
            private set;
        }

        public string Location
        {
            get;
            private set;
        }

        public bool IsFavorite
        {
            get;
            private set;
        }

        public IsaacArchive(string name, DateTime date, string location)
        {
            Name = name;
            SaveDate = date;
            Location = location;
            //check favorite status by index file
        }

        public void RenameSave(string name)
        {
            Name = name;
            //change filename
        }

        public void ToggleFavorite()
        {
            IsFavorite = !IsFavorite;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) 
                return 1;

            IsaacArchive archive = obj as IsaacArchive;

            if (this.SaveDate > archive.SaveDate)
                return -1;
            else if (this.SaveDate < archive.SaveDate)
                return 1;
            else
                return 0;
        }
    }
}
