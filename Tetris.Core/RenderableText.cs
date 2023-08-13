using System;
using System.Drawing;
using System.Numerics;

namespace Tetris.Core
{
    public class RenderableText : IRenderable
    {
        public RenderableText(string text, Color color, int x, int y)
        {
            Text = text;
            Color = color;
            Position = new Vector2(x, y);
        }

        public Color Color { get; }
        public int X { get; }
        public int Y { get; }

        public Vector2 Position { get; }

        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public string Text { get; }
    }
}
