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
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Aseprite.Content.Pipeline.Processors;
using MonoGame.Aseprite.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

/// <summary>
///     Defines a content writer that writes the content of a <see cref="TileMapContentProcessorResult"/> to an xnb
///     file.
/// </summary>
[ContentTypeWriter]
public sealed class TilemapWriter : ContentTypeWriter<TileMapContentProcessorResult>
{
    protected override void Write(ContentWriter writer, TileMapContentProcessorResult content)
    {
        writer.Write(content.Tilemap.Name);
        WriteTilesets(writer, content.Tilemap.Tilesets, content.Textures);
        WriteLayers(writer, content.Tilemap.Layers);
    }

    private void WriteTilesets(ContentWriter writer, ReadOnlySpan<RawTileset> tilesets, ReadOnlySpan<TextureContent> textures)
    {
        int count = tilesets.Length;
        writer.Write(count);

        for (int i = 0; i < count; i++)
        {
            WriteTexture(writer, textures[i]);
            WriteTileset(writer, tilesets[i]);
        }
    }

    private void WriteTexture(ContentWriter writer, TextureContent textureContent)
    {
        writer.Write(textureContent);
        writer.Write(textureContent.Name);
    }

    private void WriteTileset(ContentWriter writer, RawTileset tileset)
    {
        writer.Write(tileset.ID);
        writer.Write(tileset.Name);
        writer.Write(tileset.TileWidth);
        writer.Write(tileset.TileHeight);
    }

    private void WriteLayers(ContentWriter writer, ReadOnlySpan<RawTilemapLayer> layers)
    {
        int count = layers.Length;
        writer.Write(count);
        for (int i = 0; i < count; i++)
        {
            WriteLayer(writer, layers[i]);
        }
    }

    private void WriteLayer(ContentWriter writer, RawTilemapLayer layer)
    {
        writer.Write(layer.Name);
        writer.Write(layer.TilesetID);
        writer.Write(layer.Columns);
        writer.Write(layer.Rows);
        writer.Write(layer.Offset);
        WriteTiles(writer, layer.RawTilemapTiles);

    }

    private void WriteTiles(ContentWriter writer, ReadOnlySpan<RawTile> tiles)
    {
        int count = tiles.Length;
        writer.Write(count);
        for (int i = 0; i < count; i++)
        {
            WriteTile(writer, tiles[i]);
        }
    }

    private void WriteTile(ContentWriter writer, RawTile tile)
    {
        writer.Write(tile.TilesetTileID);
        writer.Write(tile.XFlip);
        writer.Write(tile.YFlip);
        writer.Write(tile.Rotation);
    }



    public override string GetRuntimeType(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Tilemap, MonoGame.Aseprite";
    }

    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Content.Pipeline.Readers.TilemapReader, MonoGame.Aseprite";
    }
}
