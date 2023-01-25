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
// using System.Diagnostics.CodeAnalysis;
// using Microsoft.Xna.Framework.Graphics;

// namespace MonoGame.Aseprite;

// /// <summary>
// ///     Defines a collection of uniquely named <see cref="Tileset"/> elements.
// /// </summary>
// public sealed class TilesetCollection : IEnumerable<Tileset>
// {
//     private List<Tileset> _tilesets = new();
//     private Dictionary<string, Tileset> _tilesetByName = new();

//     /// <summary>
//     ///     Gets the total number of <see cref="Tileset"/> elements in this <see cref="TilesetCollection"/>.
//     /// </summary>
//     public int Count => _tilesets.Count;

//     /// <summary>
//     ///     Gets the <see cref="Tileset"/> element from this <see cref="TilesetCollection"/> at the specified index.
//     /// </summary>
//     /// <param name="tilesetIndex">
//     ///     The index of the <see cref="Tileset"/> element in this <see cref="TilesetCollection"/> to locate.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="Tileset"/> element that was located at the specified index in this
//     ///     <see cref="TilesetCollection"/>.
//     /// </returns>
//     /// <exception cref="ArgumentOutOfRangeException">
//     ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
//     ///     <see cref="Tileset"/> elements in this <see cref="TilesetCollection"/>.  Use
//     ///     <see cref="TilesetCollection.Count"/> to determine the total number of elements.
//     /// </exception>
//     public Tileset this[int tilesetIndex] => GetTileset(tilesetIndex);

//     /// <summary>
//     ///     Gets the <see cref="Tileset"/> element from this <see cref="TilesetCollection"/> with the specified name.
//     /// </summary>
//     /// <param name="tilesetName">
//     ///     The name of the <see cref="Tileset"/> element in this <see cref="TilesetCollection"/> to locate.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="Tileset"/> element that was located with the specified name in this
//     ///     <see cref="SpriteSheet"/>.
//     /// </returns>
//     /// <exception cref="KeyNotFoundException">
//     ///     Thrown if this <see cref="TilesetCollection"/> does not contain a <see cref="Tileset"/> element with the
//     ///     specified name.
//     /// </exception>
//     public Tileset this[string tilesetName] => GetTileset(tilesetName);

//     /// <summary>
//     ///     Initializes a new instance of the <see cref="TilesetCollection"/> class.
//     /// </summary>
//     public TilesetCollection() { }

//     /// <summary>
//     ///     Creates a new <see cref="Tileset"/> and adds it to this <see cref="TilesetCollection"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name to give the <see cref="Tileset"/> that is created by this method.  Must be unique for this
//     ///     <see cref="TilesetCollection"/>.
//     /// </param>
//     /// <param name="texture">
//     ///     The <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> that is represented by the
//     ///     <see cref="Tileset"/> created by this method.
//     /// </param>
//     /// <param name="tileWidth">
//     ///     The width, in pixels, to set for each tile in the <see cref="Tileset"/> created by this method.
//     /// </param>
//     /// <param name="tileHeight">
//     ///     The height, in pixels, to set for each tile in the <see cref="Tileset"/> created by this method.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="Tileset"/> that is created by this method.
//     /// </returns>
//     /// <exception cref="InvalidOperationException">
//     ///     Thrown if this <see cref="TilesetCollection"/> already contains a <see cref="Tileset"/> element with the
//     ///     specified name.
//     /// </exception>
//     public Tileset CreateTileset(string name, Texture2D texture, int tileWidth, int tileHeight)
//     {
//         Tileset tileset = new(name, texture, tileWidth, tileHeight);
//         AddTileset(tileset);
//         return tileset;
//     }

//     /// <summary>
//     ///     Adds te given <see cref="Tileset"/> to this <see cref="TilesetCollection"/>.
//     /// </summary>
//     /// <param name="tileset">
//     ///     The <see cref="Tileset"/> to add.
//     /// </param>
//     /// <exception cref="InvalidOperationException">
//     ///     Thrown if this <see cref="TilesetCollection"/> already contains a <see cref="Tileset"/> element with the
//     ///     specified name.
//     /// </exception>
//     public void AddTileset(Tileset tileset)
//     {
//         if (_tilesetByName.ContainsKey(tileset.Name))
//         {
//             throw new InvalidOperationException($"This {nameof(TilesetCollection)} already contains a {nameof(Tileset)} with the name '{tileset.Name}'");
//         }

