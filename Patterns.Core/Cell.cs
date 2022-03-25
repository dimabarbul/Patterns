using System.Drawing;

namespace Patterns.Core;

public class Cell
{
    public Cell(Color color)
    {
        this.Color = color;
    }

    public Color Color { get; }
}
