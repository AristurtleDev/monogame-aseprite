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

using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Tilemaps;

/// <summary>
/// Defines a animated tilemap consisting of frame with tilemap layer.
/// </summary>
public sealed class AnimatedTilemap : IEnumerable<AnimatedTilemapFrame>
{
    private bool _hasBegun;
    private int _currentIndex;
    private int _direction;
    private List<AnimatedTilemapFrame> _frames = new();

    /// <summary>
    /// Gets the name of this tilemap.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the total number of tilemap frames in this tilemap.
    /// </summary>
    public int frameCount => _frames.Count;

    /// <summary>
    /// Gets the tilemap frame at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap frame to locate.</param>
    /// <returns>The tilemap frame that was located at the specified index in this tilemap.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tilemap
    /// frames in this tilemap.
    /// </exception>
    public AnimatedTilemapFrame this[int index] => GetFrame(index);

    /// <summary>
    /// Gets a value that indicates if this tilemap is currently paused.
    /// </summary>
    public bool IsPaused { get; private set; }

    /// <summary>
    /// Gets a value that indicates if this tilemap is currently animating.
    /// </summary>
    public bool IsAnimating { get; private set; }

    /// <summary>
    /// Gets a value that indicates whether the animation for this animated tilemap should loop.
    /// </summary>
    public bool IsLooping { get; }

    /// <summary>
    /// Gets a value that indicates whether the animation for this animated tilemap should play frames in reverse order.
    /// </summary>
    public bool IsReversed { get; }

    /// <summary>
    /// Gets a value that indicates whether the animation for this animated tilemap should ping-pong once reaching the
    /// last frame of animation.
    /// </summary>
    public bool IsPingPong { get; }

    /// <summary>
    /// Gets the source tilemap frame for the current animation frame.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when accessing this property before any frames have been added to this animated tilemap.
    /// </exception>
    public AnimatedTilemapFrame CurrentFrame
    {
        get
        {
            if (_frames.Count == 0)
            {
                throw new InvalidOperationException($"Unable to get current frame as no frames have been added to this animated tilemap.");
            }

            return this[_currentIndex];
        }
    }

    /// <summary>
    /// Gets or Sets an action method to invoke at the start of each animation frame.
    /// </summary>
    public Action<AnimatedTilemap>? OnFrameBegin { get; set; } = default;

    /// <summary>
    /// Gets or Sets an action method to invoke at the end of each animation frame.
    /// </summary>
    public Action<AnimatedTilemap>? OnFrameEnd { get; set; } = default;

    /// <summary>
    /// Gets or Sets an action method to invoke at the start of the animation.  This will trigger only once when the
    /// animation starts before the first frame's OnFrameBegin triggers.
    /// </summary>
    public Action<AnimatedTilemap>? OnAnimationBegin { get; set; } = default;

    /// <summary>
    /// Gets or Sets an action method to invoke each time the animation loops.  This will trigger ech time the animation
    /// loops after the last frame's OnFrameEnd triggers.
    /// </summary>
    public Action<AnimatedTilemap>? OnAnimationLoop { get; set; } = default;

    /// <summary>
    /// Gets or Sets an action method to invoke when the animation ends.  This will only trigger when the animation ends
    /// in a non-looping animation, or if a looping animation is stopped by calling the Stop method manually.
    /// </summary>
    public Action<AnimatedTilemap>? OnAnimationEnd { get; set; } = default;

    /// <summary>
    /// Gets the amount of time remaining for the current frame of animation before moving to the next frame.
    /// </summary>
    public TimeSpan CurrentFrameTimeRemaining { get; private set; }


    /// <summary>
    /// Creates a new animated tilemap.
    /// </summary>
    /// <param name="name">The name to give this animated tilemap.</param>
    /// <param name="isLooping">Indicates whether the animation for this animated tilemap should loop</param>
    /// <param name="isReversed">
    /// Indicates whether the frames for this animated tilemap should play in reverse order.
    /// </param>
    /// <param name="isPingPong">
    /// Indicates whether the animation for this animated tilemap should ping-pong once reaching the last frame of
    /// animation
    /// </param>
    public AnimatedTilemap(string name, bool isLooping = true, bool isReversed = false, bool isPingPong = false)
    {
        Name = name;
        IsLooping = isLooping;
        IsReversed = isReversed;
        IsPingPong = isPingPong;
        Reset();
    }

