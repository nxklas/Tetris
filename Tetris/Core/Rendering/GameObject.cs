using System.Drawing;
using System.Numerics;

namespace Tetris.Core.Rendering
{
    /// <summary>
    /// Represents a game object that can be rendered on screen using the <see cref="Renderer"/> class.
    /// </summary>
    public sealed class GameObject : IRenderable
    {
        public const int PieceSize = Tetromino.PieceSize;
        private Vector2 _position;

        internal GameObject(GameObject origin) : this(origin.Tetromino, origin.Position, origin.IsGhost)
        {
        }

        internal GameObject(Tetromino tetromino, Vector2 position, bool isGhost)
        {
            Tetromino = tetromino;
            _position = position;
            IsGhost = isGhost;
            Color = IsGhost ? Color.FromArgb(50, Tetromino.Color) : Tetromino.Color;
        }

        public Tetromino Tetromino { get; internal set; }
        public Vector2 Position => _position;
        public bool IsGhost { get; }
        public Color Color { get; }

        internal void MoveDown() => _position = new(_position.X, ++_position.Y);

        internal void MoveRight() => _position = new(++_position.X, _position.Y);

        internal void MoveLeft() => _position = new(--_position.X, _position.Y);

        internal void TurnRight() => Tetromino.TurnRight();

        internal void TurnLeft() => Tetromino.TurnLeft();
    }
}