namespace task07;
using System;
using System.Reflection;

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

[DisplayNameAttribute("Пример класса")]
[VersionAttribute(1, 0)]
public class SampleClass
{
    [DisplayNameAttribute("Числовое свойство")]
    public int Number { get; set; }

    [DisplayNameAttribute("Тестовый метод")]
    public void TestMethod() 
    {
    }
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var classDisplayAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (classDisplayAttr != null)
        {
            Console.WriteLine("Class display name: " + classDisplayAttr.DisplayName);
        }

        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        if (versionAttr != null)
        {
            Console.WriteLine($"Class version: {versionAttr.Major}.{versionAttr.Minor}");
        }

        Console.WriteLine("\nMethods:");
        MethodInfo[] methods = type.GetMethods();
        foreach (MethodInfo method in methods)
        {
            var attr = method.GetCustomAttribute<DisplayNameAttribute>();
            if (attr != null)
            {
                Console.WriteLine($"{method.Name} - {attr.DisplayName}");
            }
        }

        Console.WriteLine("\nProperities:");
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var attr = property.GetCustomAttribute<DisplayNameAttribute>();
            if (attr != null)
            {
                Console.WriteLine($"{property.Name} - {attr.DisplayName}");
            }
        }
    }
}
