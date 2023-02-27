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

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a named slice for a <see cref="TextureRegion"/> with a bounds, origin, and color.
/// </summary>
public class Slice
{
    /// <summary>
    ///     Gets the name assigned to this <see cref="Slice"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the rectangular bounds of this <see cref="Slice"/> relative to the bounds of the
    ///     <see cref="TextureRegion"/> it is in.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    ///     Gets the x- and y-coordinate origin point for this <see cref="Slice"/> relative to the
    ///     upper-left corner of the bonds of the <see cref="TextureRegion"/> it is in.
    /// </summary>
    public Vector2 Origin { get; }

    /// <summary>
    ///     Gets the <see cref="Microsoft.Xna.Framework.Color"/> value assigned to this 
    ///     <see cref="Slice"/>.
    /// </summary>
    public Color Color { get; }

    internal Slice(string name, Rectangle bounds, Vector2 origin, Color color) =>
        (Name, Bounds, Origin, Color) = (name, bounds, origin, color);
}