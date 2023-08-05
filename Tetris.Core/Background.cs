using System.Drawing;
using System.Numerics;

namespace Tetris.Core
{
    internal class Background : IRenderable
    {

        public Background(Color color, int width, int height)
        {
            Position = new Vector2(0, 0);
            Color = color;
            Width = width;
            Height = height;
        }

        public Vector2 Position { get; }
        public Color Color { get; }
        public int Width { get; }
        public int Height { get; }
    }
}