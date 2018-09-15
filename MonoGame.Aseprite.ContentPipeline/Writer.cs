using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.IO;

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
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MonoGame.Aseprite.SpriteReader, MonoGame.Aseprite";
        }

    }
}
