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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Defines a single cel in an frame.
/// </summary>
public abstract class AsepriteCel
{
    /// <summary>
    ///     Gets a reference to the <see cref="AsepriteLayer"/> this <see cref="AsepriteCel"/> is on.
    /// </summary>
    public AsepriteLayer Layer { get; }

    /// <summary>
    ///     Gets the x- and y-coordinate location of this <see cref="AsepriteCel"/> relative to the bounds of the 
    ///     <see cref="AsepriteFrame"/> it is in.
    /// </summary>
    internal Point Position { get; }

    /// <summary>
    ///     Gets the opacity level of this <see cref="AsepriteCel"/>.
    /// </summary>
    internal int Opacity { get; }

    /// <summary>
    ///     Gets the custom <see cref="AsepriteUserData"/> that was set for this <see cref="AsepriteCel"/> in aseprite.
    /// </summary>
    internal AsepriteUserData UserData { get; } = new();

    internal AsepriteCel(AsepriteLayer layer, Point position, int opacity) =>
        (Layer, Position, Opacity) = (layer, position, opacity);

    /// <summary>
    ///     Returns the <see cref="AsepriteLayer"/> this <see cref="AsepriteCel"/> is on as the specified type.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to return as.
    /// </typeparam>
    /// <returns>
    ///     The <see cref="AsepriteLayer"/> this <see cref="AsepriteCel"/> is on as the specified type.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the <see cref="AsepriteLayer"/> this <see cref="AsepriteCel"/> is on is not of the type specified.
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
