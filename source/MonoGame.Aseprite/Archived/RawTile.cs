// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2018-2023 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */

// namespace MonoGame.Aseprite.Processors;

// /// <summary>
// ///     Defines the raw values of a <see cref="Tile"/> used by the <see cref="TilemapProcessor"/>.
// /// </summary>
// public sealed class RawTile : IEquatable<RawTile>
// {
//     internal int TilesetTileID { get; }
//     internal bool XFlip { get; }
//     internal bool YFlip { get; }
//     internal float Rotation { get; }

//     internal RawTile(int tilesetTileID, bool xFlip, bool yFlip, float rotation) =>
//         (TilesetTileID, XFlip, YFlip, Rotation) = (tilesetTileID, xFlip, yFlip, rotation);

//     /// <summary>
//     ///     Returns a value that indicates if the specified instance of the <see cref="RawTile"/> class is
//     ///     equal to this instance of the <see cref="RawTile"/> class.
//     /// </summary>
//     /// <param name="other">
//     ///     The instance of the <see cref="RawTile"/> class to check for equality.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the specified instance of the <see cref="RawTile"/> class is equal to
//     ///     this instance of the <see cref="RawTile"/> class; otherwise, <see langword="false"/>.
//     /// </returns>
//     public bool Equals(RawTile? other) => other is not null &&
//                                                       TilesetTileID == other.TilesetTileID &&
//                                                       XFlip == other.XFlip &&
//                                                       YFlip == other.YFlip &&
//                                                       Rotation == other.Rotation;
// }
