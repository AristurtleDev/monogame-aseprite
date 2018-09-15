//------------------------------------------------------------------------
//  Sprite
//  Simple Sprite class to manage rendering a texture
//------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Aseprite
{
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
                if (this.Texture != null) { return this.Texture.Width; }
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
                if (this.Texture != null) { return this.Texture.Height; }
                else { return 0; }
            }
        }

        /// <summary>
        ///     The <see cref="RenderDefinition"/> used during rendering
        /// </summary>
        public RenderDefinition RenderDefinition { get; set; }

        #region Constructors
        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">The texture to use</param>
        public Sprite(Texture2D texture)
        {
            this.Texture = texture;
            this.Position = Vector2.Zero;
            this.RenderDefinition = new RenderDefinition();
        }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">The texture to use</param>
        /// <param name="position">The position to render at</param>
        public Sprite(Texture2D texture, Vector2 position):this(texture)
        {
            this.Position = position;
        }
        #endregion


        #region Update and Render
        /// <summary>
        ///     Updates the sprite
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        ///     Renders the Sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: this.Texture,
                position: this.Position,
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
