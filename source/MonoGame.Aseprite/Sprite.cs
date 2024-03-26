// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

/// <summary>
/// Defines a named sprite
/// </summary>
public class Sprite
{
    private TextureRegion _textureRegion;

    private Vector2 _origin;
    private Vector2 _scale;
    private float _transparency;

    /// <summary>
    /// Gets the name assigned to this <see cref="Sprite"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source <see cref="TextureRegion"/> of this <see cref="Sprite"/>.
    /// </summary>
    public TextureRegion TextureRegion
    {
        get => _textureRegion;
        protected set => _textureRegion = value;
    }

    /// <summary>
    /// Gets the width, in pixels, of this <see cref="Sprite"/>
    /// </summary>
    public int Width => _textureRegion.Bounds.Width;

    /// <summary>
    /// Gets the height, in pixels, of this <see cref="Sprite"/>
    /// </summary>
    public int Height => _textureRegion.Bounds.Height;

    /// <summary>
    /// Gets or Sets the color mask to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Gets or Sets the level of transparency, between 0.0f, and 1.0f, to apply when rendering this
    /// <see cref="Sprite"/>.
    /// </summary>
    public float Transparency
    {
        get => _transparency;
        set => _transparency = MathHelper.Clamp(value, 0.0f, 1.0f);
    }

    /// <summary>
    /// Gets or Sets the rotation, in radians, to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    /// Gets or Sets the x- and y-coordinate point of origin to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public Vector2 Origin
    {
        get => _origin;
        set => _origin = value;
    }

    /// <summary>
    /// Gets or Sets the x-coordinate point of origin to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float OriginX
    {
        get => _origin.X;
        set => _origin.X = value;
    }

    /// <summary>
    /// Gets or Sets the y-coordinate point of origin to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float OriginY
    {
        get => _origin.Y;
        set => _origin.Y = value;
    }

    /// <summary>
    /// Gets or Sets the x- and y-axis scale factor to use when rendering this <see cref="Sprite"/>.
    /// </summary>
    public Vector2 Scale
    {
        get => _scale;
        set => _scale = value;
    }

    /// <summary>
    /// Gets or Sets the x-axis scale factor to use when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float ScaleX
    {
        get => _scale.X;
        set => _scale.X = value;
    }

    /// <summary>
    /// Gets or Sets the y-axis scale factor to use when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float ScaleY
    {
        get => _scale.Y;
        set => _scale.Y = value;
    }

    /// <summary>
    /// Gets or Sets the <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for vertical and
    /// horizontal flipping when rendering this <see cref="Sprite"/>.
    /// </summary>
    public SpriteEffects SpriteEffects { get; set; }

    /// <summary>
    /// Gets or Sets a value that indicates whether to flip this <see cref="Sprite"/> horizontally when rendering.
    /// </summary>
    public bool FlipHorizontally
    {
        get => SpriteEffects.HasFlag(SpriteEffects.FlipHorizontally);
        set => SpriteEffects = value ? SpriteEffects | SpriteEffects.FlipHorizontally : SpriteEffects & ~SpriteEffects.FlipHorizontally;
    }

    /// <summary>
    /// Gets or Sets a value that indicates whether to flip this <see cref="Sprite"/> vertically when rendering.
    /// </summary>
    public bool FlipVertically
    {
        get => SpriteEffects.HasFlag(SpriteEffects.FlipVertically);
        set => SpriteEffects = value ? SpriteEffects | SpriteEffects.FlipVertically : SpriteEffects & ~SpriteEffects.FlipVertically;
    }

    /// <summary>
    /// Gets or Sets the layer depth to render this <see cref="Sprite"/> at.
    /// </summary>
    public float LayerDepth { get; set; }

    /// <summary>
    /// Gets or Sets a value that indicates if this <see cref="Sprite"/> is visible and can be rendered.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <param name="name" cref="string">The name to assign the <see cref="Sprite"/>.</param>
    /// <param name="textureRegion" cref="TextureRegion">
    /// The source <see cref="TextureRegion"/> to assign the <see cref="Sprite"/>.
    /// </param>
    public Sprite(string name, TextureRegion textureRegion)
    {
        _textureRegion = textureRegion;
        Color = Color.White;
        _transparency = 1.0f;
        Rotation = 0.0f;
        _origin = Vector2.Zero;
        _scale = Vector2.One;
        SpriteEffects = SpriteEffects.None;
        LayerDepth = 0.0f;
        IsVisible = true;
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <param name="name">The name to assign the <see cref="Sprite"/>.</param>
    /// <param name="texture">The source image for the <see cref="Sprite"/>.</param>
    public Sprite(string name, Texture2D texture)
        : this(name, new TextureRegion(name, texture, texture.Bounds)) { }

    /// <summary>
    /// Renders this <see cref="Sprite"/>.
    /// </summary>
    /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to use for rendering this<see cref="Sprite"/>.</param>
    /// <param name="position">The x- and y-coordinate location to render this <see cref="Sprite"/> at.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position) => spriteBatch.Draw(this, position);
}
