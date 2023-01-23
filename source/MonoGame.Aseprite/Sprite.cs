/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

public class Sprite : IDisposable
{
    private TextureRegion? _textureRegion;
    private Vector2 _origin;
    private float _transparency;

    /// <summary>
    ///     Gets the name of this <see cref="Sprite"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> that defines the texture and source rectangle bounds to use when
    ///     rendering this <see cref="Sprite"/>.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this sprite instance has been disposed when trying to access this texture region of the sprite.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the texture region of this sprite is null.  This should only happen if this sprite has was
    ///     disposed of prior to accessing this texture region value.
    /// </exception>
    public TextureRegion TextureRegion
    {
        get
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Sprite), $"The {nameof(Sprite)} '{Name}' was previously disposed");
            }

            return _textureRegion;
        }
    }

    /// <summary>
    ///     Gets or Sets the color mask to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or Sets the level of transparency, between 0.0f and 1.0f, to apply when rendering this
    ///     <see cref="Sprite"/>.
    /// </summary>
    public float Transparency
    {
        get => _transparency;
        set => _transparency = MathHelper.Clamp(value, 0.0f, 1.0f);
    }

    /// <summary>
    ///     Gets or Sets the rotation, in radians, to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    ///     Gets or Sets the x- and y-coordinate point of origin for this <see cref="Sprite"/> when rendering.
    /// </summary>
    public Vector2 Origin
    {
        get => _origin;
        set => _origin = value;
    }

    /// <summary>
    ///     Gets or Sets the x-coordinate point of origin for this <see cref="Sprite"/> when rendering.
    /// </summary>
    public float OriginX
    {
        get => _origin.X;
        set => _origin.X = value;
    }

    /// <summary>
    ///     Gets or Sets the y-coordinate point of origin for this <see cref="Sprite"/> when rendering.
    /// </summary>
    public float OriginY
    {
        get => _origin.Y;
        set => _origin.Y = value;
    }

    /// <summary>
    ///     Gets or Sets the <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for vertical and
    ///     horizontal flipping when rendering this <see cref="Sprite"/>.
    /// </summary>
    public SpriteEffects SpriteEffects { get; set; }

    /// <summary>
    ///     Gets or Sets the layer depth value to render this <see cref="Sprite"/> at.
    /// </summary>
    public float LayerDepth { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates if this <see cref="Sprite"/> is visible and can be rendered.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    ///     Gets a value that indicates if this sprite has been disposed of.
    /// </summary>
    [MemberNotNullWhen(false, nameof(_textureRegion))]
    public bool IsDisposed { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Sprite"/> class using a <see cref="TextureRegion"/>.
    /// </summary>
    /// <param name="textureRegion">
    ///     The <see cref="TextureRegion"/> that is represented by this <see cref="Sprite"/>.
    /// </param>
    public Sprite(TextureRegion textureRegion)
    {
        _textureRegion = textureRegion;
        Color = Color.White;
        _transparency = 1.0f;
        Rotation = 0.0f;
        _origin = Vector2.Zero;
        SpriteEffects = SpriteEffects.None;
        LayerDepth = 0.0f;
        IsVisible = true;
        Name = textureRegion.Name;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Sprite"/> class using a <see cref="TextureRegion"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to give this <see cref="Sprite"/>.
    /// </param>
    /// <param name="textureRegion">
    ///     The <see cref="TextureRegion"/> that is represented by this <see cref="Sprite"/>.
    /// </param>
    public Sprite(string name, TextureRegion textureRegion)
        : this(textureRegion) => Name = name;


    /// <summary>
    ///     Releases resources held by this sprite.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (IsDisposed)
        {
            return;
        }

        _textureRegion = null;

        IsDisposed = true;
    }


}
