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
using MonoGame.Aseprite.Content.Pipeline.ContentTypes;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

[ContentTypeWriter]
internal sealed class AnimatedTilemapContentTypeWriter : ContentTypeWriter<AnimatedTilemapContent>
{
    protected override void Write(ContentWriter writer, AnimatedTilemapContent content)
    {
        RawAnimatedTilemap rawAnimated = content.RawAnimatedTilemap;

        writer.Write(rawAnimated.Name);
        WriteTilesets(writer, rawAnimated.RawTilesets, content.Texture2DContents);
        writer.Write(rawAnimated.RawTilemapFrames);
    }

    private void WriteTilesets(ContentWriter writer, ReadOnlySpan<RawTileset> rawTilesets, ReadOnlySpan<Texture2DContent> tilesetTextures)
    {
        writer.Write(rawTilesets.Length);

        for (int i = 0; i < rawTilesets.Length; i++)
        {
            writer.Write(rawTilesets[i].Name);
            writer.Write(rawTilesets[i].ID);
            writer.Write(rawTilesets[i].TileWidth);
            writer.Write(rawTilesets[i].TileHeight);
            writer.WriteObject(tilesetTextures[i]);
        }
    }

    /// <summary>
    ///     Gets the assembly qualified name of the runtime type.
    /// </summary>
    /// <param name="targetPlatform">
    ///     The target platform.
    /// </param>
    /// <returns>
    ///     The assembly qualified name of the runtime type.
    /// </returns>
    public override string GetRuntimeType(TargetPlatform targetPlatform) =>
        "MonoGame.Aseprite.Tilemaps.AnimatedTilemap, MonoGame.Aseprite";

    /// <summary>
    ///     Gets the assembly qualified name of the runtime loader.
    /// </summary>
    /// <param name="targetPlatform">
    ///     The target platform type.
    /// </param>
    /// <returns>
    ///     The assembly qualified name of the runtime loader.
    /// </returns>
    public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
        "MonoGame.Aseprite.Content.Pipeline.Readers.AnimatedTilemapContentTypeReader, MonoGame.Aseprite";
}
