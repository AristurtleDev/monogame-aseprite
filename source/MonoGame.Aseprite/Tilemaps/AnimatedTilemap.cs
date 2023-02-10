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
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Tilemaps;

/// <summary>
/// Defines a <see cref="AnimatedTilemap"/> consisting of <see cref="AnimatedTilemapFrame"/> elements
/// </summary>
public sealed class AnimatedTilemap : IEnumerable<AnimatedTilemapFrame>
{
    private bool _hasBegun;
    private int _currentIndex;
    private int _direction;
    private List<AnimatedTilemapFrame> _frames = new();

    /// <summary>
    ///     Gets the name assigned to this <see cref="AnimatedTilemap"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the total number of <see cref="AnimatedTilemapFrame"/> elements in this <see cref="AnimatedTilemap"/>.
    /// </summary>
    public int frameCount => _frames.Count;

    /// <summary>
    ///     Gets the <see cref="AnimatedTilemapFrame"/> element at the specified index in this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="AnimatedTilemapFrame"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimatedTilemapFrame"/> element that was located at the specified index in this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of 
    ///     <see cref="AnimatedTilemapFrame"/> elements in this <see cref="AnimatedTilemap"/>.
    /// </exception>
    public AnimatedTilemapFrame this[int index] => GetFrame(index);

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AnimatedTilemap"/> is currently paused.
    /// </summary>
    public bool IsPaused { get; private set; }

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AnimatedTilemap"/> is currently animating.
    /// </summary>
    public bool IsAnimating { get; private set; }

    /// <summary>
    ///     Gets a value that indicates whether the animation this <see cref="AnimatedTilemap"/> should loop.
    /// </summary>
    public bool IsLooping { get; }

    /// <summary>
    ///     Gets a value that indicates whether the animation this <see cref="AnimatedTilemap"/> should play frames 
    ///     in reverse order.
    /// </summary>
    public bool IsReversed { get; }

    /// <summary>
    ///     Gets a value that indicates whether the animation for this <see cref="AnimatedTilemap"/> should ping-pong 
    ///     once reaching the last frame of animation.
    /// </summary>
    public bool IsPingPong { get; }

