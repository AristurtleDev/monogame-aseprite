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
///     Represents the palette of an Aseprite file.
/// </summary>
/// <param name="TransparentIndex">
///     <para>
///         The index of the <see cref="Microsoft.Xna.Framework.Color"/> value
///         in the <see cref="Colors"/> collection of this
///         <see cref="AsepritePalette"/> that represents the value of a
///         transparent pixel.
///     </para>
///     <para>
///         This value is only valid if the Color Depth mode that was set in the
///         Aseprite UI for the file was "Index Mode".
///     </para>
/// </param>
/// <param name="Colors">
///     A <see cref="ImmutableArray{T}"/> of
///     <see cref="Microsoft.Xna.Framework.Color"/> values that make up this
///     <see cref="AsepritePalette"/>.
/// </param>
public sealed record AsepritePalette(int TransparentIndex, ImmutableArray<Color> Colors);

// /// <summary>
// ///     Represents the palette of an Aseprite file.
// /// </summary>
// public sealed class AsepritePalette
// {
//     /// <summary>
//     ///     Gets the index of the <see cref="Microsoft.Xna.Framework.Color"/>
//     ///     value in this <see cref="AsepritePalette"/> that represents the
//     ///     value of a transparent pixel.
//     /// </summary>
//     /// <remarks>
//     ///     This value is only valid if the Color Depth mode that was set in
//     ///     the Aseprite UI for this file is "Indexed".
//     /// </remarks>
//     public int TransparentIndex { get; internal set; }

//     /// <summary>
//     ///     Gets a <see cref="ReadOnlyCollection{T}"/> of
//     ///     <see cref="Microsoft.Xna.Framework.Color"/> values that represent
//     ///     colors that were used in the Aseprite file.
//     /// </summary>
//     public ReadOnlyCollection<Color> Colors { get; }

//     internal AsepritePalette(Color[] colors, int transparentIndex)
//     {
//         Colors = Array.AsReadOnly<Color>(colors);
//         TransparentIndex = transparentIndex;
//     }
// }
