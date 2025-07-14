using System;
using System.IO;
using CommandLib;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private readonly string _path;

        public DirectorySizeCommand(string path)
        {
            _path = path;
        }

        public void Execute()
        {
            long size = 0;
            var files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                size += new FileInfo(file).Length;
            }
            Console.WriteLine($"Directory size: {size} bytes");
        }
    }
    public class FindFilesCommand : ICommand
    {
        private readonly string _path;
        private readonly string _pattern;

        public FindFilesCommand(string path, string pattern)
        {
            _path = path;
            _pattern = pattern;
        }

        public void Execute()
        {
            var files = Directory.GetFiles(_path, _pattern);
            Console.WriteLine($"Found files:");
            foreach (var file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }
        }
    }
}
