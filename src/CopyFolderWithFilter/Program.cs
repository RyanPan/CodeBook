using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFolderWithFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            string strRoot = Environment.CurrentDirectory;
            string sourceFolder = Path.Combine(strRoot, "确认效果图");
            string targetFolder = Path.Combine(strRoot, "确认效果图切图标注");

            Console.WriteLine("SourceRoot: {0}", sourceFolder);
            Console.WriteLine("TargetRoot: {0}", targetFolder);

            // 拷贝指定的目录
            // 递归遍历目录中的所有子文件夹和子文件，过滤掉指定的文件类型

            //CopyFile(sourceFolder, targetFolder);

            MoveFileAndFolderUnderJpgFolderToParent(targetFolder);

            Console.ReadKey(true);
        }

        private static void CopyFile(string sourceFolder, string targetFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine("Source folder does not exists: {0}", sourceFolder);
                return;
            }
            if (!Directory.Exists(targetFolder))
            {
                Console.WriteLine("Target folder does not exists: {0}", targetFolder);
                return;
            }

            Console.WriteLine("Copy content from {0} to {1}", sourceFolder, targetFolder);

            string[] arrExt = new string[] { ".psd", ".ai" };

            foreach (var f in Directory.GetFiles(sourceFolder))
            {
                if (arrExt.Contains(Path.GetExtension(f).ToLower()))
                {
                    continue;
                }

                string targetFile = Path.Combine(targetFolder, Path.GetFileName(f));
                File.Copy(f, targetFile, true);
            }

            foreach (var d in Directory.GetDirectories(sourceFolder))
            {
                string subTargetFolder = Path.Combine(targetFolder, d.Replace(Directory.GetParent(d).FullName, "").TrimStart('\\'));
                if (!Directory.Exists(subTargetFolder))
                {
                    Console.WriteLine("Target folder created: {0}", subTargetFolder);
                    Directory.CreateDirectory(subTargetFolder);
                }

                CopyFile(d, Path.Combine(targetFolder, subTargetFolder));

                // 判断是否有文件拷贝到目标目录，如果没有则删除目标目录
                if (Directory.GetFiles(subTargetFolder).Count() <= 0 && Directory.GetDirectories(subTargetFolder).Count() <= 0)
                {
                    Directory.Delete(subTargetFolder);
                }
            }
        }

        private static void MoveFileAndFolderUnderJpgFolderToParent(string rootFolder)
        {
            if (!Directory.Exists(rootFolder))
            {
                Console.WriteLine("Folder does not exists: {0}", rootFolder);
            }

            foreach (var d in Directory.GetDirectories(rootFolder))
            {
                MoveFileAndFolderUnderJpgFolderToParent(d);
            }

            string currentFolderName = rootFolder.Replace(Directory.GetParent(rootFolder).FullName, "").TrimStart('\\').ToLower();
            if (currentFolderName == "jpg")
            {
                string parent = Directory.GetParent(rootFolder).FullName;
                MoveFolderContent(rootFolder, parent);
                if (Directory.GetFiles(rootFolder).Count() == 0 && Directory.GetDirectories(rootFolder).Count() == 0)
                {
                    Directory.Delete(rootFolder);
                }
            }
        }

        private static void MoveFolderContent(string rootFolder, string parent)
        {
            foreach (var d in Directory.GetDirectories(rootFolder))
            {
                string currentFolderName = rootFolder.Replace(Directory.GetParent(rootFolder).FullName, "").TrimStart('\\').ToLower();
                string targetFolder = Path.Combine(parent, currentFolderName);
                if (Directory.GetDirectories(targetFolder).Count() > 0)
                {
                    targetFolder = Path.Combine(parent, string.Format("{0}-jpg", currentFolderName));
                }
                Directory.CreateDirectory(targetFolder);
                MoveFolderContent(d, targetFolder);
            }

            foreach (var f in Directory.GetFiles(rootFolder))
            {
                string newFileName = Path.Combine(parent, Path.GetFileName(f));
                if (File.Exists(newFileName))
                {
                    newFileName = Path.Combine(Path.GetDirectoryName(newFileName), string.Format("{0}-jpg{1}", Path.GetFileNameWithoutExtension(newFileName), Path.GetExtension(newFileName)));
                }
                File.Move(f, newFileName);
            }
        }
    }
}
