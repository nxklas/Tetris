namespace Tetris.Core
{
    partial class GameObject
    {
        internal static bool[][,] BuildPieces(TetrominoInfo info) => info switch
        {
            TetrominoInfo.I => BuildI(),
            TetrominoInfo.O => BuildO(),
            TetrominoInfo.T => BuildT(),
            TetrominoInfo.S => BuildS(),
            TetrominoInfo.Z => BuildZ(),
            TetrominoInfo.J => BuildJ(),
            TetrominoInfo.L => BuildL(),
            _ => throw new System.Exception($"Unexpected pattern: {info}; thus, unable to build pattern.")
        };

        private static bool[][,] BuildI() => new bool[][,]
        {
            new bool[,]
            {
                { false, true, false, false },
                { false, true, false, false },
                { false, true, false, false },
                { false, true, false, false }
            },
            new bool[,]
            {
                { false, false, false, false },
                { false, false, false, false },
                { true, true, true, true },
                { false, false, false, false }
            },
            new bool[,]
            {
                { false, false, true, false },
                { false, false, true, false },
                { false, false, true, false },
                { false, false, true, false }
            },
            new bool[,]
            {
                { false, false, false, false },
                { true, true, true, true },
                { false, false, false, false },
                { false, false, false, false }
            }
        };

        private static bool[][,] BuildO() => new bool[][,]
        {
            new bool[,]
            {
                { true, true },
                { true, true }
            },
            new bool[,]
            {
                { true, true },
                { true, true }
            },
            new bool[,]
            {
                { true, true },
                { true, true }
            },
            new bool[,]
            {
                { true, true },
                { true, true }
            }
        };

        private static bool[][,] BuildT() => new bool[][,]
        {
            new bool[,]
            {
                { false, true, false },
                { true, true, false },
                { false, true, false }
            },
            new bool[,]
            {
                { false, false, false },
                { true, true, true },
                { false, true, false }
            },
            new bool[,]
            {
                { false, true, false },
                { false, true, true },
                { false, true, false }
            },
            new bool[,]
            {
                { false, true, false },
                { true, true, true },
                { false, false, false }
            }
        };

        private static bool[][,] BuildS() => new bool[][,]
        {
            new bool[,]
            {
                { false, true, false },
                { true, true, false },
                { true, false, false }
            },
            new bool[,]
            {
                { false, false, false },
                { true, true, false },
                { false, true, true }
            },
            new bool[,]
            {
                { false, false, true },
                { false, true, true },
                { false, true, false }
            },
            new bool[,]
            {
                { true, true, false },
                { false, true, true },
                { false, false, false }
            }
        };

        private static bool[][,] BuildZ() => new bool[][,]
        {
            new bool[,]
            {
                { true, false, false },
                { true, true, false },
                { false, true, false }
            },
            new bool[,]
            {
                { false, false, false },
                { false, true, true },
                { true, true, false }
            },
            new bool[,]
            {
                { false, true, false },
                { false, true, true },
                { false, false, true }
            },
            new bool[,]
            {
                { false, true, true },
                { true, true, false },
                { false, false, false }
            }
        };

        private static bool[][,] BuildJ() => new bool[][,]
        {
            new bool[,]
            {
                { true, true, false },
                { false, true, false },
                { false, true, false }
            },
            new bool[,]
            {
                { false, false, false },
                { true, true, true },
                { true, false, false }
            },
            new bool[,]
            {
                { false, true,false },
                { false, true, false },
                { false, true, true }
            },
            new bool[,]
            {
                { false, false, true },
                { true, true, true },
                { false, false, false }
            }
        };

        private static bool[][,] BuildL() => new bool[][,]
        {
            new bool[,]
            {
                { false, true,false },
                { false, true, false },
                { true, true, false }
            },
            new bool[,]
            {
                { false, false, false },
                { true, true, true },
                { false, false, true }
            },
            new bool[,]
            {
                { false, true, true },
                { false, true, false },
                { false, true, false }
            },
            new bool[,]
            {
                { true, false, false },
                { true, true, true },
                { false, false, false }
            }
        };
    }
}