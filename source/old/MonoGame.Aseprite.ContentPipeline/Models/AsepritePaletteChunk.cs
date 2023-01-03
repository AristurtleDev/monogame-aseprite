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

using System.Collections.Generic;
using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values found inside a Palette chunk in an Aseprite file.
    /// </summary>
    /// <remarks>
    ///     A palette chunk contains the data for each color in the Aseprite file's
    ///     palette.
    ///     <para>
    ///         Aseprite Palette Chunk documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#palette-chunk-0x2019">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepritePaletteChunk : AsepriteChunk
    {
        /// <summary>
        ///     Gets the <see cref= "AsepritePaletteColor" /> instances that make up
        ///     this palette.
        /// </summary>
        public AsepritePaletteColor[] Colors { get; private set; }

        /// <summary>
        ///     Creates a new <see cref = "AsepritePaletteChunk" /> instance.
        /// </ summary >
        /// < param name="reader">
        ///     The<see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        internal AsepritePaletteChunk(AsepriteReader reader, bool isNewPalette)
        {
            if (isNewPalette)
            {
                ReadAsNewPalette(reader);
            }
            else
            {
                ReadAsOldPalleteA(reader);
            }

        }

        private void ReadAsOldPalleteA(AsepriteReader reader)
        {
            //  Read the number of packets
            ushort numberOfPackets = reader.ReadWORD();

            List<AsepritePaletteColor> colors = new List<AsepritePaletteColor>();

            for (int i = 0; i < numberOfPackets; i++)
            {
                //  Read the number of palette entries to skip from the last packet
                //  But we're going to ignore the value since we don't need it.
                _ = reader.ReadByte();

                //  Read the number of colors in the packet
                int numberOfColors = reader.ReadByte();

                if (numberOfColors == 0)
                {
                    numberOfColors = 256;
                }

                for (int c = 0; c < numberOfColors; c++)
                {
                    colors.Add(new AsepritePaletteColor(reader, false));
                }
            }

            Colors = colors.ToArray();

        }

        private void ReadAsNewPalette(AsepriteReader reader)
        {
            //  Read the size value
            int size = (int)reader.ReadDWORD();

            //  Read the first index value.
            int firstIndex = (int)reader.ReadDWORD();

            //  Read the last index value.
            int lastIndex = (int)reader.ReadDWORD();

            //  Per ase file spec, ignore next 8 bytes, they are reserved for future use
            reader.Ignore(8);

            Colors = new AsepritePaletteColor[size];
            for (int i = 0; i < lastIndex - firstIndex + 1; i++)
            {

                Colors[i] = new AsepritePaletteColor(reader, true);
            }
        }
    }
}
