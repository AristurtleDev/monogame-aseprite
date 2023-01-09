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

using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal abstract class AsepriteCel
{
    internal AsepriteLayer Layer { get; set; }
    internal Point Position { get; set; }
    internal int Opacity { get; set; }
    internal AsepriteUserData UserData { get; } = new();

    [MemberNotNullWhen(true, nameof(UserData))]
    internal bool HasUserData => UserData is not null;

    internal AsepriteCel(AsepriteLayer layer, Point position, int opacity) =>
        (Layer, Position, Opacity) = (layer, position, opacity);
}

// /// <summary>
// ///     Represents a cel within a frame in an Aseprite image.
// /// </summary>
// /// <param name="Layer">
// ///     The <see cref="AsepriteLayer"/> this <see cref="AsepriteCel"/> is on.
// /// </param>
// /// <param name="Position">
// ///     The x- and y-coordinate position of this <see cref="AsepriteCel"/>
// ///     relative to the bounds of the <see cref="AsepriteFrame"/> it is in.
// /// </param>
// /// <param name="Opacity">
// ///     The opacity level of this <see cref="AsepriteCel"/>.
// /// </param>
// /// <param name="UserData">
// ///     The custom user data set for this <see cref="AsepriteCel"/> in
// ///     Aseprite, if any was set; otherwise, <see langword="null"/>.
// /// </param>
// public abstract record AsepriteCel(AsepriteLayer Layer, Point Position, int Opacity, AsepriteUserData? UserData = null)
// {
//     /// <summary>
//     ///     <para>
//     ///         Indicates whether this <see cref="AsepriteCel"/> has user
//     ///         data.
//     ///     </para>
//     ///     <para>
//     ///         When this returns <see langword="true"/> it guarantees that
//     ///         the <see cref="AsepriteCel.UserData"/> value of this instance
//     ///         is not <see langword="null"/>.
//     ///     </para>
//     /// </summary>
//     [MemberNotNullWhen(true, nameof(UserData))]
//     public bool HasUserData => UserData is not null;

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
