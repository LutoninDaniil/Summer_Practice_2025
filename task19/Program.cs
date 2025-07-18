using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Generic;

public interface ICommand
{
    void Execute();
}

public interface IScheduler
{
    bool HasCommand();
    ICommand Select();
    void Add(ICommand cmd);
    void Remove(ICommand cmd);
}

public class RoundRobinScheduler : IScheduler
{
    private Queue<ICommand> Commands = new Queue<ICommand>();
    private object Lock = new object();

    public bool HasCommand()
    {
        lock (Lock) 
        { 
            return Commands.Count != 0;
        }
    }

    public ICommand Select()
    {
        lock (Lock)
        {
            if (Commands.Count == 0) 
            { 
                throw new Exception("Queue empty.");
            }

            var first = Commands.Dequeue();
            Commands.Enqueue(first);
            return first;
        }
    }

    public void Add(ICommand cmd)
    {
        if (cmd == null) 
        { 
            throw new Exception("Null command.");
        }
        lock (Lock) 
        { 
            Commands.Enqueue(cmd);
        }
    }

    public void Remove(ICommand cmd)
    {
        lock (Lock)
        {
            var tempList = new List<ICommand>();
            foreach (var command in Commands)
            {
                if (command != cmd)
                {
                    tempList.Add(command);
                }
            }
            Commands = new Queue<ICommand>(tempList);
        }
    }
}

public class TestCommand : ICommand
{
    private int id;
    private int counter = 0;
    private int MaxExecute;
    public bool IsFinished = false;

    public TestCommand(int Id, int maxExecutions = 3)
    {
        id = Id;
        MaxExecute = maxExecutions;
    }

    public void Execute()
    {
        if (counter >= MaxExecute) 
        {
            IsFinished = true;
            return; 
        }
        
        Console.WriteLine($"Поток {id} вызов {++counter}");
        Thread.Sleep(100);

        if (counter >= MaxExecute) 
        { 
            Console.WriteLine($"Поток {id} завершен"); 
        }
    }

    public bool IsCompleted => counter >= MaxExecute;
}

public class ServerThread : IDisposable
{
    private BlockingCollection<ICommand> NewCommands = new BlockingCollection<ICommand>();
    private IScheduler Scheduler;
    public volatile bool flag;
    private Thread? workThread;

    public ServerThread(IScheduler scheduler)
    {
        Scheduler = scheduler;
    }

    public void Start()
    {
        if (workThread != null) 
        { 
            throw new Exception("Already started.");
        }
        flag = true;
        workThread = new Thread(ProcessCommands) 
        { 
            IsBackground = true,
            Name = "Worker"
        };
        workThread.Start();
    }

    private void ProcessCommands()
    {
        try
        {
            while (flag == true)
            {
                if (NewCommands.Count > 0)
                {
                    while (NewCommands.TryTake(out var newCommand))
                    {
                        Scheduler.Add(newCommand);
                    }
                }

                if (Scheduler.HasCommand())
                {
                    var command = Scheduler.Select();
                    command.Execute();

                    if (command is TestCommand testCmd)
                    {
                        if (testCmd.IsCompleted)
                        {
                            Scheduler.Remove(command);
                        }
                    }
                }
                else
                {
                    var command = NewCommands.Take();
                    Scheduler.Add(command);
                }
            }
        }
        catch (ThreadInterruptedException)
        {
        }
    }

    public void AddCommand(ICommand command)
    {
        if (!flag) 
        { 
            throw new Exception("Server stopped."); 
        }
        NewCommands.Add(command);
    }

    public void HardStop()
    {
        if (Thread.CurrentThread != workThread) 
        { 
            throw new Exception("Wrong thread.");
        }
        flag = false;
        workThread?.Interrupt();
    }

    public void Dispose()
    {
        flag = false;
        NewCommands.CompleteAdding();
        workThread?.Interrupt();
        workThread?.Join();
        NewCommands.Dispose();
    }
}

class App
{
    static void Main()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);
        server.Start();

        for (int i = 1; i <= 5; i++)
        {
            server.AddCommand(new TestCommand(i));
        }

        Thread.Sleep(3000);

        Console.WriteLine("HardStop.");
        server.Dispose();
    }
}
