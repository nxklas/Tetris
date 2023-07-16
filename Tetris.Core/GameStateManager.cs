namespace Tetris.Core
{
    public sealed class GameStateManager
    {
        public GameStateManager()
        {
            State = GameState.Menu;
        }

        public GameState State { get; private set; }

        public void SetMenu() => State = GameState.Menu;

        public void SetPause() => State = GameState.Pause;

        public void SetIngame() => State = GameState.Ingame;

        public void SetGameOver() => State = GameState.GameOver;
    }
}