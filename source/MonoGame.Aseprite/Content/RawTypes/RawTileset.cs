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

namespace MonoGame.Aseprite.Content.RawTypes;

/// <summary>
/// Defines a record that represents the raw values of a tileset.
/// </summary>
public sealed record RawTileset
{
    /// <summary>
    /// Gets the unique ID assigned to the tileset represented by this raw tileset record.
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// Gets the name assigned to the tileset represented by this raw tileset record.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the raw texture record that represents the source texture of the tileset represented by this raw tileset
    /// record.
    /// </summary>
    public RawTexture RawTexture { get; }

    /// <summary>
    /// Gets the width, in pixels, of each tile in the tileset represented by this raw tileset record.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    /// Gets the height, in pixels, of each tile in the tileset represented by this raw tileset record.
    /// </summary>
    public int TileHeight { get; }

    internal RawTileset(int id, string name, RawTexture rawTexture, int tileWidth, int tileHeight) =>
        (ID, Name, RawTexture, TileWidth, TileHeight) = (id, name, rawTexture, tileWidth, tileHeight);
}
