using System;

namespace Tetris.Core.Rendering
{
    /// <summary>
    /// Represents the base class of renderers.
    /// </summary>
    public abstract class Renderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        protected Renderer()
        {
        }

        public void Draw(Scene scene)
        {
            foreach (var renderable in scene.Renderables)
                DrawRenderable(renderable);
        }

        private void DrawRenderable(IRenderable renderable)
        {
            switch (renderable)
            {
                case GameObject obj:
                    DrawObject(obj);
                    break;
                case Background background:
                    DrawBackground(background);
                    break;
                case Text text:
                    DrawText(text);
                    break;
                default:
                    throw new Exception($"Unexpected renderable: {renderable}.");
            }
        }

        protected abstract void DrawObject(GameObject obj);

        protected abstract void DrawBackground(Background background);

        protected abstract void DrawText(Text text);
    }
}