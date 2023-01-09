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

using System.Collections.Immutable;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class AsepriteTilemapCel : AsepriteCel
{
    internal Size Size { get; }
    internal List<AsepriteTile> Tiles { get; } = new();

    internal AsepriteTilemapCel(Size size, AsepriteLayer layer, Point position, int opacity)
        : base(layer, position, opacity) => Size = size;
}

// /// <summary>
// ///     Represents a cel that contains tile data in an Aseprite image.
// /// </summary>
// /// <param name="Size">
// ///     <para>
// ///         The width and height extents, in number of tiles, of this
// ///         <see cref="AsepriteTilemapCel"/>.
// ///     </para>
// ///     <para>
// ///         Note: as stated above, this is the width and height in "number of
// ///         tiles", not in pixels.
// ///     </para>
// /// </param>
// /// <param name="Tiles">
// ///     An <see cref="ImmutableArray{T}"/> of the <see cref="AsepriteTile"/>
// ///     elements that make up this <see cref="AsepriteTilemapCel"/>.  Tile order
// ///     is from top-to-bottom, read left-to-right.
// /// </param>
// /// <param name="Layer">
// ///     The <see cref="AsepriteLayer"/> this <see cref="AsepriteTilemapCel"/> is
// ///     on.
// /// </param>
// /// <param name="Position">
// ///     The x- and y-coordinate position of this
// ///     <see cref="AsepriteTilemapCel"/> relative to the bounds of the
// ///     <see cref="AsepriteFrame"/> it is in.
// /// </param>
// /// <param name="Opacity">
// ///     The opacity level of this <see cref="AsepriteTilemapCel"/>.
// /// </param>
// public sealed record AsepriteTilemapCel(Size Size, ImmutableArray<AsepriteTile> Tiles, AsepriteLayer Layer, Point Position, int Opacity)
//     : AsepriteCel(Layer, Position, Opacity)
// {
//     /// <summary>
//     ///     The <see cref="AsepriteTileset"/> used by this
//     ///     <see cref="AsepriteTilemapCel"/>.
//     /// </summary>
//     public AsepriteTileset Tileset => LayerAs<AsepriteTilemapLayer>().Tileset;
// }
