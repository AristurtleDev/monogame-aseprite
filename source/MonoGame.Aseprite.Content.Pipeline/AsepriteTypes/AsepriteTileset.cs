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

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class AsepriteTileset
{
    internal int ID { get; set; }
    internal int Count { get; set; }
    internal Size TileSize { get; set; }
    internal Size Size { get; set; }
    internal string Name { get; set; }
    internal Color[] Pixels { get; set; }

    internal Color[] this[int tileID]
    {
        get
        {
            if (tileID < 0 || tileID >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(tileID));
            }

            int len = Size.Width * Size.Height;
            return Pixels[(tileID * len)..((tileID * len) + len)];
        }
    }

    internal AsepriteTileset(int id, int count, Size tileSize, Size size, string name, Color[] pixels) =>
        (ID, Count, TileSize, Size, Name, Pixels) = (id, count, tileSize, size, name, pixels);


}

// /// <summary>
// ///     Represents a tileset used by tilemap cels in an Aseprite image.
// /// </summary>
// /// <param name="ID">
// ///     The ID of this <see cref="AsepriteTileset"/>.
// /// </param>
// /// <param name="Count">
// ///     The total number of tiles in this <see cref="AsepriteTileset"/>.
// /// </param>
// /// <param name="TileSize">
// ///     The width and height extents, in pixels, of each tile in this
// ///     <see cref="AsepriteTileset"/>.
// /// </param>
// /// <param name="Size">
// ///     The width and height extents, in pixels, of the image representation of
// ///     this <see cref="AsepriteTileset"/>.
// /// </param>
// /// <param name="Name">
// ///     The nme of this <see cref="AsepriteTileset"/>.
// /// </param>
// /// <param name="Pixels">
// ///     An <see cref="ImmutableArray{T}"/> of
// ///     <see cref="Microsoft.Xna.Framework.Color"/> elements that represent the
// ///     pixels that make up the image of this <see cref="TileSize"/>.  Pixel
// ///     order is from top-to-bottom, read left-to-right.
// /// </param>
// public sealed record AsepriteTileset(int ID, int Count, Size TileSize, Size Size, string Name, ImmutableArray<Color> Pixels)
// {
//     /// <summary>
//     ///     Returns an <see cref="ImmutableArray{T}"/> of
//     ///     <see cref="Microsoft.Xna.Framework.Color"/> elements that represent
//     ///     the pixels that make up the image of the tile in this
//     ///     <see cref="AsepriteTileset"/> with the specified
//     ///     <paramref name="tileID"/>.
//     /// </summary>
//     /// <param name="tileID">
//     ///     The ID of the tile in this <see cref="AsepriteTileset"/> to get
//     ///     the pixel data of.
//     /// </param>
//     /// <returns>
//     ///     An <see cref="ImmutableArray{T}"/> of
//     ///     <see cref="Microsoft.Xna.Framework.Color"/> elements that represent
//     ///     the pixels that make up the image of the tile in this
//     ///     <see cref="AsepriteTileset"/> with the specified
//     ///     <paramref name="tileID"/>.
//     /// </returns>
//     /// <exception cref="ArgumentOutOfRangeException">
//     ///     Thrown if the specified <paramref name="tileID"/> is less than zero
//     ///     or is greater than or equal to the total number of tiles in this
//     ///     <see cref="AsepriteTileset"/>.
//     /// </exception>
//     public ImmutableArray<Color> this[int tileID]
//     {
//         get
//         {
//             if (tileID < 0 || tileID >= Count)
//             {
//                 throw new ArgumentOutOfRangeException(nameof(tileID));
//             }

//             int len = Size.Width * Size.Height;

//             return ImmutableArray.Create(Pixels, tileID * len, len);
//         }
//     }
// }
