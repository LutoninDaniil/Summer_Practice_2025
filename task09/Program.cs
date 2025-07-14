namespace task09;
using System;
using System.Reflection;
using CommandLib;
using System.Collections.Generic;

public class App
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Input error");
            return;
        }

        var assembly = Assembly.LoadFrom(args[0]);
        WriteAssemblyMetadata(assembly);
    }

    public static void WriteAssemblyMetadata(Assembly assembly)
    {
        Type[] allTypes = assembly.GetTypes();
        foreach (Type type in allTypes)
        {
            if (type.IsClass)
            {
                PrintTypeMetadata(type);
                Console.WriteLine(new string('=', 50));
            }
        }
    }

    public static void PrintTypeMetadata(Type type)
    {
        Console.WriteLine($"Class: {type.Name}");
        PrintAttributes(type);

        Console.WriteLine("\nConstructors:");
        ConstructorInfo[] ctors = type.GetConstructors();
        for (int i = 0; i < ctors.Length; i++)
        {
            ConstructorInfo ctor = ctors[i];
            ParameterInfo[] parameters = ctor.GetParameters();
            string paramString = "";
            for (int j = 0; j < parameters.Length; j++)
            {
                paramString += parameters[j].ParameterType.Name + " " + parameters[j].Name;
                if (j < parameters.Length - 1) paramString += ", ";
            }
            Console.WriteLine($"{type.Name}({paramString})");
            PrintAttributes(ctor);
        }

        Console.WriteLine("\nMethods:");
        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
        for (int i = 0; i < methods.Length; i++)
        {
            MethodInfo method = methods[i];
            ParameterInfo[] parameters = method.GetParameters();
            string paramString = "";
            for (int j = 0; j < parameters.Length; j++)
            {
                paramString += parameters[j].ParameterType.Name + " " + parameters[j].Name;
                if (j < parameters.Length - 1) paramString += ", ";
            }
            Console.WriteLine($"{method.ReturnType.Name} {method.Name}({paramString})");
            PrintAttributes(method);
        }

        Console.WriteLine("\nProperties:");
        PropertyInfo[] properties = type.GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            string accessors = "";
            if (prop.CanRead) accessors += "get; ";
            if (prop.CanWrite) accessors += "set;";
            Console.WriteLine($"  {prop.PropertyType.Name} {prop.Name} {{ {accessors} }}");
            PrintAttributes(prop);
        }
    }

    static void PrintAttributes(MemberInfo member)
    {
        IList<CustomAttributeData> attributes = member.GetCustomAttributesData();
        if (attributes.Count == 0) return;

        Console.WriteLine("Attributes:");
        foreach (CustomAttributeData attr in attributes)
        {
            Console.Write(attr.AttributeType.Name);
            if (attr.ConstructorArguments.Count > 0)
            {
                Console.Write("(");
                for (int i = 0; i < attr.ConstructorArguments.Count; i++)
                {
                    Console.Write(attr.ConstructorArguments[i].Value);
                    if (i < attr.ConstructorArguments.Count - 1) Console.Write(", ");
                }
                Console.Write(")");
            }
            Console.WriteLine();
        }
    }
}

