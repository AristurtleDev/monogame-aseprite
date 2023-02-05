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
/// Defines the raw data for an animated tilemap.
/// </summary>
public sealed class RawAnimatedTilemap : IEquatable<RawAnimatedTilemap>
{
    private readonly RawTileset[] _rawTilesets;
    private readonly RawTilemapFrame[] _rawTilemapFrames;

    /// <summary>
    /// Gets the name of the animated tilemap.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a read-only span of the raw tileset data for the tilesets used by the layers of the animated tilemap.
    /// </summary>
    public ReadOnlySpan<RawTileset> RawTilesets => _rawTilesets;

    /// <summary>
    /// Gets a read-only span of the raw tilemap frame data for animated tilemap.
    /// </summary>
    public ReadOnlySpan<RawTilemapFrame> RawTilemapFrames => _rawTilemapFrames;

    internal RawAnimatedTilemap(string name, RawTileset[] rawTilesets, RawTilemapFrame[] rawTilemapFrames) =>
        (Name, _rawTilesets, _rawTilemapFrames) = (name, rawTilesets, rawTilemapFrames);

    public bool Equals(RawAnimatedTilemap? other) => other is not null
                                                     && Name == other.Name
                                                     && RawTilesets.SequenceEqual(other.RawTilesets);
}
