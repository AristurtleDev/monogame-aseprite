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
    ///     Provides values that describe the color depth mode
    ///     used within the Aseprite file.
    /// </summary>
    /// <remarks>
    ///     The actual values used in the Aseprite file are 8, 16, and 32
    ///     for "bits" per pixel.  However we use 1, 2, and 4 "bytes" per pixel instead.
    ///     <para>
    ///         Aseprite Color Depth Mode Values documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#header">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public enum AsepriteColorDepth
    {
        /// <summary>
        ///     Describes an Indexed color depth mode of 1 byte
        ///     per pixel (or 8 bits per pixel).
        /// </summary>
        Indexed = 1,

        /// <summary>
        ///     Describes a Grayscale color dpeth mode of 2 bytes
        ///     per pixel (or 16 bits per pixel).
        /// </summary>
        Grayscale = 2,

        /// <summary>
        ///     Descrbies a RGBA color depth mode of 4 bytes per
        ///     pixel (or 32 bits per pixel).
        /// </summary>
        RGBA = 4
    }
}
