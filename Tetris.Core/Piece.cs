using System;

namespace Tetris.Core
{
    public readonly struct Piece : IEquatable<Piece>, System.Numerics.IEqualityOperators<Piece, Piece, bool>
    {
        internal Piece(bool state, int x, int y)
        {
            State = state;
            X = x;
            Y = y;
        }

        public bool State { get; }
        public int X { get; }
        public int Y { get; }

        public bool Equals(Piece other) => this == other;

        public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is Piece other && Equals(other);
        
        public override int GetHashCode() => HashCode.Combine(State, X, Y);

        public override string ToString() => $"{State} <{X}, {Y}>";

        public static bool operator ==(Piece left, Piece right) => left.State == right.State && left.X == right.X && left.Y == right.Y;

        public static bool operator !=(Piece left, Piece right) => !(left == right);
    }
}