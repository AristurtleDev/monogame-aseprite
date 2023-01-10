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

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

[ContentTypeWriter]
public sealed class TilesetWriter : ContentTypeWriter<TilesetProcessorResult>
{
    protected override void Write(ContentWriter output, TilesetProcessorResult input)
    {
        output.Write(input.Tilesets.Count);

        for (int i = 0; i < input.Tilesets.Count; i++)
        {
            WriteTileset(output, input.Tilesets[i]);
        }
    }

    private void WriteTileset(ContentWriter output, Tileset tileset)
    {
        output.Write(tileset.ID);
        output.Write(tileset.Name);
        output.Write(tileset.TileCount);
        output.Write(tileset.TileSize.X);
        output.Write(tileset.TileSize.Y);
        output.Write(tileset.Size.X);
        output.Write(tileset.Size.Y);
        output.Write(tileset.Pixels.Length);

        for (int i = 0; i < tileset.Pixels.Length; i++)
        {
            output.Write(tileset.Pixels[i]);
        }
    }

    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Content.Pipeline.Readers.TilesetReader, MonoGame.Aseprite";
    }
}
