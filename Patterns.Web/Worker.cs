using System.Drawing;
using Microsoft.AspNetCore.SignalR;
using Patterns.Core;
using Patterns.Web.Hubs;

namespace Patterns.Web;

public class Worker
{
    private readonly AlgorithmFactory algorithmFactory;
    private readonly IHubContext<PatternsHub> hubContext;

    private readonly Dictionary<string, WorkerData> data = new();

    public Worker(AlgorithmFactory algorithmFactory, IHubContext<PatternsHub> hubContext)
    {
        this.algorithmFactory = algorithmFactory;
        this.hubContext = hubContext;
    }

    public void Start(string connectionId, AlgorithmType type, int delay, Dictionary<string, string> args)
    {
        IAlgorithm algorithm = this.algorithmFactory.Create(type, args);
        Timer timer = new(this.OnTimer, connectionId, 0, delay);

        this.data[connectionId] = new WorkerData(algorithm, timer);
    }

    private void OnTimer(object? state)
    {
        string connectionId = state as string ?? throw new ArgumentException("Expected string.", nameof(state));

        WorkerData workerData = this.data[connectionId];
        this.SendNewCells(connectionId, workerData.Algorithm.GetNext());
    }

    private class WorkerData
    {
        public WorkerData(IAlgorithm algorithm, Timer timer)
        {
            this.Algorithm = algorithm;
            this.Timer = timer;
        }

        public IAlgorithm Algorithm { get; }
        public Timer Timer { get; }
    }

    public void Stop(string connectionId)
    {
        if (!this.data.ContainsKey(connectionId))
        {
            return;
        }

        this.data[connectionId].Timer.Dispose();
        this.data.Remove(connectionId);
    }

    private void SendNewCells(string connectionId, Cell[,] cells)
    {
        this.hubContext.Clients.Client(connectionId)
            .SendAsync("NewCells", cells.GetLength(0), cells.GetLength(1), this.ConvertCells(cells))
            .Wait();
    }

    private int[] ConvertCells(Cell[,] cells)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);
        int[] numbers = new int[width * height * 3];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Color color = cells[i, j].Color;
                numbers[3 * (j * width + i)] = color.R;
                numbers[3 * (j * width + i) + 1] = color.G;
                numbers[3 * (j * width + i) + 2] = color.B;
            }
        }

        return numbers;
    }
}
