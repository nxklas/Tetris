using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris.Core
{
    public sealed class GameManager
    {
        public const int SquareSize = GameObject.SquareSize;
        public const int CellsX = 10;
        public const int CellsY = 22;
        public const int GridWidth = CellsX * SquareSize;
        public const int GridHeight = CellsY * SquareSize;
        private static readonly TetrominoInfo[] Infos = Enum.GetValues<TetrominoInfo>();
        private static readonly Random Random = new();
        private readonly GameStateManager _gameStateManager;
        private readonly Background _background;
        private readonly List<GameObject> _rigids;
        private GameObject _current;
        private GameObject _ghost;
        private GameObject _lookahead;
        private bool _dropped;

        public GameManager()
        {
            _gameStateManager = new GameStateManager();
            _gameStateManager.SetIngame();
            _background = new(System.Drawing.Color.Gray, GridWidth, GridHeight);
            _rigids = new();
            _current = GetRandomObject();
            _ghost = CreateGhost();
            _lookahead = GetRandomObject();
            _dropped = false;
        }

        public GameObject Current => _current;
        public GameState State => _gameStateManager.State;
        public int Width => _background.Width;
        public int Height => _background.Height;
        public IEnumerable<IRenderable> Renderables => new IRenderable[] { _background, _current, _ghost }.Concat(_rigids);

        public void Drop()
        {
            Drop(_current);
            _dropped = true;
        }

        public void MoveLeft()
        {
            _current.MoveLeft();
            _ghost = CreateGhost();
        }

        public void MoveRight()
        {
            _current.MoveRight();
            _ghost = CreateGhost();
        }

        public void TurnLeft()
        {
            _current.TurnLeft();
            _ghost = CreateGhost();
        }

        public void TurnRight()
        {
            _current.TurnRight();
            _ghost = CreateGhost();
        }

        public void Update()
        {
            if (State != GameState.Ingame)
                return;

            if (_dropped)
            {
                AddRigid();
                _dropped = false;
            }
            else if (!DoesCollide(_current))
                _current.Gravity();
            else
                AddRigid();
        }

        private void AddRigid()
        {
            if(_current.Position.X==0)
            {
                _gameStateManager.SetGameOver();
                return;
            }

            _rigids.Add(_current);
            _current = _lookahead;
            _lookahead = GetRandomObject();
            _ghost = CreateGhost();
        }

        private bool DoesCollide(GameObject obj)
        {
            var h = obj.Height - 1;

            for (var i = h; i > -1; i--)
            {
                var acutualY = (i + 1) * SquareSize;
                var relativeY = acutualY + obj.Position.Y;
                var row = obj.GetRow(i);

                for (var j = 0; j < row.Length; j++)
                {
                    var actualX = j * SquareSize;
                    var relativeX = actualX + obj.Position.X;
                    var obstacleHeight = GetLowestHeightAt(relativeX);

                    if (relativeY >= obstacleHeight && relativeY < obstacleHeight + SquareSize)
                    {
                        var piece = row[j];

                        if (piece.State)
                            return true;
                    }
                }
            }

            return false;
        }

        private float GetLowestHeightAt(float x)
        {
            var objs = new List<float>();

            foreach (var obj in _rigids.Where(a => x >= a.Position.X || x < a.Position.X + a.TotalWidth))
            {
                for (var i = 0; i < obj.Height; i++)
                {
                    var row = obj.GetRow(i);
                    //var found = false;

                    for (var j = 0; j < row.Length; j++)
                    {
                        var piece = row[j];

                        if (!piece.State)
                            continue;

                        var actualX = j * SquareSize;
                        var relativeX = actualX + obj.Position.X;

                        if (x >= relativeX && x < relativeX + SquareSize)
                        {
                            objs.Add((i * SquareSize) + obj.Position.Y);
                            //found = true;
                            //break;
                        }
                    }

                    //if (found)
                    //    break;
                }
            }

            return objs.Count != 0 ? objs.Min() : Height;
        }

        private void Drop(GameObject obj)
        {
            while (!DoesCollide(obj))
                obj.Gravity();
        }

        private GameObject CreateGhost()
        {
            var obj = _current.Clone();
            obj.Ghostify();
            Drop(obj);
            return obj;
        }

        private static GameObject GetRandomObject()
        {
            var index = Random.Next(Infos.Length);
            var info = Infos[index];
            var x = GetRandomX(info);
            return info switch
            {
                TetrominoInfo.I => GameObject.CreateI(x, 0),
                TetrominoInfo.O => GameObject.CreateO(x, 0),
                TetrominoInfo.T => GameObject.CreateT(x, 0),
                TetrominoInfo.S => GameObject.CreateS(x, 0),
                TetrominoInfo.Z => GameObject.CreateZ(x, 0),
                TetrominoInfo.J => GameObject.CreateJ(x, 0),
                TetrominoInfo.L => GameObject.CreateL(x, 0),
                _ => throw new Exception($"Unexpected tetromino info: {info}.")
            };
        }

        private static int GetRandomX(TetrominoInfo info)
        {
            var width = info switch
            {
                TetrominoInfo.I => GameObject.I.Width,
                TetrominoInfo.O => GameObject.O.Width,
                TetrominoInfo.T => GameObject.T.Width,
                TetrominoInfo.S => GameObject.S.Width,
                TetrominoInfo.Z => GameObject.Z.Width,
                TetrominoInfo.J => GameObject.J.Width,
                TetrominoInfo.L => GameObject.L.Width,
                _ => throw new Exception($"Unexpected tetromino info: {info}.")
            };
            var column = Random.Next(CellsX - width + 1);
            var coordinate = column * SquareSize;
            return coordinate;
        }
    }
}