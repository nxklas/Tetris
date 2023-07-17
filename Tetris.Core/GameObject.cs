using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tetris.Core
{
    public class GameObject : IRenderable
    {
        public const int SquareSize = 25;

        private GameObject(int x, int y, Color color, PatternInfo info, bool[,] pattern) : this(new Vector2(x, y), color, info, pattern)
        {
        }

        private GameObject(Vector2 position, Color color, PatternInfo info, bool[,] pattern)
        {
            Position = position;
            Color = color;
            Info = info;
            Pattern = pattern;
            TurnRight(); // To make the array declaration for the pattern more readable, values are declared from top to bottom,
                         // unlike Roslyn would read them out.
                         // Therefore, it is nessesary to turn the pattern to the right to make it be rendered in the correct direction.
        }

        public PatternInfo Info { get; }
        public bool[,] Pattern { get; private set; }
        public Color Color { get; }
        public Vector2 Position { get; private set; }
        public int Width => Pattern.GetLength(0);
        public int Height => Pattern.GetLength(1);
        public int TotalWidth => GetLength(Width);
        public int TotalHeight => GetLength(Height);

        public void Gravity()
        {
            Position = new(Position.X, Position.Y + SquareSize);
        }

        public void TurnRight()
        {
            var tilted = new bool[Height, Width];

            for (var x = Width - 1; x > -1; x--)
                for (var y = Height - 1; y > -1; y--)
                    tilted[Height - y - 1, x] = Pattern[x, y];

            Pattern = tilted;
        }

        public void TurnLeft()
        {
            var tilted = new bool[Height, Width];

            for (var x = Width - 1; x > -1; x--)
                for (var y = Height - 1; y > -1; y--)
                    tilted[y, Width - x - 1] = Pattern[x, y];

            Pattern = tilted;
        }

        public static GameObject CreateBlock(int x, int y) => new(x, y,
                                                                  Color.Yellow,
                                                                  PatternInfo.Block,
                                                                  new bool[,]
                                                                  {
                                                                      { true, true },
                                                                      { true, true }
                                                                  });

        public static GameObject CreateTriangle(int x, int y) => new(x, y,
                                                                     Color.Green,
                                                                     PatternInfo.Triangle,
                                                                     new bool[,]
                                                                     {
                                                                         { false, true, false },
                                                                         { true, true, true },
                                                                         { false, false, false }
                                                                     });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLength(int length) => SquareSize * length;
    }

    public enum PatternInfo
    {
        Block,
        Triangle,
    }
}