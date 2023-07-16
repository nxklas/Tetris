using System.Drawing;
using System.Numerics;

namespace Tetris.Core
{
    public interface IRenderable
    {
        Color Color { get; }
        Vector2 Position { get; }
        int Width { get; }
        int Height { get; }
    }
}