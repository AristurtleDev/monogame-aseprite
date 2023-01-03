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
    ///     Provides the values that describe the type of of a chunk.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Aseprite Chunk Types documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#chunk-types">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public enum AsepriteChunkType
    {
        /// <summary>
        ///     Describes an Old Palette A chunk type.  Chunks of this type
        ///     can be ignored if the <see cref="Palette"/> chunk is found.
        /// </summary>
        OldPaletteA = 0x0004,

        /// <summary>
        ///     Describes an Old Palette B chunk type.  Chunks of this type
        ///     can be ignored if the <see cref="Palette"/> chunk is found.
        /// </summary>
        OldPaletteB = 0x0011,

        /// <summary>
        ///     Descrbies a Layer chunk type.
        /// </summary>
        Layer = 0x2004,

        /// <summary>
        ///     Describes a Cel chunk type.
        /// </summary>
        Cel = 0x2005,

        /// <summary>
        ///     Describes a Cel Extra chunk type.
        /// </summary>
        CelExtra = 0x2006,

        /// <summary>
        ///     Describes a Color Profile chunk type.
        /// </summary>
        ColorProfile = 0x2007,

        /// <summary>
        ///     Describes a Mask chunk type.
        /// </summary>
        Mask = 0x2016,

        /// <summary>
        ///     Describes a Path chunk type.  This chunk type
        ///     is never used and should never be detected.
        /// </summary>
        Path = 0x2017,

        /// <summary>
        ///     Describes a Tag chunk type
        /// </summary>
        Tags = 0x2018,

        /// <summary>
        ///     Describes a Palette chunk type. If this chunk type is found
        ///     then <see cref="OldPaletteA"/> and/or <see cref="OldPaletteB"/>
        ///     chunk types can be ignored.
        /// </summary>
        Palette = 0x2019,

        /// <summary>
        ///     Describes a User Data chunk type.
        /// </summary>
        UserData = 0x2020,

        /// <summary>
        ///     Describes a Slice chunk type.
        /// </summary>
        Slice = 0x2022
    }
}
