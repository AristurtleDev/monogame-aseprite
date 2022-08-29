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

using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.Graphics
{
    /// <summary>
    ///     A frame is the rectangluar area within the spritesheet that describes a single
    ///     frame of an animation and the duration in which that frame should be rendered.
    /// </summary>
    public struct Frame
    {
        /// <summary>
        ///     Defines the rectanglular bounds within the spritesheet that should be.
        ///     rendered for for this frame.
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        ///     The amount of time, in seconds, this frame should be displayed.
        /// </summary>
        public float Duration;

        /// <summary>
        ///     Creates a new <see cref="Frame"/> instance.
        /// </summary>
        /// <param name="x">
        ///     The top-left x-coordinate position of the frame relative to the spritesheet.
        /// </param>a
        /// <param name="y">
        ///     The top-left y-coordinate position of the frame relative to the spritesheet.
        /// </param>
        /// <param name="width">
        ///     The width, in pixels, of the frame.
        /// </param>
        /// <param name="height">
        ///     The height, in pixels, of the frame.
        /// </param>
        /// <param name="duration">
        ///     The amount of time, in seconds, the frame should be displayed.
        /// </param>
        public Frame(int x, int y, int width, int height, float duration)
        {
            Bounds = new Rectangle(x, y, width, height);
            Duration = duration;
        }

        /// <summary>
        ///     Creates a new <see cref="Frame"/> structure
        /// </summary>
        /// <param name="bounds">
        ///     A <see cref="Rectangle"/> instance that describes the bounds of the frame relative
        ///     to the spritesheet.
        /// </param>
        /// <param name="duration">
        ///     The amount of time, in seconds, the frame should be displayed.
        /// </param>
        public Frame(Rectangle bounds, float duration)
        {
            Bounds = bounds;
            Duration = duration;
        }
    }
}
