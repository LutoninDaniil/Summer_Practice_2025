using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
namespace task11;

public static class ClassGenerator
{
    public static dynamic CreateCalculator()
    {
        string CalculatorClassDefinition = @"
            using System;
            public class Calculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }
            ";

        string scriptCode = CalculatorClassDefinition + "\nnew Calculator()";

        var options = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default;
        options = options.WithReferences(typeof(object).Assembly);
        options = options.WithReferences(typeof(System.Dynamic.DynamicObject).Assembly);
        options = options.WithImports("System");

        var task = CSharpScript.EvaluateAsync<dynamic>(scriptCode, options);
        task.Wait();

        return task.Result;
    }
}
