using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Lab1_150348.Comparers;
using Newtonsoft.Json;

namespace Lab1_150348
{
    public class Application
    {
        private readonly DirectoryInfo _direcory;
        private readonly Regex _regex;
        private int _maxRecursiveDepth;

        public Application(DirectoryInfo direcory, Regex regex)
        {
            _direcory = direcory;
            _regex = regex;
            Int32.TryParse(ConfigurationManager.AppSettings["recursive_depth"], out _maxRecursiveDepth);
        }

        public void WriteDirectoryName()
        {
            Console.WriteLine("Directory path:");
            Console.WriteLine(_direcory.FullName);
            Console.WriteLine();
        }

        public void WriteDirectoryTree()
        {
            Console.WriteLine("Directory tree:");
            PrintDirectoryContent(_direcory);
            Console.WriteLine();
        }

        public void WriteOldestFileCreationDate()
        {
            Console.WriteLine("Oldest element creation date:");
            Console.WriteLine(_direcory.GetOldestDirectoryChildDate());
            Console.WriteLine();
        }

        public void WriteDirectoryNameWithAttribute()
        {
            Console.WriteLine("File DOS attributes (RAHS):");
            var fileInfo = _direcory.GetRandomFile();
            Console.WriteLine($"{fileInfo.Name}: {fileInfo.GetRAHSAttributes()}");
            Console.WriteLine();
        }

        public void WriteDirectoryOrderedFirstChild()
        {
            var orderedCollection = _direcory
                .GetFiles()
                .OrderBy(info => info.FullName, new FilePatchComparer())
                .ToDictionary(info => info.Name, info => info.Length);

            Console.WriteLine();
            Console.WriteLine("First directory's child files (ordered by length and name):");
            foreach (var pair in orderedCollection)
            {
                Console.WriteLine($"{pair.Key} ({pair.Value} bytes)");
            }
        }

        public static Model.Directory FromFile(string path = ".\\output.json")
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var deserialize = serializer.Deserialize<Model.Directory>(new JsonTextReader(file));

                    Console.WriteLine($"Deserialized {path}");
                    return deserialize;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine($"Deserialization failed. File {path} does not exists.");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.Error.WriteLine($"Unable to access file {path}");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.Error.WriteLine($"Path {path} does not exists.");
            }

            return null;
        }

        public void Serialize(string filename = "output.json")
        {
            var path = Path.Combine(Environment.CurrentDirectory, filename);
            try
            {
                if(File.Exists(path)) throw new AccessViolationException();

                using (StreamWriter stream = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(stream, new Model.Directory(_direcory));
                }
            }
            catch (AccessViolationException e)
            {
                Console.Error.WriteLine($"Serialization failed. File {path} already exists.");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.Error.WriteLine($"Unable to access file {path}");
            }
            catch (ArgumentNullException e)
            {
                Console.Error.WriteLine("Path does not exists");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.Error.WriteLine($"Path {path} does not exists.");
            }
        }

        private void PrintDirectoryContent(DirectoryInfo directoryInfo, int depth = 0)
        {
            var indentation = "\t".Repeat(depth);


            foreach (var file in directoryInfo.EnumerateFiles().Where(info => _regex.IsMatch(info.Name)))
            {
                Console.WriteLine($"{indentation}{file.ToConsoleString()}");
            }

            foreach (var directory in directoryInfo.EnumerateDirectories().Where(info => _regex.IsMatch(info.Name)))
            {
                Console.WriteLine($"{indentation}{directory.ToConsoleString()}");

                if (depth < _maxRecursiveDepth) PrintDirectoryContent(directory, depth + 1);
            }
        }
    }
}