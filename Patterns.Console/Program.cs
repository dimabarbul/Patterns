using CommandLine;
using Patterns.Console.Printers;
using Patterns.Core;

namespace Patterns.Console;

internal static class Program
{
    private static IPrinter? printer;
    private static IAlgorithm? algorithm;

    public static void Main(string[] args)
    {
        ProgramArgs programArgs = ParseArgs(args);

        algorithm = new AlgorithmFactory().Create(programArgs.AlgorithmType, programArgs.AlgorithmArguments);
        using Timer timer = new(OnTimer, null, programArgs.Delay, programArgs.Delay);
        printer = programArgs.PrinterType switch
        {
            PrinterType.Ascii => new AsciiPrinter(),
            PrinterType.Color => new ColorPrinter(),
            _ => throw new Exception($"Wrong printer type {programArgs.PrinterType}."),
        };

        Thread.Sleep(int.MaxValue);
    }

    private static void OnTimer(object? state)
    {
        if (printer == null || algorithm == null)
        {
            throw new Exception("Printer or algorithm hasn't been created.");
        }

        printer.Print(algorithm.GetNext());
    }

    private static ProgramArgs ParseArgs(string[] args)
    {
        ProgramArgs result = new();

        Parser.Default
            .ParseArguments<ProgramOptions>(args)
            .WithNotParsed(_ => Environment.Exit(1))
            .WithParsed(pa =>
            {
                Dictionary<string, string> algorithmArgs = pa.AlgorithmArgumentsOptions
                    .Select(a => a.Split('=', 2).Select(s => s.Trim()).ToArray())
                    .ToDictionary(x => x[0], x => x[1]);

                algorithmArgs["width"] = pa.Width.ToString();
                algorithmArgs["height"] = pa.Height.ToString();

                result = new ProgramArgs()
                {
                    Delay = pa.Delay,
                    AlgorithmType = pa.AlgorithmType,
                    PrinterType = pa.PrinterType,
                    AlgorithmArguments = algorithmArgs,
                };
            });

        return result;
    }

    private class ProgramOptions
    {
        [Option('t', "type", Required = true, HelpText = "Algorithm type: Life, Flame.")]
        public AlgorithmType AlgorithmType { get; set; }

        [Option('w', "width", HelpText = "Width in cells.")]
        public int Width { get; set; } = 20;

        [Option('h', "height", HelpText = "Height in cells.")]
        public int Height { get; set; } = 10;

        [Option('d', "delay", HelpText = "Delay in milliseconds.")]
        public int Delay { get; set; } = 500;

        [Option('p', "printer", HelpText = "Printer type: Ascii, Color.")]
        public PrinterType PrinterType { get; set; } = PrinterType.Ascii;

        [Option('a', "arg", HelpText = "Algorithm specific arguments, e.g.: sparsity=2 ...")]
        public IEnumerable<string> AlgorithmArgumentsOptions { get; set; } = new List<string>();
    }

    private class ProgramArgs
    {
        public AlgorithmType AlgorithmType { get; set; }

        public int Delay { get; set; } = 500;

        public PrinterType PrinterType { get; set; } = PrinterType.Ascii;

        public Dictionary<string, string> AlgorithmArguments { get; set; } = new();
    }
}
