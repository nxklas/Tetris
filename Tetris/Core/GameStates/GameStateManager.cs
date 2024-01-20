using System;

namespace Tetris.Core.GameStates
{
    public sealed class GameStateManager
    {
        private static readonly GameState[] _states = Enum.GetValues<GameState>();
        private int _index;

        public GameStateManager()
        {
            _index = 1;
        }

        public GameState State
        {
            get => _states[_index];
            set => _index = (int)value;
        }
    }
}