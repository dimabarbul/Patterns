using System.Drawing;

namespace Patterns.Core.Algorithms;

public class Flame : IAlgorithm
{
    private const int HiddenBottomLinesCount = 4;

    private static readonly Color[] Palette;

    private readonly int[,] cells;
    private readonly int sparsity;

    static Flame()
    {
        Palette = new Color[256];
        for (int i = 0; i < 64; i++)
        {
            Palette[i] = Color.FromArgb((byte)(i * 4), 0, 0);
        }
        for (int i = 0; i < 64; i++)
        {
            Palette[i + 64] = Color.FromArgb(252, (byte)(i * 2), 0);
        }
        for (int i = 0; i < 128; i++)
        {
            Palette[i + 128] = Color.FromArgb(252, (byte)(i + 128), 0);
        }
    }

    public Flame(int width, int height, int sparsity)
    {
        this.sparsity = sparsity;
        this.cells = new int[width, height + HiddenBottomLinesCount];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height + HiddenBottomLinesCount; y++)
            {
                this.cells[x, y] = 0;
            }
        }
    }

    public Cell[,] GetNext()
    {
        this.CopyCells(out int[,] oldCells);

        Random random = new();
        int width = this.cells.GetLength(0);
        int height = this.cells.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            this.cells[x, height - 1] =
                this.cells[x, height - 2] =
                    (random.Next() % this.sparsity) != 0 ? 0 : Palette.Length - 1;
        }

        for (int y = this.cells.GetLength(1) - 2; y >= 1; y--)
        {
            for (int x = 0; x < this.cells.GetLength(0); x++)
            {
                this.cells[x, y - 1] = this.GetPaletteIndex(x, y, oldCells);
            }
        }

        Cell[,] result = new Cell[this.cells.GetLength(0), this.cells.GetLength(1) - HiddenBottomLinesCount];
        for (int y = 0; y < this.cells.GetLength(1) - HiddenBottomLinesCount; y++)
        {
            for (int x = 0; x < this.cells.GetLength(0); x++)
            {
                result[x, y] = new Cell(Palette[this.cells[x, y]]);
            }
        }

        return result;
    }

    private void CopyCells(out int[,] copy)
    {
        copy = new int[this.cells.GetLength(0), this.cells.GetLength(1)];

        for (int x = 0; x < this.cells.GetLength(0); x++)
        {
            for (int y = 0; y < this.cells.GetLength(1); y++)
            {
                copy[x, y] = this.cells[x, y];
            }
        }
    }

    private int GetPaletteIndex(int x, int y, int[,] oldCells)
    {
        int sum = oldCells[x, y] + oldCells[x, y + 1];
        int count = 2;

        if (x > 0)
        {
            sum += oldCells[x - 1, y];
            count++;
        }

        if (x < this.cells.GetLength(0) - 1)
        {
            sum += oldCells[x + 1, y];
            count++;
        }

        int paletteIndex = sum / count - 1;

        return paletteIndex < 0 ? 0 : paletteIndex;
    }
}