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

public sealed class AsepriteFileTests
{
    // https://github.com/AristurtleDev/monogame-aseprite/issues/93
    [Fact]
    public void TryGetSlice_True_When_Slice_Exists()
    {
        Color[] palette = Array.Empty<Color>();
        AsepriteFrame[] frames = Array.Empty<AsepriteFrame>();
        AsepriteLayer[] layers = Array.Empty<AsepriteLayer>();
        AsepriteTag[] tags = Array.Empty<AsepriteTag>();
        AsepriteTileset[] tilesets = Array.Empty<AsepriteTileset>();
        AsepriteUserData userData = new AsepriteUserData();

        string expectedSliceName = "TestSlice";

        AsepriteSlice[] slices = new AsepriteSlice[]
        {
            new AsepriteSlice(expectedSliceName, false, false, Array.Empty<AsepriteSliceKey>())
        };

        AsepriteFile aseFile = new AsepriteFile("Test", 1, 1, palette, frames, layers, tags, slices, tilesets, userData);

        Assert.True(aseFile.TryGetSlice(expectedSliceName, out AsepriteSlice? slice));
    }

    [Fact]
    public void Get_Frame_When_ZeroIndexed_False()
    {
        Color[] palette = Array.Empty<Color>();
        AsepriteFrame frame = new AsepriteFrame("Frame0", 1, 1, 1, Array.Empty<AsepriteCel>());
        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new AsepriteFrame("Frame0", 1, 1, 1, Array.Empty<AsepriteCel>()),
            new AsepriteFrame("Frame1", 1, 1, 1, Array.Empty<AsepriteCel>())
        };

        AsepriteLayer[] layers = Array.Empty<AsepriteLayer>();
        AsepriteTag[] tags = Array.Empty<AsepriteTag>();
        AsepriteTileset[] tilesets = Array.Empty<AsepriteTileset>();
        AsepriteUserData userData = new AsepriteUserData();

        AsepriteSlice[] slices = Array.Empty<AsepriteSlice>();
        AsepriteFile aseFile = new AsepriteFile("Test", 1, 1, palette, frames, layers, tags, slices, tilesets, userData);

        aseFile.ZeroIndexedFrames = false;

        AsepriteFrame expected = frames[0];
        AsepriteFrame actual = aseFile.GetFrame(1);
        Assert.Equal(expected, actual);
    }
}
