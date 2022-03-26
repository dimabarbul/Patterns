using System.Diagnostics;
using System.Drawing;
using Patterns.Core;

namespace Patterns.Wasm;

public class Worker
{
    private readonly AlgorithmFactory algorithmFactory;

    private WorkerData? data;

    public Worker(AlgorithmFactory algorithmFactory)
    {
        this.algorithmFactory = algorithmFactory;
    }

    public void Start(AlgorithmType type, int delay, Dictionary<string, string> args, Func<int[][][], Task> callback)
    {
        IAlgorithm algorithm = this.algorithmFactory.Create(type, args);
        Timer timer = new(this.OnTimer, null, 0, delay);

        this.data = new WorkerData(algorithm, timer, callback);
    }

    public void Stop(string key)
    {
        if (this.data == null)
        {
            return;
        }

        this.data.Timer.Dispose();
        this.data = null;
    }

    private void OnTimer(object? state)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Cell[,] cells = this.data!.Algorithm.GetNext();
        Console.WriteLine($"GetNext(): {sw.ElapsedMilliseconds} ms");
        this.SendNewCells(cells);
    }

    private void SendNewCells(Cell[,] cells)
    {
        this.data!.Callback(this.SerializeCells(cells));
    }

    private int[][][] SerializeCells(Cell[,] cells)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);
        int[][][] cellsArray = new int[width][][];
        for (int i = 0; i < width; i++)
        {
            cellsArray[i] = new int[height][];
            for (int j = 0; j < height; j++)
            {
                Color color = cells[i, j].Color;
                cellsArray[i][j] = new int[] { color.R, color.G, color.B };
            }
        }

        return cellsArray;
    }

    private class WorkerData
    {
        public WorkerData(IAlgorithm algorithm, Timer timer, Func<int[][][], Task> callback)
        {
            this.Algorithm = algorithm;
            this.Timer = timer;
            this.Callback = callback;
        }

        public IAlgorithm Algorithm { get; }
        public Timer Timer { get; }
        public Func<int[][][], Task> Callback { get; }
    }
}