    /// <summary>
    ///     Gets the source <see cref="AnimatedTilemapFrame"/> element for the current animation frame.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no <see cref="AnimatedTilemapFrame"/> elements have been added to this 
    ///     <see cref="AnimatedTilemap"/> prior to accessing this property.
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
    ///     Gets or Sets an <see cref="Action"/> method to invoke at the start of each animation frame.
    /// </summary>
    public Action<AnimatedTilemap>? OnFrameBegin { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> method to invoke at the end of each animation frame.
    /// </summary>
    public Action<AnimatedTilemap>? OnFrameEnd { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> method to invoke at the start of the animation.  This will trigger only
    ///     once when the animation starts before the first frame's <see cref="OnFrameBegin"/> triggers.
    /// </summary>
    public Action<AnimatedTilemap>? OnAnimationBegin { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> method to invoke each time the animation loops.  This will trigger each
    ///     time the animation loops after the last frame's <see cref="OnFrameEnd"/> triggers.
    /// </summary>
    public Action<AnimatedTilemap>? OnAnimationLoop { get; set; } = default;

    /// <summary>
    ///     Gets or Sets an <see cref="Action"/> method to invoke when the animation ends.  This will only trigger when 
    ///     the animation ends in a non-looping animation, or if a looping animation is stopped by calling the 
    ///     <see cref="Stop"/> method manually.
    /// </summary>
    public Action<AnimatedTilemap>? OnAnimationEnd { get; set; } = default;

    /// <summary>
    ///     Gets the amount of time remaining for the <see cref="CurrentFrame"/> before moving to the next frame.
    /// </summary>
    public TimeSpan CurrentFrameTimeRemaining { get; private set; }


    /// <summary>
    ///     Initializes a new instance of the <see cref="AnimatedTilemap"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="isLooping">
    ///     Indicates whether the animation for the <see cref="AnimatedTilemap"/> should loop
    /// </param>
    /// <param name="isReversed">
    ///     Indicates whether the frames for the <see cref="AnimatedTilemap"/> should play in reverse order.
    /// </param>
    /// <param name="isPingPong">
    ///     Indicates whether the animation for this <see cref="AnimatedTilemap"/> should ping-pong once reaching the 
    ///     last frame of animation
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
    ///     Updates this <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <remarks>
    ///     This should only be called once per game update cycle.
    /// </remarks>
    /// <param name="deltaTimeInMilliseconds">
    ///     The amount of time, in milliseconds, that have elapsed since the last update cycle in the game.
    /// </param>
    public void Update(float deltaTimeInMilliseconds)
    {
        GameTime fakeGameTime = new();
        fakeGameTime.ElapsedGameTime = TimeSpan.FromMilliseconds(deltaTimeInMilliseconds);
        Update(fakeGameTime);
    }

    /// <summary>
    ///     Updates this <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <remarks>
    ///     This should only be called once per game update cycle.
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
    ///     Pauses this <see cref="AnimatedTilemap"/> and prevents it from being updated until it is unpaused.
    /// </summary>
    /// <param name="resetFrameDuration">
    ///     A value that indicates whether the the duration of the <see cref="CurrentFrame"/> should be reset.  When 
    ///     this method returns <see langword="false"/>, the duration will not be reset even if this is specified as
    ///     <see langword="true"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> this <see cref="AnimatedTilemap"/> was successfully paused; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> this <see cref="AnimatedTilemap"/> 
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
    ///     Unpauses this <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <param name="advanceToNextFrame">
    ///     A value that indicates whether this <see cref="AnimatedTilemap"/> should immediately be advanced to the next
    ///     frame after unpausing.  When this method returns <see langword="false"/>, this <see cref="AnimatedTilemap"/>
    ///     will -not- be advanced to the next frame, even if this was specified as <see langword="true"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="AnimatedTilemap"/> was successfully unpaused; otherwise, 
    ///     <see langword="false"/>.  This method return <see langword="false"/> this <see cref="AnimatedTilemap"/> is 
    ///     not currently animating or if it has not already been paused.
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
    ///     Stops this <see cref="AnimatedTilemap"/> on the <see cref="CurrentFrame"/>.  This will trigger the 
    ///     <see cref="OnAnimationEnd"/> if one was set.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> this <see cref="AnimatedTilemap"/> was successfully stopped; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> this <see cref="AnimatedTilemap"/> is 
    ///     not currently animating.  If this method returns <see langword="false"/>, this indicates that the 
    ///     <see cref="OnAnimationEnd"/> action method was not invoked.
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
    ///     Resets this <see cref="AnimatedTilemap"/> back to its first frame of animation.
    /// </summary>
    /// <param name="paused">
    ///     A value that indicates whether his <see cref="AnimatedTilemap"/> should be paused after it is reset.
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
    ///     Creates and adds a new <see cref="AnimatedTilemapFrame"/> element as the next frame of animation in this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <param name="duration">
    ///     The duration to assign the <see cref="AnimatedTilemapFrame"/> created.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimatedTilemapFrame"/> created.
    /// </returns>
    public AnimatedTilemapFrame CreateFrame(TimeSpan duration)
    {
        AnimatedTilemapFrame frame = new(duration);
        AddFrame(frame);
        return frame;
    }

    /// <summary>
    ///     Adds the given <see cref="AnimatedTilemapFrame"/> as the next frame of animation in this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <param name="frame">
    ///     The <see cref="AnimatedTilemapFrame"/> to add
    /// </param>
    public void AddFrame(AnimatedTilemapFrame frame) => _frames.Add(frame);

    /// <summary>
    ///     Gets the <see cref="AnimatedTilemapFrame"/> element at the specified index in this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="AnimatedTilemapFrame"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimatedTilemapFrame"/> element that was located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of 
    ///     <see cref="AnimatedTilemapFrame"/> elements in this <see cref="AnimatedTilemap"/>.
    /// </exception>
    public AnimatedTilemapFrame GetFrame(int index)
    {
        if (index < 0 || index >= frameCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of animated tilemap frame elements in this animated tilemap.");
        }

        return _frames[index];
    }

    /// <summary>
    ///     Gets the <see cref="AnimatedTilemapFrame"/> element at the specified index in this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="AnimatedTilemapFrame"/> element to locate.
    /// </param>
    /// <param name="frame">
    ///     When this method returns <see langword="true"/>, contains the <see cref="AnimatedTilemapFrame"/> located;
    ///    otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="AnimatedTilemapFrame"/> element was located; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> when the specified index is less than
    ///     zero or is greater than or equal to the total number of <see cref="AnimatedTilemapFrame"/> elements in this 
    ///     <see cref="AnimatedTilemap"/>.
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
    ///     Removes the <see cref="AnimatedTilemapFrame"/> element at the specified index from this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="AnimatedTilemapFrame"/> element to remove.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="AnimatedTilemapFrame"/> was removed successfully; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> when the specified index is less than 
    ///     zero or is greater that or equal to the total number of <see cref="AnimatedTilemapFrame"/> elements in this 
    ///     <see cref="AnimatedTilemap"/>.
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
    ///     Removes all <see cref="AnimatedTilemapFrame"/> elements from this <see cref="AnimatedTilemap"/>.
    /// </summary>
    public void Clear() => _frames.Clear();

    /// <summary>
    ///     Draws this <see cref="AnimatedTilemap"/> using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="AnimatedTilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="AnimatedTilemap"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color) =>
        Draw(spriteBatch, position, color, Vector2.One, 0.0f);

    /// <summary>
    ///     Draws this <see cref="AnimatedTilemap"/> using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="AnimatedTilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering this <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="AnimatedTilemap"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float scale, float layerDepth) =>
        Draw(spriteBatch, position, color, new Vector2(scale, scale), layerDepth);

    /// <summary>
    ///     Draws this <see cref="AnimatedTilemap"/> using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the 
    ///     <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="animatedTilemap">
    ///     The <see cref="AnimatedTilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="AnimatedTilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, Vector2 scale, float layerDepth) =>
        spriteBatch.Draw(this, position, color, scale, layerDepth);

    /// <summary>
    ///     Returns an enumerator used to iterate through all of the <see cref="AnimatedTilemapFrame"/> elements in this 
    ///     <see cref="AnimatedTilemap"/>.  The order of elements in the enumeration is from first frame to last frame.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all of the <see cref="AnimatedTilemapFrame"/> elements in this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </returns>
    public IEnumerator<AnimatedTilemapFrame> GetEnumerator() => _frames.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator used to iterate through all of the <see cref="AnimatedTilemapFrame"/> elements in this 
    ///     <see cref="AnimatedTilemap"/>.  The order of elements in the enumeration is from first frame to last frame.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all of the <see cref="AnimatedTilemapFrame"/> elements in this 
    ///     <see cref="AnimatedTilemap"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    ///     Creates a new instance of the <see cref="AnimatedTilemap"/> class from the given 
    ///     <see cref="RawAnimatedTilemap"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> used to create graphical resources.
    /// </param>
    /// <param name="rawTilemap">
    ///     The <see cref="RawAnimatedTilemap"/> to create the <see cref="AnimatedTilemap"/> from.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimatedTilemap"/> created by this method.
    /// </returns>
    public static AnimatedTilemap FromRaw(GraphicsDevice device, RawAnimatedTilemap rawTilemap)
    {
        AnimatedTilemap animatedTilemap = new(rawTilemap.Name);

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
            AnimatedTilemapFrame animatedTilemapFrame = animatedTilemap.CreateFrame(duration);

            for (int l = 0; l < rawFrame.RawTilemapLayers.Length; l++)
            {
                RawTilemapLayer rawLayer = rawFrame.RawTilemapLayers[l];

                TilemapLayer layer = animatedTilemapFrame.CreateLayer(rawLayer.Name, tilesetLookup[rawLayer.TilesetID], rawLayer.Columns, rawLayer.Rows, rawLayer.Offset.ToVector2());

                for (int t = 0; t < rawLayer.RawTilemapTiles.Length; t++)
                {
                    RawTilemapTile rawTile = rawLayer.RawTilemapTiles[t];

                    layer.SetTile(t, rawTile.TilesetTileID, rawTile.FlipVertically, rawTile.FlipHorizontally, rawTile.Rotation);
                }
            }
        }

        return animatedTilemap;
    }

}
