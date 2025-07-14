using System;
using System.IO;
using Xunit;
using FileSystemCommands;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        var output = consoleOutput.ToString();
        Assert.Contains("Directory size: 10 bytes", output);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute();

        var output = consoleOutput.ToString();
        Assert.Contains("Found files:", output);
        Assert.Contains("file1.txt", output);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldThrowForNonexistentDirectory()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
        Directory.CreateDirectory(testDir);
        var invalidDir = Path.Combine(testDir, "nonexistent");
        var command = new DirectorySizeCommand(invalidDir);

        try
        {
            var exception = Assert.Throws<DirectoryNotFoundException>(() => command.Execute());
            Assert.Contains(invalidDir, exception.Message);
        }
        finally
        {
            Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void FindFilesCommand_ShouldReturnEmptyForNoMatches()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.log"), "Log");

        var originalOutput = Console.Out;
        try
        {
            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            var command = new FindFilesCommand(testDir, "*.txt");

            command.Execute();
            var output = consoleOutput.ToString();

            Assert.Contains("Found files:", output);
        }
        finally
        {
            Console.SetOut(originalOutput);
            Directory.Delete(testDir, true);
        }
    }
}