    /// <summary>
    /// Updates this animated tilemap.  This should only be called once per game update cycle.
    /// </summary>
    /// <param name="deltaTimeInMilliseconds">
    /// The amount of time, in milliseconds, that have elapsed since the last update cycle in the game.
    /// </param>
    public void Update(float deltaTimeInMilliseconds)
    {
        GameTime fakeGameTime = new();
        fakeGameTime.ElapsedGameTime = TimeSpan.FromMilliseconds(deltaTimeInMilliseconds);
        Update(fakeGameTime);
    }

    /// <summary>
    /// Updates this animated tilemap.  This should only be called once per game update cycle.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game timing values for the current update cycle.</param>
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
        if (_currentIndex >= _frames.Count)
        {
            if (IsLooping)
            {
                _currentIndex = 0;
                OnAnimationLoop?.Invoke(this);
            }
            else
            {
                _currentIndex = _frames.Count - 1;
                Stop();
            }
        }
    }

    private void ReverseLoopCheck()
    {
        if (_currentIndex < 0)
        {
            if (IsLooping)
            {
                _currentIndex = _frames.Count - 1;
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
        if (_currentIndex < 0 || _currentIndex >= _frames.Count)
        {
            _direction = -_direction;

            if (_direction == -1)
            {
                _currentIndex = _frames.Count - 2;
            }
            else
            {
                if (IsLooping)
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
        if (_currentIndex < 0 || _currentIndex >= _frames.Count)
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
                    _currentIndex = _frames.Count - 2;
                    OnAnimationLoop?.Invoke(this);
                }
                else
                {
                    _currentIndex = _frames.Count - 1;
                    Stop();
                }
            }
        }
    }

    /// <summary>
    /// Pauses the animation of this animated tilemap and prevents it from being updated until it is unpaused.
    /// </summary>
    /// <param name="resetFrameDuration">
    /// A value that indicates whether the the duration of the current frame of the animation of this animated tilemap
    /// should be reset.  When this method returns false, the duration will not be reset even if this is specified as
    /// true.
    /// </param>
    /// <returns>
    /// true if the animation of this animated tilemap was successfully paused; otherwise, false.  This method returns
    /// false if the animation of this animated tilemap is not currently animating or if it is already paused.
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
    /// Unpauses the animation of this animated tilemap.
    /// </summary>
    /// <param name="advanceToNextFrame">
    /// A value that indicates whether the animation of this animated tilemap should immediately be advanced to the next
    /// frame after unpausing.  When this method returns false, the animation of this animated tilemap will -not- be
    /// advanced to the next frame, even if this was specified as true.
    /// </param>
    /// <returns>
    /// true if the animation of this animated tilemap was successfully unpaused; otherwise, false.  This method return
    /// false if the animation of this animated tilemap is not currently animating or if it has not already been paused.
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
    /// Stops the animation of this animated tilemap on the current frame.  This will trigger the OnAnimationEnd action
    /// method if one was set.
    /// </summary>
    /// <returns>
    /// true if the animation of this animated tilemap was successfully stopped; otherwise, false.  This method returns
    /// false if the animation of this animated tilemap is not currently animating.  If this method returns false, this
    /// indicates that the OnAnimationEnd action method was not invoked.
    /// </returns>
    public bool Stop()
    {
        //  We can't stop something that's not animating.  This is to prevent accidentally invoking the OnAnimationEnd
        //  action
        if (!IsAnimating)
        {
            return false;
        }

        IsAnimating = false;
        OnAnimationEnd?.Invoke(this);
        return true;
    }

    /// <summary>
    /// Resets the animation for this animated tilemap back to its first frame of animation.
    /// </summary>
    /// <param name="paused">
    /// A value that indicates whether the animation for this animated tilemap should be paused after it is reset.
    /// </param>
    public void Reset(bool paused = false)
    {
        IsAnimating = true;
        IsPaused = paused;

        if (IsReversed)
        {
            _direction = -1;
            _currentIndex = _frames.Count;
        }
        else
        {
            _direction = 1;
            _currentIndex = 0;
        }

        _hasBegun = false;
    }

    /// <summary>
    /// Creates and adds a new tilemap frame as the next frame of animation in this tilemap.
    /// </summary>
    /// <param name="duration">The total amount of time the frame is displayed during the animation.</param>
    /// <returns>The tilemap frame created by this method.</returns>
    public AnimatedTilemapFrame CreateFrame(TimeSpan duration)
    {
        AnimatedTilemapFrame frame = new(duration);
        AddFrame(frame);
        return frame;
    }

    /// <summary>
    /// Adds the given tilemap frame as the next frame of animation in this tilemap.
    /// </summary>
    /// <param name="frame">The frame to add</param>
    public void AddFrame(AnimatedTilemapFrame frame) => _frames.Add(frame);

    /// <summary>
    /// Gets the tilemap frame at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap frame to locate.</param>
    /// <returns>The tilemap frame that was located at the specified index in this tilemap.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tilemap
    /// frames in this tilemap.
    /// </exception>
    public AnimatedTilemapFrame GetFrame(int index)
    {
        if (index < 0 || index >= frameCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of tilemap frames in this tilemap.");
        }

        return _frames[index];
    }

    /// <summary>
    /// Gets the tilemap frame at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap frame to locate.</param>
    /// <param name="frame">
    /// When this method returns true, contains the tilemap frame that was located; otherwise, null.
    /// </param>
    /// <returns>
    /// true if the tilemap frame was located; otherwise, false.  This method returns false when the specified index is
    /// less than zero or is greater than or equal to the total number of tilemap frames in this tilemap.
    /// </returns>
    public bool TryGetFrame(int index, out AnimatedTilemapFrame? frame)
    {
        if (index < 0 || index >= frameCount)
        {
            frame = default;
            return false;
        }

        frame = _frames[index];
        return true;
    }

    /// <summary>
    /// Removes the tilemap frame at the specified index from this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap frame to remove.</param>
    /// <returns>
    /// true if the frame was removed successfully; otherwise, false.  This method returns false when the specified
    /// index is less than zero or is greater that or equal to the total number of tilemap frames in this tilemap.
    /// </returns>
    public bool RemoveFrame(int index)
    {
        if (index < 0 || index >= frameCount)
        {
            return false;
        }

        _frames.RemoveAt(index);
        return true;
    }

    /// <summary>
    /// Removes all tilemap frames from this tilemap.
    /// </summary>
    public void Clear() => _frames.Clear();

    /// <summary>
    /// Returns an enumerator used to iterate through all of the tilemap frames in this tilemap.  The order of elements
    /// in the enumeration is from first frame to last frame.
    /// </summary>
    /// <returns>
    /// An enumerator used to iterate through all of the tilemap frames in this tilemap.
    /// </returns>
    public IEnumerator<AnimatedTilemapFrame> GetEnumerator() => _frames.GetEnumerator();

    /// <summary>
    /// Returns an enumerator used to iterate through all of the tilemap frames in this tilemap.  The order of elements
    /// in the enumeration is from first frame to last frame.
    /// </summary>
    /// <returns>
    /// An enumerator used to iterate through all of the tilemap frames in this tilemap.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Creates a new animated tilemap from a raw animated tilemap record.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="rawTilemap">The raw animated tilemap to create the animated tilemap from.</param>
    /// <returns>The animated tilemap created by this process.</returns>
    public static AnimatedTilemap FromRaw(GraphicsDevice device, RawAnimatedTilemap rawTilemap)
    {
        AnimatedTilemap tilemap = new(rawTilemap.Name);

        Dictionary<int, Tileset> tilesetLookup = new();

        for (int i = 0; i < rawTilemap.RawTilesets.Length; i++)
        {
            RawTileset rawTileset = rawTilemap.RawTilesets[i];
            Tileset tileset = Tileset.FromRaw(device, rawTileset);
            tilesetLookup.Add(rawTileset.ID, tileset);
        }

        for (int f = 0; f < rawTilemap.RawTilemapFrames.Length; f++)
        {
            RawTilemapFrame rawFrame = rawTilemap.RawTilemapFrames[f];

            TimeSpan duration = TimeSpan.FromMilliseconds(rawFrame.DurationInMilliseconds);
            AnimatedTilemapFrame tilemapFrame = tilemap.CreateFrame(duration);

            for (int l = 0; l < rawFrame.RawTilemapLayers.Length; l++)
            {
                RawTilemapLayer rawLayer = rawFrame.RawTilemapLayers[l];

                TilemapLayer layer = tilemapFrame.CreateLayer(rawLayer.Name, tilesetLookup[rawLayer.TilesetID], rawLayer.Columns, rawLayer.Rows, rawLayer.Offset.ToVector2());

                for (int t = 0; t < rawLayer.RawTilemapTiles.Length; t++)
                {
                    RawTilemapTile rawTile = rawLayer.RawTilemapTiles[t];

                    layer.SetTile(t, rawTile.TilesetTileID, rawTile.FlipVertically, rawTile.FlipHorizontally, rawTile.Rotation);
                }
            }
        }

        return tilemap;
    }

}
