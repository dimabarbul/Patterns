using System.Drawing;
using Patterns.Core;

namespace Patterns.Console.Printers;

public class AsciiPrinter : IPrinter
{
    private static readonly Dictionary<Color, char> Colors = new()
    {
        { Color.White, ' ' },
        { Color.Black, '█' },
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
        System.Console.Write(this.GetColorChar(cell.Color));
    }

    private char GetColorChar(Color color)
    {
        return Colors[color];
    }
}
