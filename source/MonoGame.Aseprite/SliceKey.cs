/* ------------------------------------------------------------------------------
    Copyright (c) 2020 Christopher Whitley

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

namespace MonoGame.Aseprite
{
    /// <summary>
    ///     Represnets the frame specific informaiton for a slice
    /// </summary>
    public struct SliceKey
    {
        /// <summary>
        ///     The frame this is for
        /// </summary>
        public int frame;

        /// <summary>
        ///     The <see cref="Rectangle"/> defintion of this
        /// </summary>
        public Rectangle bounds;

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">The frame this is for</param>
        /// <param name="bounds">The <see cref="Rectangle"/> definition of this</param>
        public SliceKey(int frame, Rectangle bounds)
        {
            this.frame = frame;
            this.bounds = bounds;
        }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">The frame this is for</param>
        /// <param name="location">A <see cref="Point"/> to describe the xy-coordinate position of the bounds</param>
        /// <param name="size">A <see cref="Point"/> to describe the width and height of the bounds</param>
        public SliceKey(int frame, Point location, Point size)
        {
            this.frame = frame;
            this.bounds = new Rectangle(location, size);
        }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">The frame this is for</param>
        /// <param name="x">The x-coordinate position of the bounds</param>
        /// <param name="y">The y-coordinate position of the bounds</param>
        /// <param name="width">The width of the bounds</param>
        /// <param name="height">The height of the bounds</param>
        public SliceKey(int frame, int x, int y, int width, int height)
        {
            this.frame = frame;
            this.bounds = new Rectangle(x, y, width, height);
        }
    }
}
