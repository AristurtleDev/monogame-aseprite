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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

public sealed class TilemapTile
{
    private SpriteEffects _spriteEffects;
    public int TilesetIndex { get; }

    /// <summary>
    ///     Gets or Sets the
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to
    ///     apply when rendering this <see cref="TilemapTile"/>.
    /// </summary>
    public SpriteEffects SpriteEffects
    {
        get => _spriteEffects;
        set => _spriteEffects = value;
    }

    /// <summary>
    ///     Gets or Sets a value that indicates if this
    ///     <see cref="TilemapTile"/> should be flipped along the x-axis
    ///     (horizontal) when rendering.
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
    ///     Gets or Sets a value that indicates if this
    ///     <see cref="TilemapTile"/> should be flipped along the y-axis
    ///     (vertical) when rendering.
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
    ///     Gets or Sets the amount of rotation, in radians, to apply when
    ///     rendering this <see cref="TilemapTile"/>.
    /// </summary>
    public float Rotation { get; set; }

    internal TilemapTile(int tilesetIndex, SpriteEffects spriteEffects) =>
        (TilesetIndex, _spriteEffects) = (tilesetIndex, spriteEffects);
}
