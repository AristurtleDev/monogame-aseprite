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

using System;
using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values found inside the Header of an Aseprite File.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Aseprite Header documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#header">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepriteHeader
    {
        private readonly AsepriteHeaderFlags _flags;

        /// <summary>
        ///     Gets the <see cref="AsepriteColorDepth"/> value that indicates
        ///     the color depth mode used.
        /// </summary>
        public AsepriteColorDepth ColorDepth { get; private set; }

        /// <summary>
        ///     Gets a value indicating if the <see cref="AsepriteLayerChunk"/>
        ///     instances will have a valid opacity value.
        /// </summary>
        public bool HasOpacity => (_flags & AsepriteHeaderFlags.HasOpacity) != 0;

        /// <summary>
        ///     Gets the palette entiry index which represents the transparent
        ///     color in all non-background layers. This value is only valid
        ///     when the <see cref="ColorDepth"/> is value is
        ///     <see cref="AsepriteColorDepth.Indexed"/>
        /// </summary>
        public byte TransparentIndex { get; private set; }

        /// <summary>
        ///     Gets the total number of frames contained within the
        ///     Aseprite file.
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        ///     Gets the width, in pixels, of the canvas.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     Gets the height, in pixels, of the canvas.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Gets the total number of colors.
        /// </summary>
        public int ColorCount { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteHeader"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        internal AsepriteHeader(AsepriteReader reader)
        {
            //  Get the basestream position of the reader
            long headerStart = reader.BaseStream.Position;

            //  Header is 128 bytes. Calcualte the base stream position that is
            //  at the end of the header
            long headerEnd = headerStart + 128;

            //  Read but ignore the file size
            _ = reader.ReadDWORD();

            //  Read and validate the magic number
            if (reader.ReadWORD() != 0xA5E0)
            {
                throw new Exception($"File given does not appear to be a valid .ase/.aseprite file!");
            }

            FrameCount = reader.ReadWORD();
            Width = reader.ReadWORD();
            Height = reader.ReadWORD();
            ColorDepth = (AsepriteColorDepth)(reader.ReadWORD() / 8);
            _flags = (AsepriteHeaderFlags)reader.ReadDWORD();

            //  Per ase file specs, the speed field is deprecated, so we'll
            //  ignroe it.
            _ = reader.ReadWORD();

            //  Per ase file specs, next two DWORDs are ignored.
            _ = reader.ReadDWORD();
            _ = reader.ReadDWORD();

            TransparentIndex = reader.ReadByte();

            //  Per ase file specs, next 3 bytes are ignored
            reader.Ignore(3);

            ColorCount = reader.ReadWORD();

            //  We're going to ignore the rest of the header as we don't need
            //  the information. For documentation though, the following is
            //  what we are skipping
            //
            //  Pixel Width
            //  Pixel Height
            //  X position of the grid
            //  Y position of the grid
            //  Grid width (zero if there is no grid, grid size is 16x16 on Aseprite default)
            //  Grid height (zero if there is no grid
            //  84 bytes which are reserved for future use.
            //
            //  Since we are skipping all this, we'll just set the reader's basestream to
            //  the end of the header position we calculated earlier
            reader.BaseStream.Position = headerEnd;
        }
    }
}
