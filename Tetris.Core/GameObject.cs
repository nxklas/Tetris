using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tetris.Core
{
    public class GameObject : IRenderable
    {
        public const int SquareSize = 25;

        private GameObject(int x, int y, int width, int height, Color color, Pattern pattern) : this(new Vector2(x, y), width, height, color, pattern)
        {
        }

        private GameObject(Vector2 position, int width, int height, Color color, Pattern pattern)
        {
            Position = position;
            Width = width;
            Height = height;
            Color = color;
            Pattern = pattern;
            Orientation = Orientation.Up;
        }

        public Pattern Pattern { get; }
        public Orientation Orientation { get; set; }
        public Color Color { get; }
        public Vector2 Position { get; set; }
        public int Width { get; }
        public int Height { get; }

        public void Gravity()
        {
            Position = new(Position.X, Position.Y + SquareSize);
        }

        public void TurnRight()
        {
            switch (Orientation)
            {
                case Orientation.Up:
                    Orientation |= Orientation.Right;
                    break;
                case Orientation.Right:
                    Orientation = Orientation.Down;
                    break;
                case Orientation.Down:
                    Orientation = Orientation.Left;
                    break;
                case Orientation.Left:
                    Orientation &= ~Orientation.Left;
                    break;
            }
        }

        public void TurnLeft()
        {
            switch (Orientation)
            {
                case Orientation.Up:
                    Orientation |= Orientation.Left;
                    break;
                case Orientation.Right:
                    Orientation &= ~Orientation.Right;
                    break;
                case Orientation.Down:
                    Orientation = Orientation.Right;
                    break;
                case Orientation.Left:
                    Orientation = Orientation.Down;
                    break;
            }
        }

        public static GameObject CreateBlock(int x, int y) => new(x, y, GetLength(2), GetLength(2), Color.Yellow, Pattern.Block);

        public static GameObject CreateTriangle(int x, int y) => new(x,y, GetLength(3), GetLength(3), Color.Green, Pattern.Triangle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLength(int size) => SquareSize * size;
    }

    public enum Orientation
    {
        /// <summary>
        /// Indicates that a pattern is not tilted.
        /// </summary>
        Up = 0,
        /// <summary>
        /// Indicates that a pattern is tilted by 90 degrees.
        /// </summary>
        Right = 1,
        /// <summary>
        /// Indicates that a pattern is tilted by 180 degrees.
        /// </summary>
        Down = 2,
        /// <summary>
        /// Indicates that a pattern is tilted by 270 degrees.
        /// </summary>
        Left = 3
    }

    public enum Pattern
    {
        Block,
        Triangle,
    }
}