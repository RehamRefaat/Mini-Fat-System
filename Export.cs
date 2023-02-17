﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime;
namespace Shell
{
    class Export
    {
        public void export(string name, string path)
        {
            int index = Program.CurrentDirectory.SearchDirectory(name);
           // index = index - 1;
            if (index != -1)
            {
                if (System.IO.Directory.Exists(path))
                {
                    
                    int fc = Program.CurrentDirectory.DirectoryTable[index].firstCluster;
                   // Console.WriteLine(fc-1);
                    int fs = Program.CurrentDirectory.DirectoryTable[index].fileSize;
                    string content = null;
                    FileEntry file = new FileEntry(name.ToCharArray(), 0x0, fc , fs, Program.CurrentDirectory, content);
                    file.ReadFile();
                    string newpath = path + "\\" + name;
                   // Console.WriteLine(newpath);
                    StreamWriter sw = new StreamWriter(newpath);
                   // Console.WriteLine("Content " + file.Content);
                    sw.Write(file.Content);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    Console.WriteLine("The System Can't Find The Path Specified In A Computer Disk");
                }

            }
            else
            {
                Console.WriteLine("File Not Exist In Virtual Disk !");
            }

        }
    }
}
