using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Lab1_150348.Comparers;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Lab1_150348
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Argument with directory path is not provided.");
                return;
            }

            if (!Directory.Exists(args[0]))
            {
                Console.Error.WriteLine($"Directory '{args[0]}' does not exists");
                return;
            }


            if (args.Length == 2)
            {
                Console.WriteLine("Deserialization");
                Console.Write(Application.FromFile(args[1]));
                Console.WriteLine();
            }

            Regex regex = new Regex(".*"); // .*json$    ^[A-z.]+$
            if (args.Length == 3)
            {
                regex = new Regex(args[2].Replace("\\", "\\\\"));
            }

            var app = new Application(new DirectoryInfo(args.FirstOrDefault()), regex);
            
            app.WriteDirectoryName();
            app.WriteDirectoryTree();
            app.WriteDirectoryNameWithAttribute();
            app.WriteOldestFileCreationDate();
            app.WriteDirectoryOrderedFirstChild();

            app.Serialize();

            Console.ReadLine();
        }
    }
}