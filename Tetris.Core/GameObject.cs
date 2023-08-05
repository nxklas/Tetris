using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tetris.Core
{
    public sealed partial class GameObject : IRenderable
    {
        public const int SquareSize = 25;
        private readonly bool[][,] _tetrominos;
        private byte _index;

        private GameObject(int x, int y, Color color, TetrominoInfo info) : this(new Vector2(x, y), color, info)
        {
        }

        private GameObject(Vector2 position, Color color, TetrominoInfo info)
        {
            Position = position;
            Color = color;
            Info = info;
            _index = 0;
            _tetrominos = BuildPieces(info);
            WriteOrientation();
        }

        public Vector2 Position { get; }
        public Color Color { get; }
        public TetrominoInfo Info { get; }
        public bool[,] Tetromino => _tetrominos[_index];
        public Orientation Orientation => Enum.GetValues<Orientation>()[_index];
        public int Width => Tetromino.GetLength(0);
        public int Height => Tetromino.GetLength(1);
        public int TotalWidth => GetLength(Width);
        public int TotalHeight => GetLength(Height);

        public void TurnRight()
        {
            if (_index < 3)
                _index++;
            else
                _index = 0;

            Assert_IndexInsideBounds();
            WriteOrientation();
        }

        public void TurnLeft()
        {
            if (_index > 0)
                _index--;
            else
                _index = 3;

            Assert_IndexInsideBounds();
            WriteOrientation();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteOrientation() => Debug.WriteLine("Current orientation: " + Orientation);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Assert_IndexInsideBounds() => Debug.Assert(_index >= 0 && _index <= 3);

        public static GameObject CreateI(int x, int y) => new(x, y, FromRgb(49, 199, 239), TetrominoInfo.I);

        public static GameObject CreateO(int x, int y) => new(x, y, FromRgb(247, 211, 8), TetrominoInfo.O);

        public static GameObject CreateT(int x, int y) => new(x, y, FromRgb(173, 77, 156), TetrominoInfo.T);

        public static GameObject CreateS(int x, int y) => new(x, y, FromRgb(66, 182, 66), TetrominoInfo.S);

        public static GameObject CreateZ(int x, int y) => new(x, y, FromRgb(239, 32, 41), TetrominoInfo.Z);

        public static GameObject CreateJ(int x, int y) => new(x, y, FromRgb(90, 101, 173), TetrominoInfo.J);

        public static GameObject CreateL(int x, int y) => new(x, y, FromRgb(239, 121, 33), TetrominoInfo.L);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLength(int length) => SquareSize * length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color FromRgb(byte r, byte g, byte b) => Color.FromArgb(r, g, b);
    }

    public enum TetrominoInfo
    {
        I,
        O,
        T,
        S,
        Z,
        J,
        L
    }

    public enum Orientation
    {
        Up,
        Right,
        Down,
        Left
    }
}