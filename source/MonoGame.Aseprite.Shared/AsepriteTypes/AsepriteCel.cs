/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a cel within a frame in an Aseprite image.
/// </summary>
public abstract class AsepriteCel
{
    /// <summary>
    ///     The layer that this cel is on.
    /// </summary>
    public AsepriteLayer Layer { get; }

    /// <summary>
    ///     The x- and y-coordinate position of this cel relative to the bounds
    ///     of the frame it is in.
    /// </summary>
    public Point Position { get; }

    /// <summary>
    ///     The x-coordinate position of this cel relative to the bounds of the
    ///     frame it is in.
    /// </summary>
    public int X => Position.X;

    /// <summary>
    ///     The y-coordinate position of this cel relative to the bounds of the
    ///     frame it is in.
    /// </summary>
    public int Y => Position.Y;

    /// <summary>
    ///     The opacity level of this cel.
    /// </summary>
    public int Opacity { get; }

    /// <summary>
    ///     The custom user data that was set for this cel in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new();

    internal AsepriteCel(AsepriteLayer layer, Point position, int opacity)
    {
        Layer = layer;
        Position = position;
        Opacity = opacity;
    }

    /// <summary>
    ///     Returns the layer reference of this cel as the specified type.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to return the layer reference of this cel asl.
    /// </typeparam>
    /// <returns>
    ///     The layer reference of this cel as the specified type.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the layer reference of this cel is not of the specified
    ///     type.
    /// </exception>
    public T LayerAs<T>() where T : AsepriteLayer
    {
        if (Layer is T asT)
        {
            return asT;
        }

        throw new InvalidOperationException($"The layer of this cel is not of type '{typeof(T)}'.  It is of type '{Layer.GetType()}'");
    }
}
