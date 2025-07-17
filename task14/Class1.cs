namespace task14;
using System;
using System.Collections.Generic;
using System.Threading;

public static class DefiniteIntegral
{
    private static readonly object locker = new object();
    private static double totalResult = 0;

    public static double Solve(double a, double b, Func<double, double> f, double step, int threadsNumber)
    {
        if (threadsNumber <= 0) 
        {
            throw new ArgumentException("threads must be > 0");
        }
        if (a >= b) 
        {
            throw new ArgumentException("a must be less than b");
        }
        
        totalResult = 0;
        List<Thread> threadsList = new List<Thread>();
        double segmentLength = (b - a) / threadsNumber;
        
        for (int i = 0; i < threadsNumber; i++)
        {
            double start = a + i * segmentLength;
            double end = (i == threadsNumber - 1) ? b : start + segmentLength;
            
            Thread thread = new Thread(() => 
            {
                double part = CalculateIntegral(f, start, end, step);
                lock (locker) 
                {
                    totalResult += part;
                }
            });
            
            threadsList.Add(thread);
            thread.Start();
        }
        
        foreach (Thread t in threadsList)
        {
            t.Join();
        }
        
        return totalResult;
    }

    public static double CalculateIntegral(Func<double, double> f, double a, double b, double step)
    {
        double sum = 0;
        double current = a;
        int stepsCount = (int)Math.Ceiling((b - a) / step);
        
        for (int i = 0; i < stepsCount; i++)
        {
            double next = current + step;
            if (next > b) next = b;
            
            double area = (f(current) + f(next)) * (next - current) / 2;
            sum += area;
            current = next;
        }
        
        return sum;
    }
}
