using Patterns.Core;

namespace Patterns.Console.Printers;

public interface IPrinter
{
    void Print(Cell[,] cells);
}
