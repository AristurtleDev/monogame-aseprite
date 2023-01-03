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

namespace MonoGame.Aseprite.ContentPipeline.Processors
{
    /// <summary>
    ///     Defines the values for the type of spritesheet to
    ///     create when processing an Aseprite file.
    /// </summary>
    public enum ProcessorSheetType
    {
        /// <summary>
        ///     Describes a horizontal spritesheet with only 1 column
        ///     and as many rows as there are frames.
        /// </summary>
        HorizontalStrip = 0,

        /// <summary>
        ///     Describes a vertical spritesheet with only 1 row and
        ///     as many columns as there are frames.
        /// </summary>
        VerticalStrip = 1,

        /// <summary>
        ///     Descrbies a packed spritesheet.
        /// </summary>
        Packed = 2
    }
}
