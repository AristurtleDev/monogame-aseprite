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

namespace MonoGame.Aseprite.RawTypes;

/// <summary>
///     Defines a class that represents the raw values of a tileset.
/// </summary>
public sealed class RawTileset : IEquatable<RawTileset>
{
    /// <summary>
    ///     Gets the unique ID assigned to the tileset.
    /// </summary>
    public int ID { get; }

    /// <summary>
    ///     Gets the name assigned to the tileset.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="RawTexture"/> that represents the source texture of the tileset.
    /// </summary>
    public RawTexture RawTexture { get; }

    /// <summary>
    ///     Gets the width, in pixels, of each tile in the tileset.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    ///     Gets the height, in pixels, of each tile in the tileset.
    /// </summary>
    public int TileHeight { get; }

    internal RawTileset(int id, string name, RawTexture rawTexture, int tileWidth, int tileHeight) =>
        (ID, Name, RawTexture, TileWidth, TileHeight) = (id, name, rawTexture, tileWidth, tileHeight);

    /// <summary>
    ///     Returns a value that indicates if the given <see cref="RawTileset"/> is equal to this
    ///     <see cref="RawTileset"/>.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="RawTileset"/> to check for equality with this <see cref="RawTileset"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the given <see cref="RawTileset"/> is equal to this <see cref="RawTileset"/>;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RawTileset? other) => other is not null
                                             && Name == other.Name
                                             && RawTexture.Equals(other.RawTexture)
                                             && TileWidth == other.TileWidth
                                             && TileHeight == other.TileHeight;
}
