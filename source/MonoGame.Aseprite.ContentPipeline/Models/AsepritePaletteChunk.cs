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
    public class AsepritePaletteChunk : AsepriteChunk
    {
        /// <summary>
        ///     Gets the<see cref= "AsepritePaletteColor" /> instances that make up
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
        public AsepritePaletteChunk(AsepriteReader reader)
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

                Colors[i] = new AsepritePaletteColor(reader);
            }
        }
    }
}
