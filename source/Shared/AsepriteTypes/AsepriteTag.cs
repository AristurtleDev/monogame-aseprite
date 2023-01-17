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
///     Defines an animation tag from an Aseprite file, which is a series of frames that are grouped together to
///     represent an animation.
/// </summary>
public sealed class AsepriteTag
{
    private Color _tagColor;

    /// <summary>
    ///     Gets the index of the <see cref="AsepriteFile"/> the animation defined by this <see cref="AsepriteTag"/>
    ///     starts on.
    /// </summary>
    public int From { get; }

    /// <summary>
    ///     Gets the index of the <see cref="AsepriteFile"/> the animation defined by this <see cref="AsepriteTag"/>
    ///     ends on.
    /// </summary>
    public int To { get; }

    /// <summary>
    ///     Gets a <see cref="AsepriteLoopDirection"/> value that defines the direction the animation defined by this
    ///     <see cref="AsepriteTag"/> loops.
    /// </summary>
    public AsepriteLoopDirection Direction { get; }

    /// <summary>
    ///     Gets the name of this <see cref="AsepriteTag"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the custom <see cref="AsepriteUserData"/> that was set for this <see cref="AsepriteTag"/>> in
    ///     Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new();

    /// <summary>
    ///     Gets a <see cref="Microsoft.Xna.Framework.Color"/> value that represents the color of this
    ///     <see cref="AsepriteTag"/>.
    /// </summary>
    public Color Color => UserData.Color ?? _tagColor;

    internal AsepriteTag(ushort from, ushort to, AsepriteLoopDirection direction, Color color, string name) =>
        (From, To, Direction, _tagColor, Name) = (from, to, direction, color, name);
}
