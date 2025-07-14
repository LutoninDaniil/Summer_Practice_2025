using PluginSystem;
namespace TestPlugin;

[PluginLoad]
public class TestPlugin
{
    public void Execute() => Console.WriteLine("Plugin 'TestPlugin' is used");
}
