/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

/// <summary>
///     Defines an animated sprite with methods to control the playing of the sprite animation.
/// </summary>
public sealed class AnimatedSprite : Sprite
{
    private int _currentIndex;
    private int _direction;
    private int _loopCount;
    private int _loopsRemaining;
    private bool _hasBegun = false;
    private double _speed = 1.0f;
    private AnimationTag _animationTag;

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AnimatedSprite"/> is currently paused.
    /// </summary>
    public bool IsPaused { get; private set; }

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AnimatedSprite"/> has completed its animation.
    /// </summary>
    public bool IsAnimating { get; private set; }

    /// <summary>
    ///     Gets or Sets a value that indicates if this <see cref="AnimatedSprite"/> plays it's frames in reverse order.
    /// </summary>
    public bool IsReversed
    {
        get => _direction == -1;
        set => _direction = value ? -1 : 1;
    }

    /// <summary>
    ///     Gets or Sets a value that indicates if this <see cref="AnimatedSprite"/> should ping-pong once reaching the
    ///     last frame of animation.
    /// </summary>
    public bool IsPingPong { get; set; }

    /// <summary>
    ///     Gets a value that indicates the total number of loops/cycles of the animation that should play for
    ///     this <see cref="AnimatedSprite"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <c>0</c> = infinite looping
    ///     </para>
    ///     <para>
    ///         If <see cref="AnimationTag.IsPingPong"/> is equal to <see langword="true"/>, each direction of the
    ///         ping-pong will count as a loop.  
    ///     </para>
    /// </remarks>
    public int LoopCount => _loopCount;

    /// <summary>
    ///     Sets the rate at which the animation is played.
    /// </summary>
    /// <remarks>
    ///     This value is clamped between <c>0.0d</c> and <see cref="double.MaxValue"/>
    /// </remarks>
    /// <remarks>
    ///     Default (normal) speed is <c>1.0d</c>
    /// </remarks>
    public double Speed
    {
        get => _speed;
        set
        {
            _speed = Math.Clamp(value, 0, double.MaxValue);
        }
    }

    /// <summary>
    ///     Gets the total number of frames in this <see cref="AnimatedSprite"/>
    /// </summary>
    public int FrameCount => _animationTag.FrameCount;

