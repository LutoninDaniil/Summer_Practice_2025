namespace task14tests;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using task14;
using ScottPlot;

public class PerformanceTests
{
    [Fact]
    public void OptimizeParameters()
    {
        double a = -100;
        double b = 100;
        var f = (double x) => Math.Sin(x);
        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        int[] threads = { 1, 2, 4, 8, 12, 16 };

        double optimalStep = steps.OrderBy(step => step).First(step => Math.Abs(DefiniteIntegral.Solve(a, b, f, step, 1)) < 1e-4);

        var results = threads.Select(t => (threads: t, time: MeasureTime(() => DefiniteIntegral.Solve(a, b, f, optimalStep, t))))
        .OrderBy(r => r.time).ToList();

        var singleTime = MeasureTime(() => DefiniteIntegral.Solve(a, b, f, optimalStep, 1));
        var (optimalThreads, multiTime) = results.First();
        double speed = (singleTime - multiTime) / singleTime * 100;

        SaveResults(optimalStep, optimalThreads, singleTime, multiTime, speed);
        GeneratePlot(results, optimalThreads);

        Assert.True(speed >= 15, $"Speed {speed:F1}% < 15%");
    }

    private static string GetSolutionDirectory()
    {
        var directory = Directory.GetCurrentDirectory();
        while (!Directory.GetFiles(directory, "*.sln").Any())
        {
            directory = Directory.GetParent(directory)?.FullName;
            if (directory == null) return Directory.GetCurrentDirectory();
        }
        return directory;
    }

    public static double MeasureTime(Action action)
    {
        return Enumerable.Repeat(action, 5).Select(a =>
        {
            var sw = Stopwatch.StartNew(); a();
            return sw.Elapsed.TotalMilliseconds;
        }).Average();
    }

    private static void SaveResults(double step, int threads, double singleTime, double multiTime, double speed)
    {
        string path = Path.Combine(GetSolutionDirectory(), "results.txt");
        File.WriteAllText(path, $@"Оптимальный шаг интегрирования: {step}
        Оптимальное число потоков: {threads}
        Время однопоточной реализации: {singleTime:F2}
        Время многопоточной реализации: {multiTime:F2}
        Ускорение: {speed:F1}%");
    }

    private static void GeneratePlot(List<(int threads, double time)> data, int optimal)
    {
        string path = Path.Combine(GetSolutionDirectory(), "graph.png");
        var plt = new Plot();
        plt.Title("Оптимизация параметров алгоритма численного интегрирования");
        plt.XLabel("Кол-во потоков");
        plt.YLabel("Время выполнения");

        plt.AddScatter(data.Select(d => (double)d.threads).ToArray(), data.Select(d => d.time).ToArray());
        plt.SaveFig(path, 800, 600);
    }
}
