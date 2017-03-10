using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace UpdateFolderFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string Htmlstring = "<p>Pargraph</p>img:<img src='http://www.baidu.com/img.png' />Some string";

            Regex reg = new Regex(@"<(.[^>]*)>");
            MatchCollection coll = reg.Matches(Htmlstring);

            string[] list = GetHtmlImageUrlList(Htmlstring);

            string str = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Console.WriteLine(str);
            return;

            //Console.WriteLine("{0:yyyyMMddHHmmssfffffff}", DateTime.Now);
            //Console.WriteLine(GuidTo16String());
            //return;

            string cet = "e0-6d-eb-25-6c-5b-38-4f-71-b9-a2-d1-eb-26-c5-1d-e2-2e-e5-57";
            HexToData("e0-6d-eb-25-6c-5b-38-4f-71-b9-a2-d1-eb-26-c5-1d-e2-2e-e5-57");
            HexToData(cet);
            return;

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

        private static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
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

        private static void ShowLength(string s)
        {
            Console.WriteLine(s.Length);
        }

        private static byte[] HexToData(string hexValue)
        {
            if (hexValue == null)
            {
                return null;
            }

            hexValue = hexValue.Replace(" ", string.Empty);
            hexValue = hexValue.Replace("-", string.Empty);
            if (hexValue.Length % 2 == 1)
            {
                // Up to you whether to pad the first or last byte
                hexValue = '0' + hexValue;
            }

            byte[] data = new byte[hexValue.Length / 2];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(hexValue.Substring(i * 2, 2), 16);
            }

            return data;
        }

        private static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}
