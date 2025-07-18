using System;
using System.Threading;
using task18;
using Xunit;

namespace task18tests
{
    public class RoundRobinSchedulerTests
    {
        [Fact]
        public void ShouldReturnCommandsInRoundRobinOrder()
        {
            var scheduler = new RoundRobinScheduler();
            var cmd1 = new MockCommand();
            var cmd2 = new MockCommand();

            scheduler.Add(cmd1);
            scheduler.Add(cmd2);

            Assert.Same(cmd1, scheduler.Select());
            Assert.Same(cmd2, scheduler.Select());
            Assert.Same(cmd1, scheduler.Select());

            scheduler.Select();
        }

        [Fact]
        public void ShouldReturnFalseForEmptyScheduler()
        {
            var scheduler = new RoundRobinScheduler();
            Assert.False(scheduler.HasCommand());
        }

        private class MockCommand : ICommand
        {
            public bool Execute() => true;
        }
    }

    public class ServerThreadLongRunningTests
    {
        [Fact]
        public void ShouldProcessLongRunningCommandsInChunks()
        {
            var scheduler = new RoundRobinScheduler();
            var server = new ServerThread(scheduler);
            server.Start();

            var fastCmd = new MockCommand();
            server.AddCommand(fastCmd);

            var longCmd = new LongRunningCommand(5);
            server.AddCommand(longCmd);

            bool completed = SpinWait.SpinUntil(() => fastCmd.IsCompleted, TimeSpan.FromSeconds(2));

            Assert.True(completed, "Complete in 2 seconds");
            server.Dispose();
        }

        private class MockCommand : ICommand
        {
            public bool IsCompleted { get; set; }

            public bool Execute()
            {
                IsCompleted = true;
                return true;
            }
        }
    }
}
