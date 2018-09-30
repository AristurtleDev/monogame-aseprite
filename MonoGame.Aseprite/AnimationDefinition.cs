//--------------------------------------------------------------------------------
//  AnimationDefinition
//  Defines the animations to be used by an AnimatedSprite
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
using System;
using System.Collections.Generic;

namespace MonoGame.Aseprite
{
    /// <summary>
    ///     The definition of an animation used by an <see cref="AnimatedSprite"/> instance.
    /// </summary>
    public class AnimationDefinition
    {
        /// <summary>
        ///     The colleciton of defined aniamtions
        /// </summary>
        public Dictionary<string, Animation> Animations { get; private set; }

        /// <summary>
        ///     The collection of defined frames used by the animations
        /// </summary>
        public List<Frame> Frames { get; private set; }

        /// <summary>
        ///     The collection of defined slices
        /// </summary>
        public Dictionary<string, Slice> Slices { get; private set; }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        public AnimationDefinition()
        {
            this.Animations = new Dictionary<string, Animation>();
            this.Frames = new List<Frame>();
            this.Slices = new Dictionary<string, Slice>();
        }

        /// <summary>
        ///     Creats a predefined instance based ont he animations and frames given
        /// </summary>
        /// <param name="animations">The dictionary of <see cref="Animation"/></param>
        /// <param name="frames">The list of <see cref="Frame"/></param>
        public AnimationDefinition(Dictionary<string, Animation> animations, List<Frame> frames)
        {
            this.Animations = animations;
            this.Frames = frames;
            this.Slices = new Dictionary<string, Slice>();
        }

        /// <summary>
        ///     Creates a predefined instance based on the animations, frames, and slices given
        /// </summary>
        /// <param name="animations">The dictionary of <see cref="Animation"/></param>
        /// <param name="frames">The list of <see cref="Frame"/></param>
        /// <param name="slices">The dictionary of <see cref="Slice"/></param>
        public AnimationDefinition(Dictionary<string, Animation> animations, List<Frame> frames, Dictionary<string, Slice> slices)
        {
            this.Animations = animations;
            this.Frames = frames;
            this.Slices = slices;
        }



        #region Add Animations
        /// <summary>
        ///     Adds the given <see cref="Animation"/> to the animations dictionary
        /// </summary>
        /// <param name="animation">The <see cref="Animation"/> to add</param>
        /// <remarks>
        ///     Animations are stored in the dictionary by name, so each animation
        ///     must have a unique name
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown if the animation provided has a name that is already present in 
        ///     the animation dictionary
        /// </exception>
        public void AddAnimation(Animation animation)
        {
            if (this.Animations.ContainsKey(animation.name))
            {
                throw new ArgumentException($"The given animation name {animation.name} already exists. Animations must have unique names");
            }
            this.Animations.Add(animation.name, animation);
        }


        /// <summary>
        ///     Adds a new <see cref="Animation"/> to the animation dictionary defined
        ///     by the given values
        /// </summary>
        /// <param name="name">The name of the animation</param>
        /// <param name="from">The starting frame</param>
        /// <param name="to">The ending frame</param>
        /// <remarks>
        ///     Animations are stored in the ditionary by name, so each animation
        ///     must have a unique name
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown if the animation provided has a name that is already present in 
        ///     the animation dictionary
        /// </exception>
        public void AddAnimation(string name, int from, int to)
        {
            this.AddAnimation(new Animation(name, from, to));
        }


        /// <summary>
        ///     Adds the collection of <see cref="Animation"/> values to the animation dictionary
        /// </summary>
        /// <param name="animations">The collection of <see cref="Animation"/> values to add</param>
        /// <remarks>
        ///     Animations are stored in the dictionary by name, so each animation
        ///     must have a unique name
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown if the animation provided has a name that is already present in 
        ///     the animation dictionary
        /// </exception>
        public void AddAnimations(IEnumerable<Animation> animations)
        {
            foreach (var animation in animations)
            {
                this.AddAnimation(animation);
            }
        }

