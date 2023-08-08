using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tetris.Core
{
    public sealed partial class GameObject : IRenderable
    {
        public const int SquareSize = 25;
        private static readonly Orientation[] Orientations = Enum.GetValues<Orientation>();
        private static readonly byte Orientations_MaxIndex;
        private readonly bool[][,] _tetrominos;
        private byte _index;
        private Vector2 _position;

        private GameObject(int x, int y, Color color, Tetromino info) : this(new Vector2(x, y), color, info)
        {
        }

        private GameObject(Vector2 position, Color color, Tetromino info)
        {
            _position = position;
            Color = color;
            Info = info;
            _index = 0;
            _tetrominos = BuildPieces(info);
            DebugState();
        }

        static GameObject()
        {
            var maxIndex = Orientations.Length - 1;
            AssertByte(maxIndex);
            Orientations_MaxIndex = unchecked((byte)maxIndex);
        }

        public Vector2 Position => _position;
        public Color Color { get; }
        public Tetromino Info { get; }
        public bool[,] Tetromino => _tetrominos[_index];
        public Orientation Orientation => Orientations[_index];
        public int Width => Tetromino.GetLength(0);
        public int Height => Tetromino.GetLength(1);
        public int TotalWidth => GetLength(Width);
        public int TotalHeight => GetLength(Height);

        internal void Gravity()
        {
            _position = new(_position.X, _position.Y + SquareSize);
        }

        internal void MoveRight()
        {
            _position = new(_position.X + SquareSize, _position.Y);
        }

        internal void MoveLeft()
        {
            _position = new(_position.X - SquareSize, _position.Y);
        }

        internal void TurnRight()
        {
            if (_index < Orientations_MaxIndex)
                _index++;
            else
                _index = 0;

            Assert_IndexInsideBounds();
            DebugState();
        }

        internal void TurnLeft()
        {
            if (_index > 0)
                _index--;
            else
                _index = Orientations_MaxIndex;

            Assert_IndexInsideBounds();
            DebugState();
        }

        public IEnumerable<Piece> GetRow(int row)
        {
            if (row < 0)
                throw new ArgumentOutOfRangeException(nameof(row));

            if (row > Width)
                throw new ArgumentOutOfRangeException(nameof(row));

            for (var i = 0; i < Width; i++)
                yield return new Piece(Tetromino[i, row], i, row);
        }

        public IEnumerable<Piece> GetColumn(int column)
        {
            if (column < 0)
                throw new ArgumentOutOfRangeException(nameof(column));

            if (column > Height)
                throw new ArgumentOutOfRangeException(nameof(column));

            for (var i = 0; i < Height; i++)
                yield return new Piece(Tetromino[column, i], column, i);
        }

        public IEnumerable<Piece> GetTop() => GetRow(0);

        public IEnumerable<Piece> GetRight() => GetColumn(Width - 1);

        public IEnumerable<Piece> GetBottom() => GetRow(Height - 1);

        public IEnumerable<Piece> GetLeft() => GetColumn(0);

        internal static GameObject CreateDebug(int x, int y) => new(x, y, Color.RebeccaPurple, Core.Tetromino.Debug);

        public static GameObject CreateI(int x, int y) => new(x, y, FromRgb(49, 199, 239), Core.Tetromino.I);

        public static GameObject CreateO(int x, int y) => new(x, y, FromRgb(247, 211, 8), Core.Tetromino.O);

        public static GameObject CreateT(int x, int y) => new(x, y, FromRgb(173, 77, 156), Core.Tetromino.T);

        public static GameObject CreateS(int x, int y) => new(x, y, FromRgb(66, 182, 66), Core.Tetromino.S);

        public static GameObject CreateZ(int x, int y) => new(x, y, FromRgb(239, 32, 41), Core.Tetromino.Z);

        public static GameObject CreateJ(int x, int y) => new(x, y, FromRgb(90, 101, 173), Core.Tetromino.J);

        public static GameObject CreateL(int x, int y) => new(x, y, FromRgb(239, 121, 33), Core.Tetromino.L);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLength(int length) => SquareSize * length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color FromRgb(byte r, byte g, byte b) => Color.FromArgb(r, g, b);
    }

    public readonly struct TetrominoInfo
    {
        public static readonly TetrominoInfo I = new(Tetromino.I, 4, 4);
        public static readonly TetrominoInfo O = new(Tetromino.O, 3, 3);

        private TetrominoInfo(Tetromino tetromino, int width, int height)
        {
            Tetromino = tetromino;
            Width = width;
            Height = height;
        }

        public Tetromino Tetromino { get; }
        public int Width { get; }
        public int Height { get; }
    }

    public enum Tetromino
    {
        I,
        O,
        T,
        S,
        Z,
        J,
        L,
        Debug
    }

    public enum Orientation
    {
        Up,
        Right,
        Down,
        Left
    }
}