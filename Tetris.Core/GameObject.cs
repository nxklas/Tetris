using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tetris.Core
{
    public sealed partial class GameObject : IRenderable, ICloneable
    {
        public const int SquareSize = 25;
        private static readonly Orientation[] Orientations = Enum.GetValues<Orientation>();
        private static readonly byte Orientations_MaxIndex;
        internal static readonly TetrominoFacts I = new(CreateI(0, 0));
        internal static readonly TetrominoFacts O = new(CreateO(0, 0));
        internal static readonly TetrominoFacts T = new(CreateT(0, 0));
        internal static readonly TetrominoFacts S = new(CreateS(0, 0));
        internal static readonly TetrominoFacts Z = new(CreateZ(0, 0));
        internal static readonly TetrominoFacts J = new(CreateJ(0, 0));
        internal static readonly TetrominoFacts L = new(CreateL(0, 0));
        private readonly bool[][,] _tetrominos;
        private byte _index;
        private Vector2 _position;
        private Color _color;
        private bool _isGhost;

        static GameObject()
        {
            var maxIndex = Orientations.Length - 1;
            AssertByte(maxIndex);
            Orientations_MaxIndex = unchecked((byte)maxIndex);
        }

        private GameObject(float x, float y, Color color, TetrominoInfo info) : this(new Vector2(x, y), color, info)
        {
        }

        private GameObject(Vector2 position, Color color, TetrominoInfo info)
        {
            _tetrominos = BuildPieces(info);
            _position = position;
            _color = color;
            Info = info;
            Index = 0;
            _isGhost = false;
        }

        public Vector2 Position => _position;
        public Color Color => _color;
        public TetrominoInfo Info { get; }
        public bool[,] Tetromino => _tetrominos[Index];
        public Orientation Orientation => Orientations[Index];
        public int Width => Tetromino.GetLength(0);
        public int Height => Tetromino.GetLength(1);
        public int TotalWidth => GetLength(Width);
        public int TotalHeight => GetLength(Height);
        private byte Index
        {
            get => _index;
            set
            {
                _index = value;
                DebugState();
            }
        }
        internal bool IsGhost => _isGhost;

        public Piece[] GetRow(int row)
        {
            if (row < 0)
                throw new ArgumentOutOfRangeException(nameof(row));

            if (row >= Height)
                throw new ArgumentOutOfRangeException(nameof(row));

            var result = new Piece[Width];

            for (var i = 0; i < result.Length; i++)
                result[i] = new Piece(Tetromino[i, row], i, row);

            return result;
        }

        public Piece[] GetColumn(int column)
        {
            if (column < 0)
                throw new ArgumentOutOfRangeException(nameof(column));

            if (column >= Width)
                throw new ArgumentOutOfRangeException(nameof(column));

            var result = new Piece[Height];

            for (var i = 0; i < result.Length; i++)
                result[i] = new Piece(Tetromino[column, i], column, i);

            return result;
        }

        public Piece[] GetTop() => GetRow(0);

        public Piece[] GetRight() => GetColumn(Width - 1);

        public Piece[] GetBottom() => GetRow(Height - 1);

        public Piece[] GetLeft() => GetColumn(0);

        public GameObject Clone() => (GameObject)(this as ICloneable).Clone();

        object ICloneable.Clone() => MemberwiseClone();

        internal void Gravity() => _position = new(_position.X, _position.Y + SquareSize);

        internal void MoveRight() => _position = new(_position.X + SquareSize, _position.Y);

        internal void MoveLeft() => _position = new(_position.X - SquareSize, _position.Y);

        internal void TurnRight()
        {
            if (Index < Orientations_MaxIndex)
                Index++;
            else
                Index = 0;

            Assert_IndexInsideBounds();
        }

        internal void TurnLeft()
        {
            if (Index > 0)
                Index--;
            else
                Index = Orientations_MaxIndex;

            Assert_IndexInsideBounds();
        }

        internal void Ghostify()
        {
            _color = Color.FromArgb(50, _color);
            _isGhost = true;
        }

        public static GameObject CreateI(float x, float y) => new(x,y, FromRgb(49, 199, 239), TetrominoInfo.I);

        public static GameObject CreateO(float x, float y) => new(x,y, FromRgb(247, 211, 8), TetrominoInfo.O);

        public static GameObject CreateT(float x, float y) => new(x, y, FromRgb(173, 77, 156), TetrominoInfo.T);

        public static GameObject CreateS(float x, float y) => new(x, y, FromRgb(66, 182, 66), TetrominoInfo.S);

        public static GameObject CreateZ(float x, float y) => new(x, y, FromRgb(239, 32, 41), TetrominoInfo.Z);

        public static GameObject CreateJ(float x, float y) => new(x, y, FromRgb(90, 101, 173), TetrominoInfo.J);

        public static GameObject CreateL(float x, float y) => new(x, y, FromRgb(239, 121, 33), TetrominoInfo.L);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLength(int length) => SquareSize * length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color FromRgb(byte r, byte g, byte b) => Color.FromArgb(255, r, g, b);
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

    internal readonly struct TetrominoFacts
    {
        private readonly GameObject _obj;

        public TetrominoFacts(GameObject obj)
        {
            _obj = obj;
        }

        public int Width => _obj.Width;
        public int Height => _obj.Height;
    }
}