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

using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Documents;

namespace MonoGame.Aseprite.ContentReaders
{
    /// <summary>
    ///     ContentTypeReader used to import a new <see cref="AsepriteDocument"/>
    ///     instance from the content pipeline.
    /// </summary>
    public class AsepriteDocumentTypeReader : ContentTypeReader<AsepriteDocument>
    {
        /// <summary>
        ///     Given a valid byte[] that represents the contents of the aserptie file
        ///     import from MonoGame.Aseprite.ContentPipeline, reads the content and
        ///     returns a new <see cref="AsepriteDocument"/> instance.
        /// </summary>
        /// <param name="input">
        ///     The binary file contents of the aseprite file import created by
        ///     the MonoGame.Aseprite.Contentpipline.
        /// </param>
        /// <param name="graphicsDevice">
        ///     The GraphicsDevice instance used for rendering.
        /// </param>
        /// <returns>
        ///     A new <see cref="AsepriteDocument"/> instance.
        /// </returns>
        public AsepriteDocument Read(byte[] input, GraphicsDevice graphicsDevice)
        {
            AsepriteDocument result;

            using (MemoryStream stream = new MemoryStream(input))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    result = ReadInternal(reader, graphicsDevice, null);
                }
            }

            return result;
        }

        /// <summary>
        ///     Allows the ContentPipline to read a new <see cref="AsepriteDocument"/>.
        /// </summary>
        /// <param name="input">
        ///     The ContentReader instance provided by the ContentManager.
        /// </param>
        /// <param name="existingInstance">
        ///     An existing instnace of the content being read.
        /// </param>
        /// <returns>
        ///     A new <see cref="AsepriteDocument"/> instance.
        /// </returns>
        protected override AsepriteDocument Read(ContentReader input, AsepriteDocument existingInstance)
        {
            return ReadInternal(input, input.GetGraphicsDevice(), existingInstance);
        }

        /// <summary>
        ///     Reads the <see cref="AsepriteDocument"/> from the base stream
        ///     of the reader provided.
        /// </summary>
        /// <param name="input">
        ///     A BinaryReader instance who's base stream contains the content of the
        ///     <see cref="AsepriteDocument"/> being read.
        /// </param>
        /// <param name="graphicsDevice">
        ///     The GraphicsDevice instance used for rendering.
        /// </param>
        /// <param name="existingInstance">
        ///     An existing instnace of the content being read.
        /// </param>
        /// <returns>
        ///     A new <see cref="AsepriteAnimationDocument"/> instance.
        /// </returns>
        private AsepriteDocument ReadInternal(BinaryReader input, GraphicsDevice device, AsepriteDocument existingInstance)
        {
            if (existingInstance != null)
            {
                return existingInstance;
            }

            AsepriteDocument result = new AsepriteDocument
            {
                TextureWidth = input.ReadInt32(),
                TextureHeight = input.ReadInt32()
            };

            int totalPixels = input.ReadInt32();
            Color[] colorData = new Color[totalPixels];

            for (int i = 0; i < totalPixels; i++)
            {
                //  Adjustment for reading color due to using BinaryWriter insetad of ContentWriter.
                colorData[i] = new Color
                {
                    R = input.ReadByte(),
                    G = input.ReadByte(),
                    B = input.ReadByte(),
                    A = input.ReadByte()
                };
            }

            result.Texture = new Texture2D(device, result.TextureWidth, result.TextureHeight);
            result.Texture.SetData<Color>(colorData);

            int totalFrames = input.ReadInt32();
            for (int i = 0; i < totalFrames; i++)
            {
                AsepriteFrame frame = new AsepriteFrame
                {
                    X = input.ReadInt32(),
                    Y = input.ReadInt32(),
                    Width = input.ReadInt32(),
                    Height = input.ReadInt32(),
                    Duration = input.ReadInt32() / 1000.0f
                };

                result.Frames.Add(frame);
            }

            int totalTags = input.ReadInt32();
            for (int i = 0; i < totalTags; i++)
            {
                AsepriteTag tag = new AsepriteTag
                {
                    Name = input.ReadString(),
                    From = input.ReadInt32(),
                    To = input.ReadInt32(),
                    Direction = (AsepriteTagDirection)input.ReadInt32()
                };

                //  Adjustment for reading color due to using BinaryWriter insetad  of ContentWriter.
                tag.Color = new Color
                {
                    R = input.ReadByte(),
                    G = input.ReadByte(),
                    B = input.ReadByte(),
                    A = input.ReadByte()
                };

                result.Tags.Add(tag.Name, tag);
            }

            int totalSlices = input.ReadInt32();
            for (int i = 0; i < totalSlices; i++)
            {
                AsepriteSlice slice = new AsepriteSlice()
                {
                    Name = input.ReadString()
                };

                //  Adjustment for reading color due to using BinaryWriter insetad of ContentWriter.
                slice.Color = new Color
                {
                    R = input.ReadByte(),
                    G = input.ReadByte(),
                    B = input.ReadByte(),
                    A = input.ReadByte()
                };

                int totalSliceKeys = input.ReadInt32();
                for (int k = 0; k < totalSliceKeys; k++)
                {
                    AsepriteSliceKey sliceKey = new AsepriteSliceKey
                    {
                        FrameIndex = input.ReadInt32(),
                        X = input.ReadInt32(),
                        Y = input.ReadInt32(),
                        Width = input.ReadInt32(),
                        Height = input.ReadInt32(),
                        HasNinePatch = input.ReadBoolean()
                    };

                    sliceKey.CenterX = sliceKey.HasNinePatch ? input.ReadInt32() : 0;
                    sliceKey.CenterY = sliceKey.HasNinePatch ? input.ReadInt32() : 0;
                    sliceKey.CenterWidth = sliceKey.HasNinePatch ? input.ReadInt32() : 0;
                    sliceKey.CenterHeight = sliceKey.HasNinePatch ? input.ReadInt32() : 0;
                    sliceKey.HasPivot = input.ReadBoolean();
                    sliceKey.PivotX = sliceKey.HasPivot ? input.ReadInt32() : 0;
                    sliceKey.PivotY = sliceKey.HasPivot ? input.ReadInt32() : 0;

                    slice.SliceKeys.Add(sliceKey.FrameIndex, sliceKey);
                }

                result.Slices.Add(slice.Name, slice);
            }

            return result;
        }
    }
}
