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
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

[ContentTypeWriter]
public sealed class TilemapWriter : ContentTypeWriter<TilemapContent>
{
    protected override void Write(ContentWriter output, TilemapContent value)
    {
        //  Tileset content
        output.Write(value.Tilesets.Count);
        for (int i = 0; i < value.Tilesets.Count; i++)
        {
            TilesetContent tileset = value.Tilesets[i];
            output.Write(tileset.Name);
            output.Write(tileset.TileCount);
            output.Write(tileset.TileWidth);
            output.Write(tileset.TileHeight);

            //  Texture Content
            output.Write(tileset.TextureContent.Width);
            output.Write(tileset.TextureContent.Height);
            output.Write(tileset.TextureContent.Pixels.Length);
            for (int j = 0; j < tileset.TextureContent.Pixels.Length; j++)
            {
                output.Write(tileset.TextureContent.Pixels[j]);
            }
        }

        //  Layer Content
        output.Write(value.Layers.Count);
        for (int i = 0; i < value.Layers.Count; i++)
        {
            TilemapLayerContent layer = value.Layers[i];
            output.Write(layer.TilesetID);
            output.Write(layer.Name);
            output.Write(layer.Columns);
            output.Write(layer.Rows);
            output.Write(layer.Offset.X);
            output.Write(layer.Offset.Y);

            //  Tile content
            output.Write(layer.Tiles.Length);
            for (int j = 0; j < layer.Tiles.Length; j++)
            {
                TileContent tile = layer.Tiles[j];
                output.Write(tile.FlipFlag);
                output.Write(tile.Rotation);
                output.Write(tile.TilesetTileID);
            }

        }
    }

    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Content.Pipeline.Readers.TilemapReader, MonoGame.Aseprite";
    }
}
