using System.Drawing;

namespace Patterns.Core.Algorithms;

public class Life : IAlgorithm
{
    private static Color aliveColor = Color.White;
    private static Color deadColor = Color.Black;

    private readonly Random random = new();
    private readonly Cell[,] cells;

    public Life(int width, int height)
    {
        this.cells = new Cell[width, height];

        this.GenerateRandomCells();
    }

    public Cell[,] GetNext()
    {
        int width = this.cells.GetLength(0);
        int height = this.cells.GetLength(1);

        Cell[,] newCells = new Cell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int aliveNeighbors = this.GetNeighborCells(this.cells, i, j)
                    .Count(c => c.Color == aliveColor);

                newCells[i, j] = new Cell(this.GetNextColor(this.cells[i, j], aliveNeighbors));
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                this.cells[i, j] = newCells[i, j];
            }
        }

        return this.cells;
    }

    private Color GetNextColor(Cell cell, int aliveNeighbors)
    {
        if (cell.Color == aliveColor)
        {
            return aliveNeighbors is 2 or 3 ? aliveColor : deadColor;
        }

        return aliveNeighbors == 3 ? aliveColor : deadColor;
    }

    private IEnumerable<Cell> GetNeighborCells(Cell[,] cells, int column, int row)
    {
        return new[]
            {
                this.GetCell(cells, column - 1, row - 1),
                this.GetCell(cells, column - 1, row),
                this.GetCell(cells, column - 1, row + 1),
                this.GetCell(cells, column, row - 1),
                this.GetCell(cells, column, row + 1),
                this.GetCell(cells, column + 1, row - 1),
                this.GetCell(cells, column + 1, row),
                this.GetCell(cells, column + 1, row + 1),
            }
            .Where(c => c != null)!;
    }

    private Cell? GetCell(Cell[,] cells, int column, int row)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        return column >= 0 && column < width && row >= 0 && row < height ?
            cells[column, row] :
            null;
    }

    private void GenerateRandomCells()
    {
        int width = this.cells.GetLength(0);
        int height = this.cells.GetLength(1);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                this.cells[i, j] = new Cell(this.GetRandomColor());
            }
        }
    }

    private Color GetRandomColor()
    {
        return this.random.Next() % 2 == 0 ?
            Color.Black :
            Color.White;
    }
}
