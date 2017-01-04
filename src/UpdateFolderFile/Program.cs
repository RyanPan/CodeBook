using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UpdateFolderFile
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestBar();
            //return;

            Console.WriteLine("不要关闭这个窗口，处理完会自动关闭！");

            //
            // 修改运行目录中的 htm 文件，将文件中包含的“http://img1”替换为“//img1”
            //

            // Set root folder
            //string strRoot = @"D:\work\YDBS_MFQJ\YDBS_MFQJ\Report\Gen\1x";
            string strRoot = Environment.CurrentDirectory;

            // Read file line by line, modify current line if needed, then write to a new file
            foreach (var f in Directory.GetFiles(strRoot))
            {
                FileInfo file = new FileInfo(f);
                if (string.Format("{0}", file.Extension).ToLower() != ".htm")
                {
                    continue;
                }

                string newFileName = Path.Combine(Path.GetDirectoryName(f), string.Format("{0}{1}", Path.GetFileNameWithoutExtension(f), ".htm0"));

                using (StreamReader reader = new StreamReader(f))
                {
                    using (FileStream newFileStream = File.Open(newFileName, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter writer = new StreamWriter(newFileStream))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();

                                line = line.Replace("http://img1", "//img1");

                                writer.WriteLine(line);
                            }
                        }
                    }
                }

                // Delete original file
                File.Delete(f);

                // Rename the new file as the original
                File.Move(newFileName, f);
            }
        }

        private static void TestBar()
        {
            Random r = new Random();
            while (true)
            {
                ConsoleProgressBar bar = new ConsoleProgressBar("Test the progressbar");
                int c = r.Next(534);
                for (int i = 1; i <= c; i++)
                {
                    bar.Update(i, c, string.Format("完成进度：{0}/{1}", i, c));
                    Thread.Sleep(100);
                }

                Console.ReadKey(true);
            }
        }
    }
}
