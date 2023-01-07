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
    private SpriteSheetFrame _spriteSheetRegion;
    private Vector2 _position;
    private Vector2 _scale;
    private SpriteEffects _spriteEffects;

    /// <summary>
    ///     Gets or Sets the <see cref="SpriteSheetRegion"/> used by this
    ///     <see cref="Sprite"/>.
    /// </summary>
    public SpriteSheetFrame SpriteSheetRegion
    {
        get => _spriteSheetRegion;
        set => _spriteSheetRegion = value;
    }

    /// <summary>
    ///     Gets or Sets a value that indicates if this <see cref="Sprite"/>
    ///     is visible and can be rendered.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    ///     Gets or Sets the x- and y-coordinate position to render this
    ///     <see cref="Sprite"/> at.
    /// </summary>
    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    /// <summary>
    ///     Gets or Sets the x-coordinate position to render this
    ///     <see cref="Sprite"/> at.
    /// </summary>
    public float X
    {
        get => _position.X;
        set => _position.X = value;
    }

    /// <summary>
    ///     Gets or Sets the y-coordinate position to render this
    ///     <see cref="Sprite"/> at.
    /// </summary>
    public float Y
    {
        get => _position.Y;
        set => _position.Y = value;
    }

    /// <summary>
    ///     Gets the width, in pixels, of this <see cref="Sprite"/>.
    ///     Gets the width, in pixels, of this sprite.
    /// </summary>
    public virtual int Width => _spriteSheetRegion.Bounds.Width;

    /// <summary>
    ///     Gets the height, in pixels, of this <see cref="Sprite"/>.
    /// </summary>
    public virtual int Height => _spriteSheetRegion.Bounds.Height;

    /// <summary>
    ///     Gets or Sets the <see cref="Color"/> value to use as the color
    ///     mask when rending this <see cref="Sprite"/>.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or Sets the amount of transparency to apply when rendering
    ///     this <see cref="Sprite"/>.
    /// </summary>
    public float Alpha { get; set; }

    /// <summary>
    ///     Gets or Sets the amount of rotation, in radians, to apply when
    ///     rendering this <see cref="Sprite"/>.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    ///     Gets or Sets the x- and y-coordinate point of origin to use when
    ///     rendering this <see cref="Sprite"/>.
    /// </summary>
    public Vector2 Origin { get; set; }

    /// <summary>
    ///     Gets or Sets the amount of x-axis (horizontal) and y-axis (vertical)
    ///     scale to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public Vector2 Scale
    {
        get => _scale;
        set => _scale = value;
    }

    /// <summary>
    ///     Gets or Sets the amount of x-axis (horizontal) scale to apply when
    ///     rendering this <see cref="Sprite"/>.
    /// </summary>
    public float ScaleX
    {
        get => _scale.X;
        set => _scale.X = value;
    }

    /// <summary>
    ///     Gets or Sets the amount of y-axis (vertical) scale to apply when
    ///     rendering this <see cref="Sprite"/>.
    /// </summary>
    public float ScaleY
    {
        get => _scale.Y;
        set => _scale.Y = value;
    }

    /// <summary>
    ///     Gets or Sets the
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to
    ///     apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public SpriteEffects SpriteEffects
    {
        get => _spriteEffects;
        set => _spriteEffects = value;
    }

    /// <summary>
    ///     Gets or Sets a value that indicates if this <see cref="Sprite"/>
    ///     should be flipped along the x-axis (horizontal) when rendering.
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
    ///     Gets or Sets a value that indicates if this <see cref="Sprite"/>
    ///     should be flipped along the y-axis (vertical) when rendering.
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
    ///     Gets or Sets the layer depth to apply when rendering this
    ///     <see cref="Sprite"/>.
    /// </summary>
    public float LayerDepth { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <param name="region">
    ///     The <see cref="SpriteSheetRegion"/> to be used by this
    ///     <see cref="Sprite"/>.
    /// </param>
    public Sprite(SpriteSheetFrame region)
        : this(region, Vector2.Zero, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <param name="region">
    ///     The <see cref="SpriteSheetRegion"/> to be used by this
    ///     <see cref="Sprite"/>.
    /// </param>
    public Sprite(SpriteSheetFrame region, Vector2 position)
        : this(region, position, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Sprite"/> class using
    ///     the values provided.
    /// </summary>
    /// <param name="region">
    ///     The <see cref="SpriteSheetRegion"/> to be used by this
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to use when rendering this
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="color">
    ///     The <see cref="Color"/> value to use as the color mask when
    ///     rendering this <see cref="Sprite"/>.
    /// </param>
    public Sprite(SpriteSheetFrame region, Vector2 position, Color color)
        : this(region, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Sprite"/> class using
    ///     the values provided.
    /// </summary>
    /// <param name="region">
    ///     The <see cref="SpriteSheetRegion"/> to be used by this
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to use when rendering this
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="color">
    ///     The <see cref="Color"/> value to use as the color mask when
    ///     rendering this <see cref="Sprite"/>.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering this
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="origin">
    ///     THe x- and y-coordinate point of origin for this
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of x-axis (horizontal) and y-axis (vertical) scale to
    ///     apply when rendering this <see cref="Sprite"/>.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to
    ///     apply when rendering this <see cref="Sprite"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="Sprite"/>.
    /// </param>
    public Sprite(SpriteSheetFrame region, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        _spriteSheetRegion = region;
        _position = position;
        _scale = scale;
        _spriteEffects = effects;

        Color = color;
        Rotation = rotation;
        Origin = origin;
        LayerDepth = layerDepth;
        Alpha = 1.0f;
        IsVisible = true;
    }

    /// <summary>
    ///     Draws this <see cref="Sprite"/> using the given
    ///     <see cref="SpriteBatch"/>
    /// </summary>
    /// <remarks>
    ///     The values for position, color, rotation, origin, scale, effects,
    ///     and layer depth that are passed to the SpriteBatch are derived from
    ///     the properties of this <see cref="Sprite"/> instance when using this
    ///     method.  To use different values, use one of the other Draw method
    ///     overloads instead.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw
        (
            texture: _spriteSheetRegion.Texture,
            position: _position,
            sourceRectangle: _spriteSheetRegion.Bounds,
            color: Color * Alpha,
            rotation: Rotation,
            origin: Origin,
            scale: _scale,
            effects: _spriteEffects,
            layerDepth: LayerDepth
        );
    }

    /// <summary>
    ///     Draws this <see cref="Sprite"/> using the given
    ///     <see cref="SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="position">
    ///     If a value is provided, overrides the <see cref="Sprite.Position"/>
    ///     value during this render only.
    /// </param>
    /// <param name="color">
    ///     If a value is provided, overrides the <see cref="Sprite.Color"/>
    ///     value during this render only.
    /// </param>
    /// <param name="rotation">
    ///     If a value is provided, overrides the <see cref="Sprite.Rotation"/>
    ///     value during this render only.
    /// </param>
    /// <param name="origin">
    ///     If a value is provided, overrides the <see cref="Sprite.Origin"/>
    ///     value during this render only.
    /// </param>
    /// <param name="scale">
    ///     If a value is provided, overrides the <see cref="Sprite.Scale"/>
    ///     value during this render only.
    /// </param>
    /// <param name="effects">
    ///     If a value is provided, overrides the
    ///     <see cref="Sprite.SpriteEffects"/> value during this render only.
    /// </param>
    /// <param name="layerDepth">
    ///     If a value is provided, overrides the
    ///     <see cref="Sprite.LayerDepth"/> value during this render only.
    /// </param>
    public virtual void Draw(SpriteBatch spriteBatch, Vector2? position = default, Color? color = default, float? rotation = default, Vector2? origin = default, Vector2? scale = default, SpriteEffects? effects = default, float? layerDepth = default)
    {
        spriteBatch.Draw
        (
            texture: _spriteSheetRegion.Texture,
            position: position ?? Position,
            sourceRectangle: _spriteSheetRegion.Bounds,
            color: color ?? Color * Alpha,
            rotation: rotation ?? Rotation,
            origin: origin ?? Origin,
            scale: scale ?? _scale,
            effects: effects ?? _spriteEffects,
            layerDepth: layerDepth ?? LayerDepth

        );
    }

    /// <summary>
    ///     Draws this <see cref="Sprite"/> using the given
    ///     <see cref="SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="destinationRectangle">
    ///     If a value is provided, overrides the <see cref="Sprite.Position"/>
    ///     and <see cref="Sprite.Scale"/> values to fit the sprite render into
    ///     the bounds given during this render only.
    /// </param>
    /// <param name="color">
    ///     If a value is provided, overrides the <see cref="Sprite.Color"/>
    ///     value during this render only.
    /// </param>
    /// <param name="rotation">
    ///     If a value is provided, overrides the <see cref="Sprite.Rotation"/>
    ///     value during this render only.
    /// </param>
    /// <param name="origin">
    ///     If a value is provided, overrides the <see cref="Sprite.Origin"/>
    ///     value during this render only.
    /// </param>
    /// <param name="effects">
    ///     If a value is provided, overrides the
    ///     <see cref="Sprite.SpriteEffects"/> value during this render only.
    /// </param>
    /// <param name="layerDepth">
    ///     If a value is provided, overrides the
    ///     <see cref="Sprite.LayerDepth"/> value during this render only.
    /// </param>
    public virtual void Draw(SpriteBatch spriteBatch, Rectangle? destinationRectangle = default, Color? color = default, float? rotation = default, Vector2? origin = default, SpriteEffects? effects = default, float? layerDepth = default)
    {
        spriteBatch.Draw
        (
            texture: _spriteSheetRegion.Texture,
            destinationRectangle: destinationRectangle ?? new Rectangle((int)X, (int)Y, Width, Height),
            sourceRectangle: _spriteSheetRegion.Bounds,
            color: color ?? Color * Alpha,
            rotation: rotation ?? Rotation,
            origin: origin ?? Origin,
            effects: effects ?? _spriteEffects,
            layerDepth: layerDepth ?? LayerDepth
        );
    }
}
