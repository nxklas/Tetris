using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Tetris.Core
{
    partial class GameObject
    {
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DebugLine(object value) => Debug.WriteLine(value);

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DebugOrientation() => DebugLine("Current orientation: " + Orientation);

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DebugTop() => DebugLine($"{Info} Top: " + FlattenPieces(GetTop()));

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DebugBottom() => DebugLine($"{Info} Bottom: " + FlattenPieces(GetBottom()));

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DebugLeft() => DebugLine($"{Info} Left: " + FlattenPieces(GetLeft()));

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DebugRight() => DebugLine($"{Info} Right: " + FlattenPieces(GetRight()));

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DebugState()
        {
            DebugOrientation();
            DebugTop();
            DebugRight();
            DebugBottom();
            DebugLeft();
        }

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Assert_IndexInsideBounds() => Debug.Assert(Index >= 0 && Index <= Orientations_MaxIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AssertByte(int value) => Debug.Assert(value >= byte.MinValue && value <= byte.MaxValue);

        internal static string FlattenPieces(IEnumerable<Piece> pieces)
        {
            var builder = new System.Text.StringBuilder();
            var count = pieces.Count();
            var last = pieces.ElementAt(count - 1);

            foreach (var piece in pieces)
                builder.Append(piece + (piece != last ? ", " : string.Empty));

            return builder.ToString();
        }
    }
}