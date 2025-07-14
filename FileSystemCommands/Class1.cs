﻿namespace FileSystemCommands;
using System;
using System.IO;
using CommandLib;
using System.Linq;

public class DirectorySizeCommand : ICommand
{
    public string DirectoryPath;
    public DirectorySizeCommand(string directoryPath)
    {
        DirectoryPath = directoryPath;
    }
     public void Execute()
    {
        var files = Directory.GetFiles(DirectoryPath, "*", SearchOption.AllDirectories);
        long size = 0;
        foreach (var file in files)
        {
            size += new FileInfo(file).Length;
        }
        Console.WriteLine($"Directory size: {size} bytes");
    }
}
public class FindFilesCommand : ICommand
{
    public string DirectoryPath;
    public string SearchPattern;
    public FindFilesCommand(string directoryPath, string searchPattern)
    {
        DirectoryPath = directoryPath;
        SearchPattern = searchPattern;
    }
    public void Execute()
    {
        if (!Directory.Exists(DirectoryPath))
        {
            throw new DirectoryNotFoundException();
        }

        var files = Directory.GetFiles(DirectoryPath, SearchPattern);
        Console.WriteLine($"Found {files.Length} files:");

        // Неоптимальный вывод через цикл
        for (int i = 0; i < files.Length; i++)
        {
            Console.WriteLine(files[i]);
        }
    }
}
