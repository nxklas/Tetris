using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tetris.Core
{
    public sealed class GameManager
    {
        public const int CellsX = 10;
        public const int CellsY = 22;
        public const int GridWidth = CellsX * GameObject.SquareSize;
        public const int GridHeight = CellsY * GameObject.SquareSize;
        private static readonly Tetromino[] Infos = Enum.GetValues<Tetromino>();
        private static readonly Random Random = new();
        private readonly GameStateManager _gameStateManager;
        private readonly Renderer _renderer;
        private readonly Background _background;
        private readonly List<GameObject> _rigids;
        private GameObject _current;
        private GameObject _lookahead;
        private bool _isRunning;
        private Stopwatch _watch;

        public GameManager(Renderer renderer)
        {
            _gameStateManager = new GameStateManager();
            _gameStateManager.SetIngame();
            _renderer = renderer;
            _background = new(System.Drawing.Color.Gray, GridWidth, GridHeight);
            _rigids = new();
            _current = GetRandomObject();
            _lookahead = GetRandomObject();
            _isRunning = false;
            _watch = new();
        }

        public GameObject Current => _current;
        public GameState State => _gameStateManager.State;
        public int Width => _background.Width;
        public int Height => _background.Height;

        public void Start()
        {
            if (_isRunning)
                return;

            _watch.Start();
            _isRunning = true;
        }

        public void Stop()
        {
            if (!_isRunning)
                return;

            _watch = new();
            _isRunning = false;
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }

        public void MoveLeft()
        {
            _current.MoveLeft();
        }

        public void MoveRight()
        {
            _current.MoveRight();
        }

        public void TurnLeft()
        {
            _current.TurnLeft();
        }

        public void TurnRight()
        {
            _current.TurnRight();
        }

        public void Update()
        {
            if (_isRunning && _watch.ElapsedMilliseconds / 500 == 1)
            {
                _watch = Stopwatch.StartNew();
                Update_Internal();
            }

            _renderer.AppendRenderable(_background);
            _renderer.Append(_current);

            _renderer.Append(_rigids);
        }

        private void Update_Internal()
        {
            if (!DoesCollide())
                _current.Gravity();
            else
                _lookahead = _current;
        }

        private bool DoesCollide()
        {
            foreach(var pieces in _current.GetBottom())
            {
                var bottom=pieces.Y + _current.Position.Y + _current.Height;
                if(bottom >=Height)
                    return true;
            }

            return false;
        }

        private static GameObject GetRandomObject()
        {
            var index = Random.Next(Infos.Length);
            var info = Infos[index];
            var x = GetRandomX();

            return info switch
            {
                Tetromino.I => GameObject.CreateI(x, 0),
                Tetromino.O => GameObject.CreateO(x, 0),
                Tetromino.T => GameObject.CreateT(x, 0),
                Tetromino.S => GameObject.CreateS(x, 0),
                Tetromino.Z => GameObject.CreateZ(x, 0),
                Tetromino.J => GameObject.CreateJ(x, 0),
                Tetromino.L => GameObject.CreateL(x, 0),
                Tetromino.Debug => GetRandomObject(),
                _ => throw new Exception($"Unexpected tetromino info: {info}.")
            };
        }

        private static int GetRandomX()
        {
            var column = Random.Next(CellsX + 1);
            var coordinate = column * GameObject.SquareSize;
            return coordinate;
        }
    }
}