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

namespace MonoGame.Aseprite.ContentPipeline.Processors
{
    /// <summary>
    ///     Defines the values of an Aseprite slice key that has been
    ///     processed and is ready to be written out.
    /// </summary>
    public struct ProcessedSliceKey
    {
        /// <summary>
        ///     The index of the frame that the slice is valid
        ///     starting on.
        /// </summary>
        public int FrameIndex;

        /// <summary>
        ///     The x-coordinate position of the slice relative to
        ///     the bounds of the frame.
        /// </summary>
        public int X;

        /// <summary>
        ///     The y-coordinate position of the slice relative to
        ///     the bounds of the frame.
        /// </summary>
        public int Y;

        /// <summary>
        ///     The width, in pixels, of the slice.
        /// </summary>
        public int Width;

        /// <summary>
        ///     The height, in pixels, of the slice.
        /// </summary>
        public int Height;

        /// <summary>
        ///     A value indicating if this slicekey has nine patch
        ///     data.
        /// </summary>
        public bool HasNinePatch;

        /// <summary>
        ///     The top-left x-coordinate position of the nine patch
        ///     center rect if this slice contains ninepatch data.
        /// </summary>
        public int CenterX;

        /// <summary>
        ///     The top-left y-coordinate position of the nine patch
        ///     center rect if this slice contains ninepatch data.
        /// </summary>
        public int CenterY;

        /// <summary>
        ///     The width, in pixels, of the nine patch center rect
        ///     if this slice contains ninepatch data.
        /// </summary>
        public int CenterWidth;

        /// <summary>
        ///     The height, in pixels, of the nine patch center rect
        ///     if this slice contains ninepatch data.
        /// </summary>
        public int CenterHeight;

        /// <summary>
        ///     Gets a value indicating if this slicekey has pivot data.
        /// </summary>
        public bool HasPivot;

        /// <summary>
        ///     The x-coordinate origin point of the pivot if this slice
        ///     contains pivot data.
        /// </summary>
        public int PivotX;

        /// <summary>
        ///     The y-coordinate origin point of the pivot if this slice
        ///     contains pivot data.
        /// </summary>
        public int PivotY;
    }
}
