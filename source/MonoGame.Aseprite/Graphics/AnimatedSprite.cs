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
using MonoGame.Aseprite.Documents;

namespace MonoGame.Aseprite.Graphics
{
    /// <summary>
    ///     An animated sprite class used to define, mange, and rendering sprite
    ///     animations based on a single spritesheet.
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        private Frame _currentFrame;

        /// <summary>
        ///     Gets the key-value collection of <see cref="Animation"/> instances
        ///     used by this animated sprite. The name of the animation is the
        ///     key for the dictionary.
        /// </summary>
        public Dictionary<string, Animation> Animations { get; private set; }

        /// <summary>
        ///     Gets the collection of <see cref="Frame"/> instances for this
        ///     animated sprite.
        /// </summary>
        public List<Frame> Frames { get; private set; }

        /// <summary>
        ///     Gets the key-value collection of <see cref="Slice"/> instances
        ///     for this animated sprite. 
        /// </summary>
        public Dictionary<string, Slice> Slices { get; private set; }

        /// <summary>
        ///     Gets the current <see cref="Animation"/> that is being played.
        /// </summary>
        public Animation CurrentAnimation { get; private set; }

        /// <summary>
        ///     Gets the current <see cref="Frame"/> in the animation that
        ///     is being rendered.
        /// </summary>
        public Frame CurrentFrame
        {
            get { return _currentFrame; }
            private set
            {
                _currentFrame = value;
                SourceRectangle = value.Bounds;
            }
        }

        /// <summary>
        ///     Gets the index of current <see cref="Frame"/> in the animation
        ///     that is being rendered. This is the index of the frame in the
        ///     <see cref="Frames"/> propery.
        /// </summary>
        public int CurrentFrameIndex { get; set; }

        /// <summary>
        ///     Gets the amount of time, in seconds, that is left to display
        ///     the current frame in the current animation.
        /// </summary>
        public double FrameTimer { get; private set; }

        /// <summary>
        ///     Gets a value indicating if this instance is currently
        ///     playing an animation.
        /// </summary>
        public bool Animating { get; set; }

        /// <summary>
        ///     Gets the width, in pixels.
        /// </summary>
        public override int Width
        {
            get
            {
                return CurrentFrame.Bounds.Width;
            }
        }

        /// <summary>
        ///     Gets the height, in pixels.
        /// </summary>
        public override int Height
        {
            get
            {
                return CurrentFrame.Bounds.Height;
            }
        }

        /// <summary>
        ///     Gets or Sets an action to invoke at the beginning of each
        ///     frame in an animation.
        /// </summary>
        public Action OnFrameBegin { get; set; }

        /// <summary>
        ///     Gets or Sets an action to invoke at the end of each frame
        ///     in an animation.
        /// </summary>
        public Action OnFrameEnd { get; set; }

        /// <summary>
        ///     Gets or Sets an action to invoke each time an animation loops.
        /// </summary>
        public Action OnAnimationLoop { get; set; }

        /// <summary>
        ///     Creates a new <see cref="AnimatedSprite"/> instance.
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">
        ///     A Texture2D that is the spritesheet containing all frames of all
        ///     animations that will be rendered.
        /// </param>
        public AnimatedSprite(Texture2D texture) : base(texture)
        {
            Frames = new List<Frame>();
            Animations = new Dictionary<string, Animation>();
            Slices = new Dictionary<string, Slice>();
            Animating = false;
        }

        /// <summary>
        ///     Creates a new <see cref="AnimatedSprite"/> instance.
        ///     Creates a new instance
        /// </summary>
        /// <param name="texture">
        ///     A Texture2D that is the spritesheet containing all frames of all
        ///     animations that will be rendered.
        /// </param>
        /// <param name="position">
        ///     The top-left xy-coordinate position of this instance.
        /// </param>
        public AnimatedSprite(Texture2D texture, Vector2 position) : this(texture)
        {
            Position = position;
        }

