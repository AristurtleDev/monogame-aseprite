//--------------------------------------------------------------------------------
//  AnimationDefinitionReader
//  Used by the ContentManger when using .Load<AnimationDefinition>() to allow
//  reading the AnimationDefinition .xnb file that was produced by the
//  contnet pipeline extension
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace MonoGame.Aseprite
{
    /// <summary>
    ///     Used by the ContentManger when using Load to allow
    ///     reading the AnimationDefinition .xnb file that was produced by the
    ///     contnet pipeline extension
    /// </summary>
    public class AnimationDefinitionReader : ContentTypeReader<AnimationDefinition>
    {
        /// <summary>
        ///     Reads the AnimationDefinition for the content manager
        /// </summary>
        /// <param name="input">Provided by the content manager when using load</param>
        /// <param name="existingInstance">Provided by the content manter when using load</param>
        /// <returns></returns>
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

            //  Read how many slice definitons there are in total
            int sliceCount = input.ReadInt32();

            //  Initilize a new dictionary for the slices
            Dictionary<string, Slice> slices = new Dictionary<string, Slice>();

            //  Read all the slice definition data
            for(int i = 0; i < sliceCount; i++)
            {
                //  Read the slice name
                string name = input.ReadString();

                //  Read the slice color
                string color = input.ReadString();

                //  the color is a hex string, so it has to be converted to a Microsoft.Xna.Framework.Color type
                Color colorActual = HextToColor(color.Replace("#", ""));


                //  Read how many keys there are in this slice
                int keyCount = input.ReadInt32();

                //  Initilize a new dictionary for the keys
                Dictionary<int, SliceKey> keys = new Dictionary<int, SliceKey>();

                //  Read all the slice key definition data
                for(int j = 0; j < keyCount; j++)
                {
                    //  read the frame number
                    int frame = input.ReadInt32();

                    //  read the x-coordinate
                    int x = input.ReadInt32();

                    //  read the y-coordinate
                    int y = input.ReadInt32();

                    //  read the width
                    int w = input.ReadInt32();

                    //  read the height
                    int h = input.ReadInt32();

                    //  Create a new slice key
                    SliceKey key = new SliceKey(frame, x, y, w, h);

                    //  Add the key to the dictionary
                    keys.Add(key.frame, key);
                }

                //  Create a new Slice
                Slice slice = new Slice(name, colorActual, keys);

                //  Add the slice to the dictionary
                slices.Add(slice.name, slice);
            }

            //  Create a new AnimationDefinition
            AnimationDefinition animationDefinition = new AnimationDefinition(animations, frames, slices);

           
            return animationDefinition;
        }

        /// <summary>
        ///     Converts a hex string into a Microsoft.Xna.Color
        /// </summary>
        /// <param name="hex">The 6 digit or 8 digit hex string without the # at the beginning</param>
        /// <returns></returns>
        private Color HextToColor(string hex)
        {
            
            if (hex.Length >= 6)
            {
                float r = (HexToByte(hex[0]) * 16 + HexToByte(hex[1])) / 255.0f;
                float g = (HexToByte(hex[2]) * 16 + HexToByte(hex[3])) / 255.0f;
                float b = (HexToByte(hex[4]) * 16 + HexToByte(hex[5])) / 255.0f;

                if (hex.Length == 8)
                {
                    float a = (HexToByte(hex[6]) * 16 + HexToByte(hex[7])) / 255.0f;
                    return new Color(r, g, b, a);
                }

                return new Color(r, g, b);
            }

            return Color.White;
        }

        /// <summary>
        ///     Lookup table for base16 digits
        /// </summary>
        private const string Hex = "0123456789ABCDEF";

        /// <summary>
        ///     Converts the given hex digit to a byte
        /// </summary>
        /// <param name="c">The hex digit as a char</param>
        /// <returns></returns>
        private byte HexToByte(char c)
        {
            return (byte)Hex.IndexOf(char.ToUpper(c));
        }

    }
}
