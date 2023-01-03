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
namespace MonoGame.Aseprite;

/// <summary>
///     Defines the values that describe the loop direction for an animation
///     in an Aseprite image.
/// </summary>
public enum LoopDirection : byte
{
    /// <summary>
    ///     Describes that the animation loops in a forward direction staring on
    ///     the first frame and ending on the last frame.
    /// </summary>
    Forward = 0,

    /// <summary>
    ///     Describes that the animation loops in a reverse direction starting
    ///     on the last frame and ending on the first frame.
    /// </summary>
    Reverse = 1,

    /// <summary>
    ///     Describes that the animation loops in a ping-pong direction starting
    ///     on the first frame and moving forward to the last frame, then moving
    ///     back in reverse to the first frame.
    /// </summary>
    PingPong = 2
}