        public AnimatedSprite(AsepriteAnimationDocument aseprite, Vector2 position)
            : this(aseprite.Texture, position)
        {

            for (int i = 0; i < aseprite.Frames.Count; i++)
            {
                Frames.Add(new Frame()
                {
                    Bounds = new Rectangle(aseprite.Frames[i].X, aseprite.Frames[i].Y, aseprite.Frames[i].Width, aseprite.Frames[i].Height),
                    Duration = aseprite.Frames[i].Duration
                });
            }

            foreach (KeyValuePair<string, AsepriteTag> kvp in aseprite.Tags)
            {
                Animations.Add(kvp.Key, new Animation(kvp.Key, kvp.Value.From, kvp.Value.To));
            }

            foreach (KeyValuePair<string, AsepriteSlice> kvp in aseprite.Slices)
            {
                Slice slice = new Slice
                {
                    Name = kvp.Value.Name,
                    Color = kvp.Value.Color,
                    Keys = new Dictionary<int, SliceKey>()
                };

                foreach (KeyValuePair<int, AsepriteSliceKey> innerKVP in kvp.Value.SliceKeys)
                {
                    SliceKey key = new SliceKey()
                    {
                        Bounds = new Rectangle(innerKVP.Value.X, innerKVP.Value.Y, innerKVP.Value.Width, innerKVP.Value.Height),
                        Frame = innerKVP.Value.FrameIndex
                    };

                    slice.Keys.Add(key.Frame, key);
                }
                Slices.Add(slice.Name, slice);
            }

            Play(Animations.First().Key);
        }

        /// <summary>
        ///     Updates this instance.
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
                if (Math.Abs(FrameTimer - CurrentFrame.Duration) < 0.0001)
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
                    if (CurrentFrameIndex > CurrentAnimation.To)
                    {
                        //  Loop back to the beginning of the animations frame
                        CurrentFrameIndex = CurrentAnimation.From;

                        //  Since we looped, invoke the loop aciton
                        OnAnimationLoop?.Invoke();
                    }

                    //  Set the CurrentFrame
                    CurrentFrame = Frames[CurrentFrameIndex];

