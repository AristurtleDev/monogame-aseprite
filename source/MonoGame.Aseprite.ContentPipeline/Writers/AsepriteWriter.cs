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
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Aseprite.ContentPipeline.Processors.Animation;
using StbImageWriteSharp;

namespace MonoGame.Aseprite.ContentPipeline
{
    [ContentTypeWriter]
    public class AsepriteWriter : ContentTypeWriter<AnimationProcessorResult>
    {
        /// <summary>
        ///     Write method that can be used without the content pipeline.
        /// </summary>
        /// <param name="input">
        ///     The <see cref="AnimationProcessorResult"/> instance created as
        ///     part of the processing.
        /// </param>
        /// <returns>
        ///     A byte[] containing the binary contents of what would have been
        ///     written to file.
        /// </returns>
        public byte[] Write(AnimationProcessorResult input)
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    WriteInternal(writer, input);
                }

                buffer = stream.ToArray();
            }

            return buffer;
        }

        protected override void Write(ContentWriter output, AnimationProcessorResult input)
        {
            WriteInternal(output, input);
        }

        private void WriteInternal(BinaryWriter output, AnimationProcessorResult input)
        {
            // --------------------------------------------------------------
            //  Output Format
            //  [int]   Width of the texture.
            //  [int]   Height of the texture.
            //  [int]   Total number of pixels.
            //  +   For each pixel
            //      [color] Color of the pixel as an XNA Color instance.
            //
            //  [int]   Total number of frames
            //  +   For each frame
            //      [int]   Top-left x-coordinate position of frame in texture.
            //      [int]   Top-left y-coordinate position of frame in texture.
            //      [int]   Width, in pixels, of frame.
            //      [int]   Height, in pixels, of frame.
            //      [float] Duration of frame, in seconds.
            //
            //  [int]   Total number of animations
            //  +   For each animation
            //      [string]    Name of animation
            //      [int]       Index of the frame the animation starts on
            //      [int]       Index of the frame the animation ends on.
            //      [int]       The direction the animation plays in
            //                      0 = Forward
            //                      1 = Reverse
            //                      2 = Ping Pong
            //      [color] Color of the animation tag in Aseprtie as an XNA Color instance.
            //
            //  [int]   Total number of slices
            //  +   For each slice
            //      [string]    Name of the slice.
            //      [color]     Color of the slice as defined in Aseprite as an XNA Color instance.
            //      [int]       Total number of slice keys.
            //      +   For each slice key
            //          [int]   Index of the frame the key is valid starting on.
            //          [int]   Top-left x-coordinate position of the key relative to the frame.
            //          [int]   Top-left y-coordinate position of the key relative to the frame.
            //          [int]   Width of the key.
            //          [int]   Height of the key.
            //          [bool]  True if the key contains ninepatch data; otherwise, false.
            //          +   If the key contains ninepatch data
            //              [int]   Top-left x-coordinate position of the center rect relative to the key
            //              [int]   Top-left y-coordinate position of the center rect relative to the key
            //              [int]   Width of the center rect
            //              [int]   Height of the center rect
            //          [bool]  True if the key contains pivot data; otherwise, false.
            //          +   If the key contains pivot data
            //              [int]   x-coordinate origin position of the pivot.
            //              [int]   y-coordinate origin position of the pivot.
            //
            //
            //
            //
            //  [int]           Total number of animations.
            //  +   For each animation
            //      [string]    Animation name.
            //      [int]       Frame the animation starts on.
            //      [int]       Frame the animation ends on.
            //      [Color]     
            //  [int]           Number of frames
            //  +   For each frame
            //      [int]       The width, in pixels, of the frame.
            //      [int]       The height, in pixels, of the frame.
            //      [float]     The duration, in seconds, of the frame.
            //      [int]       Number of pixels
            //      +   For each pixel
            //          [color] XNA Color instance.
            //
            //  [int]           Number of tags
            //  +   For each tag
            //      [string]    The name of the tag
            //      [color]     The color of the tag
            //      [int]       The frame that the tag starts on
            //      [int]       The frame that the tag ends on
            //      [int]       The direction of the tag
            //                      0 = Forward
            //                      1 = Reverse
            //                      2 = Ping Pong
            //
            //  [int]           Number of slices
            //  +   For each slice
            //      [string]    The name of the slice
            //      [int]       Flags
            //                      1 = The slice has 9-patch data
            //                      2 = The slice has pivot data
            //      [color]     Color of the slice
            //      [int]       Total number of slice keys
            //      +   For each slice key
            //          [int]       The frame that the key starts on
            //          [int]       The X position of the key
            //          [int]       The Y position of the key
            //          [int]       The width of the key
            //          [int]       The height of the slikeyce
            //          +   If the slice has 9-patch data
            //              [int]   The x position of the center key relative to the slice x position
            //              [int]   The y position of the center key relative to the slice y positino
            //              [int]   The width of the center key rect
            //              [int]   The height of the center key rect
            //          +   If the slice has pivot data
            //              [int]   The x position of the pivot origin
            //              [int]   The y position of the pivot origin
            //      
            //  
            // --------------------------------------------------------------

            output.Write(input.TextureWidth);
            output.Write(input.TextureHeight);
            output.Write(input.ColorData.Length);

            for (int i = 0; i < input.ColorData.Length; i++)
            {
                //  Adjustment for writing color due to using BinaryWriter insetad
                //  of ContentWriter.
                output.Write(input.ColorData[i].R);
                output.Write(input.ColorData[i].G);
                output.Write(input.ColorData[i].B);
                output.Write(input.ColorData[i].A);
                //output.Write(input.ColorData[i]);
            }

            output.Write(input.Frames.Count);

            for (int i = 0; i < input.Frames.Count; i++)
            {
                output.Write(input.Frames[i].X);
                output.Write(input.Frames[i].Y);
                output.Write(input.Frames[i].Width);
                output.Write(input.Frames[i].Height);
                output.Write(input.Frames[i].Duration);
            }

            output.Write(input.Animations.Count);

            foreach (KeyValuePair<string, AnimationProcessorResult.Animation> kvp in input.Animations)
            {
                AnimationProcessorResult.Animation animation = kvp.Value;
                output.Write(animation.Name);
                output.Write(animation.From);
                output.Write(animation.To);
                output.Write(animation.Direction);

                //  Adjustment for writing color due to using BinaryWriter insetad
                //  of ContentWriter.
                output.Write(animation.Color.R);
                output.Write(animation.Color.G);
                output.Write(animation.Color.B);
                output.Write(animation.Color.A);
                //output.Write(animation.Color);
            }

            output.Write(input.Slices.Count);

            foreach (KeyValuePair<string, AnimationProcessorResult.Slice> kvp in input.Slices)
            {
                AnimationProcessorResult.Slice slice = kvp.Value;
                output.Write(slice.Name);

                //  Adjustment for writing color due to using BinaryWriter insetad
                //  of ContentWriter.
                output.Write(slice.Color.R);
                output.Write(slice.Color.G);
                output.Write(slice.Color.B);
                output.Write(slice.Color.A);
                //output.Write(slice.Color);

                output.Write(slice.SliceKeys.Count);

                foreach (KeyValuePair<int, AnimationProcessorResult.SliceKey> innerKVP in slice.SliceKeys)
                {
                    AnimationProcessorResult.SliceKey sliceKey = innerKVP.Value;
                    output.Write(sliceKey.FrameIndex);
                    output.Write(sliceKey.X);
                    output.Write(sliceKey.Y);
                    output.Write(sliceKey.Width);
                    output.Write(sliceKey.Height);

                    output.Write(sliceKey.HasNinePatch);

                    if (sliceKey.HasNinePatch)
                    {
                        output.Write(sliceKey.CenterX);
                        output.Write(sliceKey.CenterY);
                        output.Write(sliceKey.CenterWidth);
                        output.Write(sliceKey.CenterHeight);
                    }

                    output.Write(sliceKey.HasPivot);

                    if (sliceKey.HasPivot)
                    {
                        output.Write(sliceKey.PivotX);
                        output.Write(sliceKey.PivotY);
                    }
                }
            }

            //  If user asked to output the generated spritesheet by providing a filepath
            //  to output to, then output the spritesheet.
            if (!string.IsNullOrWhiteSpace(input.Options.OutputSpriteSheet))
            {
                OutputSpritesheet(input);
            }
        }

        /// <summary>
        ///     This is a quick and dirty port of the MonoGame code to write a
        ///     Texture2D to png.
        //
        //      https://github.com/MonoGame/MonoGame/blob/develop/MonoGame.Framework/Platform/Graphics/Texture2D.StbSharp.cs#L75
        /// </summary>
        private unsafe void OutputSpritesheet(AnimationProcessorResult input)
        {
            using (FileStream stream = new FileStream(input.Options.OutputSpriteSheet, FileMode.OpenOrCreate))
            {
                Color[] data = null;
                
                try
                {
                    data = input.ColorData;
                    fixed (Color* ptr = &data[0])
                    {
                        ImageWriter writer = new ImageWriter();
                        writer.WritePng(ptr, input.TextureWidth, input.TextureHeight, ColorComponents.RedGreenBlueAlpha, stream);
                    }
                }
                finally
                {
                    if(data != null)
                    {
                        data = null;
                    }
                }
            }
        }


        //private void OutputSpritesheet(AnimationProcessorResult input)
        //{
        //    ImageWriter writer = new ImageWriter();
        //    writer.WritePng()

        //    using (FileStream stream = new FileStream(input.Options.OutputSpriteSheet, FileMode.Create, FileAccess.Write))
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(stream))
        //        {
        //            //  Write the png header
        //            writer.Write((byte)0x89);
        //            writer.Write((byte)0x50);
        //            writer.Write((byte)0x4E);
        //            writer.Write((byte)0x47);
        //            writer.Write((byte)0x0D);
        //            writer.Write((byte)0x0A);
        //            writer.Write((byte)0x1A);
        //            writer.Write((byte)0x0A);

        //            byte[] chunkData;

        //            // ------------------------------------
        //            //  IHDR chunk
        //            // ------------------------------------
        //            //  create the chunk data
        //            using (MemoryStream chunkStream = new MemoryStream())
        //            {
        //                using (BinaryWriter chunkWriter = new BinaryWriter(chunkStream))
        //                {
        //                    //  Write the IHDR header
        //                    chunkWriter.Write(new byte[] { Convert.ToByte('I'), Convert.ToByte('H'), Convert.ToByte('D'), Convert.ToByte('R') });

        //                    //  Write the IHDR data
        //                    chunkWriter.Write(BigEndianInt32(input.TextureWidth));
        //                    chunkWriter.Write(BigEndianInt32(input.TextureHeight));
        //                    //chunkWriter.Write(BitConverter.GetBytes(input.TextureWidth));    //  Width (4 bytes)
        //                    //chunkWriter.Write(BitConverter.GetBytes(input.TextureHeight));   //  Height (4 bytes)
        //                    chunkWriter.Write((byte)8);                                      //  Bit depth
        //                    chunkWriter.Write((byte)6);                                      //  Color type
        //                    chunkWriter.Write((byte)0);                                      //  Compression method (must be 0)
        //                    chunkWriter.Write((byte)0);                                      //  Filter method (must be 0)
        //                    chunkWriter.Write((byte)0);                                      //  Interlace method (0 = no interlace)
        //                }

        //                chunkData = chunkStream.ToArray();
        //            }

        //            writer.Write(BigEndianInt32(chunkData.Length - 4));             //  Length of IHDR chunk data minus the 4 bytes type field
        //            writer.Write(chunkData);                                        //  Chunk type + chunk data
        //            writer.Write(BigEndianInt32((int)Utils.Crc32(chunkData)));      //  CRC of chunk type + chunk data


        //            // ------------------------------------
        //            //  IDAT chunk
        //            // ------------------------------------
        //            using (MemoryStream chunkStream = new MemoryStream())
        //            {
        //                using (BinaryWriter chunkWriter = new BinaryWriter(chunkStream))
        //                {
        //                    //  Write the PLTE header
        //                    chunkWriter.Write(new byte[] { Convert.ToByte('I'), Convert.ToByte('D'), Convert.ToByte('A'), Convert.ToByte('T') });


        //                    //List<byte[]> scanLines = new List<byte[]>();

        //                    byte[] scanLines = new byte[(input.ColorData.Length * 4) + input.TextureHeight];
        //                    for (int i = 0, b = 0; i < input.ColorData.Length; i++, b += 4)
        //                    {
        //                        if (i % input.TextureWidth == 0)
        //                        {
        //                            //  Beginning of a scaneline, write the filter method
        //                            scanLines[b] = (byte)0;
        //                            scanLines[b + 1] = input.ColorData[i].R;
        //                            scanLines[b + 2] = input.ColorData[i].G;
        //                            scanLines[b + 3] = input.ColorData[i].B;
        //                            scanLines[b + 4] = input.ColorData[i].A;
        //                            b++;
        //                        }
        //                        else
        //                        {
        //                            scanLines[b] = input.ColorData[i].R;
        //                            scanLines[b + 1] = input.ColorData[i].G;
        //                            scanLines[b + 2] = input.ColorData[i].B;
        //                            scanLines[b + 3] = input.ColorData[i].A;
        //                        }
        //                    }

        //                    byte[] compressedScanlines;
        //                    MemoryStream decompressedStream = new MemoryStream(scanLines);
        //                    using (MemoryStream compressStream = new MemoryStream())
        //                    {
        //                        using (DeflateStream deflateStream = new DeflateStream(compressStream, CompressionLevel.Fastest))
        //                        {
        //                            decompressedStream.CopyTo(deflateStream);
        //                            deflateStream.Close();
        //                            compressedScanlines = compressStream.ToArray();
        //                        }
        //                    }

        //                    //using (MemoryStream compressStream = new MemoryStream())
        //                    //using (var zs = new MonoGame.Framework.Utilities.Deflate.DeflateStream(compressStream, Framework.Utilities.Deflate.CompressionMode.Compress, Framework.Utilities.Deflate.CompressionLevel.Level4))
        //                    //{
        //                    //    decompressedStream.CopyTo(zs);
        //                    //    zs.Close();
        //                    //    compressedScanlines = compressStream.ToArray();
        //                    //}

        //                    //  Write zlib header
        //                    chunkWriter.Write((byte)0x78);
        //                    chunkWriter.Write((byte)0x5E);
        //                    chunkWriter.Write(compressedScanlines);

        //                }
        //                chunkData = chunkStream.ToArray();
        //            }

        //            writer.Write(BigEndianInt32(chunkData.Length - 4));                 //  Length of the IDAT chunk data minus the 4 bytes type field
        //            writer.Write(chunkData);                                            //  Chunk type + chunk data.
        //            writer.Write(BigEndianInt32((int)Utils.Crc32(chunkData)));      //  CRC of chunk type + chunk data


        //            // ------------------------------------
        //            //  IEND chunk
        //            // ------------------------------------
        //            using (MemoryStream chunkStream = new MemoryStream())
        //            {
        //                using (BinaryWriter chunkWriter = new BinaryWriter(chunkStream))
        //                {
        //                    //  Write the IEND header
        //                    chunkWriter.Write(new byte[] { Convert.ToByte('I'), Convert.ToByte('E'), Convert.ToByte('N'), Convert.ToByte('D') });
        //                }
        //                chunkData = chunkStream.ToArray();
        //            }

        //            writer.Write(BigEndianInt32(0));                                    // IEND is always 0 length
        //            writer.Write(chunkData);                                            //  Chunk type + chunk data.
        //            writer.Write(BigEndianInt32((int)Utils.Crc32(chunkData)));      //  CRC of chunk type + chunk data
        //        }
        //    }
        //}



        private byte[] BigEndianInt32(int value)
        {
            byte[] result = new byte[4];
            result[0] = (byte)(value >> 24);
            result[1] = (byte)(value >> 16);
            result[2] = (byte)(value >> 8);
            result[3] = (byte)(value);

            return result;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MonoGame.Aseprite.AsepriteContentTypeReader, MonoGame.Aseprite";
        }
    }
}
