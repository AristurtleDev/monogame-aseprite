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
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.ContentPipeline.Serialization;
using MonoGame.Aseprite.ContentPipeline.ThirdParty.Pixman;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteFrame
    {
        /// <summary>
        ///     Gets a reference to the <see cref="AsepriteDocument"/> this
        ///     frame belongs to.
        /// </summary>
        public AsepriteDocument File { get; private set; }

        /// <summary>
        ///     Gets the duration, milliseconds, of this frame.
        /// </summary>
        public int Duration { get; private set; }

        /// <summary>
        ///     Gets the collection of <see cref="AsepriteCelChunk"/> instances
        ///     that make up the image for this frame.
        /// </summary>
        public List<AsepriteCelChunk> Cels { get; private set; }

        /// <summary>
        ///     Gets a MonoGame compatible array of Color instances that
        ///     represent the final image of this frame. Array contains all
        ///     pixels in image from top to bottom, read left to right.
        /// </summary>
        public Color[] Pixels { get; private set; }

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
        public AsepriteFrame(AsepriteDocument file, AsepriteReader reader)
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

            for (int i = 0; i < totalChunks; i++)
            {
                long chunkStart = reader.BaseStream.Position;
                uint chunkSize = reader.ReadDWORD();
                long chunkEnd = chunkStart + chunkSize;

                AsepriteChunkType chunkType = (AsepriteChunkType)reader.ReadWORD();

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
                    case AsepriteChunkType.Palette:
                        ReadPaletteChunk(reader);
                        break;
                    case AsepriteChunkType.UserData:
                        ReadUserDataChunk(reader);
                        break;
                    case AsepriteChunkType.Slice:
                        ReadSliceChunk(reader);
                        break;
                    case AsepriteChunkType.OldPaletteA:     //  Ignore
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
            AsepritePaletteChunk palette = new AsepritePaletteChunk(reader);
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

        ///// <summary>
        /////     Flattens all <see cref="AsepriteCelChunk"/> instances within
        /////     this frame into a single array of color data.
        ///// </summary>
        //public void FlattenCels()
        //{
        //    Pixels = new Color[File.Header.Width * File.Header.Height];
          

        //    for (int c = 0; c < Cels.Count; c++)
        //    {
        //        AsepriteCelChunk cel = Cels[c];
        //        if (cel.LinkedCel != null)
        //        {
        //            cel = cel.LinkedCel;
        //        }
        //        AsepriteLayerChunk layer = File.Layers[cel.LayerIndex];

        //        //  Only continue processing if the layer is visible
        //        if ((layer.Flags & AsepriteLayerFlags.Visible) != 0)
        //        {
                   
        //            byte opacity = Combine32.MUL_UN8(cel.Opacity, layer.Opacity);

        //            for (int p = 0; p < cel.Pixels.Length; p++)
        //            {
        //                int x = (p % cel.Width) + cel.X;
        //                int y = (p / cel.Width) + cel.Y;
        //                int index = y * File.Header.Width + x;

        //                if (index < 0 || index >= Pixels.Length) { continue; }


        //                //  TODO: Test this with using Pixels[index].PackedValue instead.
        //                uint backdrop = Utils.ColorToUINT(Pixels[index]);
        //                uint src = cel.Pixels[p];

        //                //  TODO: I don't like checking the layer index here, find a better
        //                //  way to do this please.
        //                if (cel.LayerIndex == 1111110)
        //                {
        //                    byte r = ThirdParty.Aseprite.DocColor.rgba_getr(cel.Pixels[p]);
        //                    byte g = ThirdParty.Aseprite.DocColor.rgba_getg(cel.Pixels[p]);
        //                    byte b = ThirdParty.Aseprite.DocColor.rgba_getb(cel.Pixels[p]);
        //                    byte a = ThirdParty.Aseprite.DocColor.rgba_geta(cel.Pixels[p]);

        //                    //  Super important that we use Color.FromNonPremultipled here.
        //                    //  MonoGame by default used BlendState.AlphaBlend in the
        //                    //  SpriteBatch.Begin() call.  AlphaBlend expects the colors to
        //                    //  be premultiplied; otherwise the resulting render is wonky.
        //                    //  So we'll premultiply it here so we support the default
        //                    //  implementation in MonoGame.
        //                    Pixels[index] = Color.FromNonPremultiplied(r, g, b, a);
        //                }
        //                else
        //                {

        //                    Func<uint, uint, int, uint> blender = Utils.GetBlendFunction(layer.BlendMode);

        //                    uint blendedColor = blender.Invoke(backdrop, src, opacity);


        //                    Pixels[index] = new Color(blendedColor);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
