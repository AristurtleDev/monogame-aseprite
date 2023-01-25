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
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

/// <summary>
/// Defines a tilemap with a tilemap frames.
/// </summary>
public sealed class Tilemap : IEnumerable<TilemapFrame>
{
    private List<TilemapFrame> _frames = new();

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
    public TilemapFrame this[int index] => GetFrame(index);

    /// <summary>
    /// Gets a value that indicates if this tilemap is currently paused.
    /// </summary>
    public bool IsPaused { get; private set; }

    /// <summary>
    /// Gets a value that indicates if this tilemap is currently animating.
    /// </summary>
    public bool IsAnimating { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Tilemap"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="Tilemap"/>.
    /// </param>
    public Tilemap(string name) => Name = name;

    public void Update()

    /// <summary>
    /// Gets the tilemap frame at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap frame to locate.</param>
    /// <returns>The tilemap frame that was located at the specified index in this tilemap.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tilemap
    /// frames in this tilemap.
    /// </exception>
    public TilemapFrame GetFrame(int index)
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
    public bool TryGetFrame(int index, out TilemapFrame? frame)
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
    public IEnumerator<TilemapFrame> GetEnumerator() => _frames.GetEnumerator();

    /// <summary>
    /// Returns an enumerator used to iterate through all of the tilemap frames in this tilemap.  The order of elements
    /// in the enumeration is from first frame to last frame.
    /// </summary>
    /// <returns>
    /// An enumerator used to iterate through all of the tilemap frames in this tilemap.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