//         _tilesets.Add(tileset);
//         _tilesetByName.Add(tileset.Name, tileset);
//     }

//     /// <summary>
//     ///     Returns a value that indicates whether this <see cref="TilesetCollection"/> contains a <see cref="Tileset"/>
//     ///     element with the specified name.
//     /// </summary>
//     /// <param name="tilesetName">
//     ///     The name of the <see cref="Tileset"/> element to locate in this <see cref="TilesetCollection"/>.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if this <see cref="TilesetCollection"/> contains a <see cref="Tileset"/> with the
//     ///     specified name; otherwise, <see langword="false"/>.
//     /// </returns>
//     public void ContainsTileset(string tilesetName) => _tilesetByName.ContainsKey(tilesetName);

//     /// <summary>
//     ///     Gets the <see cref="Tileset"/> element from this <see cref="TilesetCollection"/> at the specified index.
//     /// </summary>
//     /// <param name="tilesetIndex">
//     ///     The index of the <see cref="Tileset"/> element in this <see cref="TilesetCollection"/> to locate.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="Tileset"/> element that was located at the specified index in this
//     ///     <see cref="TilesetCollection"/>.
//     /// </returns>
//     /// <exception cref="ArgumentOutOfRangeException">
//     ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
//     ///     <see cref="Tileset"/> elements in this <see cref="TilesetCollection"/>.  Use
//     ///     <see cref="TilesetCollection.Count"/> to determine the total number of elements.
//     /// </exception>
//     public Tileset GetTileset(int tilesetIndex)
//     {
//         if (tilesetIndex < 0 || tilesetIndex >= _tilesets.Count)
//         {
//             throw new ArgumentOutOfRangeException(nameof(tilesetIndex), $"{nameof(tilesetIndex)} cannot be less than zero or greater than or equal to the number of tilesets in this {nameof(TilesetCollection)}.");
//         }

//         return _tilesets[tilesetIndex];
//     }

//     /// <summary>
//     ///     Gets the <see cref="Tileset"/> element from this <see cref="TilesetCollection"/> with the specified name.
//     /// </summary>
//     /// <param name="tilesetName">
//     ///     The name of the <see cref="Tileset"/> element in this <see cref="TilesetCollection"/> to locate.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="Tileset"/> element that was located with the specified name in this
//     ///     <see cref="TilesetCollection"/>.
//     /// </returns>
//     /// <exception cref="KeyNotFoundException">
//     ///     Thrown if this <see cref="TilesetCollection"/> does not contain a <see cref="Tileset"/> element with the
//     ///     specified name.
//     /// </exception>
//     public Tileset GetTileset(string tilesetName)
//     {
//         if (_tilesetByName.TryGetValue(tilesetName, out Tileset? tileset))
//         {
//             return tileset;
//         }

//         throw new KeyNotFoundException($"No {nameof(Tileset)} with the name '{tilesetName}' is present in this {nameof(TilesetCollection)}.");
//     }

//     /// <summary>
//     ///     Gets the <see cref="Tileset"/> element from this <see cref="TilesetCollection"/> at the specified index.
//     /// </summary>
//     /// <param name="tilesetIndex">
//     ///     The index of the <see cref="Tileset"/> element in this <see cref="TilesetCollection"/> to locate.
//     /// </param>
//     /// <param name="tileset">
//     ///     When this method returns <see langword="true"/>, contains the <see cref="Tileset"/> located; otherwise,
//     ///     <see langword="null"/>.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if a <see cref="Tileset"/> element was located at the specified index; otherwise,
//     ///     <see langword="false"/>.  This method returns <see langword="false"/> if the index specified is less than
//     ///     zero or is greater than or equal to the total number of <see cref="Tileset"/> elements in this
//     ///     <see cref="TilesetCollection"/>.  Use <see cref="TilesetCollection.Count"/> to determine the total number of
//     ///     elements.
//     /// </returns>
//     public bool TryGetTileset(int tilesetIndex, [NotNullWhen(true)] out Tileset? tileset)
//     {
//         tileset = default;

//         if (tilesetIndex < 0 || tilesetIndex >= _tilesets.Count)
//         {
//             return false;
//         }

//         tileset = _tilesets[tilesetIndex];
//         return true;
//     }

