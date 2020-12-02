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

using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MonoGame.Aseprite.ContentPipeline
{
    [ContentTypeWriter]
    public class AsepriteWriter : ContentTypeWriter<AsepriteProcessorResult>
    {
        protected override void Write(ContentWriter output, AsepriteProcessorResult input)
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
                output.Write(input.ColorData[i]);
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

            foreach (KeyValuePair<string, AsepriteProcessorResult.Animation> kvp in input.Animations)
            {
                AsepriteProcessorResult.Animation animation = kvp.Value;
                output.Write(animation.Name);
                output.Write(animation.From);
                output.Write(animation.To);
                output.Write(animation.Direction);
                output.Write(animation.Color);
            }

            output.Write(input.Slices.Count);

            foreach (KeyValuePair<string, AsepriteProcessorResult.Slice> kvp in input.Slices)
            {
                AsepriteProcessorResult.Slice slice = kvp.Value;
                output.Write(slice.Name);
                output.Write(slice.Color);
                output.Write(slice.SliceKeys.Count);

                foreach (KeyValuePair<int, AsepriteProcessorResult.SliceKey> innerKVP in slice.SliceKeys)
                {
                    AsepriteProcessorResult.SliceKey sliceKey = innerKVP.Value;
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


            //output.Write(input.Frames.Count);


            //for (int i = 0; i < input.Frames.Count; i++)
            //{
            //    output.Write(input.Header.Width);
            //    output.Write(input.Header.Height);
            //    output.Write(input.Frames[i].Duration / 1000.0f);
            //    output.Write(input.Frames[i].Pixels.Length);

            //    for (int p = 0; p < input.Frames[i].Pixels.Length; p++)
            //    {
            //        output.Write(input.Frames[i].Pixels[p]);
            //    }

            //}

            //output.Write(input.Tags.Count);

            //for (int i = 0; i < input.Tags.Count; i++)
            //{
            //    output.Write(input.Tags[i].Name);
            //    output.Write(input.Tags[i].Color);
            //    output.Write(input.Tags[i].From);
            //    output.Write(input.Tags[i].To);
            //    output.Write((int)input.Tags[i].Direction);
            //}

            //output.Write(input.Slices.Count);

            //for (int i = 0; i < input.Slices.Count; i++)
            //{
            //    output.Write(input.Slices[i].Name);
            //    output.Write((int)input.Slices[i].Flags);
            //    output.Write(input.Slices[i].UserDataColor);
            //    output.Write(input.Slices[i].TotalKeys);

            //    for (int j = 0; j < input.Slices[i].TotalKeys; j++)
            //    {
            //        output.Write(input.Slices[i].Keys[j].Frame);
            //        output.Write(input.Slices[i].Keys[j].X);
            //        output.Write(input.Slices[i].Keys[j].Y);
            //        output.Write(input.Slices[i].Keys[j].Width);
            //        output.Write(input.Slices[i].Keys[j].Height);

            //        if ((input.Slices[i].Flags & AsepriteSliceFlags.HasNinePatch) != 0)
            //        {
            //            output.Write(input.Slices[i].Keys[j].CenterX);
            //            output.Write(input.Slices[i].Keys[j].CenterY);
            //            output.Write(input.Slices[i].Keys[j].CenterWidth);
            //            output.Write(input.Slices[i].Keys[j].CenterHeight);
            //        }

            //        if ((input.Slices[i].Flags & AsepriteSliceFlags.HasPivot) != 0)
            //        {
            //            output.Write(input.Slices[i].Keys[j].PivotX);
            //            output.Write(input.Slices[i].Keys[j].PivotY);
            //        }
            //    }
            //}
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MonoGame.Aseprite.AsepriteContentTypeReader, MonoGame.Aseprite";
        }
    }
}
