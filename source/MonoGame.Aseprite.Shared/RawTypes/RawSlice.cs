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
///     Defines a class that represents the raw values of a slice.
/// </summary>
public class RawSlice : IEquatable<RawSlice>
{
    /// <summary>
    ///     Gets the name assigned to the slice.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the rectangular bounds of the slice.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    ///     Gets the x- and y-coordinate point of the slice.
    /// </summary>
    public Vector2 Origin { get; }

    /// <summary>
    ///     Gets the color of the slice.
    /// </summary>
    public Color Color { get; }

    internal RawSlice(string name, Rectangle bounds, Vector2 origin, Color color) =>
        (Name, Bounds, Origin, Color) = (name, bounds, origin, color);

    /// <summary>
    ///     Returns a value that indicates if the given <see cref="RawSlice"/> is equal to this
    ///     <see cref="RawSlice"/>.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="RawSlice"/> to check for equality with this <see cref="RawSprite"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the given <see cref="RawSlice"/> is equal to this 
    ///     <see cref="RawSlice"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RawSlice? other) => other is not null
                                            && Name == other.Name
                                            && Bounds == other.Bounds
                                            && Origin == other.Origin
                                            && Color == other.Color;
}
