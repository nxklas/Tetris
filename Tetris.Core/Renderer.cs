using System;
using System.Collections.Generic;

namespace Tetris.Core
{
    public abstract class Renderer
    {
        private readonly List<IRenderable> _renderQueue;

        protected Renderer()
        {
            _renderQueue = new();
        }

        public abstract void Draw(GameState state);

        internal void Append(params GameObject[] objs) => AppendRenderable(objs);

        internal void AppendRenderable(params IRenderable[] renderables)
        {
            if (renderables.Length == 0)
                Throw.ArgumentException_NoElementsToAppend(nameof(renderables));

            _renderQueue.AddRange(renderables);
        }

        internal void Append(IList<GameObject> gameObjects)
        {
            _renderQueue.AddRange(gameObjects);
        }

        /// <summary>
        /// Walks the whole render queue, removes passed elements of <typeparamref name="T"/>, and performs the action <paramref name="render"/> on each element.
        /// </summary>
        /// <param name="render">The method that defines how to render.</param>
        protected void WalkQueue<T>(Action<T> render) where T : IRenderable
        {
            var count = _renderQueue.Count;

            while (count > 0)
            {
                var index = count - 1;

                if (_renderQueue[index] is T obj)
                {
                    _renderQueue.RemoveAt(index);
                    render.Invoke(obj);
                }

                count--;
            }
        }

        private static class Throw
        {
            [System.Diagnostics.CodeAnalysis.DoesNotReturn]
            public static object ArgumentException_NoElementsToAppend(string paramName) =>
                throw new ArgumentException($"Could not append to render queue. Parameter {paramName} must contains at least one element.", paramName);
        }
    }
}