using Tetris.Core;

namespace Tetris
{
    public sealed partial class Form1 : Form
    {
        private readonly SystemDrawingRenderer _renderer;
        private readonly GameManager _gameManager;

        public Form1()
        {
            InitializeComponent();
            _renderer = new SystemDrawingRenderer();
            _gameManager = new GameManager(_renderer);
            _gameManager.Start();
            _timer1.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            _renderer.InitializeGraphics(e.Graphics);
            _renderer.Draw(_gameManager.State);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    _gameManager.Drop();
                    break;
                case Keys.A:
                    _gameManager.MoveLeft();
                    break;
                case Keys.D:
                    _gameManager.MoveRight();
                    break;
                case Keys.Left:
                    _gameManager.TurnLeft();
                    break;
                case Keys.Right:
                    _gameManager.TurnRight();
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _gameManager.Update();
            Refresh();
        }
    }
}