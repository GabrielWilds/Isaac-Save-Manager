using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsaacSaveManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Binding of Isaac Save Manager (console edition)");
            Console.WriteLine();
            Console.WriteLine(SaveDataAccess.IsaacDirectory);
            Console.WriteLine(SaveDataAccess.IsaacSave);
            Console.WriteLine();
            SaveDataAccess.CheckCurrentSave();
            //IsaacArchive[] archives = SaveDataAccess.GetArchiveList();
            //SaveDataAccess.RestoreArchivedSave(archives[archives.Length - 1]);
            Console.ReadKey();
        }
    }
}
