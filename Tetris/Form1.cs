using System;
using System.Drawing;
using System.Windows.Forms;
using Tetris.Core;
using Tetris.Core.Rendering;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private readonly SystemDrawingRenderer _renderer;
        private readonly GameManager _game;
        private readonly FpsCounter _fpsCounter;
        private Bitmap _bitmap;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            _renderer = new();
            _game = new();
            _fpsCounter = new();
            _bitmap = new(250, 250);
            timer1.Start();
            _fpsCounter.Start();
            GameUpdated();
        }

        private void AssignBitmap() => _bitmap = new(250, 200);

        private void GameUpdated()
        {
            AssignBitmap();

            var size = GameManager.TileSize;
            var lookahead = _game.Lookahead;
            var width = lookahead.Tetromino.TotalHeight;
            var height = lookahead.Tetromino.TotalWidth;

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    if (lookahead.Tetromino[x / size, y / size])
                        _bitmap.SetPixel(x, y, lookahead.Color);

            pictureBox1.Image = _bitmap;
            timer1.Interval = _game.UpdateInterval;
            label1.Text = $"Score: {_game.Score}";
            
            if (_game.IsGameOver)
                timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _game.Update();
            Refresh();
            GameUpdated();
            _fpsCounter.Update();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            _renderer.InitializeGraphics(e.Graphics);
            _renderer.Draw(_game.CurrentScene);
            Text = "Fps: " + _fpsCounter.Fps;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    _game.TryTurnLeft();
                    break;
                case Keys.Right:
                    _game.TryTurnRight();
                    break;
                case Keys.A:
                    _game.TryMoveLeft();
                    break;
                case Keys.D:
                    _game.TryMoveRight();
                    break;
                case Keys.Space:
                    _game.TryDrop();
                    break;
            }

            Refresh();
        }
    }
}