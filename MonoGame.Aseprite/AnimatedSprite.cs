using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Aseprite
{
    public class AnimatedSprite : Sprite
    {

        /// <summary>
        ///     The <see cref="AnimationDefinition"/> that describes the animations and
        ///     frames of animations for this animated sprite
        /// </summary>
        private AnimationDefinition _animationDefinition;
        
        /// <summary>
        ///     The current <see cref="Animation"/> that is being played
        /// </summary>
        public Animation CurrentAnimation { get; private set; }

        /// <summary>
        ///     The current <see cref="Frame"/> in the animation
        /// </summary>
        public Frame CurrentFrame { get; private set; }
        
        /// <summary>
        ///     The index of the current <see cref="Frame"/>
        /// </summary>
        public int CurrentFrameIndex { get; set; }


        /// <summary>
        ///     How much time is left in the current frame of animation
        /// </summary>
        public double FrameTimer { get; private set; }

        /// <summary>
        ///     Is the animation playing
        /// </summary>
        public bool Animating { get; set; }





        #region Actions
        /// <summary>
        ///     Invoked at the beginning of a frame
        /// </summary>
        public Action OnFrameBegin { get; set; }

        /// <summary>
        ///     Invoked at the end of a frame
        /// </summary>
        public Action OnFrameEnd { get; set; }

        /// <summary>
        ///     Invoked each time an animation has ended and is now looping back to frame 0
        /// </summary>
        public Action OnAnimationLoop { get; set; }
        #endregion Actions


        #region Constructors
        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">The spritesheet texture containing all frames of animation</param>
        /// <param name="animationDefinition">The <see cref="AnimationDefinition"/> to use</param>
        public AnimatedSprite(Texture2D texture, AnimationDefinition animationDefinition):base(texture)
        {
            this._animationDefinition = animationDefinition;
            Play(this._animationDefinition.Animations.First().Key);
            this.CurrentAnimation = this._animationDefinition.Animations.First().Value;
            this.CurrentFrame = this._animationDefinition.Frames[this.CurrentAnimation.from];
            this.FrameTimer = this.CurrentFrame.duration;
        }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">The spritesheet texture containing all frames of animation</param>
        /// <param name="animationDefinition">The <see cref="AnimationDefinition"/> to use</param>
        /// <param name="position"></param>
        public AnimatedSprite(Texture2D texture, AnimationDefinition animationDefinition, Vector2 position):this(texture, animationDefinition)
        {
            this.Position = position;
        }
        #endregion Constructors



        public override void Update(GameTime gameTime)
        {
            if(this.Animating)
            {

                if(this.FrameTimer == this.CurrentFrame.duration)
                {
                    //  We're at the beginning of the frame so invoke the
                    //  Action
                    this.OnFrameBegin?.Invoke();
                }

                //  Decrement the frame timer
                this.FrameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

                //  Check if we need to move on to the next frame
                if(this.FrameTimer <= 0)
                {
                    //  We're now at the end of a frame, so invoke the action
                    this.OnFrameEnd?.Invoke();

                    //  Increment the frame index
                    this.CurrentFrameIndex += 1;

                    //  Check that we are still within the bounds of the animations frames
                    if(this.CurrentFrameIndex > this.CurrentAnimation.to)
                    {
                        //  Loop back to the beginning of the animations frame
                        this.CurrentFrameIndex = this.CurrentAnimation.from;

                        //  Since we looped, invoke the loop aciton
                        this.OnAnimationLoop?.Invoke();
                    }

                    //  Set the CurrentFrame
                    this.CurrentFrame = this._animationDefinition.Frames[this.CurrentFrameIndex];

                    //  Set the Duration
                    this.FrameTimer = this.CurrentFrame.duration;

                }
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: this.Texture,
                position: this.Position,
                sourceRectangle: this.CurrentFrame.frame,
                color: RenderDefinition.Color,
                rotation: RenderDefinition.Rotation,
                origin: RenderDefinition.Origin,
                scale: RenderDefinition.Scale,
                effects: RenderDefinition.SpriteEffect,
                layerDepth: RenderDefinition.LayerDepth);
        }


        #region Helper Methods
        public void Play(string animationName)
        {
            //  If the current animation that is playing is the same as the
            //  name provided, just return back
            if (this.CurrentAnimation.name == animationName) { return; }

            if (this._animationDefinition.Animations.ContainsKey(animationName))
            {
                this.CurrentAnimation = this._animationDefinition.Animations[animationName];
                this.CurrentFrameIndex = this.CurrentAnimation.from;
                this.CurrentFrame = this._animationDefinition.Frames[this.CurrentFrameIndex];
                this.FrameTimer = this.CurrentFrame.duration;
                this.Animating = true;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"No animation exists with the given name {animationName}");
            }
        }
        #endregion Helper Methods


        #region AnimationDefinition Utilities
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
            this._animationDefinition.AddAnimation(animation);
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
           this._animationDefinition.AddAnimation(new Animation(name, from, to));
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
            this._animationDefinition.AddAnimations(animations);
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
            this._animationDefinition.AddAnimations(animations);
        }
        
        /// <summary>
        ///     Adds the given <see cref="Frame"/> to the colleciton of frames
        /// </summary>
        /// <param name="frame">The <see cref="Frame"/> to add</param>
        public void AddFrame(Frame frame)
        {
            this._animationDefinition.AddFrame(frame);
        }

        /// <summary>
        ///     Adds a new <see cref="Frame"/> to the collection of frames based
        ///     on the given values
        /// </summary>
        /// <param name="sourceRectangle">The source rectangle that the frame describes</param>
        /// <param name="duration">The amount of time in milliseconds the frame should be displayed</param>
        public void AddFrame(Rectangle sourceRectangle, int duration)
        {
            this._animationDefinition.AddFrame(new Frame(sourceRectangle, duration));
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
            this._animationDefinition.AddFrame(new Frame(x, y, width, height, duration));
        }

        /// <summary>
        ///     Adds the collection of <see cref="Frame"/> values to the collection of frames
        /// </summary>
        /// <param name="frames">The collection of <see cref="Frame"/> values to add</param>
        public void AddFrames(IEnumerable<Frame> frames)
        {
            this._animationDefinition.AddFrames(frames);
        }

        /// <summary>
        ///     Adds the given <see cref="Frame"/> values to the collection of frames
        /// </summary>
        /// <param name="frames">The <see cref="Frame"/> values to add</param>
        public void AddFrames(params Frame[] frames)
        {
            this._animationDefinition.AddFrames(frames);
        }
        #endregion AnimationDefinition Utilities


    }
}