                    //  Set the Duration
                    FrameTimer = (double)CurrentFrame.Duration;
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
                sourceRectangle: CurrentFrame.Bounds,
                color: Color,
                rotation: Rotation,
                origin: Origin,
                scale: Scale,
                effects: SpriteEffect,
                layerDepth: LayerDepth);
        }

        /// <summary>
        ///     Given the name of an animation, sets that animation as the
        ///     current animation to play and starts it.
        /// </summary>
        /// <param name="animationName">
        ///     The name of the animation to play.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if there is no animation exists with the name given
        ///     in <paramref name="animationName"/>
        /// </exception>
        public void Play(string animationName)
        {
            //  If the current animation that is playing is the same as the
            //  name provided, just return back
            if (CurrentAnimation.Name == animationName) { return; }

            if (Animations.TryGetValue(animationName, out Animation animation))
            {
                CurrentAnimation = animation;
                CurrentFrameIndex = CurrentAnimation.From;
                CurrentFrame = Frames[CurrentFrameIndex];
                FrameTimer = CurrentFrame.Duration;
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
        /// <param name="sliceName">
        ///     The name of the slice
        /// </param>
        /// <returns>
        ///     The color of the slice
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if no <see cref="Slice"/> instance exists with the name given
        ///     in <paramref name="sliceName"/>
        /// </exception>
        public Color GetSliceColor(string sliceName)
        {
            //  Ensure we have a slice defined with the given name
            if (Slices.TryGetValue(sliceName, out Slice slice))
            {
                //  Return the color of the slice
                return slice.Color;
            }
            else
            {
                //  No slice exists with the given name, throw error
                throw new ArgumentOutOfRangeException($"No slice exists with the given name {sliceName}");
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
        public bool TryGetCurrentFrameSlice(string sliceName, out SliceKey sliceKey)
        {
            //  Ensure that we have a slice defined with the given name
            if (Slices.TryGetValue(sliceName, out Slice slice))
            {
                //  Ensure we have a slice key at the current animation frame index
                if (slice.Keys.TryGetValue(CurrentFrameIndex, out sliceKey))
                {
                    //  Update the xy-coordinate of the slicekey bounds to match the positiona
                    //  data of this animated sprite.
                    sliceKey.Bounds.X += (int)Position.X;
                    sliceKey.Bounds.Y += (int)Position.Y;
                    return true;
                }
                else
                {
                    //  There is no slicekey for the current frame index, so we return false.
                    return false;
                }
            }
            else
            {
                //  No slice exists with the given name, throw error
                throw new ArgumentOutOfRangeException($"No slice exists with the given name {sliceName}");
            }
        }

        /// <summary>
        ///     Adds the given <see cref="Animation"/> to the animations dictionary
        /// </summary>
        /// <param name="animation">
        ///     The <see cref="Animation"/> to add
        /// </param>
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
            Animations.Add(animation.Name, animation);
        }

        /// <summary>
        ///     Given the name, starting frame, and ending frame, creates a new
        ///     <see cref="Animation"/> instances and adds it to the animation dictionary.
        /// </summary>
        /// <param name="name">
        ///     The name of the animation
        /// </param>
        /// <param name="from">
        ///     The starting frame
        /// </param>
        /// <param name="to">
        ///     The ending frame
        /// </param>
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
            Animations.Add(name, new Animation(name, from, to));
        }

        /// <summary>
        ///     Given a collection of <see cref="Animation"/> instances, adds them
        ///     to the animation dictionary.
        /// </summary>
        /// <param name="animations">
        ///     The collection of <see cref="Animation"/> instances to add
        /// </param>
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
            foreach (Animation animation in animations)
            {
                Animations.Add(animation.Name, animation);
            }
        }

        /// <summary>
        ///     Given <see cref="Animations"/> instances, adds them to the animation
        ///     dictionary.
        /// </summary>
        /// <param name="animations"
        ///     >The <see cref="Animation"/> instances to add
        ///     </param>
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
                Animations.Add(animations[i].Name, animations[i]);
            }
        }

        /// <summary>
        ///     Given a <see cref="Frame"/> instance, adss it to the collection
        ///     of frames.
        /// </summary>
        /// <param name="frame">
        ///     The <see cref="Frame"/> to add
        /// </param>
        public void AddFrame(Frame frame)
        {
            Frames.Add(frame);
        }

        /// <summary>
        ///     Given the boundry of a frame and the duration, in seconds, creates
        ///     a new <see cref="Frame"/> instance and adds it to the collection of
        ///     frames.
        /// </summary>
        /// <param name="bounds">
        ///     A <see cref="Rectangle"/> instance that describes the boundry
        ///     of the frame relative to the spritesheet.
        /// <param name="duration">
        ///     The amount of time, in seconds, the frame should be displayed
        /// </param>
        public void AddFrame(Rectangle bounds, float duration)
        {
            Frames.Add(new Frame(bounds, duration));
        }

        /// <summary>
        ///     Given the boundry values of a frame and the duration, in seconds, creates
        ///     a new <see cref="Frame"/> instance and adds it to the collection of
        ///     frames.
        /// </summary>
        /// <param name="x">
        ///     The top-left x-coordinate position of the frame relative to the
        ///     spritesheet.
        /// </param>
        /// <param name="y">
        ///     The top-left y-coordinate position of the frame relative to the
        ///     spritesheet.
        /// </param>
        /// <param name="width">
        ///     The width, in pixels, of the frame.
        /// </param>
        /// <param name="height">
        ///     The height, in pixels, of the frame.
        /// </param>
        /// <param name="duration">
        ///     The amount of time, in seconds, that the frame should be displayed.
        /// </param>
        public void AddFrame(int x, int y, int width, int height, float duration)
        {
            Frames.Add(new Frame(x, y, width, height, duration));
        }

        /// <summary>
        ///     Given a collection of <see cref="Frame"/> instances, add it to the
        ///     collection of frames.
        ///     Adds the collection of <see cref="Frame"/> values to the collection of frames
        /// </summary>
        /// <param name="frames">
        ///     The collection of <see cref="Frame"/> instances to add.
        /// </param>
        public void AddFrames(IEnumerable<Frame> frames)
        {
            foreach (Frame frame in frames)
            {
                Frames.Add(frame);
            }
        }

        /// <summary>
        ///     Given <see cref="Frame"/> instnaces, adds them to the collectio of frames.
        /// </summary>
        /// <param name="frames">
        ///     The <see cref="Frame"/> instances to add.
        /// </param>
        public void AddFrames(params Frame[] frames)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                Frames.Add(frames[i]);
            }
        }

        /// <summary>
        ///     Given a <see cref="Slice"/> instance, adds it to the collection
        ///     of slices.
        /// </summary>
        /// <param name="slice">
        ///     The <see cref="Slice"/> instance to add.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(Slice slice)
        {
            Slices.Add(slice.Name, slice);
        }

        /// <summary>
        ///     Given the name and the key-value pair of <see cref="SliceKey"/>
        ///     instances, creates a new <see cref="Slice"/> instance and adds it
        ///     to the collection of slices.
        /// </summary>
        /// <param name="name">
        ///     The name of the <see cref="Slice"/>.
        /// </param>
        /// <param name="keys">
        ///     The key-value pair collection of <see cref="SliceKey"/> instances
        ///     that belong to the slice.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, Dictionary<int, SliceKey> keys)
        {
            Slices.Add(name, new Slice(name, keys));
        }

        /// <summary>
        ///     Given the name and a collection of <see cref="SliceKey"/> instances,
        ///     creates a new <see cref="Slice"/> instance and adds it to the
        ///     collection of slices.
        /// </summary>
        /// <param name="name">
        ///     The name of the <see cref="Slice"/>.
        /// </param>
        /// <param name="keys">
        ///     The collection of <see cref="SliceKey"/> instances that
        ///     belong to the slice.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, IEnumerable<SliceKey> keys)
        {
            Slices.Add(name, new Slice(name, keys));
        }

        /// <summary>
        ///     Given the name and <see cref="SliceKey"/> instances,
        ///     creates a new <see cref="Slice"/> instance and adds it to the
        ///     collection of slices.
        /// </summary>
        /// <param name="name">
        ///     The name of the <see cref="Slice"/>.
        /// </param>
        /// <param name="keys">
        ///     The <see cref="SliceKey"/> instances that belong to the slice.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, params SliceKey[] keys)
        {
            Slices.Add(name, new Slice(name, keys));
        }

        /// <summary>
        ///     Given the name, color, and the key-value pair of <see cref="SliceKey"/>
        ///     instances, creates a new <see cref="Slice"/> instance and adds it
        ///     to the collection of slices.
        /// </summary>
        /// <param name="name">
        ///     The name of the <see cref="Slice"/>.
        /// </param>
        /// <param name="color">
        ///     The color of the <see cref="Slice"/>.
        /// </param>
        /// <param name="keys">
        ///     The key-value pair collection of <see cref="SliceKey"/> instances
        ///     that belong to the slice.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, Color color, Dictionary<int, SliceKey> keys)
        {
            Slices.Add(name, new Slice(name, color, keys));
        }

        /// <summary>
        ///     Given the name, color, and collection of <see cref="SliceKey"/>
        ///     instances, creates a new <see cref="Slice"/> instance and adds it
        ///     to the collection of slices.
        /// </summary>
        /// <param name="name">
        ///     The name of the <see cref="Slice"/>.
        /// </param>
        /// <param name="color">
        ///     The color of the <see cref="Slice"/>.
        /// </param>
        /// <param name="keys">
        ///     The collection of <see cref="SliceKey"/> instances that belong
        ///     to the slice.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, Color color, IEnumerable<SliceKey> keys)
        {
            Slices.Add(name, new Slice(name, color, keys));
        }

        /// <summary>
        ///     Given the name, color, and <see cref="SliceKey"/> instances,
        ///     creates a new <see cref="Slice"/> instance and adds it to the
        ///     collection of slices.
        /// </summary>
        /// <param name="name">
        ///     The name of the <see cref="Slice"/>.
        /// </param>
        /// <param name="color">
        ///     The color of the <see cref="Slice"/>.
        /// </param>
        /// <param name="keys">
        ///     The <see cref="SliceKey"/> instances that belong to the slice.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        /// </exception>
        public void AddSlice(string name, Color color, params SliceKey[] keys)
        {
            Slices.Add(name, new Slice(name, color, keys));
        }

        /// <summary>
        ///     Given a collection of <see cref="Slice"/> instances, adds them to the
        ///     collection of slices.
        /// </summary>
        /// <param name="slices">
        ///     The collection of <see cref="Slice"/> instances to add.
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        ///     or if any of the slices in the given collection have the same name
        /// </exception>
        public void AddSlices(IEnumerable<Slice> slices)
        {
            foreach (Slice slice in slices)
            {
                Slices.Add(slice.Name, slice);
            }
        }

        /// <summary>
        ///     Given <see cref="Slice"/> instances, add them to the colleciton of
        ///     slices.
        /// </summary>
        /// <param name="slices">
        ///     The <see cref="Slice"/> instances to add.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the slice collection already contains a slice with the same name
        ///     or if any of the slices in the given collection have the same name
        /// </exception>
        public void AddSlices(params Slice[] slices)
        {
            for (int i = 0; i < slices.Length; i++)
            {
                Slices.Add(slices[i].Name, slices[i]);
            }
        }
    }
}
