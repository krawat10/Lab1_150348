using System.IO;

namespace Lab1_150348.Model {
    public class File
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public long FileSize { get; set; }

        public File(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            FullName = fileInfo.FullName;
            FileSize =  fileInfo.Length;
        }

        public File()
        {
            
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}