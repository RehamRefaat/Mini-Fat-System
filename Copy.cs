using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Copy
    {
        public void copy(string source,string destination)
        {
           // Program.CurrentDirectory.ReadDirectory();
            int counter_of_filecopy = 0;//اضافة من عندي هستخدمة في طباعة 
            int Sourceindex = Program.CurrentDirectory.SearchDirectory(source);
            if (Sourceindex != -1)
            {
                string Input;
                int name_start = destination.LastIndexOf('\\');
                string name = destination.Substring(name_start + 1);
               // Console.WriteLine(name);
                int destinatonindex = Program.CurrentDirectory.SearchDirectory(destination);
                if (destination != Program.CurrentPath)//هل مكانه صح
                {
                    if (destinatonindex != -1 )//الحاجة اللي عاوزة انسخها انها موحوده قبل كدا
                    {

                        Console.WriteLine("Overwrite " + destination + "(Yes/No):");
                        Input = Console.ReadLine();
                        if (Input.ToUpper() == "YES")
                        {
                            int fc = Program.CurrentDirectory.DirectoryTable[Sourceindex].firstCluster;//هل بتاعت s or d
                            int fsize = Program.CurrentDirectory.DirectoryTable[Sourceindex].fileSize;
                            DirectoryEntry DE = new DirectoryEntry(source.ToCharArray(), 0x0, fc, fsize);

                            //int dfc = Program.CurrentDirectory.DirectoryTable[destinatonindex].firstCluster;//هل بتاعت s or d
                            //Directory dir = new Directory(destination.ToCharArray(), 0x10, dfc, Program.CurrentDirectory);//خاص بالمكان اللي انا عاوزة اضيف فيه
                            //dir.DirectoryTable.Add(DE);
                            //dir.WriteDirctory();
                            //Program.CurrentDirectory.DirectoryTable.Add(dir);
                            //Program.CurrentDirectory.WriteDirctory();
                            //Console.WriteLine("\t"+ ++counter_of_filecopy + "file(s) copied.");
                            int index = Program.CurrentDirectory.SearchDirectory(name);
                            int dfc = Program.CurrentDirectory.DirectoryTable[index].firstCluster;
                            Directory dir = new Directory(name.ToCharArray(), 0x10, dfc, Program.CurrentDirectory);
                            dir.DirectoryTable.Add(DE);
                            dir.WriteDirctory();
                            Program.CurrentDirectory.DirectoryTable.RemoveAt(index);
                            Program.CurrentDirectory.DirectoryTable.Add(dir);
                            Program.CurrentDirectory.WriteDirctory();
                            Console.WriteLine("\t" + ++counter_of_filecopy + "file(s) copied.");
                        }
                        else
                        {
                            Console.WriteLine("\t0 file(s) copied.");
                        }
                    }
                    else
                    {

                        int fc = Program.CurrentDirectory.DirectoryTable[Sourceindex].firstCluster;//هل بتاعت s or d
                        int fsize = Program.CurrentDirectory.DirectoryTable[Sourceindex].fileSize;
                        DirectoryEntry DE = new DirectoryEntry(source.ToCharArray(), 0x0, fc, fsize);

                        //خاص بالمكان اللي انا عاوزة اضيف فيه
                        int index = Program.CurrentDirectory.SearchDirectory(name);
                        int dfc=Program.CurrentDirectory.DirectoryTable[index].firstCluster;
                        Directory dir = new Directory(name.ToCharArray(), 0x10, dfc, Program.CurrentDirectory);
                        dir.DirectoryTable.Add(DE);
                        dir.WriteDirctory();
                        Program.CurrentDirectory.DirectoryTable.RemoveAt(index);
                        Program.CurrentDirectory.DirectoryTable.Add(dir);
                        Program.CurrentDirectory.WriteDirctory();
                        Console.WriteLine("\t" + ++counter_of_filecopy + "file(s) copied.");
                        
                    }
                }
                else { Console.WriteLine("The file cannot be copied onto itself.");}
            }
            else
            {
                Console.WriteLine("The system cannot find the path specified.");
            }

        }
    }
}
