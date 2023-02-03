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
/// Defines an animation tag in aseprite.
/// </summary>
public sealed class AsepriteTag
{
    private Color _tagColor;

    /// <summary>
    /// Gets the index of the first frame of animation defined by this tag.
    /// </summary>
    public int From { get; }

    /// <summary>
    /// Gets the index of the last frame of animation defined by this tag.
    /// </summary>
    public int To { get; }

    /// <summary>
    /// Gets the loop direction of the animation defined by this tag.
    /// </summary>
    public AsepriteLoopDirection Direction { get; }

    /// <summary>
    /// Gets the name of this tag.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the custom userdata set for this tag in aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new();

    /// <summary>
    /// Gets the color of this tag.
    /// </summary>
    public Color Color => UserData.Color ?? _tagColor;

    internal AsepriteTag(ushort from, ushort to, AsepriteLoopDirection direction, Color color, string name) =>
        (From, To, Direction, _tagColor, Name) = (from, to, direction, color, name);
}
