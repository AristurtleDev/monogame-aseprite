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

using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.AsepriteTypes;

internal sealed class AsepriteTileset
{
    internal Color[] Pixels { get; }
    internal int ID { get; }
    internal int TileCount { get; }
    internal int TileWidth { get; }
    internal int TileHeight { get; }
    internal string Name { get; }
    internal int Width { get; }
    internal int Height { get; }

    internal Color[] this[int tileId]
    {
        get
        {
            if (tileId < 0 || tileId >= TileCount)
            {
                throw new ArgumentOutOfRangeException(nameof(tileId));
            }

            int len = TileWidth * TileHeight;
            return Pixels[(tileId * len)..((tileId * len) + len)];
        }
    }

    internal AsepriteTileset(int id, int count, int tileWidth, int tileHeight, string name, Color[] pixels)
    {
        ID = id;
        TileCount = count;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Name = name;
        Pixels = pixels;
        Width = tileWidth;
        Height = tileHeight * TileCount;
    }
}
