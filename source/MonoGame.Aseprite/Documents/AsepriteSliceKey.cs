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

namespace MonoGame.Aseprite.Documents
{
    public sealed class AsepriteSliceKey
    {
        /// <summary>
        ///     Gets the index of the frame that the slice is valid
        ///     starting on.
        /// </summary>
        public int FrameIndex { get; internal set; }

        /// <summary>
        ///     Gets the x-coordinate position of the slice relative to
        ///     the bounds of the frame.
        /// </summary>
        public int X { get; internal set; }

        /// <summary>
        ///     Gets the y-coordinate position of the slice relative to
        ///     the bounds of the frame.
        /// </summary>
        public int Y { get; internal set; }

        /// <summary>
        ///     Gets the width, in pixels, of the slice.
        /// </summary>
        public int Width { get; internal set; }

        /// <summary>
        ///     Gets the height, in pixels, of the slice.
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        ///     Gets a value indicating if this slicekey has nine patch
        ///     data.
        /// </summary>
        public bool HasNinePatch { get; internal set; }

        /// <summary>
        ///     Gets the top-left x-coordinate position of the nine patch
        ///     center rect if this slice contains ninepatch data.
        /// </summary>
        public int CenterX { get; internal set; }

        /// <summary>
        ///     Gets the top-left y-coordinate position of the nine patch
        ///     center rect if this slice contains ninepatch data.
        /// </summary>
        public int CenterY { get; internal set; }

        /// <summary>
        ///     Gets the width, in pixels, of the nine patch center rect
        ///     if this slice contains ninepatch data.
        /// </summary>
        public int CenterWidth { get; internal set; }

        /// <summary>
        ///     Gets the height, in pixels, of the nine patch center rect
        ///     if this slice contains ninepatch data.
        /// </summary>
        public int CenterHeight { get; internal set; }

        /// <summary>
        ///     Gets a value indicating if this slicekey has pivot data.
        /// </summary>
        public bool HasPivot { get; internal set; }

        /// <summary>
        ///     Gets the x-coordinate origin point of the pivot if this slice
        ///     contains pivot data.
        /// </summary>
        public int PivotX { get; internal set; }

        /// <summary>
        ///     Gets the y-coordinate origin point of the pivot if this slice
        ///     contains pivot data.
        /// </summary>
        public int PivotY { get; internal set; }

        /// <summary>
        ///     Gets a <see cref="Rectangle"/> represntation of the boundry of the slice.
        /// </summary>
        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);

        /// <summary>
        ///     Creates a new <see cref="AsepriteSliceKey"/> instance.
        /// </summary>
        internal AsepriteSliceKey() { }
    }
}
