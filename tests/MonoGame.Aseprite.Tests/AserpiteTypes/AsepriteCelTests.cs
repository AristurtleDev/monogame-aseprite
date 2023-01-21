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

public sealed class AsepriteCelTests
{
    [Fact]
    public void AsepriteCel_LayerAs_ReturnsLayer()
    {
        AsepriteTileset tileset = new(0, 0, 0, 0, string.Empty, Array.Empty<Color>());
        AsepriteTilemapLayer layer = new(tileset, 0, AsepriteBlendMode.Normal, 255, string.Empty);
        AsepriteCel cel = new AsepriteImageCel(0, 0, Array.Empty<Color>(), layer, Point.Zero, 0);

        AsepriteTilemapLayer actual = cel.LayerAs<AsepriteTilemapLayer>();
        Assert.Equal(layer, actual);
    }
}
