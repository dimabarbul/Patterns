using Microsoft.AspNetCore.SignalR;
using Patterns.Core;

namespace Patterns.Web.Hubs;

public class PatternsHub : Hub
{
    private readonly Worker worker;

    public PatternsHub(Worker worker)
    {
        this.worker = worker;
    }

    public Task Start(AlgorithmType type, int delay, Dictionary<string, string> args)
    {
        this.worker.Start(this.Context.ConnectionId, type, delay, args);

        return Task.CompletedTask;
    }

    public Task Stop()
    {
        this.worker.Stop(this.Context.ConnectionId);

        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return this.Stop();
    }
}
