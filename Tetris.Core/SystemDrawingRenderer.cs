using System;
using System.Diagnostics;
using System.Drawing;

namespace Tetris.Core
{
    /// <summary>
    /// Represents a renderer that uses <see cref="Graphics"/> for rendering.
    /// </summary>
    /// <remarks>Call <see cref="InitializeGraphics(Graphics)"/> after creating a new instance before use!</remarks>
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public sealed class SystemDrawingRenderer : Renderer
    {
        private Graphics? _graphics;

        public SystemDrawingRenderer() : base()
        {
            _graphics = null;
        }

        public void InitializeGraphics(Graphics graphics) => _graphics = graphics;

        public override void Draw(GameState state)
        {
            if (_graphics == null)
                Throw.NullReferenceException_GraphicsUninitialized();

            switch (state)
            {
                case GameState.Menu:
                    DrawMenu();
                    break;
                case GameState.Pause:
                    DrawPause();
                    break;
                case GameState.Ingame:
                    DrawIngame();
                    break;
                case GameState.GameOver:
                    DrawGameOver();
                    break;
                default:
                    throw new Exception($"Unexpected game state: {state}.");
            }
        }

        private void DrawMenu()
        {
            throw new NotImplementedException();
        }

        private void DrawPause()
        {
            throw new NotImplementedException();
        }

        private void DrawIngame()
        {
            DrawBackground();
            DrawGameObjects();
        }

        private void DrawGameOver()
        {
            throw new NotImplementedException();
        }

        private void DrawBackground() => WalkQueue<Background>(DrawBackground);

        private void DrawBackground(Background background)
        {
            var size = GameObject.SquareSize;
            var width = background.Width;
            var height = background.Height;
            var cells = (width > height ? width : height) / size;
            var color = background.Color;
            var brush = new SolidBrush(color);

            Debug.Assert(_graphics != null);

            _graphics.FillRectangle(brush, 0, 0, width, height);

            for (var i = 0; i < cells; i++)
                _graphics.DrawLine(Pens.Black, i * size, 0, i * size, height);

            for (var i = 0; i < cells; i++)
                _graphics.DrawLine(Pens.Black, 0, i * size, width, i * size);
        }

        private void DrawGameObjects() => WalkQueue<GameObject>(DrawGameObject);

        private void DrawGameObject(GameObject obj)
        {
            var size = GameObject.SquareSize;
            var color = obj.Color;
            var brush = new SolidBrush(color);

            Debug.Assert(_graphics != null);

            for (var i = 0; i < obj.Width; i++)
            {
                var x = obj.Position.X + (i * size);

                for (var j = 0; j < obj.Height; j++)
                {
                    var y = obj.Position.Y + (j * size);

                    if (obj.Tetromino[i, j])
                        _graphics.FillRectangle(brush, x, y, size, size);
#if DEBUG
                    else
                        _graphics.FillRectangle(Brushes.Black, x, y, size, size);
#endif
                }
            }
        }

        private static class Throw
        {
            [System.Diagnostics.CodeAnalysis.DoesNotReturn]
            public static object NullReferenceException_GraphicsUninitialized() =>
                throw new NullReferenceException($"Graphics object was not initialized. Please call {nameof(InitializeGraphics)}() before use.");
        }
    }
}