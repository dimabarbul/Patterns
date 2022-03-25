using CommandLine;
using Patterns.Console.Printers;
using Patterns.Core;

namespace Patterns.Console;

internal static class Program
{
    private static IPrinter printer;
    private static IAlgorithm algorithm;

    public static void Main(string[] args)
    {
        ProgramArgs programArgs = ParseArgs(args);

        algorithm = new AlgorithmFactory().Create(programArgs.AlgorithmType, programArgs.AlgorithmArguments);
        Timer timer = new(OnTimer, null, programArgs.Delay, programArgs.Delay);
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
        printer.Print(algorithm.GetNext());
    }

    private static ProgramArgs ParseArgs(string[] args)
    {
        ProgramArgs result = new();

        Parser.Default
            .ParseArguments<ProgramOptions>(args)
            .WithNotParsed(_ => throw new Exception("Cannot parse."))
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
        [Option('t', "type", Required = true)] public AlgorithmType AlgorithmType { get; set; }

        [Option('w', "width")] public int Width { get; set; } = 20;

        [Option('h', "height")] public int Height { get; set; } = 10;

        [Option('d', "delay")] public int Delay { get; set; } = 500;

        [Option('p', "printer")] public PrinterType PrinterType { get; set; } = PrinterType.Ascii;

        [Option('a', "arg")] public IEnumerable<string> AlgorithmArgumentsOptions { get; set; } = new List<string>();
    }

    private class ProgramArgs
    {
        public AlgorithmType AlgorithmType { get; set; }

        public int Delay { get; set; } = 500;

        public PrinterType PrinterType { get; set; } = PrinterType.Ascii;

        public Dictionary<string, string> AlgorithmArguments { get; set; } = new();
    }
}
