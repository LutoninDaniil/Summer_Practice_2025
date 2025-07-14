using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;
namespace PluginSystem;

public class PluginLoadAttribute : Attribute
{
    public string[] Dependencies { get; }
    public PluginLoadAttribute(params string[] dependencies) => Dependencies = dependencies;
}

public class PluginSystem
{
    public static void Run()
    {
        var pluginFolder = Path.Combine("Plugins");
        var plugins = Directory.EnumerateFiles(pluginFolder, "*.dll")
            .Select(Assembly.LoadFrom)
            .SelectMany(a => a.GetExportedTypes())
            .Where(t => t.GetCustomAttribute<PluginLoadAttribute>() != null)
            .ToList();
        
        plugins.ForEach(t => 
        {
            var instance = Activator.CreateInstance(t);
            t.GetMethod("Execute")?.Invoke(instance, null);
        });
    }
}

public class App
{
    public static void Main(string[] args) => PluginSystem.Run();
}
