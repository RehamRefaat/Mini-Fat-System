using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class RemoveDirectory
    {
        public void rd(string name)
        {
            int index = Program.CurrentDirectory.SearchDirectory(name);
            if (index != -1)
            {
               // Console.WriteLine(index.ToString());    
                int fc = Program.CurrentDirectory.DirectoryTable[index].firstCluster;
                //Console.WriteLine(fc);
                Directory d = new Directory(name.ToCharArray(), 0x10, fc,Program.CurrentDirectory);
                d.DeleteDirectory();
                //FAT_Table.PrintFatTable();
            }
            else
            {
                Console.WriteLine($"The system cannot find the file specified.");
            }
        }
    }
}
