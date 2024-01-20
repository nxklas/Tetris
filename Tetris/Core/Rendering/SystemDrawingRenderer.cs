using System.Diagnostics;
using System.Drawing;

namespace Tetris.Core.Rendering
{
    /// <summary>
    /// Represents a renderer that uses <see cref="Graphics"/> for rendering.
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public sealed class SystemDrawingRenderer : Renderer
    {
        private Graphics? _graphics;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDrawingRenderer"/> class.
        /// </summary>
        public SystemDrawingRenderer() : base()
        {
        }

        public void InitializeGraphics(Graphics graphics) => _graphics = graphics;

        protected override void DrawObject(GameObject obj)
        {
            const int size = GameObject.PieceSize;
            var tetromino = obj.Tetromino;
            var position = obj.Position;
            var pieces = tetromino.Pieces;
            var color = obj.Color;
            var brush = new SolidBrush(color);

            Debug.Assert(_graphics != null);

            for (var i = 0; i < tetromino.Height; i++)
            {
                var y = (position.Y + i) * size;

                for (var j = 0; j < tetromino.Width; j++)
                {
                    var x = (position.X + j) * size;

                    if (pieces[j, i])
                        _graphics.FillRectangle(brush, x, y, size, size);
#if DEBUG
                    else
                        _graphics.FillRectangle(new SolidBrush(Color.FromArgb(25, Color.Black)), x, y, size, size);
#endif
                }
            }
        }

        protected override void DrawBackground(Background background)
        {
            var size = GameManager.TileSize;
            var width = background.TotalWidth;
            var height = background.TotalHeight;
            var xCells = width / size;
            var yCells = height / size;
            var color = background.Color;
            var brush = new SolidBrush(color);

            Debug.Assert(_graphics != null);

            _graphics.FillRectangle(brush, 0, 0, width, height);

            for (var i = 0; i < xCells; i++)
                _graphics.DrawLine(Pens.Black, i * size, 0, i * size, height);

            for (var i = 0; i < yCells; i++)
                _graphics.DrawLine(Pens.Black, 0, i * size, width, i * size);

            for (var i = 0; i < background.Height; i++)
            {
                var y = i * size;

                for (var j = 0; j < background.Width; j++)
                {
                    var x = j * size;

                    color = background.Grid[j, i];

                    if (color != default)
                        _graphics.FillRectangle(new SolidBrush(color), x, y, size, size);
                }
            }
        }

        protected override void DrawText(Text text)
        {
            var brush = new SolidBrush(text.Color);
            Debug.Assert(_graphics != null);
            _graphics.DrawString(text.Str, text.Font, brush, text.Position.X, text.Position.Y);
        }
    }
}