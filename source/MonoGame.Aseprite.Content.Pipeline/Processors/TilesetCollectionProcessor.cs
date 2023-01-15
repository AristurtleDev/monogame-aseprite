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
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Provides method for processing all <see cref="Tileset"/> elements in
///     an <see cref="AsepriteFile"/> as an instance of the
///     <see cref="TilesetCollectionContent"/> class.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Collection Processor - MonoGame.Aseprite")]
public sealed class TilesetCollectionProcessor : ContentProcessor<AsepriteFile, TilesetCollectionContent>
{
    /// <summary>
    ///     Processes all <see cref="Tileset"/> elements in an
    ///     <see cref="AsepriteFile"/>.  The result is a new instance of the
    ///     <see cref="TilesetCollectionContent"/> class containing the
    ///     <see cref="TilesetContent"/> for each <see cref="Tileset"/>
    ///     processed, to be written to the xnb file.
    /// </summary>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The context of the content processor.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="TilesetCollectionContent"/> class
    ///     containing the <see cref="TilesetContent"/> for each
    ///     <see cref="Tileset"/> processed, to be written to the xnb file.
    /// </returns>
    public override TilesetCollectionContent Process(AsepriteFile file, ContentProcessorContext context)
    {
        TilesetCollectionContent content = new();

        for (int i = 0; i < file.Tilesets.Count; i++)
        {
            Tileset tileset = file.Tilesets[i];
            TextureContent textureContent = new(tileset.Width, tileset.Height, tileset.Pixels);
            TilesetContent tilesetContent = new(tileset.Name, tileset.TileCount, tileset.TileWidth, tileset.TileHeight, textureContent);
            content.Tilesets.Add(tilesetContent);
        }

        return content;
    }
}
