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
            var pattern = obj.Pattern;
            var orientation = obj.Orientation;
            var color = obj.Color;
            var brush = new SolidBrush(color);

            switch (pattern)
            {
                case Pattern.Block:
                    _graphics!.FillRectangle(brush, obj.Position.X, obj.Position.Y, obj.Width, obj.Height);
                    break;
                case Pattern.Triangle:
                    switch (orientation)
                    {
                        case Orientation.Up:
                            _graphics!.FillRectangle(brush, obj.Position.X + GameObject.SquareSize, obj.Position.Y, GameObject.SquareSize, GameObject.SquareSize);
                            _graphics!.FillRectangle(brush, obj.Position.X, obj.Position.Y + GameObject.SquareSize, obj.Width, GameObject.SquareSize);
                            break;
                        case Orientation.Right:
                            _graphics!.FillRectangle(brush, obj.Position.X + obj.Width - GameObject.SquareSize, obj.Position.Y + GameObject.SquareSize, GameObject.SquareSize, GameObject.SquareSize);
                            _graphics!.FillRectangle(brush, obj.Position.X + GameObject.SquareSize, obj.Position.Y, GameObject.SquareSize, obj.Height);
                            break;
                        case Orientation.Down:

                            break;
                        case Orientation.Left:

                            break;
                        default:
                            throw new Exception($"Could not render pattern. Orientation \"{orientation}\" does not exist.");
                    }
                    break;
                default:
                    throw new Exception($"Could not render pattern \"{pattern}\". It does not exist.");
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