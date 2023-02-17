using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Type
    {
        public void type(string name)
        {
            int index = Program.CurrentDirectory.SearchDirectory(name);
            Program.CurrentDirectory.ReadDirectory();
            if ( index != -1)
            {

                int fc =  Program.CurrentDirectory.DirectoryTable[index].firstCluster;
                int fs = Program.CurrentDirectory.DirectoryTable[index].fileSize;
                string content = null;
               // Console.WriteLine(fc);
                FileEntry file = new FileEntry(name.ToCharArray(),0x0,fc,fs,Program.CurrentDirectory,content);
                file.ReadFile();
                Console.WriteLine(file.Content);

            }
            else
            {
                Console.WriteLine("System Can't Find File Specify !");
            }
        }
    }
}
