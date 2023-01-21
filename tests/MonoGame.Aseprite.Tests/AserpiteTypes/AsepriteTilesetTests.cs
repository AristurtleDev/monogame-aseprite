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
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Tests;

public class AsepriteTilesetTests
{
    [Fact]
    public void AsepriteTileset_IndexerPropertyTest()
    {
        int tileWidth = 2;
        int tileHeight = 2;
        int tileCount = 2;

        Color[] tile0 = new Color[4]
        {
            Color.Red, Color.Red,
            Color.Red, Color.Red
        };

        Color[] tile1 = new Color[4]
        {
            Color.Green, Color.Green,
            Color.Green, Color.Green
        };

        Color[] pixels = new Color[8];
        Array.Copy(tile0, 0, pixels, 0, tile0.Length);
        Array.Copy(tile1, 0, pixels, 4, tile1.Length);

        AsepriteTileset tileset = new(0, tileCount, tileWidth, tileHeight, "tileset", pixels);

        Color[] actualTile0 = tileset[0].ToArray();
        Assert.Equal(tile0, actualTile0);

        Color[] actualTile1 = tileset[1].ToArray();
        Assert.Equal(tile1, actualTile1);



    }
}
