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
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes a single frame in an Aseprite file to generate a texture based on the
///     frame processed.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Texture Processor - MonoGame.Aseprite")]
public sealed class SingleFrameContentProcessor : CommonProcessor<ContentImporterResult<AsepriteFile>, SingleFrameContentProcessorResult>
{
    /// <summary>
    ///     Gets or Sets the index of the frame in the Aseprite file to process.
    /// </summary>
    [DisplayName("(Aseprite) Frame Index")]
    public int FrameIndex { get; set; } = 0;

    /// <summary>
    ///     Gets or Sets a value tha indicates whether only image data that is on visible layers should be processed.
    /// </summary>
    [DisplayName("(Aseprite) Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Gets or Sets whether image data on a layer tha was marked as the background layer in Aseprite should be
    ///     processed.
    /// </summary>
    [DisplayName("(Aseprite) Include Background Layer")]
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    ///     Gets or Sets whether <see cref="AsepriteCel"/> elements that are on a <see cref="AsepriteTilemapLayer"/>
    ///     should be processed.
    /// </summary>
    [DisplayName("(Aseprite) Include Tilemap Layers")]
    public bool IncludeTilemapLayers { get; set; } = true;

    /// <summary>
    ///     Processes the image of a single frame in an Aseprite file.
    /// </summary>
    /// <param name="content">
    ///     The result of the content importer that contains the Aseprite file to process.
    /// </param>
    /// <param name="context">
    ///     The content processor context that provides contextual information about the content being processed.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SingleFrameContentProcessorResult"/> class that contains the result of this
    ///     method.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///     Thrown if the <see cref="FrameIndex"/> property is less than zero or is greater than or equal to the total
    ///     number of <see cref="AsepriteFrame"/> elements in the <see cref="AsepriteFile"/> being processed.
    /// </exception>
    public override SingleFrameContentProcessorResult Process(ContentImporterResult<AsepriteFile> content, ContentProcessorContext context)
    {
        SingleFrameProcessorConfiguration options = new()
        {
            FrameIndex = FrameIndex,
            IncludeBackgroundLayer = IncludeBackgroundLayer,
            IncludeTilemapLayers = IncludeTilemapLayers,
            OnlyVisibleLayers = OnlyVisibleLayers
        };

        RawTexture rawTexture = SingleFrameProcessor.CreateRawTexture(content.Data, options);
        TextureContent textureContent = CreateTextureContent(rawTexture, content.FilePath, context);
        textureContent.Name = rawTexture.Name;
        return new SingleFrameContentProcessorResult(rawTexture.Name, textureContent);
    }
}
