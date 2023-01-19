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
///     An animation based on animation cycles from a spritesheet.
/// </summary>
public sealed class Animation
{
    private int _currentIndex;
    private int _direction;

    /// <summary>
    ///     Gets the animation cycle that is used by this animation.
    /// </summary>
    public AnimationCycle Cycles { get; }

    /// <summary>
    ///     Gets a value that indicates if this animation is paused.
    /// </summary>
    public bool IsPaused { get; set; }

    /// <summary>
    ///     Gets a value that indicates if this animation has completed.
    /// </summary>
    public bool IsAnimating { get; private set; }

    /// <summary>
    ///     Get the current frame of animation.
    /// </summary>
    public AnimationFrame CurrentFrame => Cycles.Frames[_currentIndex];

    /// <summary>
    ///     Gets or Sets an action to perform at the start of each frame.
    /// </summary>
    public Action<Animation>? OnFrameBegin { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an action to perform at the end of each frame.
    /// </summary>
    public Action<Animation>? OnFrameEnd { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an action to perform at the start of the animation.
    ///     This will trigger only once when the animation starts before the
    ///     first frame's OnFrameBegin triggers.
    /// </summary>
    public Action<Animation>? OnAnimationBegin { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an action to perform each time the animation loops.
    ///     This will trigger each time the animation loops after the last
    ///     frame's OnFrameEnd triggers.
    /// </summary>
    public Action<Animation>? OnAnimationLoop { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an action to perform when the animation ends. This
    ///     will only trigger when the animation ends in a non-looping
    ///     animation, or if a looping animation is manually stopped.
    /// </summary>
    public Action<Animation>? OnAnimationEnd { get; set; } = default;

    /// <summary>
    ///     Gets the amount of time remaining for the current frame of animation
    ///     before moving to the next frame.
    /// </summary>
    public TimeSpan CurrentFrameTimeRemaining { get; private set; }

    internal Animation(AnimationCycle animation)
    {
        Cycles = animation;
        Reset();
    }

    /// <summary>
    ///     Updates this animation. This should be called once per game update
    ///     cycle.
    /// </summary>
    /// <param name="deltaTimeInMilliseconds">
    ///     The amount of time, in milliseconds, that have elapsed since the
    ///     last update cycle.
    /// </param>
    public void Update(float deltaTimeInMilliseconds)
    {
        GameTime fakeGameTime = new();
        fakeGameTime.ElapsedGameTime = TimeSpan.FromMilliseconds(deltaTimeInMilliseconds);
        Update(fakeGameTime);
    }

    /// <summary>
    ///     Updates this animation. This should be called once per game update
    ///     cycle.
    /// </summary>
    /// <param name="gameTime">
    ///     A snapshot of the game timing values for the current update cycle.
    /// </param>
    public void Update(GameTime gameTime)
    {
        if (!IsAnimating || IsPaused)
        {
            return;
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

        switch (Cycles.IsReversed, Cycles.IsPingPong)
        {
            case (true, true):
                ReversePingPongLoopCheck();
                break;
            case (true, false):
                ReverseLoopCheck();
                break;
            case (false, true):
                PingPongLoopCheck();
                break;
            case (false, false):
                LoopCheck();
                break;
        }

        CurrentFrameTimeRemaining = CurrentFrame.Duration;
    }

    private void LoopCheck()
    {
        if (_currentIndex >= Cycles.Frames.Length)
        {
            if (Cycles.IsLooping)
            {
                _currentIndex = 0;
                OnAnimationLoop?.Invoke(this);
            }
            else
            {
                _currentIndex = Cycles.Frames.Length - 1;
                Stop();
            }
        }
    }

    private void ReverseLoopCheck()
    {
        if (_currentIndex < 0)
        {
            if (Cycles.IsLooping)
            {
                _currentIndex = Cycles.Frames.Length - 1;
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
        if (_currentIndex < 0 || _currentIndex >= Cycles.Frames.Length)
        {
            _direction = -_direction;

            if (_direction == -1)
            {
                _currentIndex = Cycles.Frames.Length - 2;
            }
            else
            {
                if (Cycles.IsLooping)
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
        if (_currentIndex < 0 || _currentIndex >= Cycles.Frames.Length)
        {
            _direction = -_direction;

            if (_direction == 1)
            {
                _currentIndex = 1;
            }
            else
            {
                if (Cycles.IsLooping)
                {
                    _currentIndex = Cycles.Frames.Length - 2;
                    OnAnimationLoop?.Invoke(this);
                }
                else
                {
                    _currentIndex = Cycles.Frames.Length - 1;
                    Stop();
                }
            }
        }
    }

    /// <summary>
    ///     Pauses this animation and prevents it from being updated until
    ///     it is unpaused.
    /// </summary>
    /// <param name="resetFrameDuration">
    ///     A value that indicates if the duration of the current frame should
    ///     be reset.  When this method returns false, the frame duration will
    ///     not be reset even if this is specified as true.
    /// </param>
    /// <returns>
    ///     true if this animation is successfully paused; otherwise, false.
    ///     This will return false if the animation is not currently animating
    ///     or if the animation is already paused.
    /// </returns>
    public bool Pause(bool resetFrameDuration = false)
    {
        //  We can only pause something that is animating and is not already
        //  paused.  This is to prevent improper usage that could accidentally
        //  reset frame duration if it was set to true
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
    ///     Unpauses this animation .
    /// </summary>
    /// <param name="advanceToNextFrame">
    ///     A value that indicates if this animation should immediately be
    ///     advanced to the next frame.  When this method returns false,
    ///     the animation will not be advanced to the next frame even if this
    ///     was specified as true.
    /// </param>
    /// <returns>
    ///     true if this animation is successfully unpaused; otherwise, false.
    ///     This will return false if the animation is not currently animating
    ///     or if the animation is not paused.
    /// </returns>
    public bool Unpause(bool advanceToNextFrame = false)
    {
        //  We can't unpause something that's not animating and also isn't
        //  paused.  This is to prevent improper usage that could accidentally
        //  advance to the next frame if it was set to true
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
    ///     Ends this animation on the current frame.  This will trigger the
    ///     OnAnimationEnd action if one was set
    /// </summary>
    /// <returns>
    ///     true if this animation was successfully stopped; otherwise, false.
    ///     This will only return false if the animation is not currently
    ///     animating.  When this returns false, this also indicates that the
    ///     OnAnimationEnd action was not triggered.
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

    /// <summary>
    ///     Resets this animation back to it's first frame.
    /// </summary>
    /// <param name="paused">
    ///     A value indicating if the animation should be paused after it si
    ///     reset.
    /// </param>
    public void Reset(bool paused = false)
    {
        IsAnimating = true;
        IsPaused = paused;

        if (Cycles.IsReversed)
        {
            _direction = -1;
            _currentIndex = Cycles.Frames.Length;
        }
        else
        {
            _direction = 1;
            _currentIndex = 0;
        }

        OnAnimationBegin?.Invoke(this);
    }
}
