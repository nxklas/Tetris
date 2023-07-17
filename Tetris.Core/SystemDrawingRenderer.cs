using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.CompilerServices;

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
            WalkQueue<GameObject>(DrawPatterns);
        }

        private void DrawGameOver()
        {
            throw new NotImplementedException();
        }

        private void DrawPatterns(GameObject obj)
        {
            var color = obj.Color;
            var brush = new SolidBrush(color);

            for (var i = 0; i < obj.Width; i++)
                for (var j = 0; j < obj.Height; j++)
                    if (obj.Pattern[i, j])
                    {
                        var x = obj.Position.X + (i * GameObject.SquareSize);
                        var y = obj.Position.Y + (j * GameObject.SquareSize);

                        _graphics!.FillRectangle(brush, x, y, GameObject.SquareSize, GameObject.SquareSize);
                    }
        }

        private static class Throw
        {
            [DoesNotReturn]
            public static object NullReferenceException_GraphicsUninitialized() =>
                throw new NullReferenceException($"Graphics object was not initializes. Please call {nameof(InitializeGraphics)}() before use.");
        }
    }
}