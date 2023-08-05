using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Timers;

namespace Tetris.Core
{
    public sealed class GameManager
    {
        private readonly GameStateManager _gameStateManager;
        private readonly Background _background;
        private readonly Renderer _renderer;
        private readonly Timer _timer;
        private bool _started;
        private readonly GameObject[] _gameObjects;

        public GameManager(int width, int height, Renderer renderer)
        {
            _gameStateManager = new GameStateManager();
            _gameStateManager.SetIngame();
            _background = new(System.Drawing.Color.Gray, width, height);
            _gameObjects = new GameObject[] { GameObject.CreateJ(150, 150) };
            _renderer = renderer;
            _timer = new Timer(250);
            _timer.Elapsed += (sender, args) => Update();
            _started = false;
        }

        public GameObject Current => _gameObjects[0];
        public GameState State => _gameStateManager.State;
        public int Width => _background.Width;
        public int Height => _background.Height;

        public void Start()
        {
            if (_started)
                return;
            _timer.Start();
            _started = true;
        }

        public void Stop()
        {
            if (!_started)
                return;
            _timer.Stop();
            _started = false;
        }

        private void Update()
        {
            _renderer.AppendRenderable(_background);
            _renderer.AppendRenderable(_gameObjects);
            Redraw?.Invoke(this, new EventArgs());
        }

        private bool DoesCollide()
        {


            return false;
        }

        public event EventHandler? Redraw;
    }

    internal sealed class Rigid : IEnumerable<GameObject>
    {
        private readonly List<GameObject> _objects;

        public Rigid()
        {
            _objects = new List<GameObject>();
        }

        public int GetHighestObjAt(int column)
        {
            var a = _objects.Where(a => a.Width >= column && a.Width + GameObject.SquareSize < column).Max(a => a.Height);
            return a;
        }

        public IEnumerator<GameObject> GetEnumerator() => _objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}