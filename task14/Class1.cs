namespace task14;
using System;
using System.Threading;
using System.Collections.Generic;

public class DefiniteIntegral
{
    public static object locker = new object();
    public static double totalSum = 0;

    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        if (threadsNumber <= 0) throw new ArgumentException("Threads number must be > 0");
        if (a >= b) throw new ArgumentException("Invalid interval: a must be less than b");

        totalSum = 0;
        double segment = (b - a) / threadsNumber;
        List<Thread> threads = new List<Thread>();

        for (int i = 0; i < threadsNumber; i++)
        {
            double start = a + i * segment;
            double end = (i == threadsNumber - 1) ? b : start + segment;

            Thread thread = new Thread(() =>
            {
                double part = CalculateIntegral(start, end, function, step);
                lock (locker)
                {
                    totalSum += part;
                }
            });

            threads.Add(thread);
            thread.Start();
        }

        foreach (Thread t in threads)
        {
            t.Join();
        }

        return totalSum;
    }

    public static double CalculateIntegral(double a, double b, Func<double, double> function, double step)
    {
        double sum = 0;
        double current = a;

        while (current < b)
        {
            double next = current + step;
            if (next > b) next = b;

            double area = (function(current) + function(next)) * (next - current) / 2;
            sum += area;
            current = next;
        }

        return sum;
    }
}
