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
///     Defines a content process that processes all <see cref="AsepriteFrame"/>, <see cref="AsepriteTag"/>, and
///     <see cref="AsepriteSlice"/> elements in a <see cref="AsepriteFile"/> and generates a spritesheet.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Spritesheet Processor - MonoGame.Aseprite")]
internal sealed class SpriteSheetContentProcessor : CommonProcessor<ContentImporterResult<AsepriteFile>, SpriteSheetContentProcessorResult>
{
    /// <summary>
    ///     Gets or Sets a value that indicates whether only <see cref="AsepriteCel"/> elements that are on a visible
    ///     <see cref="AsepriteLayer"/> should be processed.
    /// </summary>
    [DisplayName("(Aseprite) Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Gets or Sets whether <see cref="AsepriteCel"/> elements that are on a <see cref="AsepriteLayer"/> that was
    ///     marked as the background layer in Aseprite should be processed.
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
    ///     Gets or Sets a value that indicates whether <see cref="AsepriteFrame"/> elements that are detected as
    ///     duplicates should be merged into a single element.
    /// </summary>
    [DisplayName("(Aseprite) Merge Duplicate Frames")]
    public bool MergeDuplicateFrames { get; set; } = true;

    /// <summary>
    ///     Gets or Sets the amount of transparent pixels to add between the edge of the generated texture for the
    ///     spritesheet and the frame regions within it.
    /// </summary>
    [DisplayName("(Aseprite) Border Padding")]
    public int BorderPadding { get; set; } = 0;

    /// <summary>
    ///     Gets or Sets the amount of transparent pixels to add between each frame region in the generated texture for
    ///     the spritesheet.
    /// </summary>
    [DisplayName("(Aseprite) Spacing")]
    public int Spacing { get; set; } = 0;

    /// <summary>
    ///     Gets or Sets the amount of transparent pixels to add around the edge of each frame region in the generated
    ///     texture for the spritesheet.
    /// </summary>
    [DisplayName("(Aseprite) Inner Padding")]
    public int InnerPadding { get; set; } = 0;

    /// <summary>
    ///     Processes a spritesheet from an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="content">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The <see cref="ContentProcessorContext"/> that provides contextual information  about the content being
    ///     processed.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="RawSpriteSheet"/> class that contains the results of this
    ///     method.
    /// </returns>
    public override SpriteSheetContentProcessorResult Process(ContentImporterResult<AsepriteFile> content, ContentProcessorContext context)
    {
        SpriteSheetProcessorConfiguration options = new()
        {
            OnlyVisibleLayers = OnlyVisibleLayers,
            IncludeBackgroundLayer = IncludeBackgroundLayer,
            IncludeTilemapLayers = IncludeTilemapLayers,
            MergeDuplicateFrames = MergeDuplicateFrames,
            BorderPadding = BorderPadding,
            InnerPadding = InnerPadding,
            Spacing = Spacing
        };

        RawSpriteSheet rawSpriteSheet = SpriteSheetProcessor.GetRawSpriteSheet(content.Data, options);
        TextureContent textureContent = CreateTextureContent(rawSpriteSheet.Texture, content.FilePath, context);
        textureContent.Name = rawSpriteSheet.Texture.Name;
        return new(rawSpriteSheet, textureContent);
    }
}
