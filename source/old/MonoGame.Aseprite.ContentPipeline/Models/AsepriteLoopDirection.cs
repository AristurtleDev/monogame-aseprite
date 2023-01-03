/* ------------------------------------------------------------------------------
    Copyright (c) 2022 Christopher Whitley

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
------------------------------------------------------------------------------ */

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values that describe the direction of an Aseprite
    ///     animation tag.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Aseprite Loop Direction documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#tags-chunk-0x2018">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public enum AsepriteLoopDirection
    {
        /// <summary>
        ///     Describes an animation tag that plays in a forward direction from
        ///     the starting frame to the ending frame.
        /// </summary>
        Forward = 0,

        /// <summary>
        ///     Describes an animation tag that plays in a reverse direction from
        ///     the ending frame to the starting frame.
        /// </summary>
        Reverse = 1,

        /// <summary>
        ///     Describes an animation tag that plays first in a forward direction from
        ///     the starting frame to the ending frame, then plays in a reverse direction
        ///     from the ending frame to the starting frame.
        /// </summary>
        PingPong = 2
    }
}
