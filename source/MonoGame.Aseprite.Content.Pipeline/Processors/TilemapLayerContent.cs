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

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines the content of a tilemap layer.
/// </summary>
public sealed class TilemapLayerContent
{
    private TileContent[] _tiles;

    internal int TilesetID { get; }
    internal string Name { get; }
    internal int Columns { get; }
    internal int Rows { get; }
    internal ReadOnlySpan<TileContent> Tiles => _tiles;
    internal Point Offset { get; }

    internal TilemapLayerContent(string name, int tilesetId, int columns, int rows, Point offset, TileContent[] tiles) =>
        (Name, TilesetID, Columns, Rows, Offset, _tiles) = (name, TilesetID, columns, rows, offset, tiles);
}
