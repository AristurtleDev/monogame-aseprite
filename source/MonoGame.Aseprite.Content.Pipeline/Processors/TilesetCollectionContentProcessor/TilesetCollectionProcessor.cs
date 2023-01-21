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
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes all <see cref="AsepriteTileset"/> elements in an
///     <see cref="AsepriteFile"/> as a collection.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Collection Processor - MonoGame.Aseprite")]
public sealed class TilesetCollectionContentProcessor : CommonProcessor<ContentImporterResult<AsepriteFile>, TilesetCollectionContentProcessorResult>
{
    /// <summary>
    ///     Processes all <see cref="AsepriteTileset"/> elements in an <see cref="AsepriteFile"/> as a collection.
    /// </summary>
    /// <param name="content">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The <see cref="ContentProcessorContext"/> that provides contextual information  about the content being
    ///     processed.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="TilesetCollectionContentProcessorResult"/> class that contains the result
    ///     of this method.
    /// </returns>
    public override TilesetCollectionContentProcessorResult Process(ContentImporterResult<AsepriteFile> content, ContentProcessorContext context)
    {
        RawTileset[] tilesets = TilesetCollectionProcessor.Process(content.Data);
        TextureContent[] textures = ProcessTextures(tilesets, content.FilePath, context);

        return new(tilesets, textures);
    }

    private TextureContent[] ProcessTextures(ReadOnlySpan<RawTileset> tilesets, string sourceFilePath, ContentProcessorContext context)
    {
        TextureContent[] textures = new TextureContent[tilesets.Length];
        for (int i = 0; i < tilesets.Length; i++)
        {
            RawTileset tileset = tilesets[i];
            TextureContent texture = CreateTextureContent(tileset.Texture, sourceFilePath, context);
            texture.Name = tileset.Texture.Name;
            textures[i] = texture;
        }
        return textures;
    }
}
