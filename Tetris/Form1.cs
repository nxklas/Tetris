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
            _gameManager = new GameManager(Width, Height, _renderer);
            _gameManager.Redraw += (sender, args) => Refresh();
            _gameManager.Start();
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
                case Keys.D:
                    _gameManager.Current.TurnRight();
                    break;
                case Keys.A:
                    _gameManager.Current.TurnLeft();
                    break;
            }
        }
    }
}