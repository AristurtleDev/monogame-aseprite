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

namespace MonoGame.Aseprite.Documents
{
    /// <summary>
    ///     A class that provides the bounds an duration of a single
    ///     frame for an <see cref="AsepriteDocument"/>.
    /// </summary>
    public sealed class AsepriteFrame
    {
        /// <summary>
        ///     Gets the top-left x-coordinate position of the frame relative
        ///     to the top-left of the texture.
        /// </summary>
        public int X { get; internal set; }

        /// <summary>
        ///     Gets the top-left y-coordinate position of the frame relative
        ///     to the top-left of the texture.
        /// </summary>
        public int Y { get; internal set; }

        /// <summary>
        ///     Gets the width, in pixels, of the frame.
        /// </summary>
        public int Width { get; internal set; }

        /// <summary>
        ///     Gets the height, in pixels, of the frame.
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        ///     Gets the duration, in seconds, of the frame.
        /// </summary>
        public float Duration { get; internal set; }

        /// <summary>
        ///     Creates a <see cref="AsepriteFrame"/>
        /// </summary>
        internal AsepriteFrame() { }
    }
}
