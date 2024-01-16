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
///     Defines a tile in a <see cref="AsepriteTilemapCel"/>
/// </summary>
public sealed class AsepriteTile : IEquatable<AsepriteTile>
{
    /// <summary>
    ///     Gets the id of the source tile in the <see cref="AsepriteTileset"/> that contains the source image data for 
    ///     this <see cref="AsepriteTile"/>.
    /// </summary>
    public int TilesetTileID { get; }

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AsepriteTile"/> is 
    ///     flipped on the x-axis.
    /// </summary>
    public bool XFlip { get; }

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AsepriteTile"/> is 
    ///     flipped on the y-axis.
    /// </summary>
    public bool YFlip { get; }

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AsepriteTile"/> is 
    ///     flipped on its diagonal-axis.
    /// </summary>
    public bool DFlip { get; }

    internal AsepriteTile(int tilesetTileId, bool xFlip, bool yFlip, bool dFlip) =>
        (TilesetTileID, XFlip, YFlip, DFlip) = (tilesetTileId, xFlip, yFlip, dFlip);

    /// <summary>
    ///     Returns a value that indicates whether the specified <see cref="AsepriteTile"/>. is equal to this 
    ///     <see cref="AsepriteTile"/>..
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="AsepriteTile"/> to check for equality with this <see cref="AsepriteTile"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified <see cref="AsepriteTile"/> is equal to this 
    ///     <see cref="AsepriteTile"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(AsepriteTile? other) => other is not null &&
                                               XFlip == other.XFlip &&
                                               YFlip == other.YFlip &&
                                               DFlip == other.DFlip &&
                                               TilesetTileID == other.TilesetTileID;
}
