/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

public class Sprite
{
    private Vector2 _position;
    private Vector2 _scale;
    private SpriteEffects _spriteEffects;

    /// <summary>
    ///     Gets the <see cref="Texture2D"/> that represents the image of this
    ///     <see cref="Sprite"/>.
    /// </summary>
    public Texture2D? Texture { get; private set; }

    /// <summary>
    ///     Gets or Sets the x- and y-coordinate position of this sprite.
    /// </summary>
    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    /// <summary>
    ///     Gets or Sets the x-coordinate position of this sprite.
    /// </summary>
    public float X
    {
        get => _position.X;
        set => _position.X = value;
    }

    /// <summary>
    ///     Gets or Sets the y-coordinate position of this sprite.
    /// </summary>
    public float Y
    {
        get => _position.Y;
        set => _position.Y = value;
    }

    /// <summary>
    ///     Gets the width, in pixels, of this sprite.
    /// </summary>
    public virtual int Width => Texture?.Width ?? 0;

    /// <summary>
    ///     Gets the height, in pixels, of this sprite.
    /// </summary>
    public virtual int Height => Texture?.Height ?? 0;

    /// <summary>
    ///     Gets or Sets the bounds of the source rectangle to use when
    ///     rendering this sprite.  Defines the area within the
    ///     <see cref="Texture"/> that is rendered.
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
    ///     Gets or Sets the x- and y-coordinate point of origin to use when
    ///     rendering.
    /// </summary>
    public Vector2 Origin { get; set; }

    /// <summary>
    ///     Gets or Sets the x- and y-scale values to apply when rendering.
    /// </summary>
    public Vector2 Scale
    {
        get => _scale;
        set => _scale = value;
    }

    /// <summary>
    ///     Gets or Sets the x-scale value to apply when rendering.
    /// </summary>
    public float ScaleX
    {
        get => _scale.X;
        set => _scale.X = value;
    }

    /// <summary>
    ///     Gets or Sets the y-scale value to apply when rendering.
    /// </summary>
    public float ScaleY
    {
        get => _scale.Y;
        set => _scale.Y = value;
    }

    /// <summary>
    ///     Gets or Sets the sprite effects to apply when rendering.
    /// </summary>
    public SpriteEffects SpriteEffects
    {
        get => _spriteEffects;
        set => _spriteEffects = value;
    }

    /// <summary>
    ///     Gets or Sets a value that indicates if this sprite should be
    ///     flipped along the horizontal axis when rendering.
    /// </summary>
    public bool FlipHorizontally
    {
        get => _spriteEffects.HasFlag(SpriteEffects.FlipHorizontally);
        set
        {
            _spriteEffects = value ?
                             _spriteEffects | SpriteEffects.FlipHorizontally :
                             _spriteEffects & ~SpriteEffects.FlipHorizontally;

        }
    }

    /// <summary>
    ///     Gets or Sets a value that indicates if this sprite should be
    ///     flipped along the vertical axis when rendering.
    /// </summary>
    public bool FlipVertically
    {
        get => _spriteEffects.HasFlag(SpriteEffects.FlipVertically);
        set
        {
            _spriteEffects = value ?
                             _spriteEffects | SpriteEffects.FlipVertically :
                             _spriteEffects & ~SpriteEffects.FlipVertically;

        }
    }

    /// <summary>
    ///     Gets or SEts the layer depth to set for this sprite when rendering.
    /// </summary>
    public float LayerDepth { get; set; }

    /// <summary>
    ///     Initializes a new instance of the Sprite class.
    /// </summary>
    /// <param name="texture">
    ///     The Texture2D to use when rendering this sprite.
    /// </param>
    public Sprite(Texture2D texture)
    {
        _position = Vector2.Zero;
        _scale = Vector2.One;
        _spriteEffects = SpriteEffects.None;

        Texture = texture;
        SourceRectangle = Texture.Bounds;
        Color = Color.White;
        Rotation = 0.0f;
        Origin = Vector2.Zero;
        LayerDepth = 0.0f;
    }

    /// <summary>
    ///     Initializes a new instance of the Sprite class.
    /// </summary>
    /// <param name="texture">
    ///     The Texture2D to use when rendering this sprite.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render this sprite at.
    /// </param>
    public Sprite(Texture2D texture, Vector2 position) : this(texture) => _position = position;

    /// <summary>
    ///     Renders this sprite using the given SpriteBatch with the values
    ///     from the properties of this sprite.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    public virtual void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw
        (
            texture: Texture,
            position: _position,
            sourceRectangle: SourceRectangle,
            color: Color,
            rotation: Rotation,
            origin: Origin,
            scale: _scale,
            effects: _spriteEffects,
            layerDepth: LayerDepth
        );
    }

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="destinationRectangle">
    ///     The rectangular area that defines the destination to render the
    ///     texture of this sprite.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    public virtual void Render(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color) =>
        spriteBatch.Draw(Texture, destinationRectangle, color);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="destinationRectangle">
    ///     The rectangular area that defines the destination to render the
    ///     texture of this sprite.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    public virtual void Render(SpriteBatch spriteBatch, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color) =>
        spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render the texture at.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    public virtual void Render(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color) =>
        spriteBatch.Draw(Texture, position, sourceRectangle, color);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render the texture at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    public virtual void Render(SpriteBatch spriteBatch, Vector2 position, Color color) =>
        spriteBatch.Draw(Texture, position, color);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render the texture at.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation to apply to the texture when rendering.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate origin point.
    /// </param>
    /// <param name="scale">
    ///     The scale to render the texture at.
    /// </param>
    /// <param name="effects">
    ///     The SpriteEffects to apply to the texture when rendering.
    /// </param>
    /// <param name="layerDepth">
    ///     THe layer depth to apply when rendering the texture.
    /// </param>
    public virtual void Render(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(Texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render the texture at.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation to apply to the texture when rendering.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate origin point.
    /// </param>
    /// <param name="scale">
    ///     The x- and y-scale value to use when rendering the texture.
    /// </param>
    /// <param name="effects">
    ///     The SpriteEffects to apply to the texture when rendering.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the texture.
    /// </param>
    public virtual void Render(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(Texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="destinationRectangle">
    ///     The rectangular area that defines the destination to render the
    ///     texture of this sprite.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation to apply to the texture when rendering.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate origin point.
    /// </param>
    /// <param name="effects">
    ///     The SpriteEffects to apply to the texture when rendering.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the texture.
    /// </param>
    public virtual void Render(SpriteBatch spriteBatch, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
}
