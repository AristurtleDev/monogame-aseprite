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

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class AsepriteTile
{
    internal int TilesetTileID { get; set; }
    internal int XFlip { get; set; }
    internal int YFlip { get; set; }
    internal int Rotation { get; set; }

    internal AsepriteTile(int tilesetTileID, int xFlip, int yFlip, int rotation) =>
        (TilesetTileID, XFlip, YFlip, Rotation) = (tilesetTileID, xFlip, yFlip, rotation);
}

// /// <summary>
// ///     Represents the tile data for a tile of a tilemap cel in an Aseprite
// ///     image.
// /// </summary>
// /// <param name="TilesetTileId">
// ///     The ID of the tile in the <see cref="AsepriteTileset"/> used by this
// ///     <see cref="AsepriteTile"/>.
// /// </param>
// /// <param name="XFlip">
// ///     <para>
// ///         The x-flip value of this tile.
// ///     </para>
// ///     <para>
// ///         As of the current Aseprite 1.3-beta, tile x-flip has not been
// ///         implemented.  As such, this value will always be zero.
// ///         <see href="https://github.com/aseprite/aseprite/issues/3603"/>
// ///     </para>
// /// </param>
// /// <param name="YFlip">
// ///     <para>
// ///         The y-flip value of this tile.
// ///     </para>
// ///     <para>
// ///         As of the current Aseprite 1.3-beta, tile y-flip has not been
// ///         implemented.  As such, this value will always be zero.
// ///         <see href="https://github.com/aseprite/aseprite/issues/3603"/>
// ///     </para>
// /// </param>
// /// <param name="Rotation">
// ///     <para>
// ///         The 90deg clockwise rotation value of this tile.
// ///     </para>
// ///     <para>
// ///         As of the current Aseprite 1.3-beta, tile rotation has not been
// ///         implemented.  As such, this value will always be zero.
// ///         <see href="https://github.com/aseprite/aseprite/issues/3603"/>
// ///     </para>
// /// </param>
// public sealed record AsepriteTile(int TilesetTileId, int XFlip, int YFlip, int Rotation);
