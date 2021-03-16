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
using MonoGame.Aseprite.Documents;

namespace MonoGame.Aseprite.Graphics
{
    /// <summary>
    ///     Simple sprite classed used for managing and rendering a Texture2D
    /// </summary>
    public class Sprite
    {
        //  Holds the top-left xy-coordiante position value.
        private Vector2 _position;

        /// <summary>
        ///     Gets the Texture2D used when rendering.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        ///     Gets or Sets the top-left xy-coordinate position.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        ///     Gets or Sets the top-left x-coordinate position of the sprite.
        /// </summary>
        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        /// <summary>
        ///     Gets or Sets the top-left y-coordinate position of the sprite.
        /// </summary>
        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        /// <summary>
        ///     Gets the width, in pixels.
        /// </summary>
        public virtual int Width
        {
            get
            {
                if (Texture != null) { return Texture.Width; }
                else { return 0; }
            }
        }

        /// <summary>
        ///     Gets the height, in pixels.
        /// </summary>
        public virtual int Height
        {
            get
            {
                if (Texture != null) { return Texture.Height; }
                else { return 0; }
            }
        }

        /// <summary>
        ///     Gets or Sets the bounds of the source rectangle to use
        ///     when rendering. Defines the area within the <see cref="Texture"/>
        ///     that is rendered.
        /// </summary>
        public Rectangle SourceRectangle { get; set; }

        /// <summary>
        ///     Gets or Sets the color mask to use when rendering.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        ///     Gets or Sets the amount of rotation to apply when rendering.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        ///     Gets or Sets the xy-coordinate to use as the origin point
        ///     when rendering.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        ///     Gets or Sets the x and y scale values too apply when rendering.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        ///     Gets or sets the SpriteEffects to use when rendering.
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        ///     Gets or Sets the layer depth to set when rendering.
        /// </summary>
        public float LayerDepth { get; set; }

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        internal Sprite()
        {
            Position = Vector2.Zero;

            SourceRectangle = new Rectangle();
            Color = Color.White;
            Rotation = 0.0f;
            Origin = Vector2.Zero;
            Scale = Vector2.One;
            SpriteEffect = SpriteEffects.None;
            LayerDepth = 0.0f;
        }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">
        ///     The Texture2D used when rendering.
        /// </param>
        public Sprite(Texture2D texture) : this()
        {
            Position = Vector2.Zero;
            Texture = texture;
            SourceRectangle = texture.Bounds;
        }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">
        ///     The Texture2D used when rendering.
        /// </param>
        /// <param name="position">
        ///     The xy-coordinate position to render this sprite at.
        /// </param>
        public Sprite(Texture2D texture, Vector2 position) : this(texture)
        {
            Position = position;
        }

        /// <summary>
        ///     Creates a new <see cref="Sprite"/> instance.
        /// </summary>
        /// <param name="aseprite">
        ///     An <see cref="AsepriteDocument"/> instace created by
        ///     importing from the content pipeline.
        /// </param>
        public Sprite(AsepriteDocument aseprite) : this(aseprite.Texture) { }

        /// <summary>
        ///     Creates a new <see cref="Sprite"/> instance.
        /// </summary>
        /// <param name="aseprite">
        ///     An <see cref="AsepriteDocument"/> instace created by
        ///     importing from the content pipeline.
        /// </param>
        /// <param name="position">
        ///     The xy-coordinate position to render this sprite at.
        /// </param>
        public Sprite(AsepriteDocument aseprite, Vector2 position)
            : this(aseprite.Texture, position) { }

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        /// <param name="gameTime">
        ///     Snapshot of the current game timeing values.
        /// </param>
        public virtual void Update(GameTime gameTime)
        {
            Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        /// <param name="deltaTime">
        ///     The amount of time, in seconds, that has passed since the last update.
        ///     This value should come from GameTime.ElapsedGameTime.TotalSeconds
        /// </param>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        ///     Renders this instance.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance to use when rendering.
        /// </param>
        public virtual void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: Texture,
                position: Position,
                sourceRectangle: SourceRectangle,
                color: Color,
                rotation: Rotation,
                origin: Origin,
                scale: Scale,
                effects: SpriteEffect,
                layerDepth: LayerDepth);
        }

        /// <summary>
        ///     Centers the origin of the rendered sprite.
        /// </summary>
        public virtual void CenterOrigin()
        {
            Origin = new Vector2(SourceRectangle.Width, SourceRectangle.Height) * 0.5f;
        }
    }
}
