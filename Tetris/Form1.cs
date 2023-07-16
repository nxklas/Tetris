using Tetris.Core;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private readonly SystemDrawingRenderer _renderer;
        private readonly GameManager _gameManager;
        private GameObject[] _gameObject;

        public Form1()
        {
            InitializeComponent();
            _renderer = new SystemDrawingRenderer();
            _gameManager = new GameManager();
            _gameObject = new GameObject[] { GameObject.CreateTriangle(50, 50), GameObject.CreateBlock(200, 100) };
            _idleTimer.Start();
        }

        private void _idleTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            _renderer.InitializeGraphics(e.Graphics);
            _renderer.Append(_gameObject);
            _renderer.Draw(_gameManager.State);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    _gameObject[0].TurnRight();
                    break;
                case Keys.A:
                    _gameObject[0].TurnLeft();
                    break;
            }
        }
    }
}