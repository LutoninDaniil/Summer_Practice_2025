namespace CommandLib;
using System;
using System.Reflection;
public interface ICommand
{
    void Execute();
}
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }
    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }
    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}
