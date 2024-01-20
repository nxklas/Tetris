using System.Drawing;
using System.Numerics;

namespace Tetris.Core.Rendering
{
    /// <summary>
    /// Represents a text that can be rendered on screen using the <see cref="Renderer"/> class.
    /// </summary>
    public sealed class Text : IRenderable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="str">The actual text that is rendered.</param>
        /// <param name="color">The color in which the text is rendered.</param>
        /// <param name="x">The x-coordinate where the text is rendered.</param>
        /// <param name="y">The y-coordinate where the text is rendered.</param>
        /// <param name="font">The font in which the text is rendered.</param>
        internal Text(string str, int x, int y, Color color, Font font) : this(str, new Vector2(x, y), color, font)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="str">The actual text that is rendered.</param>
        /// <param name="color">The color in which the text is rendered</param>
        /// <param name="position">The position where the text is rendered.</param>
        /// <param name="font">The font in which the text is rendered.</param>
        internal Text(string str, Vector2 position, Color color, Font font)
        {
            Str = str;
            Position = position;
            Color = color;
            Font = font;
        }

        /// <summary>
        /// Represents the actual text that is rendered.
        /// </summary>
        public string Str { get; }
        /// <summary>
        /// Represents the position where the text is rendered.
        /// </summary>
        public Vector2 Position { get; }
        /// <summary>
        /// Represents the color in which the text is rendered.
        /// </summary>
        public Color Color { get; }
        /// <summary>
        /// Represents the font in which the text is rendered.
        /// </summary>
        public Font Font { get; }
    }
}