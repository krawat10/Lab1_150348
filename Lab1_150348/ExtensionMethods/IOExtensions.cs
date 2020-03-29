using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab1_150348
{
    public static class IOExtensions
    {
        public static DateTime GetOldestDirectoryChildDate(this DirectoryInfo directoryInfo, bool recursive = true)
        {
            return directoryInfo
                .EnumerateFileSystemInfos("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Max(info => info.CreationTime);
        }

        public static string ToConsoleString(this FileSystemInfo fileSystemInfo) =>
            fileSystemInfo switch
            {
                DirectoryInfo directoryInfo =>
                $"{directoryInfo.Name} (folder, {directoryInfo.EnumerateFileSystemInfos().Count()} items, {directoryInfo.GetRAHSAttributes()})",
                FileInfo fileInfo =>
                $"{fileInfo.Name} (file, {fileInfo.Length} bytes, {fileInfo.GetRAHSAttributes()})",
                _ => String.Empty
            };

        public static FileInfo GetRandomFile(this DirectoryInfo directoryInfo, bool recursive = true)
        {
            var files = directoryInfo
                .EnumerateFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            var count = files.Count();

            var fileIndex = new Random().Next(0, files.Count() - 1);

            return files.Skip(fileIndex).FirstOrDefault();
        }

        public static string GetRAHSAttributes(this FileSystemInfo fileSystemInfo, bool recursive = true)
        {
            StringBuilder builder = new StringBuilder("____");
            if (fileSystemInfo.Attributes.HasFlag(FileAttributes.ReadOnly)) builder[0] = 'R';
            if (fileSystemInfo.Attributes.HasFlag(FileAttributes.Archive)) builder[1] = 'A';
            if (fileSystemInfo.Attributes.HasFlag(FileAttributes.Hidden)) builder[2] = 'H';
            if (fileSystemInfo.Attributes.HasFlag(FileAttributes.System)) builder[3] = 'S';

            return builder.ToString();
        }
    }
}