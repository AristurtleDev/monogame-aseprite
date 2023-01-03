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
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Aseprite.ContentPipeline.Processors;

namespace MonoGame.Aseprite.ContentPipeline
{
    [ContentTypeWriter]
    public sealed class AsepriteDocumentWriter : ContentTypeWriter<AsepriteDocumentProcessorResult>
    {
        /// <summary>
        ///     Writes the contents of the <see cref="AsepriteDocumentProcessorResult"/> instance
        ///     to an byte array.
        /// </summary>
        /// <remarks>
        ///     This overload should only be used when writing the process results of
        ///     an Aseprite file outside of the Content Pipeline Tool.
        /// </remarks>
        /// <param name="input">
        ///     The <see cref="AsepriteDocumentProcessorResult"/> instance to write the values of.
        /// </param>
        /// <returns>
        ///     A array of bytes containing the binary encoded contents of the
        ///     <see cref="AsepriteDocumentProcessorResult"/> instance's values.
        /// </returns>
        public byte[] Write(AsepriteDocumentProcessorResult input)
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

        /// <summary>
        ///     Writes the contents of the <see cref="AsepriteDocumentProcessorResult"/> instance
        ///     to disk as a binary encoded XNB file.
        /// </summary>
        /// <param name="output">
        ///     The <see cref="ContentWriter"/> instance used to write to disk.
        /// </param>
        /// <param name="input">
        ///     The <see cref="AsepriteDocumentProcessorResult"/> instance to write the values of.
        /// </param>
        protected override void Write(ContentWriter output, AsepriteDocumentProcessorResult input)
        {
            WriteInternal(output, input);
        }

        /// <summary>
        ///     Internal write method
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        private void WriteInternal(BinaryWriter output, AsepriteDocumentProcessorResult input)
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
            //      [bool]  Is this a one-shot animation.
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
            // --------------------------------------------------------------

            output.Write(input.TextureWidth);
            output.Write(input.TextureHeight);
            output.Write(input.ColorData.Length);

            for (int i = 0; i < input.ColorData.Length; i++)
            {
                //  Adjustment for writing color due to using BinaryWriter insetad of ContentWriter.
                output.Write(input.ColorData[i].R);
                output.Write(input.ColorData[i].G);
                output.Write(input.ColorData[i].B);
                output.Write(input.ColorData[i].A);
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

            foreach (KeyValuePair<string, ProcessedAnimation> kvp in input.Animations)
            {
                ProcessedAnimation animation = kvp.Value;
                output.Write(animation.Name);
                output.Write(animation.From);
                output.Write(animation.To);
                output.Write(animation.Direction);

                //  Adjustment for writing color due to using BinaryWriter insetad of ContentWriter.
                output.Write(animation.Color.R);
                output.Write(animation.Color.G);
                output.Write(animation.Color.B);
                output.Write(animation.Color.A);

                output.Write(animation.IsOneShot);
            }

            output.Write(input.Slices.Count);

            foreach (KeyValuePair<string, ProcessedSlice> kvp in input.Slices)
            {
                ProcessedSlice slice = kvp.Value;
                output.Write(slice.Name);

                //  Adjustment for writing color due to using BinaryWriter insetad of ContentWriter.
                output.Write(slice.Color.R);
                output.Write(slice.Color.G);
                output.Write(slice.Color.B);
                output.Write(slice.Color.A);

                output.Write(slice.SliceKeys.Count);

                foreach (KeyValuePair<int, ProcessedSliceKey> innerKVP in slice.SliceKeys)
                {
                    ProcessedSliceKey sliceKey = innerKVP.Value;
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
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MonoGame.Aseprite.ContentReaders.AsepriteDocumentTypeReader, MonoGame.Aseprite";
        }
    }
}
