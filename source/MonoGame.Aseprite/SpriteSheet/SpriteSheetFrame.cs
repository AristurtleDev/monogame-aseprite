// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2018-2023 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */

// using System.Collections;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;

// namespace MonoGame.Aseprite;

// public sealed class TextureRegion : IEnumerable<SpriteSheetFrameRegion>
// {
//     private Dictionary<string, SpriteSheetFrameRegion> _regions = new();

//     public TextureRegion TextureRegion { get; }

//     /// <summary>
//     ///     Gets the duration of this <see cref="TextureRegion"/> when it
//     ///     is used as part of an animation.
//     /// </summary>
//     public TimeSpan Duration { get; }

//     /// <summary>
//     ///     The total number of <see cref="SpriteSheetFrameRegion"/> elements
//     ///     in this <see cref="TextureRegion"/>.
//     /// </summary>
//     public int RegionCount => _regions.Count;

//     internal TextureRegion(TextureRegion textureRegion, TimeSpan duration) =>
//         (TextureRegion, Duration) = (textureRegion, duration);

//     /// <summary>
//     ///     Creates and adds a new <see cref="SpriteSheetFrameRegion"/> to this
//     ///     <see cref="TextureRegion"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name of the <see cref="SpriteSheetFrameRegion"/> to add.
//     /// </param>
//     /// <param name="bounds">
//     ///     The rectangular bounds of the <see cref="SpriteSheetFrameRegion"/>,
//     ///     relative to the upper-left corner of this
//     ///     <see cref="TextureRegion"/>.
//     /// </param>
//     /// <param name="color">
//     ///     The color of the <see cref="SpriteSheetFrameRegion"/>.
//     /// </param>
//     /// <param name="centerBounds">
//     ///     The rectangular bounds of the center of the
//     ///     <see cref="SpriteSheetFrameRegion"/>, relative to the upper-left
//     ///     corner of the <paramref name="bounds"/>, if the
//     ///     <see cref="SpriteSheetFrameRegion"/> is a nine-patch region;
//     ///     otherwise, <see langword="null"/>.
//     /// </param>
//     /// <param name="pivot">
//     ///     The x- and y-coordinate position of the pivot point for the
//     ///     <see cref="SpriteSheetFrameRegion"/>, relative to the upper-left
//     ///     corner of the <paramref name="bounds"/>, if the
//     ///     <see cref="SpriteSheetFrameRegion"/> has a pivot value; otherwise,
//     ///     <see langword="null"/>.
//     /// </param>
//     /// <exception cref="ArgumentException">
//     ///     Thrown if the <paramref name="name"/> given is
//     ///     <see langword="null"/> or an empty string.
//     /// </exception>
//     /// <exception cref="InvalidOperationException">
//     ///     Thrown if this <see cref="TextureRegion"/> already contains a
//     ///     <see cref="SpriteSheetFrameRegion"/> with the
//     ///     <paramref name="name"/> given.
//     /// </exception>
//     public void AddRegion(string name, Rectangle bounds, Color color, Rectangle? centerBounds = default, Point? pivot = default)
//     {
//         if (string.IsNullOrEmpty(name))
//         {
//             throw new ArgumentException($"{nameof(name)} cannot be null or an empty string.", nameof(name));
//         }

//         if (_regions.ContainsKey(name))
//         {
//             throw new InvalidOperationException();
//         }

//         SpriteSheetFrameRegion region = new(name, bounds, color, centerBounds, pivot);
//         _regions.Add(name, region);
//     }

//     /// <summary>
//     ///     Returns the <see cref="SpriteSheetFrameRegion"/> with the specified
//     ///     <paramref name="name"/> in this <see cref="TextureRegion"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name of the <see cref="SpriteSheetFrameRegion"/>.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="SpriteSheetFrameRegion"/> with the specified
//     ///     <paramref name="name"/> in this <see cref="TextureRegion"/>.
//     /// </returns>
//     /// <exception cref="KeyNotFoundException">
//     ///     Thrown if this <see cref="TextureRegion"/> does not contain a
//     ///     <see cref="SpriteSheetFrameRegion"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </exception>
//     public SpriteSheetFrameRegion GetRegion(string name)
//     {
//         if (_regions.TryGetValue(name, out SpriteSheetFrameRegion? region))
//         {
//             return region;
//         }

//         throw new KeyNotFoundException();
//     }

//     /// <summary>
//     ///     Removes the <see cref="SpriteSheetFrameRegion"/> with the specified
//     ///     <paramref name="name"/> from this <see cref="TextureRegion"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name of the <see cref="SpriteSheetFrameRegion"/> to remove.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the <see cref="SpriteSheetFrameRegion"/>
//     ///     was found and removed successfully;
//     ///     otherwise <see langword="false"/>.  This method returns
//     ///     <see langword="false"/> if no <see cref="SpriteSheetFrameRegion"/>
//     ///     was fond in this <see cref="TextureRegion"/>.
//     /// </returns>
//     public bool RemoveRegion(string name) => _regions.Remove(name);

//     /// <summary>
//     ///     Removes all <see cref="SpriteSheetFrameRegion"/> elements from this
//     ///     <see cref="TextureRegion"/>.
//     /// </summary>
//     public void ClearRegions() => _regions.Clear();

//     /// <summary>
//     ///     Returns an enumerator that iterates over the
//     ///     <see cref="SpriteSheetFrameRegion"/> elements in this
//     ///     <see cref="TextureRegion"/>.
//     /// </summary>
//     /// <returns>
//     ///     An enumerator that iterates over the
//     ///     <see cref="SpriteSheetFrameRegion"/> elements in this
//     ///     <see cref="TextureRegion"/>.
//     /// </returns>
//     public IEnumerator<SpriteSheetFrameRegion> GetEnumerator()
//     {
//         foreach (KeyValuePair<string, SpriteSheetFrameRegion> kvp in _regions)
//         {
//             yield return kvp.Value;
//         }
//     }

//     /// <summary>
//     ///     Returns an enumerator that iterates over the
//     ///     <see cref="SpriteSheetFrameRegion"/> elements in this
//     ///     <see cref="TextureRegion"/>.
//     /// </summary>
//     /// <returns>
//     ///     An enumerator that iterates over the
//     ///     <see cref="SpriteSheetFrameRegion"/> elements in this
//     ///     <see cref="TextureRegion"/>.
//     /// </returns>
//     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
// }
