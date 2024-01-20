using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Tetris.Core.Rendering
{
    /// <summary>
    /// Represents a background that can be rendered on screen using the <see cref="Renderer"/> class.
    /// </summary>
    public sealed class Background : IRenderable
    {
        public int TileSize = GameManager.TileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="Background"/> class.
        /// </summary>
        /// <param name="cellX">The count of cells in the width.</param>
        /// <param name="cellsY">The count of cells in the height.</param>
        internal Background(int cellX, int cellsY)
        {
            Grid = new Color[cellX, cellsY];
            Width = cellX;
            Height = cellsY;
            TotalWidth = cellX * TileSize;
            TotalHeight = cellsY * TileSize;
            Position = new();
            Color = Color.Gray;
            InitGrid();
        }

        public Color[,] Grid { get; }
        public int Width { get; }
        public int Height { get; }
        /// <summary>
        /// Represents the width of the background.
        /// </summary>
        public int TotalWidth { get; }
        /// <summary>
        /// Represents the height of the background.
        /// </summary>
        public int TotalHeight { get; }
        /// <summary>
        /// Represents position where the background is rendered. (0, 0)
        /// </summary>
        public Vector2 Position { get; }
        /// <summary>
        /// Represents the color in which the background is rendered.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// Adds a dropped tetromino to the grid.
        /// </summary>
        /// <param name="obj">The game object that stores the tetromino to add.</param>
        /// <param name="clearedRows">The number of rows that a cleared when full.</param>
        internal void RigidifyTetromino(GameObject obj, out int clearedRows, out bool gameOver)
        {
            var tetromino = obj.Tetromino;

            for (var y = 0; y < tetromino.Height; y++)
            {
                var gridY = (int)obj.Position.Y + y;

                for (var x = 0; x < tetromino.Width; x++)
                {
                    if (tetromino[x, y])
                    {
                        var gridX = (int)obj.Position.X + x;

                        try
                        {
                            Grid[gridX, gridY] = obj.Tetromino.Color;
                            Debug.WriteLine("Doesnt hurts");
                        }
                        catch
                        {
                            Debug.WriteLine("Hurts");
                        }
                    }
                }
            }

            clearedRows = ClearFullLine();
            gameOver = IsTopUnclear();
        }

        private bool IsTopUnclear()
        {
            if (Enumerable.Range(0, Width - 1).Any(column => IsntFree(column, 0)))
                return true;

            return false;
        }

        internal bool IsntFree(int x, int y)
        {
            try
            {
                return Grid[x, y] != default;
            }
            catch
            {
                Debug.WriteLine($"max_x: {Width}; actual_x: {x}");
                Debug.WriteLine($"max_y: {Height}; actual_y: {y}");
                return false;
            }
        }

        private void InitGrid()
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    Grid[x, y] = default;
        }

        /// <summary>
        /// Clears all lines that are full and moves each element on the grid down by the returned value.
        /// </summary>
        /// <returns>An <see cref="int"/> instance that represents the number of cleared rows.</returns>
        private int ClearFullLine()
        {
            var i = 0;

            for (var row = Height - 1; row > -1; row--)
                if (Enumerable.Range(0, Width - 1).All(column => IsntFree(column, row)))
                {
                    Clear(row);
                    Drop(row);
                    i += ClearFullLine() + 1;
                }

            return i;
        }

        private void Clear(int row)
        {
            for (var column = 0; column < Width; column++)
                Grid[column, row] = default;
        }

        private void Drop(int deleteRow)
        {
            for (var row = deleteRow - 1; row > -1; row--)
            {
                var rowPlusOne = row + 1;

                for (var column = 0; column < Width; column++)
                    if (rowPlusOne < Height)
                        Grid[column, rowPlusOne] = Grid[column, row];
            }
        }
    }
}