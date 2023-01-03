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
using System.IO;
using System.IO.Compression;
using System.Linq;
using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values found inside a Cel chunk in an Aseprite file.
    /// </summary>
    /// <remarks>
    ///     A Cel in Aseprite contains the pixel data for a single layer within a single frame.
    ///     <para>
    ///         Aseprite Cel Chunk documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#cel-chunk-0x2005">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepriteCelChunk : AsepriteChunk
    {
        /// <summary>
        ///     Gets the index of the <see cref="AsepriteLayerChunk"/> that this
        ///     cel belongs to.
        /// </summary>
        public int LayerIndex { get; private set; }

        /// <summary>
        ///     Gets the top-left x-coordinate position of this cel within the
        ///     frame.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        ///     Gets the top-left y-coordinate position of this cel within the
        ///     frame.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        ///     Gets the level of opacity of this cel.
        /// </summary>
        public byte Opacity { get; private set; }

        /// <summary>
        ///     Gets a value that indicates what type of cel this is.
        ///     (Raw, Compressed, or Linked)
        /// </summary>
        public AsepriteCelType CelType { get; private set; }

        /// <summary>
        ///     Gets the <see cref="AsepriteCelChunk"/> this one is linked
        ///     to if marked as linked in the Aspeirte file.
        /// </summary>
        public AsepriteCelChunk LinkedCel { get; private set; }

        /// <summary>
        ///     Gets the width, in pixels, of this cel.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     Gets the height, in pixels, of this cel.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Gets the raw pixel data contained in this cel.
        /// </summary>
        public byte[] PixelData { get; private set; }

        /// <summary>
        ///     Gets an array of packed uint values for this cel that
        ///     represents the pixels.
        /// </summary>
        public uint[] Pixels { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteCelChunk"/> instance.
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        /// <param name="frame">
        ///     The <see cref="AsepriteFrame"/> this cel is contained within.
        /// </param>
        /// <param name="dataSize">
        ///     The total byte size of the data for this cel chunk.
        /// </param>
        internal AsepriteCelChunk(AsepriteReader reader, AsepriteFrame frame, int dataSize)
        {
            //  We need to cache the position of the reader before reading any data so we can
            //  calculate the amount of data to read later for the pixel info.
            long readerPos = reader.BaseStream.Position;

            LayerIndex = reader.ReadWORD();
            X = reader.ReadSHORT();
            Y = reader.ReadSHORT();
            Opacity = reader.ReadByte();
            CelType = (AsepriteCelType)reader.ReadWORD();

            //  Per ase file spec, ignore next 7 bytes, they are reserved for future use.
            reader.Ignore(7);

            if (CelType == AsepriteCelType.Raw || CelType == AsepriteCelType.Compressed)
            {
                Width = reader.ReadWORD();
                Height = reader.ReadWORD();

                //  Calculate the remaning data to read in the cel chunk
                long bytesToRead = dataSize - (reader.BaseStream.Position - readerPos);

                //  Read the remaning bytes into a buffer
                byte[] buffer = reader.ReadBytes((int)bytesToRead);

                if (CelType == AsepriteCelType.Raw)
                {
                    //  For raw cel, the buffer is the raw pixel data
                    PixelData = new byte[buffer.Length];
                    Buffer.BlockCopy(buffer, 0, PixelData, 0, buffer.Length);
                }
                else
                {
                    //  For compressed, we need to deflate the buffer. First, we'll put it in a
                    //  memory stream to work with
                    MemoryStream compressedStream = new MemoryStream(buffer);

                    //  The first 2 bytes of the compressed stream are the zlib header informaiton,
                    //  and we need to ignore them before we attempt to deflate
                    _ = compressedStream.ReadByte();
                    _ = compressedStream.ReadByte();

                    //  Now we can deflate the compressed stream
                    using (MemoryStream decompressedStream = new MemoryStream())
                    {
                        using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                        {
                            deflateStream.CopyTo(decompressedStream);
                            PixelData = decompressedStream.ToArray();
                        }
                    }
                }

                Pixels = new uint[Width * Height];
                if (frame.File.Header.ColorDepth == AsepriteColorDepth.RGBA)
                {
                    for (int i = 0, b = 0; i < Pixels.Length; i++, b += 4)
                    {
                        Pixels[i] = Utils.BytesToPacked(PixelData[b], PixelData[b + 1], PixelData[b + 2], PixelData[b + 3]);
                    }
                }
                else if(frame.File.Header.ColorDepth == AsepriteColorDepth.Grayscale)
                {
                    for(int i = 0, b = 0; i < Pixels.Length; i++, b += 2)
                    {
                        Pixels[i] = Utils.BytesToPacked(PixelData[b], PixelData[b], PixelData[b], PixelData[b + 1]);
                    }
                }
                else if(frame.File.Header.ColorDepth == AsepriteColorDepth.Indexed)
                {
                    for(int i = 0; i < Pixels.Length; i++)
                    {
                        int paletteIndex = PixelData[i];
                        if (paletteIndex == frame.File.Header.TransparentIndex)
                        {
                            Pixels[i] = Utils.BytesToPacked(0, 0, 0, 0);
                        }
                        else
                        {
                            AsepritePaletteColor paletteColor = frame.File.Palette.Colors[paletteIndex];
                            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(paletteColor.PackedValue);
                            Pixels[i] = Utils.BytesToPacked(paletteColor.Red, paletteColor.Green, paletteColor.Blue, paletteColor.Alpha);
                        }
                    }
                }
                else
                {
                    throw new Exception($"Unrecognized color depth mode. {frame.File.Header.ColorDepth}");
                }
            }
            else if (CelType == AsepriteCelType.Linked)
            {
                ushort linkedFrame = reader.ReadWORD();

                //  Get a refrence to the cel this cel is linked to.
                LinkedCel = frame.File.Frames[linkedFrame].Cels
                                                          .FirstOrDefault(c => c.LayerIndex == LayerIndex);
            }
        }
    }
}
