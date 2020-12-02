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
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite
{
    /// <summary>
    ///     A <see cref="Sprite"/> which makes use of <see cref="AnimationDefinition"/> in order
    ///     to provide aniamtions of hte sprite form a spritesheet.
    /// </summary>
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
        public AnimatedSprite(Texture2D texture, AnimationDefinition animationDefinition) : base(texture)
        {
            _animationDefinition = animationDefinition;
            Play(_animationDefinition.Animations.First().Key);
            CurrentAnimation = _animationDefinition.Animations.First().Value;
            CurrentFrame = _animationDefinition.Frames[CurrentAnimation.from];
            FrameTimer = CurrentFrame.duration;
        }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">The spritesheet texture containing all frames of animation</param>
        /// <param name="animationDefinition">The <see cref="AnimationDefinition"/> to use</param>
        /// <param name="position"></param>
        public AnimatedSprite(Texture2D texture, AnimationDefinition animationDefinition, Vector2 position) : this(texture, animationDefinition)
        {
            Position = position;
        }

        public AnimatedSprite(AsepriteImportResult aseprite, Vector2 position) : base()
        {
            Position = position;
            Texture = aseprite.Texture;


            _animationDefinition = new AnimationDefinition();
            for (int i = 0; i < aseprite.Frames.Count; i++)
            {
                _animationDefinition.AddFrame(new Frame()
                {
                    frame = new Rectangle(aseprite.Frames[i].X, aseprite.Frames[i].Y, aseprite.Frames[i].Width, aseprite.Frames[i].Height),
                    duration = aseprite.Frames[i].Duration
                });
            }

            foreach (KeyValuePair<string, AsepriteImportResult.Animation> kvp in aseprite.Animations)
            {
                _animationDefinition.AddAnimation(kvp.Value.Name, kvp.Value.From, kvp.Value.To);
            }

            foreach (KeyValuePair<string, AsepriteImportResult.Slice> kvp in aseprite.Slices)
            {
                Slice slice = new Slice();
                slice.name = kvp.Value.Name;
                slice.color = kvp.Value.Color;
                slice.keys = new Dictionary<int, SliceKey>();

                foreach (KeyValuePair<int, AsepriteImportResult.SliceKey> innerKVP in kvp.Value.SliceKeys)
                {
                    SliceKey key = new SliceKey()
                    {
                        bounds = new Rectangle(innerKVP.Value.X, innerKVP.Value.Y, innerKVP.Value.Width, innerKVP.Value.Height),
                        frame = innerKVP.Value.FrameIndex
                    };

                    slice.keys.Add(key.frame, key);
                }
                _animationDefinition.AddSlice(slice);
            }

            Play(_animationDefinition.Animations.First().Key);
            CurrentAnimation = _animationDefinition.Animations.First().Value;
            CurrentFrame = _animationDefinition.Frames[CurrentAnimation.from];
            FrameTimer = CurrentFrame.duration;

        }
        #endregion Constructors


        /// <summary>
        ///     Updates this
        /// </summary>
        /// <param name="deltaTime">
        ///     The amount of time, in seconds, that have passed since
        ///     the last update.  Usually gathered from GameTime.ElapsedTime.TotalSeconds
        /// </param>
        public override void Update(float deltaTime)
        {
            if (Animating)
            {
                //  Using an epsilon of 0.0001 to check for equality between
                //  the Framtimer (double) and duration (float).  This is to handle
                //  edge cases where precision loss in the float may cause this to
                //  skip.
                if (Math.Abs(FrameTimer - CurrentFrame.duration) < 0.0001)
                {
                    //  We're at the beginning of the frame so invoke the
                    //  Action
                    OnFrameBegin?.Invoke();
                }

                //  Decrement the frame timer
                FrameTimer -= deltaTime;

                //  Check if we need to move on to the next frame
                if (FrameTimer <= 0)
                {
                    //  We're now at the end of a frame, so invoke the action
                    OnFrameEnd?.Invoke();

                    //  Increment the frame index
                    CurrentFrameIndex += 1;

                    //  Check that we are still within the bounds of the animations frames
                    if (CurrentFrameIndex > CurrentAnimation.to)
                    {
                        //  Loop back to the beginning of the animations frame
                        CurrentFrameIndex = CurrentAnimation.from;

                        //  Since we looped, invoke the loop aciton
                        OnAnimationLoop?.Invoke();
                    }

                    //  Set the CurrentFrame
                    CurrentFrame = _animationDefinition.Frames[CurrentFrameIndex];

                    //  Set the Duration
                    FrameTimer = (double)CurrentFrame.duration;

                }
            }
        }


        /// <summary>
        ///     Renders to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: Texture,
                position: Position,
                sourceRectangle: CurrentFrame.frame,
                color: RenderDefinition.Color,
                rotation: RenderDefinition.Rotation,
                origin: RenderDefinition.Origin,
                scale: RenderDefinition.Scale,
                effects: RenderDefinition.SpriteEffect,
                layerDepth: RenderDefinition.LayerDepth);
        }


        #region Helper Methods
        /// <summary>
        ///     Plays the animation that matches the given name
        /// </summary>
        /// <param name="animationName">The name of the animation to play</param>
        public void Play(string animationName)
        {
            //  If the current animation that is playing is the same as the
            //  name provided, just return back
            if (CurrentAnimation.name == animationName) { return; }

            if (_animationDefinition.Animations.ContainsKey(animationName))
            {
                CurrentAnimation = _animationDefinition.Animations[animationName];
                CurrentFrameIndex = CurrentAnimation.from;
                CurrentFrame = _animationDefinition.Frames[CurrentFrameIndex];
                FrameTimer = CurrentFrame.duration;
                Animating = true;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"No animation exists with the given name {animationName}");
            }
        }

        /// <summary>
        ///     Gets the defined color of the slice with the given name
        /// </summary>
        /// <param name="sliceName">The name of the slice</param>
        /// <returns>
        ///     The color of the slice
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice name provided does not exist in the animation definitions slice dictionary
        /// </exception>
        public Color GetSliceColor(string sliceName)
        {
            //  Ensure we have a slice defined with the given name
            if (_animationDefinition.Slices.ContainsKey(sliceName))
            {
                //  Return the color of the slice
                return _animationDefinition.Slices[sliceName].color;
            }
            else
            {
                //  No slice exists with the given name, throw error
                throw new ArgumentException($"The animation definition does not contain a slice definition with the name {sliceName}");
            }

        }

        /// <summary>
        ///     Get the Rectangle definition of the slice at the current frame of 
        ///     animation, if there is a slice key defined for the frame
        /// </summary>
        /// <param name="sliceName">The name of the slice</param>
        /// <returns>
        ///     A Rectangle definition of the frame slice, at the xy-coordinate of
        ///     this sprite.  If no slice key exists for the current frame, 
        ///     null is returned.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice name provided does not exist in the animation definitions slice dictionary
        /// </exception>
        public Rectangle? GetCurrentFrameSlice(string sliceName)
        {
            //  Ensure that we have a slice defined with the given name
            if (_animationDefinition.Slices.ContainsKey(sliceName))
            {
                //  Get the slice
                Slice slice = _animationDefinition.Slices[sliceName];

                //  Ensure we have a slice key at the current animation frame index
                if (slice.keys.ContainsKey(CurrentFrameIndex))
                {
                    //  Get the slice key
                    SliceKey sliceKey = slice.keys[CurrentFrameIndex];

                    //  Get the rectangle that represents the bounds of the slicekey
                    Rectangle rect = sliceKey.bounds;

                    //  Update the xy-coordinate of the rect to match the positional data of this sprite
                    rect.X += (int)Position.X;
                    rect.Y += (int)Position.Y;

                    //  return the rectangle
                    return rect;
                }
                else
                {
                    //  There is no slicekey for the current frame index, so we return null
                    return null;
                }
            }
            else
            {
                //  No slice exists with the given name, throw error
                throw new ArgumentException($"The animation definition does not contain a slice definition with the name {sliceName}");
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
            _animationDefinition.AddAnimation(animation);
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
            _animationDefinition.AddAnimation(new Animation(name, from, to));
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
            _animationDefinition.AddAnimations(animations);
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
            _animationDefinition.AddAnimations(animations);
        }

        /// <summary>
        ///     Adds the given <see cref="Frame"/> to the colleciton of frames
        /// </summary>
        /// <param name="frame">The <see cref="Frame"/> to add</param>
        public void AddFrame(Frame frame)
        {
            _animationDefinition.AddFrame(frame);
        }

        /// <summary>
        ///     Adds a new <see cref="Frame"/> to the collection of frames based
        ///     on the given values
        /// </summary>
        /// <param name="sourceRectangle">The source rectangle that the frame describes</param>
        /// <param name="duration">The amount of time in seconds the frame should be displayed</param>
        public void AddFrame(Rectangle sourceRectangle, float duration)
        {
            _animationDefinition.AddFrame(new Frame(sourceRectangle, duration));
        }

        /// <summary>
        ///     Adds a new <see cref="Frame"/> to the collection of frames based
        ///     on the given values
        /// </summary>
        /// <param name="x">The x-coordinate position of the source rectangle for the frame</param>
        /// <param name="y">The y-coordinate position of the source rectangle for the frame</param>
        /// <param name="width">The width of the source rectangle for the frame</param>
        /// <param name="height">The height of the source rectangle for the frame</param>
        /// <param name="duration">The amount of time in seconds the farme shoudl be displayed</param>
        public void AddFrame(int x, int y, int width, int height, float duration)
        {
            _animationDefinition.AddFrame(new Frame(x, y, width, height, duration));
        }

        /// <summary>
        ///     Adds the collection of <see cref="Frame"/> values to the collection of frames
        /// </summary>
        /// <param name="frames">The collection of <see cref="Frame"/> values to add</param>
        public void AddFrames(IEnumerable<Frame> frames)
        {
            _animationDefinition.AddFrames(frames);
        }

        /// <summary>
        ///     Adds the given <see cref="Frame"/> values to the collection of frames
        /// </summary>
        /// <param name="frames">The <see cref="Frame"/> values to add</param>
        public void AddFrames(params Frame[] frames)
        {
            _animationDefinition.AddFrames(frames);
        }

        /// <summary>
        ///     Adds the given <see cref="Slice"/> to the collection of slices
        /// </summary>
        /// <param name="slice">The <see cref="Slice"/> to add</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(Slice slice)
        {
            _animationDefinition.AddSlice(slice);
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> with the given name and keys
        ///     and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the <see cref="Slice"/></param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, Dictionary<int, SliceKey> keys)
        {
            _animationDefinition.AddSlice(name, keys);
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> with the given name and keys
        ///     and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the <see cref="Slice"/></param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, IEnumerable<SliceKey> keys)
        {
            _animationDefinition.AddSlice(name, keys);
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> with the given name and keys
        ///     and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the <see cref="Slice"/></param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, params SliceKey[] keys)
        {
            _animationDefinition.AddSlice(name, keys);
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> with the given name and keys
        ///     and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the <see cref="Slice"/></param>
        /// <param name="color">The color of the <see cref="Slice"/></param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, Color color, Dictionary<int, SliceKey> keys)
        {
            _animationDefinition.AddSlice(name, color, keys);
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> with the given name and keys
        ///     and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the <see cref="Slice"/></param>
        /// <param name="color">The color of the <see cref="Slice"/></param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, Color color, IEnumerable<SliceKey> keys)
        {
            _animationDefinition.AddSlice(name, color, keys);
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> with the given name and keys
        ///     and adds it to the collection of slices
        /// </summary>
        /// <param name="name">The name of the <see cref="Slice"/></param>
        /// <param name="color">The color of the <see cref="Slice"/></param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> for the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, Color color, params SliceKey[] keys)
        {
            _animationDefinition.AddSlice(name, color, keys);
        }

        /// <summary>
        ///     Adds the give collection of <see cref="Slice"/> structures to the collection
        /// </summary>
        /// <param name="slices">The <see cref="Slice"/> structures to add</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        ///     or if any of the slices in the given collection have the same name
        /// </exception>
        public void AddSlices(IEnumerable<Slice> slices)
        {
            foreach (Slice slice in slices)
            {
                _animationDefinition.AddSlice(slice);
            }
        }

        /// <summary>
        ///     Adds the give collection of <see cref="Slice"/> structures to the collection
        /// </summary>
        /// <param name="slices">The <see cref="Slice"/> structures to add</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        ///     or if any of the slices in the given collection have the same name
        /// </exception>
        public void AddSlices(params Slice[] slices)
        {
            foreach (Slice slice in slices)
            {
                _animationDefinition.AddSlice(slice);
            }
        }
        #endregion AnimationDefinition Utilities


    }
}
