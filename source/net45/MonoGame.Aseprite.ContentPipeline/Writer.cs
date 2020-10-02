//--------------------------------------------------------------------------------
//  Writer
//  Writes the processed information to an .xnb file
//--------------------------------------------------------------------------------
//
//                              License
//  
//    Copyright(c) 2018 Chris Whitley
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in
//    all copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//    THE SOFTWARE.
//--------------------------------------------------------------------------------
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TWrite = MonoGame.Aseprite.ContentPipeline.Models.AsepriteModel;

namespace MonoGame.Aseprite.ContentPipeline
{
    [ContentTypeWriter]
    public class Writer : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            //  Write how many frames there are in total
            output.Write(value.frames.Count);

            //  Write the data about the frames
            for(int i = 0; i < value.frames.Count; i++)
            {
                //  x-coordinate value
                output.Write(value.frames[i].frame.x);

                //  y-coordinate value
                output.Write(value.frames[i].frame.y);

                //  (w)idth value
                output.Write(value.frames[i].frame.w);

                //  (h)eight value
                output.Write(value.frames[i].frame.h);

                //  Write the duration of the frame
                output.Write(value.frames[i].duration);

            }

            //  Write how many animation defintions there are in total
            output.Write(value.meta.frameTags.Count);

            //  Write the data about the animation definition
            for(int i = 0; i < value.meta.frameTags.Count; i++)
            {
                //  write the name of the animation
                output.Write(value.meta.frameTags[i].name);

                //  write the starting (from) frame
                output.Write(value.meta.frameTags[i].from);

                //  write the ending (to) frame
                output.Write(value.meta.frameTags[i].to);
            }

            //  Write how many slice definitions there are in total
            output.Write(value.meta.slices.Count);

            //  Write the data about the slices
            for(int i = 0; i < value.meta.slices.Count; i++)
            {
                //  Write the name of the slice
                output.Write(value.meta.slices[i].name);

                //  Write the color of the slice
                output.Write(value.meta.slices[i].color);

                //  Write how many keys there are for the slice
                output.Write(value.meta.slices[i].keys.Count);

                //  Write the data about the keys for the slice
                for(int j = 0; j < value.meta.slices[i].keys.Count; j++)
                {
                    //  write the frame for the slice
                    output.Write(value.meta.slices[i].keys[j].frame);

                    //  write the key x-coordinate
                    output.Write(value.meta.slices[i].keys[j].bounds.x);

                    //  write the key y-coordinate
                    output.Write(value.meta.slices[i].keys[j].bounds.y);

                    //  write the key width
                    output.Write(value.meta.slices[i].keys[j].bounds.w);

                    //  write the key height
                    output.Write(value.meta.slices[i].keys[j].bounds.h);
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MonoGame.Aseprite.AnimationDefinitionReader, MonoGame.Aseprite";
        }

    }
}
