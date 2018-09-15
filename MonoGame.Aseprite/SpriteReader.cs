using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Aseprite
{
    public class SpriteReader : ContentTypeReader<AnimationDefinition>
    {
        protected override AnimationDefinition Read(ContentReader input, AnimationDefinition existingInstance)
        {
            //  Read how many frames there are in total
            int frameCount = input.ReadInt32();

            //  Initialize a new list of frames
            List<Frame> frames = new List<Frame>();

            //  Read all the data about the frames
            for(int i = 0; i < frameCount; i++)
            {
                //  Read the x-coordinate value
                int x = input.ReadInt32();

                //  Read the y-coordinate value
                int y = input.ReadInt32();

                //  Read the (w)idth value
                int w = input.ReadInt32();

                //  Read the (h)eight value
                int h = input.ReadInt32();

                //  Read the duration value
                int duration = input.ReadInt32();

                //  Create a frame
                Frame frame = new Frame(x, y, w, h, duration);

                //  Store the frame
                frames.Add(frame);
            }

            //  Read how many animation definitions there are in total
            int animationCount = input.ReadInt32();

            //  Initilize a new dictionary for the animations
            Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

            //  Read all the animation definition data
            for(int i = 0; i < animationCount; i++)
            {
                //  Read the animation name
                string name = input.ReadString();

                //  Read the starting (from) frame
                int from = input.ReadInt32();

                //  Read the ending (to) frame
                int to = input.ReadInt32();

                //  Create a new animation definition
                Animation animation = new Animation(name, from, to);

                //  Store the animation
                animations.Add(animation.name, animation);
            }

            //  Create a new AnimationDefinition
            AnimationDefinition animationDefinition = new AnimationDefinition(animations, frames);

           
            return animationDefinition;


        }
    }
}
