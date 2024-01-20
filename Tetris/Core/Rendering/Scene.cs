using System;
using System.Collections.Generic;
using System.Numerics;
using Tetris.Core.GameStates;

namespace Tetris.Core.Rendering
{
    public readonly struct Scene : IEqualityOperators<Scene, Scene, bool>, IEquatable<Scene>
    {
        private readonly List<IRenderable> _renderables;

        public Scene(GameState gameState, ref IRenderable renderable) : this(gameState, [])
        {
        }

        public Scene(GameState gameState, params IRenderable[] renderable)
        {
            GameState = gameState;
            _renderables = [.. renderable];
        }

        public GameState GameState { get; }
        public IEnumerable<IRenderable> Renderables => _renderables;

        public static bool operator ==(Scene left, Scene right) => left.Equals(right);

        public static bool operator !=(Scene left, Scene right) => !(left == right);

        public bool Equals(Scene other) => GameState == other.GameState && Renderables == other.Renderables;

        public override bool Equals(object? obj) => obj is Scene scene && Equals(scene);

        public override int GetHashCode() => HashCode.Combine(_renderables, GameState);
    }
}