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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Defines a single tile in a <see cref="AsepriteTilemapCel"/>.
/// </summary>
public sealed class AsepriteTile
{
    /// <summary>
    ///     Gets the ID of the tile in the <see cref="AsepriteTileset"/> that contains the image pixel data for this
    ///     <see cref="AsepriteTile"/>.
    /// </summary>
    public int TilesetTileID { get; }

    /// <summary>
    ///     Gets the x-flip value for this <see cref="AsepriteTile"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         As of the current Aseprite 1.3-beta-21, tile x-flip has not been implemented.  As such, this value will
    ///         always be 0.
    ///     </para>
    ///     <see href="https://github.com/aseprite/aseprite/issues/3603"/>
    /// </remarks>
    public int XFlip { get; }

    /// <summary>
    ///     Gets the y-flip value for this <see cref="AsepriteTile"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         As of the current Aseprite 1.3-beta-21, tile y-flip has not been implemented.  As such, this value will
    ///         always be 0.
    ///     </para>
    ///     <see href="https://github.com/aseprite/aseprite/issues/3603"/>
    /// </remarks>
    public int YFlip { get; }

    /// <summary>
    ///     Gets the 90deg clockwise rotation value for this <see cref="AsepriteTile"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         As of the current Aseprite 1.3-beta-21, tile rotation has not been implemented.  As such, this value
    ///         will always be 0.
    ///     </para>
    ///     <see href="https://github.com/aseprite/aseprite/issues/3603"/>
    /// </remarks>
    public int Rotation { get; }

    internal AsepriteTile(int tilesetTileId, int xFlip, int yFlip, int rotation) =>
        (TilesetTileID, XFlip, YFlip, Rotation) = (tilesetTileId, xFlip, yFlip, rotation);
}
