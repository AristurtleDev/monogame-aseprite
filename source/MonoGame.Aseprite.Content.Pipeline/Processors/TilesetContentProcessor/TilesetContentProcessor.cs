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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes a single <see cref="AsepriteTileset"/> in an
///     <see cref="AsepriteFile"/>.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Processor - MonoGame.Aseprite")]
public sealed class TilesetContentProcessor : CommonProcessor<ContentImporterResult<AsepriteFile>, TilesetContentProcessorResult>
{
    /// <summary>
    ///     Gets or Sets the name of the <see cref="AsepriteTileset"/> element in the <see cref="AsepriteFile"/> to
    ///     process.
    /// </summary>
    [DisplayName("Tileset Name")]
    public string TilesetName { get; set; } = string.Empty;

    /// <summary>
    ///     Processes a single <see cref="AsepriteTileset"/> in an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="content">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The <see cref="ContentProcessorContext"/> that provides contextual information  about the content being
    ///     processed.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="TilesetContentProcessorResult"/> class that contains the result of this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the <see cref="AsepriteFile"/> does not contain a <see cref="AsepriteTileset"/> element with the
    ///     name specified in the <see cref="TilesetName"/> property.
    /// </exception>
    public override TilesetContentProcessorResult Process(ContentImporterResult<AsepriteFile> content, ContentProcessorContext context)
    {
        TilesetProcessorResult result = TilesetProcessor.Process(content.Data, TilesetName);
        TextureContent textureContent = CreateTextureContent(content.FileName, result.Pixels, result.Width, result.Height, context);
        textureContent.Name = result.Name;
        return new(result.Name, result.TileWidth, result.TileHeight, textureContent);
    }
}
