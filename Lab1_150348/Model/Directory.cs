using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab1_150348.Model
{
    public class Directory
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public long FileSystemCount { get; set; }

        public IEnumerable<Directory> Directories { get; set; }
        public IEnumerable<File> Files { get; set; }
        public Directory(DirectoryInfo directoryInfo)
        {
            Name = directoryInfo.Name;
            FullName = directoryInfo.FullName;
            FileSystemCount = directoryInfo.EnumerateFileSystemInfos().Count();
            Directories = directoryInfo.EnumerateDirectories().Select(info => new Directory(info));
            Files = directoryInfo.EnumerateFiles().Select(info => new File(info));
        }

        public Directory()
        {
            
        }

        public override string ToString()
        {
            var result = FullName;
            result += Environment.NewLine;

            var content = new List<string>();

            content.AddRange(Directories.Select(directory => $"--{directory.Name}"));
            content.AddRange(Files.Select(directory => $"--{directory.Name}"));
            
            result += content.Aggregate((s, s1) => s + "\n" + s1);

            return result;
        }
    }
}