//     /// <summary>
//     ///     Gets the <see cref="Tileset"/> element from this <see cref="TilesetCollection"/> with the specified name.
//     /// </summary>
//     /// <param name="tilesetName">
//     ///     The name of the <see cref="Tileset"/> element in this <see cref="TilesetCollection"/> to locate.
//     /// </param>
//     /// <param name="tileset">
//     ///     When this method returns <see langword="true"/>, contains the <see cref="Tileset"/> located; otherwise,
//     ///     <see langword="null"/>.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if a <see cref="Tileset"/> element was located with the specified name; otherwise,
//     ///     <see langword="false"/>.  This method returns <see langword="false"/> if this
//     ///     <see cref="TilesetCollection"/> does not contain a <see cref="Tileset"/> element with the specified name.
//     /// </returns>
//     public bool TryGetTileset(string tilesetName, [NotNullWhen(true)] out Tileset? tileset) =>
//         _tilesetByName.TryGetValue(tilesetName, out tileset);

//     /// <summary>
//     ///     Removes the <see cref="Tileset"/> element at the specified index from this <see cref="TilesetCollection"/>.
//     /// </summary>
//     /// <param name="tilesetIndex">
//     ///     The index of the <see cref="Tileset"/> element to remove from this <see cref="TilesetCollection"/>.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the <see cref="Tileset"/> was removed successfully; otherwise,
//     ///     <see langword="false"/>.  This method return <see langword="false"/> if the specified index is less than
//     ///     zero or is greater than or equal to the total number of <see cref="Tileset"/> elements in this
//     ///     <see cref="TilesetCollection"/>.  Use <see cref="TilesetCollection.Count"/> to determine the total number of
//     ///     elements.
//     /// </returns>
//     public bool RemoveTileset(int tilesetIndex)
//     {
//         if (tilesetIndex < 0 || tilesetIndex >= Count)
//         {
//             return false;
//         }

//         Tileset tileset = _tilesets[tilesetIndex];
//         return RemoveTileset(tileset);
//     }

//     /// <summary>
//     ///     Removes the <see cref="Tileset"/> element with the specified name from this <see cref="TilesetCollection"/>.
//     /// </summary>
//     /// <param name="tilesetName">
//     ///     The name of the <see cref="Tileset"/> element to remove.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the <see cref="Tileset"/> was removed successfully; otherwise,
//     ///     <see langword="false"/>.  This method returns <see langword="false"/> if this
//     ///     <see cref="TilesetCollection"/> does not contain a <see cref="Tileset"/> element with the specified name.
//     /// </returns>
//     public bool RemoveTileset(string tilesetName)
//     {
//         if (_tilesetByName.TryGetValue(tilesetName, out Tileset? tileset))
//         {
//             return RemoveTileset(tileset);
//         }

//         return false;
//     }

//     /// <summary>
//     ///     Removes the given <see cref="Tileset"/> from this <see cref="TilesetCollection"/>.
//     /// </summary>
//     /// <param name="tileset">
//     ///     The <see cref="Tileset"/> to remove from this <see cref="TilesetCollection"/>.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the <see cref="Tileset"/> was removed successfully; otherwise,
//     ///     <see langword="false"/>.  This method returns <see langword="false"/> if this
//     ///     <see cref="TilesetCollection"/> does not contain the given <see cref="Tileset"/>.
//     /// </returns>
//     public bool RemoveTileset(Tileset tileset) =>
//         _tilesets.Remove(tileset) && _tilesetByName.Remove(tileset.Name);

//     /// <summary>
//     ///     Removes all <see cref="Tileset"/> elements from this <see cref="TilesetCollection"/>.
//     /// </summary>
//     public void Clear()
//     {
//         _tilesetByName.Clear();
//         _tilesets.Clear();
//     }

//     /// <summary>
//     ///     Returns an enumerator that iterates through all of the <see cref="Tileset"/> elements in this
//     ///     <see cref="TilesetCollection"/>.
//     /// </summary>
//     /// <returns>
//     ///     An enumerator that iterates through all of the <see cref="Tileset"/> elements in this
//     ///     <see cref="TilesetCollection"/>.
//     /// </returns>
//     public IEnumerator<Tileset> GetEnumerator() => _tilesets.GetEnumerator();

//     /// <summary>
//     ///     Returns an enumerator that iterates through all of the <see cref="Tileset"/> elements in this
//     ///     <see cref="TilesetCollection"/>.
//     /// </summary>
//     /// <returns>
//     ///     An enumerator that iterates through all of the <see cref="Tileset"/> elements in this
//     ///     <see cref="TilesetCollection"/>.
//     /// </returns>
//     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
// }
