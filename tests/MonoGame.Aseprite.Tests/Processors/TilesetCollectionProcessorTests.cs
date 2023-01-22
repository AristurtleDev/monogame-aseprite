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

using MonoGame.Aseprite.Processors;

namespace MonoGame.Aseprite.Tests;

public sealed class TilesetCollectionProcessorTests
{
    [Fact]
    public void TilesetCollectionProcessor_GetRawTilesetsTest()
    {
        string path = FileUtils.GetLocalPath("tileset-collection-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTileset[] tilesets = TilesetCollectionProcessor.GetRawTilesets(aseFile);

        Assert.Equal(2, tilesets.Length);
        Assert.Equal("tileset-1", tilesets[0].Name);
        Assert.Equal("tileset-2", tilesets[1].Name);
    }

    [Fact]
    public void TilesetCollectionProcessor_GetRawTilesets_DuplicateNamedTilesets_ThrowsException()
    {
        string path = FileUtils.GetLocalPath("tileset-collection-processor-duplicate-name-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        Exception ex = Record.Exception(() => TilesetCollectionProcessor.GetRawTilesets(aseFile));

        Assert.IsType<InvalidOperationException>(ex);
    }
}
