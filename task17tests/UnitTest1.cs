namespace task17tests;
using System;
using System.Threading;
using task17;
using Xunit;

public class CommandsTests
{
    [Fact]
    public void ShouldDoHardStop()
    {
        var server = new MockServerThread();
        server.Start();

        var command = new ServerThread.HardStopCommand(server);
        command.Execute();

        Assert.True(server.HardStopCalled);
        server.Dispose();
    }

    [Fact]
    public void ShouldDoSoftStop()
    {
        var server = new MockServerThread();
        server.Start();

        var command = new ServerThread.SoftStopCommand(server);
        command.Execute();

        Assert.True(server.SoftStopCalled);
        server.Dispose();
    }

    private class MockServerThread : ServerThread
    {
        public bool HardStopCalled { get; set; }
        public bool SoftStopCalled { get; set; }

        public override void SoftStop(bool skipThreadCheck = false)
        {
            SoftStopCalled = true;
            base.SoftStop(true);
        }

        public override void HardStop(bool skipThreadCheck = false)
        {
            HardStopCalled = true;
            base.HardStop(true);
        }
    }
}
