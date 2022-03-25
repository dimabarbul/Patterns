using System.Drawing;
using Patterns.Core;

namespace Patterns.Console.Printers;

public class ColorPrinter : IPrinter
{
    private static readonly Dictionary<Color, ConsoleColor> Colors = new()
    {
        { Color.White, ConsoleColor.White },
        { Color.Black, ConsoleColor.Black },
        { Color.Blue, ConsoleColor.Blue },
        { Color.Green, ConsoleColor.Green },
        { Color.Red, ConsoleColor.Red },
        { Color.Yellow, ConsoleColor.Yellow },
        { Color.Cyan, ConsoleColor.Cyan },
        { Color.Gray, ConsoleColor.Gray },
        { Color.Magenta, ConsoleColor.Magenta },
    };

    private bool isFirstTime = true;

    public void Print(Cell[,] cells)
    {
        if (this.isFirstTime)
        {
            System.Console.Clear();
            System.Console.CursorVisible = false;
        }

        this.isFirstTime = false;

        int width = cells.GetLength(0);
        int height = cells.GetLength(1);
        for (int i = 0; i < height; i++)
        {
            System.Console.SetCursorPosition(0, i);

            for (int j = 0; j < width; j++)
            {
                this.PrintCell(cells[j, i]);
            }
        }
    }

    private void PrintCell(Cell cell)
    {
        System.Console.BackgroundColor = Colors[cell.Color];
        System.Console.Write(' ');
    }
}
