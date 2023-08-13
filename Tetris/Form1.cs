using Tetris.Core;

namespace Tetris
{
    public sealed partial class Form1 : Form
    {
        private readonly GameManager _gameManager;
        private readonly SystemDrawingRenderer _renderer;

        public Form1()
        {
            InitializeComponent();
            _gameManager = new GameManager();
            _renderer = new SystemDrawingRenderer();
            _gameTimer.Start();
            _renderTimer.Start();
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

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            _renderer.AppendRenderables(_gameManager.Renderables);
            if(_gameManager.State == GameState.GameOver)
            {
                var text = new RenderableText("Game Over", Color.Red, 150,150);
                _renderer.AppendRenderables(text);
            }
            Refresh();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            _gameManager.Update();
        }
    }
}