        /// <summary>
        ///     Adds the given <see cref="Animation"/> values to the animation dictionary
        /// </summary>
        /// <param name="animations">The <see cref="Animation"/> values to add</param>
        /// <remarks>
        ///     Animations are stored in the dictionary by name, so each animation
        ///     must have a unique name
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown if the animation provided has a name that is already present in 
        ///     the animation dictionary
        /// </exception>
        public void AddAnimations(params Animation[] animations)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                this.AddAnimation(animations[i]);
            }
        }

        #endregion Add Animations


        #region Add Frames
        /// <summary>
        ///     Adds the given <see cref="Frame"/> to the colleciton of frames
        /// </summary>
        /// <param name="frame">The <see cref="Frame"/> to add</param>
        public void AddFrame(Frame frame)
        {
            this.Frames.Add(frame);
        }

        /// <summary>
        ///     Adds a new <see cref="Frame"/> to the collection of frames based
        ///     on the given values
        /// </summary>
        /// <param name="sourceRectangle">The source rectangle that the frame describes</param>
        /// <param name="duration">The amount of time in milliseconds the frame should be displayed</param>
        public void AddFrame(Rectangle sourceRectangle, int duration)
        {
            this.AddFrame(new Frame(sourceRectangle, duration));
        }

        /// <summary>
        ///     Adds a new <see cref="Frame"/> to the collection of frames based
        ///     on the given values
        /// </summary>
        /// <param name="x">The x-coordinate position of the source rectangle for the frame</param>
        /// <param name="y">The y-coordinate position of the source rectangle for the frame</param>
        /// <param name="width">The width of the source rectangle for the frame</param>
        /// <param name="height">The height of the source rectangle for the frame</param>
        /// <param name="duration">The amount of time in milliseconds the farme shoudl be displayed</param>
        public void AddFrame(int x, int y, int width, int height, int duration)
        {
            this.AddFrame(new Frame(x, y, width, height, duration));
        }

        /// <summary>
        ///     Adds the collection of <see cref="Frame"/> values to the collection of frames
        /// </summary>
        /// <param name="frames">The collection of <see cref="Frame"/> values to add</param>
        public void AddFrames(IEnumerable<Frame> frames)
        {
            foreach (var frame in frames)
            {
                this.AddFrame(frame);
            }
        }

        /// <summary>
        ///     Adds the given <see cref="Frame"/> values to the collection of frames
        /// </summary>
        /// <param name="frames">The <see cref="Frame"/> values to add</param>
        public void AddFrames(params Frame[] frames)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                this.AddFrame(frames[i]);
            }
        }
        #endregion Add Frames


        #region Add Slices
        /// <summary>
        ///     Adds a new <see cref="Slice"/> structrue to the collection of slices
        /// </summary>
        /// <param name="slice">The <see cref="Slice"/> structure to add</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the name of the slice being added alredy exists in the slice collection
        /// </exception>
        public void AddSlice(Slice slice)
        {
            if (this.Slices.ContainsKey(slice.name))
            {
                throw new ArgumentException($"The given slice name {slice.name} already exists. Slices must have unique names");
            }
            this.Slices.Add(slice.name, slice);
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="keys">The colleciton of <see cref="SliceKey"/> structures for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the name of the slice alredy exists in the slice collection
        /// </exception>
        public void AddSlice(string name, Dictionary<int, SliceKey> keys )
        {
            AddSlice(new Slice(name, keys));
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="keys">The colleciton of <see cref="SliceKey"/> structures for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the name of the slice alredy exists in the slice collection
        /// </exception>
        public void AddSlice(string name, IEnumerable<SliceKey> keys)
        {
            AddSlice(new Slice(name, keys));
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="keys">The colleciton of <see cref="SliceKey"/> structures for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the name of the slice alredy exists in the slice collection
        /// </exception>
        public void AddSlice(string name, params SliceKey[] keys)
        {
            AddSlice(new Slice(name, keys));
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="color">The color of the slice</param>
        /// <param name="keys">The colleciton of <see cref="SliceKey"/> structures for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the name of the slice alredy exists in the slice collection
        /// </exception>
        public void AddSlice(string name, Color color, Dictionary<int, SliceKey> keys)
        {
            AddSlice(new Slice(name, color, keys));
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="color">The color of the slice</param>
        /// <param name="keys">The colleciton of <see cref="SliceKey"/> structures for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the name of the slice alredy exists in the slice collection
        /// </exception>
        public void AddSlice(string name, Color color, IEnumerable<SliceKey> keys)
        {
            AddSlice(new Slice(name, color, keys));
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="color">The color of the slice</param>
        /// <param name="keys">The colleciton of <see cref="SliceKey"/> structures for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the name of the slice alredy exists in the slice collection
        /// </exception>
        public void AddSlice(string name, Color color, params SliceKey[] keys)
        {
            AddSlice(new Slice(name, color, keys));
        }
        #endregion Add Slices
    }


    /// <summary>
    ///     Represents the definition of an animation
    /// </summary>
    public struct Animation
    {
        /// <summary>
        ///     The name of the animation
        /// </summary>
        public string name;

        /// <summary>
        ///     The starting frame
        /// </summary>
        public int from;

        /// <summary>
        ///     The ending frame
        /// </summary>
        public int to;

        /// <summary>
        ///     Creates a new <see cref="Animation"/> structure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public Animation(string name, int from, int to)
        {
            this.name = name;
            this.from = from;
            this.to = to;
        }
    }

    /// <summary>
    ///     Represents the definition of a frame
    /// </summary>
    public struct Frame
    {
        /// <summary>
        ///     Defines the area within the spritesheet that the frame is in
        /// </summary>
        public Rectangle frame;

        /// <summary>
        ///     The amount of time in millisecons the frame should be displayed
        /// </summary>
        public int duration;

        /// <summary>
        ///     Creates a new <see cref="Frame"/> structure
        /// </summary>
        /// <param name="x">The x-coordinate position of the frame</param>
        /// <param name="y">The y-coordinate position of the frame</param>
        /// <param name="width">The width of the frame</param>
        /// <param name="height">The height of the frame</param>
        /// <param name="duration">The amount of time in milliseconds the frame should be displayed</param>
        public Frame(int x, int y, int width, int height, int duration)
        {
            this.frame = new Rectangle(x, y, width, height);
            this.duration = duration;
        }

        /// <summary>
        ///     Creates a new <see cref="Frame"/> structure
        /// </summary>
        /// <param name="frame">The <see cref="Rectangle"/> definition of the frame, defining the xy-coordinate and the width and height</param>
        /// <param name="duration">The amount of time in milliseconds the frame should be displayed</param>
        public Frame(Rectangle frame, int duration)
        {
            this.frame = frame;
            this.duration = duration;
        }
    }

    /// <summary>
    ///     Represents a sliced (predefined) area of a frame of animation
    /// </summary>
    public struct Slice
    {
        /// <summary>
        ///     The name of the slice
        /// </summary>
        public string name;

        /// <summary>
        ///     The color of the slice
        /// </summary>
        public Color color;

        /// <summary>
        ///     The colleciton of <see cref="SliceKey"/> structures
        /// </summary>
        public Dictionary<int, SliceKey> keys;

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> structures</param>
        public Slice(string name, Dictionary<int, SliceKey> keys)
        {
            this.name = name;
            this.color = Color.White;
            this.keys = keys;
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The naem of the slice</param>
        /// <param name="keys">A collection of <see cref="SliceKey"/> structures to add to the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of the <see cref="SliceKey"/> structures in the collection given contain,
        ///     the same name
        /// </exception>
        public Slice(string name, IEnumerable<SliceKey> keys)
        {
            this.name = name;
            this.color = Color.White;
            this.keys = new Dictionary<int, SliceKey>();
            
            foreach(SliceKey key in keys)
            {
                if(!this.keys.ContainsKey(key.frame))
                {
                    this.keys.Add(key.frame, key);
                }
                else
                {
                    throw new ArgumentException($"The slice {name} already contains a SliceKey for frame {key.frame}.");
                }
            }
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The naem of the slice</param>
        /// <param name="keys">A collection of <see cref="SliceKey"/> structures to add to the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of the <see cref="SliceKey"/> structures in the collection given contain,
        ///     the same name
        /// </exception>
        public Slice(string name, params SliceKey[] keys)
        {
            this.name = name;
            this.color = Color.White;
            this.keys = new Dictionary<int, SliceKey>();
            foreach (SliceKey key in keys)
            {
                if (!this.keys.ContainsKey(key.frame))
                {
                    this.keys.Add(key.frame, key);
                }
                else
                {
                    throw new Exception($"The slice {name} already contains a SliceKey for frame {key.frame}.");
                }
            }
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> structures</param>
        /// <param name="color">The color of the slice</param>
        public Slice(string name, Color color, Dictionary<int, SliceKey> keys)
        {
            this.name = name;
            this.color = color;
            this.keys = keys;
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The naem of the slice</param>
        /// <param name="keys">A collection of <see cref="SliceKey"/> structures to add to the slice</param>
        /// <param name="color">The color of the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of the <see cref="SliceKey"/> structures in the collection given contain,
        ///     the same name
        /// </exception>
        public Slice(string name, Color color, IEnumerable<SliceKey> keys)
        {
            this.name = name;
            this.color = color;
            this.keys = new Dictionary<int, SliceKey>();

            foreach (SliceKey key in keys)
            {
                if (!this.keys.ContainsKey(key.frame))
                {
                    this.keys.Add(key.frame, key);
                }
                else
                {
                    throw new Exception($"The slice {name} already contains a SliceKey for frame {key.frame}.");
                }
            }
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The naem of the slice</param>
        /// <param name="keys">A collection of <see cref="SliceKey"/> structures to add to the slice</param>
        /// <param name="color">The color of the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of the <see cref="SliceKey"/> structures in the collection given contain,
        ///     the same name
        /// </exception>
        public Slice(string name, Color color, params SliceKey[] keys)
        {
            this.name = name;
            this.color = color;
            this.keys = new Dictionary<int, SliceKey>();
            foreach (SliceKey key in keys)
            {
                if (!this.keys.ContainsKey(key.frame))
                {
                    this.keys.Add(key.frame, key);
                }
                else
                {
                    throw new Exception($"The slice {name} already contains a SliceKey for frame {key.frame}.");
                }
            }
        }

    }

    /// <summary>
    ///     Represnets the frame specific informaiton for a slice
    /// </summary>
    public struct SliceKey
    {
        /// <summary>
        ///     The frame this is for
        /// </summary>
        public int frame;

        /// <summary>
        ///     The <see cref="Rectangle"/> defintion of this
        /// </summary>
        public Rectangle bounds;

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">The frame this is for</param>
        /// <param name="bounds">The <see cref="Rectangle"/> definition of this</param>
        public SliceKey(int frame, Rectangle bounds)
        {
            this.frame = frame;
            this.bounds = bounds;
        }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">The frame this is for</param>
        /// <param name="location">A <see cref="Point"/> to describe the xy-coordinate position of the bounds</param>
        /// <param name="size">A <see cref="Point"/> to describe the width and height of the bounds</param>
        public SliceKey(int frame, Point location, Point size)
        {
            this.frame = frame;
            this.bounds = new Rectangle(location, size);
        }

        /// <summary>
        ///     Creates a new <see cref="SliceKey"/> structure
        /// </summary>
        /// <param name="frame">The frame this is for</param>
        /// <param name="x">The x-coordinate position of the bounds</param>
        /// <param name="y">The y-coordinate position of the bounds</param>
        /// <param name="width">The width of the bounds</param>
        /// <param name="height">The height of the bounds</param>
        public SliceKey(int frame, int x, int y, int width, int height)
        {
            this.frame = frame;
            this.bounds = new Rectangle(x, y, width, height);
        }
    }
}
