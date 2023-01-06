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
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a cel within a frame in an Aseprite image.
/// </summary>
/// <param name="Layer">
///     The <see cref="AsepriteLayer"/> this <see cref="AsepriteCel"/> is on.
/// </param>
/// <param name="Position">
///     The x- and y-coordinate position of this <see cref="AsepriteCel"/>
///     relative to the bounds of the <see cref="AsepriteFrame"/> it is in.
/// </param>
/// <param name="Opacity">
///     The opacity level of this <see cref="AsepriteCel"/>.
/// </param>
public abstract record AsepriteCel(AsepriteLayer Layer, Point Position, int Opacity)
{
    public AsepriteUserData UserData { get; } = new(default, default);

    /// <summary>
    ///     Returns the <see cref="AsepriteCel.Layer"/> reference of this
    ///     instance of the <see cref="AsepriteCel"/> class as the specified
    ///     type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     A type that is derived from <see cref="AsepriteLayer"/>.
    /// </typeparam>
    /// <returns>
    ///     The <see cref="AsepriteCel.Layer"/> reference of this instance of
    ///     the <see cref="AsepriteCel"/> class as the specified type
    ///     <typeparamref name="T"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the <see cref="AsepriteCel.Layer"/> reference of this
    ///     instance of the <see cref="AsepriteCel"/> class is not of the
    ///     specified type <typeparamref name="T"/>.
    /// </exception>
    public T LayerAs<T>() where T : AsepriteLayer
    {
        if (Layer is T asT)
        {
            return asT;
        }

        throw new InvalidOperationException($"This cel is not on a layer of type '{typeof(T)}'");
    }
}

// /// <summary>
// ///     Represents a cel within a frame in an Aseprite image.
// /// </summary>
// public abstract class AsepriteCel
// {
//     /// <summary>
//     ///     Gets the <see cref="AsepriteLayer"/> that this
//     ///     <see cref="AsepriteCel"/> is on.
//     /// </summary>
//     public AsepriteLayer Layer { get; }

//     /// <summary>
//     ///     Gets the x- and y-coordinate position of this
//     ///     <see cref="AsepriteCel"/>, relative to the bounds of the
//     ///     <see cref="AsepriteFrame"/> it is in.
//     /// </summary>
//     public Point Position { get; }

//     /// <summary>
//     ///     Gets the opacity level of this <see cref="AsepriteCel"/>.
//     /// </summary>
//     public int Opacity { get; }

//     /// <summary>
//     ///     Gets the <see cref="UserData"/> that was set for this
//     ///     <see cref="AsepriteCel"/> in Aseprite.
//     /// </summary>
//     public AsepriteUserData UserData { get; } = new();

//     internal AsepriteCel(AsepriteLayer layer, int x, int y, int opacity)
//     {
//         Layer = layer;
//         Position = new(x, y);
//         Opacity = opacity;
//     }

//     /// <summary>
//     ///     Returns the <see cref="AsepriteCel.Layer"/> reference of this
//     ///     instance of the <see cref="AsepriteCel"/> class as the specified
//     ///     type <typeparamref name="T"/>.
//     /// </summary>
//     /// <typeparam name="T">
//     ///     A type that is derived from <see cref="AsepriteLayer"/>.
//     /// </typeparam>
//     /// <returns>
//     ///     The <see cref="AsepriteCel.Layer"/> reference of this instance of
//     ///     the <see cref="AsepriteCel"/> class as the specified type
//     ///     <typeparamref name="T"/>.
//     /// </returns>
//     /// <exception cref="InvalidOperationException">
//     ///     Thrown if the <see cref="AsepriteCel.Layer"/> reference of this
//     ///     instance of the <see cref="AsepriteCel"/> class is not of the
//     ///     specified type <typeparamref name="T"/>.
//     /// </exception>
//     public T LayerAs<T>() where T : AsepriteLayer
//     {
//         if (Layer is T asT)
//         {
//             return asT;
//         }

//         throw new InvalidOperationException($"This cel is not on a layer of type '{typeof(T)}'");
//     }
// }
