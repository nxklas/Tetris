using System.Drawing;
using System.Numerics;

namespace Tetris.Core.Rendering
{
    /// <summary>
    /// Provides the basic information of a renderable object that can be rendered on screen using the <see cref="Renderer"/> class.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Represents position where the object is rendered.
        /// </summary>
        Vector2 Position { get; }
        /// <summary>
        /// Represents the color in which the object is rendered.
        /// </summary>
        Color Color { get; }
    }
}
