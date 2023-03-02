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

namespace MonoGame.Aseprite.Sprites;

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
        IsReversed = tag.IsReversed;
        IsPingPong = tag.IsPingPong;
        _loopCount = tag.LoopCount;
        _loopsRemaining = _loopCount;

        IsAnimating = false;
        IsPaused = true;

        _currentIndex = 0;

        if (IsReversed)
        {
            _currentIndex = _animationTag.Frames.Length;
        }

        TextureRegion = CurrentFrame.TextureRegion;
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
        GameTime fakeGameTime = new();
        fakeGameTime.ElapsedGameTime = TimeSpan.FromSeconds(deltaTimeInSeconds);
        Update(fakeGameTime);
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

        CurrentFrameTimeRemaining -= gameTime.ElapsedGameTime;

        if (CurrentFrameTimeRemaining <= TimeSpan.Zero)
        {
            AdvanceFrame();
        }
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
                }

                _currentIndex = IsReversed ? _animationTag.FrameCount - 1 : 0;
                OnAnimationLoop?.Invoke(this);
            }
            else
            {
                _currentIndex += -_direction;
                Stop();
            }
        }

        // bool shouldLoop = _loopCount == 0 || _loopsRemaining > 0;

        // switch (IsReversed, IsPingPong)
        // {
        //     case (true, true):
        //         ReversePingPongLoopCheck();
        //         break;
        //     case (true, false):
        //         ReverseLoopCheck();
        //         break;
        //     case (false, true):
        //         PingPongLoopCheck();
        //         break;
        //     case (false, false):
        //         LoopCheck();
        //         break;
        // }

        TextureRegion = CurrentFrame.TextureRegion;
        CurrentFrameTimeRemaining = CurrentFrame.Duration;
    }

    private void LoopCheck()
    {
        if (_currentIndex >= _animationTag.Frames.Length)
        {
            if (_loopCount == 0 || _loopsRemaining > 0)
            {
                ReduceLoopsRemaining();
                _currentIndex = 0;
                OnAnimationLoop?.Invoke(this);
            }
            else
            {
                _currentIndex = _animationTag.Frames.Length - 1;
                Stop();
            }
        }
    }

    private void ReverseLoopCheck()
    {
        if (_currentIndex < 0)
        {
            if (_loopCount == 0 || _loopsRemaining > 0)
            {
                ReduceLoopsRemaining();
                _currentIndex = _animationTag.Frames.Length - 1;
                OnAnimationLoop?.Invoke(this);
            }
            else
            {
                _currentIndex = 0;
                Stop();
            }
        }
    }

    private void PingPongLoopCheck()
    {
        if (_currentIndex < 0 || _currentIndex >= _animationTag.Frames.Length)
        {
            ReduceLoopsRemaining();
        }

        if (_currentIndex < 0 || _currentIndex >= _animationTag.Frames.Length)
        {
            _direction = -_direction;

            if (_direction == -1)
            {
                _currentIndex = _animationTag.Frames.Length - 2;
            }
            else
            {
                if (_loopCount == 0 || _loopsRemaining-- > 0)
                {
                    _currentIndex = 1;
                    OnAnimationLoop?.Invoke(this);
                }
                else
                {
                    _currentIndex = 0;
                    Stop();
                }
            }
        }
    }

    private void ReversePingPongLoopCheck()
    {
        if (_currentIndex < 0 || _currentIndex >= _animationTag.Frames.Length)
        {
            _direction = -_direction;

            if (_direction == 1)
            {
                _currentIndex = 1;
            }
            else
            {
                if (_loopCount == 0 || _loopsRemaining-- > 0)
                {
                    _currentIndex = _animationTag.Frames.Length - 2;
                    OnAnimationLoop?.Invoke(this);
                }
                else
                {
                    _currentIndex = _animationTag.Frames.Length - 1;
                    Stop();
                }
            }
        }
    }

    private void ReduceLoopsRemaining()
    {
        _loopsRemaining = Math.Max(--_loopsRemaining, 0);
    }

    public bool Play(int? loopCount = default)
    {
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

        _currentIndex = 0;

        if (IsReversed)
        {
            _currentIndex = _animationTag.Frames.Length - 1;
        }

        TextureRegion = CurrentFrame.TextureRegion;
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
        OnAnimationEnd?.Invoke(this);
        return true;
    }

    // /// <summary>
    // ///     Resets this <see cref="AnimatedSprite"/> back to its first frame of animation.
    // /// </summary>
    // /// <param name="paused">
    // ///     A value that indicates whether this <see cref="AnimatedSprite"/> should be paused after it is reset.
    // /// </param>
    // public void Reset(bool paused = false)
    // {
    //     IsAnimating = true;
    //     IsPaused = paused;

    //     if (IsReversed)
    //     {
    //         _direction = -1;
    //         _currentIndex = _animationTag.Frames.Length;
    //     }
    //     else
    //     {
    //         _direction = 1;
    //         _currentIndex = 0;
    //     }

    //     TextureRegion = CurrentFrame.TextureRegion;
    //     OnAnimationBegin?.Invoke(this);
    // }
}
