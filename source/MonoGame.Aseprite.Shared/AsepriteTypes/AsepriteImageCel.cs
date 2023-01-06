/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2023 Christopher Whitley

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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a cel that contains image data on a frame in an Aseprite
///     image.
/// </summary>
/// <param name="Size">
///     The width and height extents, in pixels, of this
///     <see cref="AsepriteImageCel"/>.
/// </param>
/// <param name="Pixels">
///     An <see cref="ImmutableArray{T}"/> of
///     <see cref="Microsoft.Xna.Framework.Color"/> values where each index
///     represents the color of a pixel for this <see cref="AsepriteImageCel"/>.
///     Pixel order is from top-to-bottom, read left-to-right.
/// </param>
/// <param name="Layer">
///     The <see cref="AsepriteLayer"/> this <see cref="AsepriteImageCel"/> is
///     on.
/// </param>
/// <param name="Position">
///     The x- and y-coordinate position of this <see cref="AsepriteImageCel"/>
///     relative to the bounds of the <see cref="AsepriteFrame"/> it is in.
/// </param>
/// <param name="Opacity">
///     The opacity level of this <see cref="AsepriteImageCel"/>.
/// </param>
public sealed record AsepriteImageCel(Size Size, ImmutableArray<Color> Pixels, AsepriteLayer Layer, Point Position, int Opacity)
    : AsepriteCel(Layer, Position, Opacity);


// /// <summary>
// ///     Represents a cel that contains image data on a frame in an Aseprite
// ///     image.
// /// </summary>
// public sealed class AsepriteImageCel : AsepriteCel
// {
//     /// <summary>
//     ///     The width and height extents, in pixels, of this
//     ///     <see cref="AsepriteImageCel"/>.
//     /// </summary>
//     public Size Size { get; }

//     /// <summary>
//     ///     A <see cref="ReadOnlyCollection{T}"/> of
//     ///     <see cref="Microsoft.Xna.Framework.Color"/> values where each index
//     ///     represents the color of a pixel for this
//     ///     <see cref="AsepriteImageCel"/>.  Pixel order is from top-to-bottom,
//     ///     read left-to-right.
//     /// </summary>
//     public ReadOnlyCollection<Color> Pixels { get; }

//     internal AsepriteImageCel(Size size, Color[] pixels, AsepriteLayer layer, Point position, int opacity)
//         : base(layer, position, opacity)
//     {
//         Size = size;
//         Pixels = Array.AsReadOnly<Color>(pixels);
//     }
// }
