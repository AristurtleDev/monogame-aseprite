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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
/// Defines the direction for an animation defined by a tag in aseprite
/// </summary>
public enum AsepriteLoopDirection : byte
{
    /// <summary>
    /// Defines that the animation for the tag is played in a forward direction from the first frame of animation to the
    /// last.
    /// </summary>
    Forward = 0,

    /// <summary>
    /// Defines that the animation for the tag is played in reversed from the last frame of animation to the first.
    /// </summary>
    Reverse = 1,

    /// <summary>
    /// Defines that the animation for the tag ping-pongs by first going from the first frame of animation to the last
    /// then playing in reverse from the last frame of animation to the first.
    /// </summary>
    PingPong = 2
}
