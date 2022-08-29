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
    ///     Represents the frame specific boundry infromation for a slice.
    /// </summary>
    public struct SliceKey
    {
        /// <summary>
        ///     The index of the frame that this slicekey is valid for.
        /// </summary>
        public int Frame;

        /// <summary>
        ///     A rectangle that describes the bounds of this slicekey.
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        ///     The color of this slicekey.
        /// </summary>
        public Color Color;

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">
        ///     The index of the frame that this slicekey is valid for.
        /// </param>
        /// <param name="bounds">
        ///     A rectangle that describes the bounds of this slicekey.
        /// </param>
        public SliceKey(int frame, Rectangle bounds)
            : this(frame, bounds.X, bounds.Y, bounds.Width, bounds.Height, Color.White) { }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">
        ///     The index of the frame that this slicekey is valid for.
        /// </param>
        /// <param name="bounds">
        ///     A rectangle that describes the bounds of this slicekey.
        /// </param>
        /// <param name="color">
        ///     The color of this slicekey.
        /// </param>
        public SliceKey(int frame, Rectangle bounds, Color color)
            : this(frame, bounds.X, bounds.Y, bounds.Width, bounds.Height, color) { }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">
        ///     The index of the frame that this slicekey is valid for.
        /// </param>
        /// <param name="location">
        ///     A Point instance that descrbies the top-left xy-coordinate position of the bounds
        ///     of this slicekey.
        /// </param>
        /// <param name="size">
        ///     A Point instance that descrbies the width and height, in pixels, of the bounds
        ///     of the slicekey.
        /// </param>
        public SliceKey(int frame, Point location, Point size)
            : this(frame, location.X, location.Y, size.X, size.Y, Color.White) { }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">
        ///     The index of the frame that this slicekey is valid for.
        /// </param>
        /// <param name="location">
        ///     A Point instance that descrbies the top-left xy-coordinate position of the bounds
        ///     of this slicekey.
        /// </param>
        /// <param name="size">
        ///     A Point instance that descrbies the width and height, in pixels, of the bounds
        ///     of the slicekey.
        /// </param>
        /// <param name="color">
        ///     The color of this slicekey.
        /// </param>
        public SliceKey(int frame, Point location, Point size, Color color)
            : this(frame, location.X, location.Y, size.X, size.Y, color) { }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">
        ///     The index of the frame that this slicekey is valid for.
        /// </param>
        /// <param name="x">
        ///     The top-left x-coordinate position of hte bounds of this slicekey.
        /// </param>
        /// <param name="y">
        ///     The top-left y-coordinate position of the bounds of this slicekey.
        /// </param>
        /// <param name="width">
        ///     The width, in pixels, of the bounds of this slicekey.
        /// </param>
        /// <param name="height">
        ///     The height, in pixels, of the bounds of this slicekey.
        /// </param>
        public SliceKey(int frame, int x, int y, int width, int height)
            : this(frame, x, y, width, height, Color.White) { }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">
        ///     The index of the frame that this slicekey is valid for.
        /// </param>
        /// <param name="x">
        ///     The top-left x-coordinate position of hte bounds of this slicekey.
        /// </param>
        /// <param name="y">
        ///     The top-left y-coordinate position of the bounds of this slicekey.
        /// </param>
        /// <param name="width">
        ///     The width, in pixels, of the bounds of this slicekey.
        /// </param>
        /// <param name="height">
        ///     The height, in pixels, of the bounds of this slicekey.
        /// </param>
        /// <param name="color">
        ///     The color of this slicekey.
        /// </param>
        public SliceKey(int frame, int x, int y, int width, int height, Color color)
        {
            Frame = frame;
            Bounds = new Rectangle(x, y, width, height);
            Color = color;
        }
    }
}
