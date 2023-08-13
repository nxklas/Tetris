using System.Diagnostics;

namespace Tetris.Core
{
    public sealed class GameStateManager
    {
        private GameState _gameState;

        public GameStateManager()
        {
            _gameState = GameState.Menu;
        }

        public GameState State
        {
            get => _gameState;
            private set
            {
                _gameState = value;
                Debug.WriteLine($"Game state changed to: {value}.");
            }
        }

        public void SetMenu() => State = GameState.Menu;

        public void SetPause() => State = GameState.Pause;

        public void SetIngame() => State = GameState.Ingame;

        public void SetGameOver() => State = GameState.GameOver;
    }
}