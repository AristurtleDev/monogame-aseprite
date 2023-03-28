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
///     Defines an animation tag in aseprite.
/// </summary>
public sealed class AsepriteTag
{
    internal Color _tagColor;

    /// <summary>
    ///     Gets the index of the first <see cref="AsepriteFrame"/> of animation defined by this 
    ///     <see cref="AsepriteTag"/>.
    /// </summary>
    public int From { get; }

    /// <summary>
    ///     Gets the index of the last see cref="AsepriteFrame"/> of animation defined by this 
    ///     <see cref="AsepriteTag"/>.
    /// </summary>
    public int To { get; }

    /// <summary>
    ///     Gets the <see cref="AsepriteLoopDirection"/> of the animation defined by this <see cref="AsepriteTag"/>.
    /// </summary>
    public AsepriteLoopDirection Direction { get; }

    /// <summary>
    ///     Gets the repeat count of the animation defined by this <see cref="AsepriteTag"/>
    /// </summary>
    /// <remarks>
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Value</term>
    ///             <description>Meaning</description>
    ///         </listheader>
    ///         <item>
    ///             <term>0</term>
    ///             <description>Infinite</description>
    ///         </item>
    ///         <item>
    ///             <term>1</term>
    ///             <description>Plays once (for ping-pong, it plays just in one direction)</description>
    ///         </item>
    ///         <item>
    ///             <term>2</term>
    ///             <description>Plays twice (for ping-pong, it plays once in one direction, and once in reverse)</description>
    ///         </item>
    ///         <item>
    ///             <term>N</term>
    ///             <description>Plays N times</description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public int Repeat { get; }

    /// <summary>
    ///     Gets the name assigned to this <see cref="AsepriteTag"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="AsepriteUserData"/> set for this <see cref="AsepriteTag"/>.
    /// </summary>
    public AsepriteUserData UserData { get; } = new();

    /// <summary>
    ///     Gets the color assigned to this <see cref="AsepriteTag"/>.
    /// </summary>
    public Color Color => UserData.Color ?? _tagColor;

    internal AsepriteTag(ushort from, ushort to, AsepriteLoopDirection direction, ushort repeat, Color color, string name) =>
        (From, To, Direction, Repeat, _tagColor, Name) = (from, to, direction, repeat, color, name);
}
