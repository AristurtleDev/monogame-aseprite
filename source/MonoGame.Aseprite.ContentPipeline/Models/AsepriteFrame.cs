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

using System;
using System.Collections.Generic;
using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values found inside a Frame within in an Aseprite file.
    /// </summary>
    /// <remarks>
    ///     A frame within Aseprite is comprised of multiple chunks
    ///     <para>
    ///         Aseprite Frame documentation: 
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#frames">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepriteFrame
    {
        /// <summary>
        ///     Gets a reference to the <see cref="AsepriteDocument"/> this
        ///     frame belongs to.
        /// </summary>
        public AsepriteDocument File { get; private set; }

        /// <summary>
        ///     Gets the duration, in milliseconds, of this frame.
        /// </summary>
        public int Duration { get; private set; }

        /// <summary>
        ///     Gets the collection of <see cref="AsepriteCelChunk"/> instances
        ///     that make up the image for this frame.
        /// </summary>
        public List<AsepriteCelChunk> Cels { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteFrame"/> instance.
        /// </summary>
        /// <param name="file">
        ///     The <see cref="AsepriteDocument"/> instance this frame belongs to.
        /// </param>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        internal AsepriteFrame(AsepriteDocument file, AsepriteReader reader)
        {
            Cels = new List<AsepriteCelChunk>();

            File = file;

            //  Ignore the total bytes in frame value, we don't need it
            _ = reader.ReadDWORD();

            //  Read and validate the magic number
            if (reader.ReadWORD() != 0xF1FA)
            {
                throw new Exception($"Invalid frame header, please ensure the .ase/.aseprite file is valid");
            }

            //  The next field contains the chunk count, but this is the old chunk count field
            int oldChunkCount = reader.ReadWORD();

            //  Aseprite stores the frame duration value as milliseconds
            Duration = reader.ReadWORD();

            //  Per ase file spec, the next two bytes are reserved for future use
            reader.Ignore(2);

            //  This field contains the chunk count, but is the new chunk count field.
            uint newChunkCount = reader.ReadDWORD();

            //  Per ase file spec, if the new chunk count filed is 0, then we use the old field
            //  value instad.
            int totalChunks = newChunkCount == 0 ? oldChunkCount : (int)newChunkCount;

            //  A value indicating if the new palette chunk was found. When it is found
            //  then we can skip reading the old palette chunk.
            bool newPaletteChunkFound = false;
            for (int i = 0; i < totalChunks; i++)
            {
                long chunkStart = reader.BaseStream.Position;
                uint chunkSize = reader.ReadDWORD();
                long chunkEnd = chunkStart + chunkSize;

                AsepriteChunkType chunkType = (AsepriteChunkType)reader.ReadWORD();
                System.Diagnostics.Debug.WriteLine($"Chunk Type: {chunkType}");

                switch (chunkType)
                {
                    case AsepriteChunkType.Layer:
                        ReadLayerChunk(reader);
                        break;
                    case AsepriteChunkType.Cel:
                        ReadCelChunk(reader, (int)chunkSize - 6);
                        break;
                    case AsepriteChunkType.Tags:
                        ReadTagChunk(reader);
                        break;
                    case AsepriteChunkType.OldPaletteA:
                        if (newPaletteChunkFound)
                        {
                            reader.BaseStream.Position = chunkEnd;
                        }
                        else
                        {
                            ReadOldPalleteAChunk(reader);
                        }
                        break;
                    case AsepriteChunkType.Palette:
                        ReadPaletteChunk(reader);
                        newPaletteChunkFound = true;
                        break;
                    case AsepriteChunkType.UserData:
                        ReadUserDataChunk(reader);
                        break;
                    case AsepriteChunkType.Slice:
                        ReadSliceChunk(reader);
                        break;
                    case AsepriteChunkType.OldPaletteB:     //  Ignore
                    case AsepriteChunkType.CelExtra:        //  Ignore
                    case AsepriteChunkType.ColorProfile:    //  Ignore
                    case AsepriteChunkType.Mask:            //  Ignore
                    case AsepriteChunkType.Path:            //  Ignore
                        //  Since we are ignoreing these chunk types, we need to ensure that the
                        //  reader's basestream position is set to where the end of the ignored 
                        //  chunk would be.
                        reader.BaseStream.Position = chunkEnd;
                        break;
                    default:
                        throw new Exception("Invalid chunk type detected");
                }
            }
        }

        /// <summary>
        ///     Reads a <see cref="AsepriteLayerChunk"/> from the underlying
        ///     stream of the provided <see cref="AsepriteReader"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        private void ReadLayerChunk(AsepriteReader reader)
        {
            AsepriteLayerChunk layer = new AsepriteLayerChunk(reader);
            File.Layers.Add(layer);
            reader.LastChunkRead = layer;
        }

        /// <summary>
        ///     Reads a <see cref="AsepriteCelChunk"/> from the underlying stream
        ///     of the provided <see cref="AsepriteReader"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        /// <param name="dataSize">
        ///     The size of the cel chunks data, in bytes. 
        /// </param>
        private void ReadCelChunk(AsepriteReader reader, int dataSize)
        {
            AsepriteCelChunk cel = new AsepriteCelChunk(reader, this, dataSize);
            Cels.Add(cel);
            reader.LastChunkRead = cel;

        }

        /// <summary>
        ///     Reads a <see cref="AsepriteTagChunk"/> from the underlying stream
        ///     of the provided <see cref="AsepriteReader"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        private void ReadTagChunk(AsepriteReader reader)
        {
            ushort tagCount = reader.ReadWORD();

            //  Per ase file spec, ignore the next eight bytes, they are reserved for future use.
            reader.Ignore(8);

            for (int i = 0; i < tagCount; i++)
            {
                AsepriteTagChunk tag = new AsepriteTagChunk(reader);
                File.Tags.Add(tag);
            }

        }

        /// <summary>
        ///     Reads a <see cref="AsepritePaletteChunk"/> from the underlying stream
        ///     of the provided <see cref="AsepriteReader"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        private void ReadPaletteChunk(AsepriteReader reader)
        {
            AsepritePaletteChunk palette = new AsepritePaletteChunk(reader, true);
            File.Palette = palette;
            reader.LastChunkRead = palette;
        }

        private void ReadOldPalleteAChunk(AsepriteReader reader)
        {
            AsepritePaletteChunk palette = new AsepritePaletteChunk(reader, false);
            File.Palette = palette;
            reader.LastChunkRead = palette;
        }

        /// <summary>
        ///     Reads the <see cref="IAsepriteUserData"/> from the underlying stream
        ///     of the provided <see cref="AsepriteReader"/> instance and applies
        ///     it to the most reacently read <see cref="AsepriteChunk"/>
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        private void ReadUserDataChunk(AsepriteReader reader)
        {
            reader.LastChunkRead.ReadUserData(reader);
        }

        /// <summary>
        ///     Reads a <see cref="AsepriteSliceChunk"/> from the underlying stream
        ///     of the provided <see cref="AsepriteReader"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        private void ReadSliceChunk(AsepriteReader reader)
        {
            AsepriteSliceChunk slice = new AsepriteSliceChunk(reader);
            File.Slices.Add(slice);
            reader.LastChunkRead = slice;
        }
    }
}
