/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

public sealed class SpriteSheetAnimation
{
    private int _currentIndex;
    private int _direction;

    public string Name { get; }
    public bool IsLooping { get; set; }
    public bool IsReversed { get; set; }
    public bool IsPingPong { get; set; }
    public bool IsPaused { get; set; }
    public bool IsAnimating { get; private set; }
    public SpriteSheetFrame[] Frames { get; }
    public SpriteSheetFrame CurrentFrame => Frames[_currentIndex];
    public Action? OnFrameBegin { get; set; } = default;
    public Action? OnFrameEnd { get; set; } = default;
    public Action? OnAnimationLoop { get; set; } = default;
    public Action? OnAnimationEnd { get; set; } = default;
    // public SpriteSheetAnimationFrame[] Frames { get; }
    // public SpriteSheetAnimationFrame CurrentFrame => Frames[_currentIndex];

    public TimeSpan CurrentFrameTimeRemaining { get; private set; }

    public SpriteSheetAnimation(string name, SpriteSheetFrame[] frames, bool isLooping = true, bool isReversed = false, bool isPingPong = false)
    {
        Name = name;
        IsLooping = isLooping;
        IsReversed = isReversed;
        IsPingPong = isPingPong;
        IsAnimating = true;
        Frames = frames;

        if (isReversed)
        {
            _direction = -1;
            _currentIndex = frames.Length - 1;
        }
        else
        {
            _direction = 1;
            _currentIndex = 0;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (IsAnimating && !IsPaused)
        {
            if (CurrentFrameTimeRemaining == CurrentFrame.Duration)
            {
                OnFrameBegin?.Invoke();
            }

            CurrentFrameTimeRemaining -= gameTime.ElapsedGameTime;

            if (CurrentFrameTimeRemaining <= TimeSpan.Zero)
            {
                AdvanceFrame();
            }
        }
    }

    private void AdvanceFrame()
    {
        OnFrameEnd?.Invoke();

        _currentIndex += _direction;

        switch (IsReversed, IsPingPong)
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
        if (_currentIndex >= Frames.Length)
        {
            if (IsLooping)
            {
                _currentIndex = 0;
                OnAnimationLoop?.Invoke();
            }
            else
            {
                _currentIndex = Frames.Length - 1;
                IsAnimating = false;
                OnAnimationEnd?.Invoke();
            }
        }
    }

    private void ReverseLoopCheck()
    {
        if (_currentIndex < 0)
        {
            if (IsLooping)
            {
                _currentIndex = Frames.Length - 1;
                OnAnimationLoop?.Invoke();
            }
            else
            {
                _currentIndex = 0;
                IsAnimating = false;
                OnAnimationEnd?.Invoke();
            }
        }
    }

    private void PingPongLoopCheck()
    {
        if (_currentIndex < 0 || _currentIndex >= Frames.Length)
        {
            _direction = -_direction;

            if (_direction == -1)
            {
                _currentIndex = Frames.Length - 2;
            }
            else
            {
                if (IsLooping)
                {
                    _currentIndex = 1;
                    OnAnimationLoop?.Invoke();
                }
                else
                {
                    _currentIndex = 0;
                    IsAnimating = false;
                    OnAnimationEnd?.Invoke();
                }
            }
        }
    }

    private void ReversePingPongLoopCheck()
    {
        if (_currentIndex < 0 || _currentIndex >= Frames.Length)
        {
            _direction = -_direction;

            if (_direction == 1)
            {
                _currentIndex = 1;
            }
            else
            {
                if (IsLooping)
                {
                    _currentIndex = Frames.Length - 2;
                    OnAnimationLoop?.Invoke();
                }
                else
                {
                    _currentIndex = Frames.Length - 1;
                    IsAnimating = false;
                    OnAnimationEnd?.Invoke();
                }
            }
        }
    }

    public bool Pause(bool resetFrameDuration = false)
    {
        //  We can only pause something that is animating and is not already
        //  paused.  This is to prevent improper usage that could accidentally
        //  reset frame duration if it was set to true
        if (IsAnimating && !IsPaused)
        {
            IsPaused = true;

            if (resetFrameDuration)
            {
                CurrentFrameTimeRemaining = CurrentFrame.Duration;
            }

            return true;
        }

        return false;
    }

    public bool Unpause(bool advanceToNextFrame = false)
    {
        //  We can't unpause something that's not animating and also isn't
        //  paused.  This is to prevent improper usage that could accidentally
        //  advance to the next frame if it was set to true
        if (IsAnimating && IsPaused)
        {
            IsPaused = false;

            if (advanceToNextFrame)
            {
                AdvanceFrame();
            }

            return true;
        }

        return false;
    }

    public bool Stop()
    {
        //  We can't stop something that's not animating.  This is to prevent
        //  accidentally invoking the OnAnimationEnd action
        if (IsAnimating)
        {
            IsAnimating = false;
            OnAnimationEnd?.Invoke();
            return true;
        }

        return false;
    }
}
