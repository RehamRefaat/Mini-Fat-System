using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    internal class Dir
    {
        public void dir()
        {
            
            int fileCounter = 0;
            int directoryCounter = 0;
            int fileSizeCounter = 0;//علشان اقدر اطبع التوتال سايز
            Console.WriteLine("Directory of : "+Program.CurrentPath+"\\");
            Program.CurrentDirectory.ReadDirectory();
            //Console.WriteLine(Program.CurrentDirectory.DirectoryTable.Count);
            for (int i = 0; i < Program.CurrentDirectory.DirectoryTable.Count; i++)
            {
                byte attr = Program.CurrentDirectory.DirectoryTable[i].fileAttribute;//عاوزة اتاكد منها
                //اتعدلت بعد تحويل الفايل نيم ل كراكتر اراي
                string str = new string(Program.CurrentDirectory.DirectoryTable[i].fileName);
              
                if (attr==0x0)//معناها ان ده فايل
                {
                    Console.WriteLine("\t\t" + (Program.CurrentDirectory.DirectoryTable[i].fileSize).ToString() + " " +str);
                    fileCounter++;
                    fileSizeCounter += Program.CurrentDirectory.DirectoryTable[i].fileSize;//عاوزة اتاكد منها 
                }
                else//معناها انه دايركتوري
                {
                    Console.WriteLine(" <DIR> \t\t"+str);
                    directoryCounter++;
                }
            }
            Console.WriteLine(" "+fileCounter.ToString() + "  File(s)  " + fileSizeCounter.ToString() + "  bytes");
            Console.WriteLine(" "+directoryCounter.ToString() +"  Dir(s)  "+ FAT_Table.getFreeSpace() + "  bytes free");//فانكشن تم اضافتها في الفات

        }

    }
}
