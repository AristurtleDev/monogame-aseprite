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

namespace MonoGame.Aseprite.RawTypes;

/// <summary>
///     Defines a class that represents the raw values of a nine-patch slice.
/// </summary>
public sealed class RawNinePatchSlice : RawSlice, IEquatable<RawNinePatchSlice>
{

    /// <summary>
    ///     Gets the rectangular bounds of the center of the nine-patch slice.
    /// </summary>
    public Rectangle CenterBounds { get; }

    internal RawNinePatchSlice(string name, Rectangle bounds, Rectangle centerBounds, Vector2 origin, Color color)
        : base(name, bounds, origin, color) => CenterBounds = centerBounds;

    /// <summary>
    ///     Returns a value that indicates if the given <see cref="RawNinePatchSlice"/> is equal to this
    ///     <see cref="RawNinePatchSlice"/>.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="RawNinePatchSlice"/> to check for equality with this <see cref="RawNinePatchSlice"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the given <see cref="RawNinePatchSlice"/> is equal to this 
    ///     <see cref="RawNinePatchSlice"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RawNinePatchSlice? other) => other is not null
                                                    && CenterBounds == other.CenterBounds
                                                    && Name == other.Name
                                                    && Bounds == other.Bounds
                                                    && Origin == other.Origin
                                                    && Color == other.Color;
}
