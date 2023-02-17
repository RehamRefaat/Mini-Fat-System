using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime;
namespace Shell
{
    class Virtual_Disk
    {   
      static public Directory Initialize_Virtual_Disk()
        {
            Directory root = new Directory("R:".ToCharArray(), 0x10, 5, null);
            // بتأكد لو الفايل مكتوب فيه فخلاص مش محتاجه اعيد كتابه فيه من تاني 
           
            if (!File.Exists("F:\\Shell Randa\\ShellLastRanda\\ShellLast\\ShellLast\\ShellLast\\Shell\\Shell\\Shell\\Virtual_Disk.txt"))
           // if (stream.Length == 0)
            {
               
                FileStream stream = new FileStream(@"F:\Shell Randa\ShellLastRanda\ShellLast\ShellLast\ShellLast\Shell\Shell\Shell\Virtual_Disk.txt", FileMode.OpenOrCreate, FileAccess.Write);
                FAT_Table.InitializeFatTable();
                for (int i = 0; i < 1024 * 1024; i++)
                {
                    // بخلي السوبر بلوك باصفار
                    if (i < 1024)
                    {
                        stream.WriteByte((byte)'0');
                    }
                    // بحجز مكان للفات تابل
                    else if (i >= 1024 && i < 1024 * 5)
                    {
                        stream.WriteByte((byte)'*');
                    }
                    // باقي المساحة بخليها فاضيه
                    else
                    {
                        stream.WriteByte((byte)'#');
                    }
                }
                stream.Close();
                //FAT_Table.InitializeFatTable();
                
                root.WriteDirctory();
                
                FAT_Table.WriteFatTable();
              
            }
            else//لو الفايل موجود
            {
                root.ReadDirectory();
                FAT_Table.GetFatTable();
                
                //root = new Directory("R:".ToCharArray(), 0x10, 5, null);
                //root.WriteDirctory();
               
               // FAT_Table.PrintFatTable();
            }
            return root;
           
        }
        // وظيفة الفانكشن بتستقبل اراي من البايت والاندكس عشان تبدأ تكتب في البلوك اللي مشار له بالاندكس وتكتب فيه الاراي من البايت المبعوت لها
       static public void WriteBlock(byte[] bytes,int index)
        {
           
            FileStream stream = new FileStream(@"F:\Shell Randa\ShellLastRanda\ShellLast\ShellLast\ShellLast\Shell\Shell\Shell\Virtual_Disk.txt", FileMode.Open, FileAccess.ReadWrite);
            stream.Seek(index*1024, SeekOrigin.Begin);
            if (bytes.Length <= 1024)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    stream.Write(bytes, i, 1);
                }
            }
            stream.Close();
        }
        // وظيفتها تقف عند اندكس معين وتقرا بلوك وتبعتهولي
       static  public byte[] Get_Block(int index)
        {
            byte[] block = new byte[1024];
            FileStream stream = new FileStream(@"F:\Shell Randa\ShellLastRanda\ShellLast\ShellLast\ShellLast\Shell\Shell\Shell\Virtual_Disk.txt", FileMode.Open, FileAccess.ReadWrite);
            stream.Seek(index * 1024, SeekOrigin.Begin);
            for (int i = 0; i < block.Length; i++)
            {
                stream.Read(block, i, 1);
            }
            stream.Close();
            return block;
           
        }
    }
}
