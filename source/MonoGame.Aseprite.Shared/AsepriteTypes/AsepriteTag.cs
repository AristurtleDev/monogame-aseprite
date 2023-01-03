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
///     Represents an animation tag in an Aseprite image.
/// </summary>
public sealed class AsepriteTag
{
    private Color _tagColor;

    /// <summary>
    ///     The starting index of the frame for the animation of this tag.
    /// </summary>
    public int From { get; }

    /// <summary>
    ///     The ending index of the frame for the animation of this tag.
    /// </summary>
    public int To { get; }

    /// <summary>
    ///     The animation loop direction of the animation of this tag.
    /// </summary>
    public LoopDirection Direction { get; }

    /// <summary>
    ///     The name of this tag.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The custom user data that was set for this tag in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new();

    /// <summary>
    ///     The color value of this tag.
    /// </summary>
    public Color Color => UserData.Color ?? _tagColor;

    internal AsepriteTag(ushort from, ushort to, LoopDirection direction, Color color, string name)
    {
        From = from;
        To = to;
        Direction = direction;
        _tagColor = color;
        Name = name;
    }
}
