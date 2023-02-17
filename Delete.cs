using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Delete
    {
        public void delete(string fname)//بيمسح فايلات بس 
        {
            int index = Program.CurrentDirectory.SearchDirectory(fname);
            if(index !=-1)
            {
                byte attr = Program.CurrentDirectory.DirectoryTable[index].fileAttribute;//عاوزة اتاكد منها
                if (attr == 0x0)//معناها ان ده فايل
                {
                    int fc = Program.CurrentDirectory.DirectoryTable[index].firstCluster;
                    int fsize = Program.CurrentDirectory.DirectoryTable[index].fileSize;
                    FileEntry file = new FileEntry(fname.ToCharArray(), attr,fc,fsize, Program.CurrentDirectory,null);
                    file.DeleteFile();
                    Directory d = new Directory(fname.ToCharArray(), 0x0, fc, Program.CurrentDirectory);
                    d.DeleteDirectory();
                    // Program.CurrentDirectory.DirectoryTable.RemoveAt(index);
                }
                else 
                {
                    Console.WriteLine($"The system cannot find the file specified.");
                }
            }
            else
            {
                Console.WriteLine($"The system cannot find the file specified.");
            }
        }
    }
}
