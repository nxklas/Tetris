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
        private void DebugLine(object obj) => Debug.WriteLine(obj);

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DebugOrientation() => DebugLine("Current orientation: " + Orientation);

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DebugTop() => DebugLine($"{Info} Top: " + FlattenPieces(GetTop()));

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DebugBottom() => DebugLine($"{Info} Bottom: " + FlattenPieces(GetBottom()));

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DebugLeft() => DebugLine($"{Info} Left: " + FlattenPieces(GetLeft()));

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DebugRight() => DebugLine($"{Info} Right: " + FlattenPieces(GetRight()));

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DebugState()
        {
            DebugOrientation();
            DebugTop();
            DebugRight();
            DebugBottom();
            DebugLeft();
        }

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Assert_IndexInsideBounds() => Debug.Assert(_index >= 0 && _index <= Orientations_MaxIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AssertByte(int value) => Debug.Assert(value >= byte.MinValue && value <= byte.MaxValue);

        private static string FlattenPieces(IEnumerable<Piece> pieces)
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