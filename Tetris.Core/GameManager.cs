namespace Tetris.Core
{
    public sealed class GameManager
    {
        private readonly GameStateManager _gameStateManager;

        public GameManager()
        {
            _gameStateManager = new GameStateManager();
            _gameStateManager.SetIngame();
        }

        public GameState State => _gameStateManager.State;
    }
}