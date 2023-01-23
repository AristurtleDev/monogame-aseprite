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

/// <summary>
///     Defines a named rectangular region that represents the location and extents of a region within a source texture.
/// </summary>
public class TextureRegion : IDisposable
{
    private Texture2D? _texture;

    /// <summary>
    ///     Gets the name of this texture region.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the source texture used by this texture region.
    /// </summary>
    public Texture2D Texture
    {
        get
        {
            if (_texture is Texture2D texture)
            {
                return texture;
            }

            throw new ObjectDisposedException(nameof(TextureRegion), $"The {nameof(TextureRegion)} '{Name}' was previously disposed.");
        }
    }

    public bool IsDisposed { get; private set; }

    /// <summary>
    ///     Gets the rectangular bounds that define the location and width and height extents, in pixels of the region
    ///     within the source texture that is represented by this texture region.
    /// </summary>
    public Rectangle Bounds { get; }

    internal TextureRegion(string name, Texture2D texture, Rectangle bounds) =>
        (Name, _texture, Bounds) = (name, texture, bounds);

    ~TextureRegion() => Dispose(false);

    /// <summary>
    ///     Releases resources held by this texture region.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool isDisposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            _texture = null;
        }

        IsDisposed = true;
    }
}
