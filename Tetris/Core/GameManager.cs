using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using Tetris.Core.GameStates;
using Tetris.Core.Rendering;

namespace Tetris.Core
{
    /// <summary>
    /// Represents the game manager, the main controlling unit for <see cref="Tetris"/>, that enables controlling the current tetromino, getting access to score, and much more.
    /// </summary>
    public sealed class GameManager
    {
        /// <summary>
        /// Represents the size of each tetromino and tetromino cell.
        /// </summary>
        public const int TileSize = 25;
        /// <summary>
        /// Represents the count of cells on the x-axis.
        /// </summary>
        public const int CellsX = 10;
        /// <summary>
        /// Represents the count of cells on the y-axis.
        /// </summary>
        public const int CellsY = 22;
        /// <summary>
        /// Represents the width, in pixels, of the game.
        /// </summary>
        public const int GridWidth = CellsX * TileSize;
        /// <summary>
        /// Represents the height, in pixels, of the game.
        /// </summary>
        public const int GridHeight = CellsY * TileSize;
        private readonly Background _background;
        private readonly GameStateManager _gameStateManager;
        private readonly Random _rand;
        private readonly bool _isCurrentInitialized;
        private GameObject _current;
        private GameObject _ghost;
        private GameObject _lookahead;
        private Text _gameOverText;
        private int _score;
        private int _clearedLineMultiplicator;
        private int _level;
        private int _updateInterval;

        public GameManager()
        {
            _background = new(CellsX, CellsY);
            _gameStateManager = new();
            _rand = new();
            _isCurrentInitialized = false;
            _current = NewTetromino();
            _isCurrentInitialized = true;
            _ghost = NewGhost();
            _lookahead = NewTetromino();
            _gameOverText = new("Game Over", new(25, 25), Color.AliceBlue, new(FontFamily.GenericMonospace, 12f, FontStyle.Italic));
            _score = 0;
            _clearedLineMultiplicator = 2;
            _level = 1;
            _updateInterval = 500;
        }

        public GameState State
        {
            get => _gameStateManager.State;
            set => _gameStateManager.State = value;
        }
        public bool IsIngame => State == GameState.InGame;
        public bool IsGameOver => State == GameState.GameOver;

        public int Width => _background.Width;
        public int Height => _background.Height;
        public GameObject Lookahead
        {
            private set => _lookahead = value;
            get => _lookahead;
        }
        public int Score => _score;
        public int ClearedLineMultiplicator => _clearedLineMultiplicator;
        public int Level => _level;
        public int UpdateInterval => _updateInterval;
        public Scene CurrentScene => State switch
        {
            GameState.InGame => GameScene,
            GameState.GameOver => GameOverScene,
            _ => throw new Exception($"Unexpected game state while scene creation: {State}.")
        };
        public Scene GameScene => new(GameState.InGame, _background, _current, _ghost);
        public Scene GameOverScene => new(GameState.GameOver, _background, _gameOverText);

        public void Update()
        {
            if (!DoesCollide(_current))
                _current.MoveDown();
            else
            {
                _background.RigidifyTetromino(_current, out var clearedRows, out var gameOver);
                _score += (clearedRows * ClearedLineMultiplicator) + 1;
                State = gameOver ? GameState.GameOver : GameState.InGame;

                if (IsIngame)
                {
                    SwitchObj();
                    AssignGhost();

                    if (clearedRows >= 4)
                    {
                        // We level up here
                        var val = _updateInterval - 50;

                        if (val >= 1)
                            _updateInterval = val;

                        _level++;
                        _clearedLineMultiplicator++;

                        Debug.WriteLine($"Score increased by: {(clearedRows * ClearedLineMultiplicator) + 1}; cleared rows: {clearedRows}");
                    }
                }
            }
        }

        /// <summary>
        /// Tries to turn the current tetromino in counterclockwise manner.
        /// </summary>
        /// <remarks>
        /// It fails on the following circumstances:
        /// <br><b>∗</b> The game is over.</br>
        /// <br><b>∗</b> Turning the tetromino to the left would cause it to break through the wall or in rigid tetrominos.</br>
        /// </remarks>
        /// <returns><see langword="true"/> if the tetromino was successfully turned; otherwise, <see langword="false"/>.</returns>
        public bool TryTurnLeft()
        {
            if (!IsIngame)
                return false;

            if (DoesCollide(_current.Tetromino.GetPreviewLeft(), _current.Position))
                return false;

            _current.TurnLeft();
            AssignGhost();
            return true;
        }