    /// <summary>
    ///     Gets the source <see cref="AnimationFrame"/> of the current frame of animation for this 
    ///     <see cref="AnimatedSprite"/>.
    /// </summary>
    public AnimationFrame CurrentFrame => _animationTag.Frames[_currentIndex];

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> method to invoke at the start of each frame of animation.
    /// </summary>
    public Action<AnimatedSprite>? OnFrameBegin { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> method to invoke at the end of each frame of animation.
    /// </summary>
    public Action<AnimatedSprite>? OnFrameEnd { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> method to invoke at the start of the animation.
    /// </summary>
    /// <remarks>
    ///     This will trigger only once when the animation starts before the the first frame's 
    ///     <see cref="OnFrameBegin"/> triggers.
    /// </remarks>
    public Action<AnimatedSprite>? OnAnimationBegin { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> to invoke each time the animation loops.
    /// </summary>
    /// <remarks>
    ///     This will trigger each time the animation loops after the last frame's <see cref="OnFrameEnd"/> triggers.
    /// </remarks>
    public Action<AnimatedSprite>? OnAnimationLoop { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> method to invoke when the animation ends.
    /// </summary>
    /// <remarks>
    ///     This will only trigger when the animation ends in a non-looping animation, or if a looping animation is 
    ///     stopped by calling <see cref="Stop"/> manually.
    /// </remarks>
    public Action<AnimatedSprite>? OnAnimationEnd { get; set; } = default;

    /// <summary>
    ///     Gets the amount of time remaining for the <see cref="CurrentFrame"/> before moving to the next frame.
    /// </summary>
    public TimeSpan CurrentFrameTimeRemaining { get; private set; }

    internal AnimatedSprite(AnimationTag tag)
        : base(tag.Name, tag.Frames[0].TextureRegion)
    {
        _animationTag = tag;
        Reset();
    }

    /// <summary>
    ///     Updates this <see cref="AnimatedSprite"/>.
    /// </summary>
    /// <remarks>
    ///     This should only be called once per update cycle.
    /// </remarks>
    /// <param name="deltaTimeInSeconds">
    ///     The amount of time, in seconds, that have elapsed since the last update cycle in the game.
    /// </param>
    public void Update(double deltaTimeInSeconds)
    {
        Update(TimeSpan.FromSeconds(deltaTimeInSeconds));
    }

    /// <summary>
    ///     Updates this <see cref="AnimatedSprite"/>.
    /// </summary>
    /// <remarks>
    ///     This should only be called once per update cycle.
    /// </remarks>
    /// <param name="gameTime">
    ///     A snapshot of the game timing values for the current update cycle.
    /// </param>
    public void Update(GameTime gameTime)
    {
        Update(gameTime.ElapsedGameTime);
    }

    /// <summary>
    ///     Updates this <see cref="AnimatedSprite"/>.
    /// </summary>
    /// <remarks>
    ///     This should only be called once per update cycle.
    /// </remarks>
    /// <param name="elapsedTime">
    ///     The amount of time, that have elapsed since the last update cycle in the game.
    /// </param>
    public void Update(in TimeSpan elapsedTime)
    {
        if (!IsAnimating || IsPaused)
        {
            return;
        }

        if (!_hasBegun)
        {
            _hasBegun = true;
            OnAnimationBegin?.Invoke(this);
        }

        if (CurrentFrameTimeRemaining == CurrentFrame.Duration)
        {
            OnFrameBegin?.Invoke(this);
        }

        CurrentFrameTimeRemaining -= elapsedTime * Speed;

        if (CurrentFrameTimeRemaining <= TimeSpan.Zero)
        {
            AdvanceFrame();
        }
    }

    /// <summary>
    ///     Sets the current frame of animation for this <see cref="AnimatedSprite"/>.
    /// </summary>
    /// <param name="frameIndex">
    ///     The index of the frame to set. Value must be greater than zero and less than the total count of frames. You 
    ///     can use <see cref="FrameCount"/> to determine the total number of frames.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="frameIndex"/> value provided is less than zero or is greater than or equal to
    ///     the total number of frames in this <see cref="AnimatedSprite"/>.
    /// </exception>
    public void SetFrame(int frameIndex)
    {
        if (frameIndex < 0 || frameIndex >= FrameCount)
        {
            throw new ArgumentOutOfRangeException(nameof(frameIndex), $"{nameof(frameIndex)} must be greater than zero and less than the total number of frames in this AnimatedSprite");
        }

        _currentIndex = frameIndex;
        TextureRegion = CurrentFrame.TextureRegion;
        CurrentFrameTimeRemaining = CurrentFrame.Duration;
    }

    private void AdvanceFrame()
    {
        OnFrameEnd?.Invoke(this);

        _currentIndex += _direction;

        if (_currentIndex >= _animationTag.FrameCount || _currentIndex < 0)
        {
            bool shouldLoop = _loopCount == 0 || _loopsRemaining > 1;

            if (shouldLoop)
            {
                ReduceLoopsRemaining();

                if (IsPingPong)
                {
                    _direction = -_direction;

                    //  Adjust the current index again after ping ponging so we don't repeat the 
                    //  same frame twice in a row
                    _currentIndex += _direction * 2;

                }
                else
                {
                    _currentIndex = IsReversed ? _animationTag.FrameCount - 1 : 0;
                }
                OnAnimationLoop?.Invoke(this);
            }
            else
            {
                _currentIndex += -_direction;
                Stop();
            }
        }

        TextureRegion = CurrentFrame.TextureRegion;
        CurrentFrameTimeRemaining = CurrentFrame.Duration;
    }

    private void ReduceLoopsRemaining()
    {
        _loopsRemaining = Math.Max(--_loopsRemaining, 0);
    }

    /// <summary>
    ///     Starts the animation for this <see cref="AnimatedSprite"/>
    /// </summary>
    /// <param name="loopCount">
    ///     <para>
    ///         When a value is provided, specifies the total number of loop/cycles to perform before stopping the
    ///         animation. 
    ///     </para>
    ///     <para>
    ///         When <see langword="null"/> is provided, loop count will default to the value defined in the
    ///         <see cref="AnimationTag"/> used to create this <see cref="AnimatedSprite"/>
    ///     </para>
    ///     <para>
    ///         <c>0</c> = infinite looping
    ///     </para>
    ///     <para>
    ///         If <see cref="AnimationTag.IsPingPong"/> is equal to <see langword="true"/>, each direction of the
    ///         ping-pong will count as a loop.  
    ///     </para>
    /// </param>
    /// <param name="startingFrame">
    ///     <para>
    ///         When this value is provided, specifies the frame to start the animation at
    ///     </para>
    ///     <para>
    ///         When <see langword="null"/> is provided, play will start at frame 0 of the animation.
    ///     </para>
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if animation play was successfully started for this <see cref="AnimatedSprite"/>;
    ///     otherwise, <see langword="false"/>.  This method returns <see langword="false"/> if the animation is already
    ///     playing (when <see cref="IsAnimating"/> equals <see langword="true"/>).
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="startingFrame"/> value provided is less than zero or is greater than or equal to
    ///     the total number of frames in this <see cref="AnimatedSprite"/>.
    /// </exception>
    public bool Play(int? loopCount = default, int? startingFrame = 0)
    {
        if (startingFrame < 0 || startingFrame >= FrameCount)
        {
            throw new ArgumentOutOfRangeException(nameof(startingFrame), $"{nameof(startingFrame)} must be greater than zero and less than the total number of frames in this AnimatedSprite");
        }

        //  Cannot play something that's already playing
        if (IsAnimating)
        {
            return false;
        }

        if (loopCount is null)
        {
            loopCount = _animationTag.LoopCount;
        }

        _loopCount = loopCount.Value;
        _loopsRemaining = _loopCount;

        IsAnimating = true;
        IsPaused = false;

        _currentIndex = startingFrame ?? 0;

        if (IsReversed)
        {
            _currentIndex = _animationTag.Frames.Length - 1;
        }

        TextureRegion = CurrentFrame.TextureRegion;
        CurrentFrameTimeRemaining = CurrentFrame.Duration;
        _hasBegun = false;

        return true;
    }


    /// <summary>
    ///     Paused this <see cref="AnimatedSprite"/> and prevents it from being updated until it is unpaused.
    /// </summary>
    /// <param name="resetFrameDuration">
    ///     A value that indicates whether the <see cref="CurrentFrameTimeRemaining"/> should be reset.  When this
    ///     method returns <see langword="false"/>, this indicates the <see cref="CurrentFrameTimeRemaining"/> was not
    ///     reset even if this was specified as <see langword="true"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="AnimatedSprite"/> was successfully paused; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this <see cref="AnimatedSprite"/>
    ///     is not currently animating or if it is already paused.
    /// </returns>
    public bool Pause(bool resetFrameDuration = false)
    {
        //  We can only pause something that is animating and is not already paused.  This is to prevent improper usage
        //  that could accidentally reset frame duration if it was set to true.
        if (!IsAnimating || IsPaused)
        {
            return false;
        }

        IsPaused = true;

        if (resetFrameDuration)
        {
            CurrentFrameTimeRemaining = CurrentFrame.Duration;
        }

        return true;
    }

    /// <summary>
    ///     Unpaused this <see cref="AnimatedSprite"/>.
    /// </summary>
    /// <param name="advanceToNextFrame">
    ///     A value that indicates whether this <see cref="AnimatedSprite"/> should immediately advance to the next 
    ///     frame after unpausing.  When this method returns <see langword="false"/>, this <see cref="AnimatedSprite"/>
    ///     will -not- be advanced to the next frame, even if this was specified as <see langword="true"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="AnimatedSprite"/> was successfully unpaused; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this <see cref="AnimatedSprite"/>
    ///     is not currently animating or if it is not paused.
    /// </returns>
    public bool Unpause(bool advanceToNextFrame = false)
    {
        //  We can't unpause something that's not animating and also isn't paused.  This is to prevent improper usage
        //  that could accidentally advance to the next frame if it was set to true.
        if (!IsAnimating || !IsPaused)
        {
            return false;
        }

        IsPaused = false;

        if (advanceToNextFrame)
        {
            AdvanceFrame();
        }

        return true;
    }

    /// <summary>
    ///     Stops this <see cref="AnimatedSprite"/> on the current frame.  
    /// </summary>
    /// <remarks>
    ///     This will trigger the <see cref="OnAnimationEnd"/> action if one was set.
    /// </remarks>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="AnimatedSprite"/> was successfully stopped; otherwise,
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this <see cref="AnimatedSprite"/>
    ///     is not currently animating.  If this method returns <see langword="false"/>, this also indicates that the
    ///     <see cref="OnAnimationEnd"/> was not triggered.
    /// </returns>
    public bool Stop()
    {
        //  We can't stop something that's not animating.  This is to prevent
        //  accidentally invoking the OnAnimationEnd action
        if (!IsAnimating)
        {
            return false;
        }

        IsAnimating = false;
        IsPaused = true;
        OnAnimationEnd?.Invoke(this);
        return true;
    }

    /// <summary>
    ///     Resets this <see cref="AnimatedSprite"/> back to its initial state as defined by the 
    ///     <see cref="AnimationTag"/> used to create it.  You will need to call <see cref="Play(int?, int?)"/>
    ///     after resetting to start the playback of the animation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is useful if you've adjusted the <see cref="IsReversed"/> or 
    ///         <see cref="IsPingPong"/> properties, or specified a override to the loop count when
    ///         initially playing the animation.
    ///     </para>
    ///     <para>
    ///         This also resets the <see cref="Speed"/> to <c>1.0d</c>.
    ///     </para>
    /// </remarks>
    public void Reset()
    {
        IsReversed = _animationTag.IsReversed;
        IsPingPong = _animationTag.IsPingPong;
        _loopCount = _animationTag.LoopCount;
        _loopsRemaining = _loopCount;

        IsAnimating = false;
        IsPaused = true;

        Speed = 1.0d;

        _currentIndex = 0;
        if (IsReversed)
        {
            _currentIndex = _animationTag.Frames.Length - 1;
        }

        TextureRegion = CurrentFrame.TextureRegion;
        CurrentFrameTimeRemaining = CurrentFrame.Duration;
        _hasBegun = false;
    }
}
