using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime;
namespace Shell
{
    class Import
    {
        public void import(string path)
        {
            //بتاكد ان الفايل موجود اصلا
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                int size = content.Length;
                int name_start = path.LastIndexOf('\\');
                string name = path.Substring(name_start+1);
                int index = Program.CurrentDirectory.SearchDirectory(name);
                if(index == -1)
                {
                   // int FC;
                   
                    if(size > 0 )
                    {
                       // Console.WriteLine("Ava " + FAT_Table.Available_Block_In_FatTable());
                        FileEntry file = new FileEntry(name.ToCharArray(), 0x0, 0, size,Program.CurrentDirectory, content);
                        file.WriteFile();
                       // Console.WriteLine(file.firstCluster);
                        DirectoryEntry dir = new DirectoryEntry(name.ToCharArray(), 0x0, file.firstCluster, size);
                        Program.CurrentDirectory.DirectoryTable.Add(dir);
                        Program.CurrentDirectory.WriteDirctory();
                        
                    }
                    else
                    {
                        FileEntry file = new FileEntry(name.ToCharArray(), 0x0, 0, 0, Program.CurrentDirectory, content);
                        file.WriteFile();
                    }
                }
                else
                {
                    Console.WriteLine("The File Already Exist");
                }
            }
            else
            {
                Console.WriteLine("This File Not Exist");
            }
        }
    }
}