        /// <summary>
        /// Tries to turn the current tetromino in clockwise manner.
        /// </summary>
        /// <remarks>
        /// It fails on the following circumstances:
        /// <br><b>∗</b> The game is over.</br>
        /// <br><b>∗</b> Turning the tetromino to the right would cause it to break through the wall or in rigid tetrominos.</br>
        /// </remarks>
        /// <returns><see langword="true"/> if the tetromino was successfully turned; otherwise, <see langword="false"/>.</returns>
        public bool TryTurnRight()
        {
            if (!IsIngame)
                return false;

            if (DoesCollide(_current.Tetromino.GetPreviewRight(), _current.Position))
                return false;

            _current.TurnRight();
            AssignGhost();
            return true;
        }

        /// <summary>
        /// Tries to move the current tetromino to the left.
        /// </summary>
        /// <remarks>
        /// It fails on the following circumstances:
        /// <br><b>∗</b> The game is over.</br>
        /// <br><b>∗</b> Moving the tetromino to the left would cause it to break through the wall or in rigid tetrominos.</br>
        /// </remarks>
        /// <returns><see langword="true"/> if the tetromino was successfully moved; otherwise, <see langword="false"/>.</returns>
        public bool TryMoveLeft()
        {
            if (!IsIngame)
                return false;

            var buffer = new GameObject(_current);
            _current.MoveLeft();

            if (DoesCollide(_current))
            {
                _current = buffer;
                return false;
            }

            AssignGhost();
            return true;
        }

        /// <summary>
        /// Tries to move the current tetromino to the right.
        /// </summary>
        /// <remarks>
        /// It fails on the following circumstances:
        /// <br><b>∗</b> The game is over.</br>
        /// <br><b>∗</b> Moving the tetromino to the right would cause it to break through the wall or in rigid tetrominos.</br>
        /// </remarks>
        /// <returns><see langword="true"/> if the tetromino was successfully moved; otherwise, <see langword="false"/>.</returns>
        public bool TryMoveRight()
        {
            if (!IsIngame)
                return false;

            var buffer = new GameObject(_current);
            _current.MoveRight();

            if (DoesCollide(_current))
            {
                _current = buffer;
                return false;
            }

            AssignGhost();
            return true;
        }

        /// <summary>
        /// Tries to drop the current tetromino to the ground.
        /// </summary>
        /// <remarks>
        /// It fails on the following circumstances:
        /// <br><b>∗</b> The game is over.</br>
        /// </remarks>
        /// <returns><see langword="true"/> if the tetromino was successfully dropped; otherwise, <see langword="false"/>.</returns>
        public bool TryDrop() => TryDrop(ref _current);

        /// <summary>
        /// Determines whether the specified game object collides with the wall or ridig tetrominos.
        /// </summary>
        /// <param name="obj">The game object to check.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> does collide; otherwise, <see langword="false"/>.</returns>
        private bool DoesCollide(GameObject obj) => DoesCollide(obj.Tetromino.Pieces, obj.Position);

        /// <summary>
        /// Determines whether the specified <see cref="bool"/>[], by considering the specified position, collides with the wall or ridig tetrominos.
        /// </summary>
        /// <param name="pieces">A <see cref="bool"/>[] that represents a totromino.</param>
        /// <param name="position">The position where <paramref name="pieces"/> is located on the playground.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> does collide; otherwise, <see langword="false"/>.</returns>
        private bool DoesCollide(bool[,] pieces, Vector2 position)
        {
            for (var y = pieces.GetLength(1) - 1; y > -1; y--)
            {
                var gridY = (int)position.Y + y + 1;

                for (var x = 0; x < pieces.GetLength(0); x++)
                    if (pieces[x, y])
                    {
                        var gridX = (int)position.X + x;

                        if (gridX < 0 || gridX >= Width
                            || gridY < 0 || gridY >= Height
                            || _background.IsntFree(gridX, gridY))
                            return true;
                    }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void AssignGhost() => _ghost = NewGhost();

        private void SwitchObj()
        {
            _current = new GameObject(_lookahead.Tetromino, _lookahead.Position, false);
            Lookahead = NewTetromino();
        }

        private GameObject NewTetromino()
        {
            var tetromino = _isCurrentInitialized ? Tetromino.GetRandom(_current.Tetromino) : Tetromino.GetRandom();
            var column = _rand.Next(CellsX - tetromino.Width + 1);

            return new GameObject(tetromino, new(column, 0), false);

        }

        private GameObject NewGhost()
        {
            var ghost = new GameObject(_current.Tetromino, _current.Position, true);
            TryDrop(ref ghost);
            return ghost;
        }

        private bool TryDrop(ref GameObject obj)
        {
            if (!IsIngame)
                return false;

            while (!DoesCollide(obj))
                obj.MoveDown();

            return true;
        }
    }
}