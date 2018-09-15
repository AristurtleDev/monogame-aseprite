using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Aseprite
{
    public class AnimationDefinition
    {
        public Dictionary<string, Animation> Animations { get; private set; }
        public List<Frame> Frames { get; private set; }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        public AnimationDefinition()
        {
            this.Animations = new Dictionary<string, Animation>();
            this.Frames = new List<Frame>();
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
            if(this.Animations.ContainsKey(animation.name))
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
            foreach(var animation in animations)
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
            foreach(var frame in frames)
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
    }



    public struct Animation
    {
        public string name;
        public int from;
        public int to;

        public Animation(string name, int from, int to)
        {
            this.name = name;
            this.from = from;
            this.to = to;
        }
    }

    public struct Frame
    {
        public Rectangle frame;
        public int duration;

        public Frame(int x, int y, int width, int height, int duration)
        {
            this.frame = new Rectangle(x, y, width, height);
            this.duration = duration;
        }

        public Frame(Rectangle frame, int duration)
        {
            this.frame = frame;
            this.duration = duration;
        }

    }
}
