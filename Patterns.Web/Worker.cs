using System.Drawing;
using System.Text.Json;
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
        Timer timer = new(this.OnTimer, connectionId, delay, delay);

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
        this.hubContext.Clients.Client(connectionId).SendAsync("NewCells", this.SerializeCells(cells))
            .Wait();
    }

    private string[][] SerializeCells(Cell[,] cells)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);
        string[][] cellsArray = new string[width][];
        for (int i = 0; i < width; i++)
        {
            cellsArray[i] = new string[height];
            for (int j = 0; j < height; j++)
            {
                Color color = cells[i, j].Color;
                cellsArray[i][j] = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
        }

        return cellsArray;
    }
}
