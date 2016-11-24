using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace File1000s
{
    class Program
    {
        static void Main(string[] args)
        {
            int nFileCount = 10;
            DateTime dtStart;
            DateTime dtEnd;
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string str = File.ReadAllText(Path.Combine(dir.FullName, "template.txt"));

            string outDir = Path.Combine(dir.FullName, "out");
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            ThreadPool.SetMaxThreads(10, 10);
            CountdownEvent countdown = new CountdownEvent(nFileCount);

            dtStart = DateTime.Now;
            //for (int i = 0; i < 10; i++)
            //{
            //    File.WriteAllText(Path.Combine(dir.FullName, string.Format("out\\file{0:0000}.txt", i)), str);
            //}
            //for (int i = 0; i < nFileCount; i++)
            //{
            //    Thread.Sleep(100);
            //}

            for (int i = 0; i < nFileCount; i++)
            {
                CreateFileClass fileClass = new CreateFileClass(i, dir, str, countdown);
                ThreadPool.QueueUserWorkItem(new WaitCallback(fileClass.CreateFile));
            }

            countdown.Wait();
            //WaitHandle.WaitAll(mreArray);
            dtEnd = DateTime.Now;

            TimeSpan ts = dtEnd - dtStart;
            Console.WriteLine("Time esplsed(ms): {0}", ts.TotalMilliseconds);
        }
    }

    public class CreateFileClass
    {
        private int index;
        private DirectoryInfo dir;
        private string content;
        private CountdownEvent cde;

        public CreateFileClass(int nIndex, DirectoryInfo strDir, string strContent, CountdownEvent cd)
        {
            this.index = nIndex;
            this.dir = strDir;
            this.content = strContent;
            this.cde = cd;
        }

        public void CreateFile(object obj)
        {
            File.WriteAllText(Path.Combine(dir.FullName, string.Format("out\\file{0:0000}.txt", index)), content);
            //Thread.Sleep(100);
            cde.Signal();
        }
    }
}
