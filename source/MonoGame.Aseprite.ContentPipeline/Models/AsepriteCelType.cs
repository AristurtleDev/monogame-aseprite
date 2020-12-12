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

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provieds the values for the type of cel
    /// </summary>
    /// <remarks>
    ///     The Cel type descrbies how the pixel data within the cel chunk is provided.
    ///     <para>
    ///         Aseprite Cel Type Values documentation: 
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#cel-chunk-0x2005">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public enum AsepriteCelType
    {
        /// <summary>
        ///     Cel contains raw pixel data.
        /// </summary>
        Raw = 0,

        /// <summary>
        ///     Cel is linked to another cel and the linked cel's data
        ///     should be used instead.
        /// </summary>
        Linked = 1,

        /// <summary>
        ///     Cel contains compressed data and needs to be decompressed
        ///     before reading it.
        /// </summary>
        Compressed = 2
    }
}
