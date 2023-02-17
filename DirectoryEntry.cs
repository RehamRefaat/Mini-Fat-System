using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Shell
{
    class DirectoryEntry
    {
        public char[] fileName = new char[11];
        public byte fileAttribute;
        public byte[] fileEmpty = new byte[12];
        public int fileSize;
        public int firstCluster;
        
        public DirectoryEntry(char[] name, byte attr, int fcluster,int fSize)
        {
            // Console.WriteLine(name[0].ToString() + " " + fcluster.ToString());
            // عملت الحركه دي عشان لو اليوزر دخله اسم اكبر من 11 بايت كان بيقبله عادي وبيخزنه في الفايل نيم فعملت الكوندشن دي عشان اضمن ان السيز ميزدش عن 11

            //اتعدلت بعد تحويل الفايل نيم ل كراكتر اراي
            byte[] convert = Encoding.ASCII.GetBytes(name);
            char [] newname = Encoding.ASCII.GetChars(convert);
            int len = newname.Length;
            fileName = newname;
         
            fileAttribute = attr;          
            firstCluster = fcluster;
            fileSize = fSize;
        }
        public DirectoryEntry()
        { }
        // وظيفتها تاخد الديركتوري انتري الواحد وترجعلي البايتس اللي فيه
        public byte[] getBytes()
        {
            byte[] b = new byte[32];
            byte[] fc = BitConverter.GetBytes(firstCluster);
            byte[] fz = BitConverter.GetBytes(fileSize);
            for (int i = 0; i < 32; i++)
            {
                if (i < 11)
                {   
                    //////////////////////////
                    //اتعدلت بعد تحويل الفايل نيم ل كراكتر اراي
                    if (i<fileName.Length) b[i] = Encoding.ASCII.GetBytes(fileName)[i];
                }
                else if(i == 11)
                {
                    b[i] = fileAttribute;
                }
                else if(i>=12 && i<24)
                {
                    b[i] = fileEmpty[i-12];
                }
                else if(i>=24 && i<28)
                {
                    b[i] = fc[i - 24];
                }
                else if (i >= 28 && i < 32)
                {
                    b[i] = fz[i - 28];
                }
            }
           return b;
        }
        // وظيفتها تاخد ارراي من البايتس وتكتبهم في مكونات الديركتوري انتري 
        public DirectoryEntry getDirectoryEntry(byte[] b)
        {
            // Console.WriteLine(firstCluster);
            // شاكين في البت كونفيرتر
            //
            DirectoryEntry d = new DirectoryEntry();
            char[] fn=new char[11];
            byte[] fc = new byte[4];
            byte[] fs = new byte[4];
            for (int i = 0; i < 32; i++)
            {
                if (i < 11)
                {
                    //اتعدلت بعد تحويل الفايل نيم ل كراكتر اراي
                    fn[i] =(char)(b)[i]; 
                }
                else if (i == 11)
                {
                    d.fileAttribute = b[i] ;
                }
                else if (i >= 12 && i < 24)
                {
                    d.fileEmpty[i - 12] = b[i] ;
                }
                
                else if (i >= 24 && i < 28)
                {
                    fc[i - 24]  = b[i] ;
                }
                else if (i >= 28 && i < 32)
                {
                    fs[i - 28] = b[i] ;
                }
            }
            d.fileName = fn;
            d.firstCluster = BitConverter.ToInt32(fc, 0);
           // Console.WriteLine(firstCluster);
            d.fileSize = BitConverter.ToInt32(fs, 0);
            return d;
        }
        public DirectoryEntry getDirectoryEntry()
        {
            return this;
        }
    }
}