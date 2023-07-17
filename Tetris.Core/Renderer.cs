﻿using System;

namespace Tetris.Core
{
    public abstract class Renderer
    {
        private readonly System.Collections.Generic.List<IRenderable> _renderQueue;

        protected Renderer()
        {
            _renderQueue = new();
        }

        public abstract void Draw(GameState state);

        public void Append(params GameObject[] objs) => AppendRenderable(objs);

        protected void AppendRenderable(params IRenderable[] objs)
        {
            if (objs.Length == 0)
                Throw.ArgumentException_NoElementsToAppend(nameof(objs));

            _renderQueue.AddRange(objs);
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