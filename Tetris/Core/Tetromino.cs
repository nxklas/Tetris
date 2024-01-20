using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tetris.Core
{
    public readonly partial struct Tetromino : IEqualityOperators<Tetromino, Tetromino, bool>, IEquatable<Tetromino>
    {
        public const int PieceSize = GameManager.TileSize;
        private static readonly System.Collections.Generic.List<Kind> _kinds = [.. Enum.GetValues<Kind>()];
        private static readonly Random _rand = new();
        private readonly Orientater _orientater;
        private readonly Kind _kind;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tetromino"/> struct.
        /// </summary>
        private Tetromino(Kind kind)
        {
            _orientater = new(kind);
            _kind = kind;
            Color = kind switch
            {
                Kind.I => FromRgb(49, 199, 239),
                Kind.O => FromRgb(247, 211, 8),
                Kind.T => FromRgb(173, 77, 156),
                Kind.S => FromRgb(66, 182, 66),
                Kind.Z => FromRgb(239, 32, 41),
                Kind.J => FromRgb(90, 101, 173),
                Kind.L => FromRgb(239, 121, 33),
                _ => throw new Exception($"Unexpected tetromino kind: {kind}.")
            };
        }

        public bool this[int x, int y] => Pieces[x, y];
        public bool[,] Pieces => _orientater.Pieces;
        public string Name => _kind.ToString();
        public Color Color { get; }
        public int Width => Pieces.GetLength(0);
        public int Height => Pieces.GetLength(1);
        public int TotalWidth => GetLength(Width);
        public int TotalHeight => GetLength(Height);

        public static bool operator ==(Tetromino left, Tetromino right) => left.Equals(right);

        public static bool operator !=(Tetromino left, Tetromino right) => !(left == right);

        public bool Equals(Tetromino other) => _kind == other._kind;

        public override bool Equals(object? obj) => obj is Tetromino tetromino && Equals(tetromino);

        public override int GetHashCode() => HashCode.Combine(_kind, Pieces);

        internal void TurnRight() => _orientater.TurnRight();

        internal bool[,] GetPreviewRight() => _orientater.GetPreviewRight();

        internal void TurnLeft() => _orientater.TurnLeft();

        internal bool[,] GetPreviewLeft() => _orientater.GetPreviewLeft();

        public static Tetromino GetRandom(Tetromino notToBe) => GetRandom(notToBe, 0);

        public static Tetromino GetRandom() => new(_kinds[_rand.Next(_kinds.Count)]);

        public static Tetromino CreateI() => new(Kind.I);

        public static Tetromino CreateO() => new(Kind.O);

        public static Tetromino CreateT() => new(Kind.T);

        public static Tetromino CreateS() => new(Kind.S);

        public static Tetromino CreateZ() => new(Kind.Z);

        public static Tetromino CreateJ() => new(Kind.J);

        public static Tetromino CreateL() => new(Kind.L);

        private static Tetromino GetRandom(Tetromino notToBe, int depth)
        {
            const int Max_Depth = 10;
            var tetromino = GetRandom();

            if (tetromino != notToBe)
                return tetromino;

            if (++depth < Max_Depth)
                return GetRandom(notToBe, depth);

            var index = _kinds.FindIndex(kind => kind == notToBe._kind);

            if (index + 1 >= _kinds.Count)
                index--;
            else
                index++;

            return new(_kinds[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color FromRgb(byte r, byte g, byte b) => Color.FromArgb(255, r, g, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLength(int length) => length * PieceSize;

        private enum Kind
        {
            I,
            O,
            T,
            S,
            Z,
            J,
            L
        }

        private class Orientater
        {
            private readonly bool[][,] _pieces;
            private readonly byte _orientations;
            private byte _index;

            public Orientater(Kind kind)
            {
                _pieces = BuildPieces(kind);
                var orientations = _pieces.Length - 1;
                AssertByte(orientations);
                _orientations = unchecked((byte)orientations);
                _index = 0;
            }

            public bool[,] Pieces => _pieces[Index];
            public byte Index
            {
                get => _index;
                private set
                {
                    _index = value;
                    DebugOrientation();
                }
            }

            public void TurnRight()
            {
                if (Index < _orientations)
                    Index++;
                else
                    Index = 0;

                Assert_IndexInsideBounds();
            }

            public bool[,] GetPreviewRight()
            {
                TurnRight();
                var pieces = Pieces;
                TurnLeft();
                return pieces;
            }

            public void TurnLeft()
            {
                if (Index > 0)
                    Index--;
                else
                    Index = _orientations;

                Assert_IndexInsideBounds();
            }

            public bool[,] GetPreviewLeft()
            {
                TurnLeft();
                var pieces = Pieces;
                TurnRight();
                return pieces;
            }

            [Conditional("DEBUG")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void DebugOrientation() => Debug.WriteLine("Current orientation: " + Index);

            [Conditional("DEBUG")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void Assert_IndexInsideBounds() => Debug.Assert(Index >= 0 && Index <= _orientations);

            [Conditional("DEBUG")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void AssertByte(int value) => Debug.Assert(value >= byte.MinValue && value <= byte.MaxValue);
        }
    }
}