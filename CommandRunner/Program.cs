using System;
using System.Reflection;
using CommandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string dllPath = "FileSystemCommands.dll";

            Assembly assembly = Assembly.LoadFrom(dllPath);

            ICommand sizeCommand = CreateCommand(assembly, "DirectorySizeCommand", @"C:\Test");
            ICommand findCommand = CreateCommand(assembly, "FindFilesCommand", @"C:\Test", "*.txt");

            sizeCommand?.Execute();
            findCommand?.Execute();
        }

        static ICommand CreateCommand(Assembly assembly, string className, params object[] parameters)
        {
            Type type = assembly.GetType($"FileSystemCommands.{className}");
            if (type != null)
            {
                return (ICommand)Activator.CreateInstance(type, parameters);
            }
            Console.WriteLine($"Command {className} not found");
            return null;
        }
    }
}
