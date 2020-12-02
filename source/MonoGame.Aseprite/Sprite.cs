/* ------------------------------------------------------------------------------
    Copyright (c) 2020 Christopher Whitley

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:
    
    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
------------------------------------------------------------------------------ */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite
{
    /// <summary>
    ///     Simple sprite classed used for managing and rendering a Texture2D
    /// </summary>
    public class Sprite
    {
        /// <summary>
        ///     The texture used to when rendering
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        ///     The xy-coordinate position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        ///     The width of the sprite
        /// </summary>
        public int Width
        {
            get
            {
                if (Texture != null) { return Texture.Width; }
                else { return 0; }
            }
        }

        /// <summary>
        ///     The height of the sprite
        /// </summary>
        public int Height
        {
            get
            {
                if (Texture != null) { return Texture.Height; }
                else { return 0; }
            }
        }

        /// <summary>
        ///     The <see cref="RenderDefinition"/> used during rendering
        /// </summary>
        public RenderDefinition RenderDefinition { get; set; }

        #region Constructors
        internal Sprite()
        {
            Position = Vector2.Zero;
            RenderDefinition = new RenderDefinition();
        }
        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">The texture to use</param>
        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Position = Vector2.Zero;
            RenderDefinition = new RenderDefinition();
        }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">The texture to use</param>
        /// <param name="position">The position to render at</param>
        public Sprite(Texture2D texture, Vector2 position) : this(texture)
        {
            Position = position;
        }
        #endregion


        #region Update and Render
        /// <summary>
        ///     Updates the sprite
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        ///     Updates the sprite
        /// </summary>
        /// <param name="deltaTime">
        ///     The amount of time in seconds that has passed since the last update.
        ///     This value should come from GameTime.ElapsedGameTime.TotalSeconds
        /// </param>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        ///     Renders the Sprite
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to use when rendering</param>
        public virtual void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: Texture,
                position: Position,
                sourceRectangle: RenderDefinition.SourceRectangle,
                color: RenderDefinition.Color,
                rotation: RenderDefinition.Rotation,
                origin: RenderDefinition.Origin,
                scale: RenderDefinition.Scale,
                effects: RenderDefinition.SpriteEffect,
                layerDepth: RenderDefinition.LayerDepth);
        }
        #endregion
    }
}
