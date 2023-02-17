using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Directory : DirectoryEntry
    {
       
       public List<DirectoryEntry> DirectoryTable = new List<DirectoryEntry>();
       // List<DirectoryEntry> DirectoryTable2 = new List<DirectoryEntry>();
        public Directory Parent = null;

        public Directory(char[] name, byte attr, int fcluster, Directory parent) : base(name, attr, fcluster,0)
        {
            if (parent != null)
            {
                this.Parent = parent;
            }
        }
      
        public void WriteDirctory()
        {
            //for (int i = 0; i < DirectoryTable.Count; i++)
            //{
            //    Console.WriteLine("file name  "+DirectoryTable[i].fileName[i] + " first cluster " + DirectoryTable[i].firstCluster);
            //}
            byte[] DEB = new byte[32];
            byte[] DTB = new byte[32 * DirectoryTable.Count];
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                DEB = DirectoryTable[i].getBytes();
                for (int j = i*32; j < 32*(i+1); j++)
                {
                    DTB[j] = DEB[j%32];
                }
            }
            
            double d = Math.Ceiling(DTB.Length / 1024.0);
            int number_of_full_size_block = DTB.Length / 1024;
            int No_Of_RequiredBlocks =(int)d;
            int Remainder = DTB.Length % 1024;
            int FatIndex = 0;
            int LastIndex = -1;
            if (No_Of_RequiredBlocks <= FAT_Table.getAvailableBlocks())
            {
                if(firstCluster != 0)
                {
                    FatIndex = firstCluster;
                }
                else
                {
                    FatIndex = FAT_Table.Available_Block_In_FatTable();
                    firstCluster = FatIndex;
                }
                byte[] count = new byte[1024];
                List<byte[]> Block = new List<byte[]>();
                for (int i = 0; i < DTB.Length; i++)
                {       
                    count[i%1024] = DTB[i];
                    if ((i + 1) % 1024 == 0)
                    {
                        Block.Add(count);
                    }
                }
                if (Remainder > 0)
                {
                    byte[] arr = new byte[1024];
                    int start = number_of_full_size_block * 1024;
                    for (int i = start; i < (start + Remainder); i++)
                    {
                        arr[i%1024] = DTB[i];
                    }
                    Block.Add(arr);                
                }
                //Console.WriteLine("write  :  firstCluster " + FatIndex.ToString() + "   " + FAT_Table.Get_Next(FatIndex));
                //Console.WriteLine("write  :  firstCluster " + firstCluster.ToString() + "   " + FAT_Table.Get_Next(FatIndex));
                for (int i = 0; i < Block.Count; i++)
                {
                    Virtual_Disk.WriteBlock(Block[i], FatIndex);
                    FAT_Table.Set_Next(FatIndex, -1);
                    if (LastIndex != -1)
                    {
                        FAT_Table.Set_Next(LastIndex, FatIndex);
                    }
                    LastIndex = FatIndex;
                    FatIndex = FAT_Table.Available_Block_In_FatTable();                    
                }
                if (this.firstCluster != 0)
                {
                    if (this.DirectoryTable.Count == 0)
                    {
                        FAT_Table.Set_Next(this.firstCluster, 0);
                        this.firstCluster = 0;                      
                    }                
                }
                FAT_Table.WriteFatTable();
            }
            else
            {
                Console.WriteLine("There Is No Avaialble Space");
            }
        }
        public void ReadDirectory()
        {
            DirectoryTable = new List<DirectoryEntry>();
            //DirectoryTable = new List<DirectoryEntry>();
            List<byte> ls = new List<byte>();
            byte[] b = new byte[32];
            int FatIndex = 0;
            int Next= 0;
            //if (firstCluster != 0)
            //{
            //    FatIndex = firstCluster;
            //}
            //else
            //{
            //    FatIndex = FAT_Table.Available_Block_In_FatTable();
            //    firstCluster = FatIndex;
            //}
            //Console.WriteLine("read   :  firstCluster "+this.firstCluster.ToString()+"   " + FAT_Table.Get_Next(this.firstCluster));
            if (this.firstCluster != 0 && FAT_Table.Get_Next(this.firstCluster) != 0)
            {
               // Console.WriteLine("first " + firstCluster);
                FatIndex = firstCluster;
                Next = FAT_Table.Get_Next(FatIndex);
                do
                {
                    ls.AddRange(Virtual_Disk.Get_Block(FatIndex));
                    FatIndex = Next;
                    if (FatIndex != -1)
                    {
                        Next = FAT_Table.Get_Next(FatIndex);
                    }
                } while (Next != -1);
                for (int i = 0; i < ls.Count; i++)  
                {        
                    b[i % 32] = ls[i];
                    if ((i + 1) % 32 == 0)
                    {
                        DirectoryEntry d = getDirectoryEntry(b);
                        if (d.fileName[0] != '\0')
                        {
                            DirectoryTable.Add(d);
                        }                 
                    }                           
                }
            }
        }
        //void cleanDirectoryTable()
        //{
        //   // int j = 0;
        //    for (int i = 0; i < (DirectoryTable.Count);i++)
        //    {
                
        //       // Console.WriteLine("DirectoryTable[i].firstCluster  --> " + DirectoryTable[i].firstCluster);
        //        if (DirectoryTable[i].fileName[0] == 0 )
        //        {
        //            DirectoryTable.RemoveAt(i);
                  
        //        }
        //        if (i < DirectoryTable.Count - 1)
        //        {
        //            if (DirectoryTable[i].fileName.Equals(DirectoryTable[i + 1].fileName))
        //            {
        //                DirectoryTable.RemoveAt(i);
        //            }
        //        }
               
        //    }
        //}
        //وظيفة الفانكشن دي اني اديها اسم فولدر او فايل وتتاكدلي الاسم ده موجود عندي ولا لا فلو موجود بتقولي في انهي صف ولو مش موجود بترجع -1 
        public int SearchDirectory(string fileName)
        {
            //Console.WriteLine("filename "+fileName);
            ReadDirectory();
            // cleanDirectoryTable();
            //for (int i = 0; i < DirectoryTable.Count; i++)
            //{
            //    Console.WriteLine("name " + DirectoryTable[i].fileName[0] + " first cluster " + DirectoryTable[i].firstCluster);
            //}
            if (fileName.Length < 11)
            {
                for (int i = fileName.Length; i < 11; i++)

                    fileName += '\0';
            
            }
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                // عملت الفور لوب دي عشان احول الفايل نيم من بايت لكراكتر لاني مخزناه كراكتر 
               //char[] FN = new char[DirectoryTable[i].fileName.Length];
               // for (int j = 0; j < FN.Length; j++)
               // {
               //     FN[j] = (char)DirectoryTable[i].fileName[j];
                  
               // }

               string convert =new string(DirectoryTable[i].fileName);//هنا حولت من الكراكتر اراي الي استرنج علشان اعرف استخدمة في فانكشن EQUALS
                //Console.WriteLine(convert);
                //اتعدلت بعد تحويل الفايل نيم ل كراكتر اراي
                if (fileName.Equals(convert))
                {
                    return i;
                }
            }
            
            return -1;
           
        }

        //بتعدلي محتوي صف(داتا انتري) معين موجود عندي
        public void UpdateContent(DirectoryEntry d)
        {   
            ReadDirectory();
            //هستدعيها علشان تعبءالليست بتاعت الديركتوري تابل بتاعي 
            // cleanDirectoryTable();
            //عاوزة ادور علي الصف اللي هعمل عليه التعديل              
            //لو كان العنصر اللي ببحث عنه موجود هترحعلي مكانه لو مش موجود هترجعلي -1 
             //اتعدلت بعد تحويل الفايل نيم ل كراكتر اراي
            string str = new string(fileName);
            int index = SearchDirectory(str);//بتحول الاراي بايت الي استرنج
            if (index !=-1)//بتاكد هل هو موجود ولالا
            {
                DirectoryTable.RemoveAt(index);// بدل ما اعدل القيم الموجودة في الداتا انتري  همسحة واكتبه مره تاني
                DirectoryTable.Insert(index, d);//بتاخد المكان اللي عاوزة اضيف فيه والقيمة اللي هضيفها
                WriteDirctory();  
            }
        }
         // عندي حالتين 
        //  فلازم امسحهم كلهم files or sub directory الحالة الاولي ان الديركتوري يحتوي علي 
       //   فلازم اروح امسح نفسيparentمن ال supdirectoryالحالة التانية ممكن الديركتوري يكون      
        public void DeleteDirectory()
        {
            //cleanDirectoryTable();
            if (firstCluster != 0)
            {

                int index = firstCluster;
                int next = FAT_Table.Get_Next(index);
                do
                {

                    FAT_Table.Set_Next(index, 0);
                    index = next;
                    if (index != -1)
                    {
                        next = FAT_Table.Get_Next(index);//احنا كدا رحنا للفات تابل خلينا كل القيم بتاعت الديريكتوري جه باصفار معناها لو حد هيسال عن المكان ده هيلاقيه متاحة فيقدر يكتب فيه 
                    }
                } while (index != -1);
            }
                
                if (Parent!=null)// ولالا لو في هروح امسح نفسي PARENT هسال ليا 
                {
                    Parent.ReadDirectory();//  محتاج يقرا العناصر اللي جواهPARENT  
                     //اتعدلت بعد تحويل الفايل نيم ل كراكتر اراي
                    string str = new string(fileName);
                    int I = Parent.SearchDirectory(str);//هشوف نفسي في انهي صف
                    if (I!=-1)
                    {
                        Parent.DirectoryTable.RemoveAt(I);//هنا بسمح نفسي 
                        Parent.WriteDirctory();//هيكتب نفسه مره تانيهPARENTال 
                        //علشان عملت تعديل علي فات تابل هروح اكتبه مره تانيه
                    }
                }
                FAT_Table.WriteFatTable();
            
        }
    }
}