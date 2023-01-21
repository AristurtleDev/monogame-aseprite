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

    // /// <summary>
    // ///     Processes a spritesheet from an <see cref="AsepriteFile"/>.
    // /// </summary>
    // /// <param name="content">
    // ///     The <see cref="AsepriteFile"/> to process.
    // /// </param>
    // /// <param name="context">
    // ///     The <see cref="ContentProcessorContext"/> that provides contextual information  about the content being
    // ///     processed.
    // /// </param>
    // /// <returns>
    // ///     A new instance of the <see cref="SpriteSheetProcessorResult"/> class that contains the results of this
    // ///     method.
    // /// </returns>
    // public override SpriteSheetProcessorResult Process(ContentImporterResult<AsepriteFile> content, ContentProcessorContext context)
    // {
    //     ReadOnlySpan<AsepriteFrame> frames = content.Data.Frames;
    //     ReadOnlySpan<AsepriteTag> tags = content.Data.Tags;
    //     _frameWidth = content.Data.CanvasWidth;
    //     _frameHeight = content.Data.CanvasHeight;

    //     int frameCount = frames.Length;
    //     Color[][] flattenedFrames = FlattenFrames(frames);
    //     BuildDuplicateMap(flattenedFrames);

    //     if (MergeDuplicateFrames)
    //     {
    //         frameCount -= _duplicateMap.Count;
    //     }

    //     double sqrt = Math.Sqrt(frameCount);
    //     _columns = (int)Math.Ceiling(sqrt);
    //     _rows = (frameCount + _columns - 1) / _columns;

    //     _width = (_columns * _frameWidth) +
    //              (BorderPadding * 2) +
    //              (Spacing * (_columns - 1)) +
    //              (InnerPadding * 2 * _columns);

    //     _height = (_rows * _frameHeight) +
    //               (BorderPadding * 2) +
    //               (Spacing * (_rows - 1)) +
    //               (InnerPadding * 2 * _rows);

    //     Color[] pixels = new Color[_width * _height];

    //     GenerateImage(flattenedFrames, pixels);
    //     TextureContent textureContent = CreateTextureContent(content.FileNameWithoutExtension, pixels, _width, _height, context);

    //     TextureRegionContent[] regions = new TextureRegionContent[frames.Length];
    //     GenerateTextureRegions(frames, regions);

    //     AnimationContent[] animations = new AnimationContent[tags.Length];
    //     GenerateAnimationContent(tags, frames, animations);

    //     return new(content.FileNameWithoutExtension, textureContent, regions, animations);
    // }

    // private Color[][] FlattenFrames(ReadOnlySpan<AsepriteFrame> frames)
    // {
    //     Color[][] result = new Color[frames.Length][];

    //     for (int i = 0; i < frames.Length; i++)
    //     {
    //         AsepriteFrame frame = frames[i];
    //         result[i] = frame.FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer);
    //     }

    //     return result;
    // }

    // private void BuildDuplicateMap(Color[][] frames)
    // {
    //     _duplicateMap = new();
    //     Dictionary<Color[], int> checkedFrames = new();

    //     for (int i = 0; i < frames.GetLength(0); i++)
    //     {
    //         if (!checkedFrames.ContainsKey(frames[i]))
    //         {
    //             checkedFrames.Add(frames[i], i);
    //         }
    //         else
    //         {
    //             _duplicateMap.Add(i, checkedFrames[frames[i]]);
    //         }
    //     }
    // }

    // private void GenerateImage(Color[][] frames, Span<Color> image)
    // {
    //     int offset = 0;

    //     for (int fNum = 0; fNum < frames.GetLength(0); fNum++)
    //     {
    //         if (MergeDuplicateFrames && _duplicateMap.ContainsKey(fNum))
    //         {
    //             offset++;
    //             continue;
    //         }

    //         int column = (fNum - offset) % _columns;
    //         int row = (fNum - offset) / _columns;

    //         Color[] frame = frames[fNum];
    //         CopyFrameToImage(frame, image, column, row);
    //     }
    // }

    // private void CopyFrameToImage(ReadOnlySpan<Color> frame, Span<Color> image, int column, int row)
    // {
    //     for (int i = 0; i < frame.Length; i++)
    //     {
    //         int x = (i % _frameWidth) + (column * _frameWidth) +
    //                 BorderPadding +
    //                 (Spacing * column) +
    //                 (InnerPadding * (column + column + 1));

    //         int y = (i / _frameWidth) + (row * _frameHeight) +
    //                 BorderPadding +
    //                 (Spacing * row) +
    //                 (InnerPadding * (row + row + 1));

    //         int index = y * _width + x;
    //         image[index] = frame[i];
    //     }
    // }

    // private void GenerateTextureRegions(ReadOnlySpan<AsepriteFrame> frames, Span<TextureRegionContent> regions)
    // {
    //     Dictionary<int, TextureRegionContent> originalToDuplicateLookup = new();
    //     int offset = 0;

    //     for (int i = 0; i < frames.Length; i++)
    //     {
    //         if (MergeDuplicateFrames && _duplicateMap.ContainsKey(i))
    //         {
    //             TextureRegionContent original = originalToDuplicateLookup[_duplicateMap[i]];
    //             TextureRegionContent duplicate = new($"frame_{i}", original.Bounds);
    //             regions[i] = duplicate;
    //             offset++;
    //             continue;
    //         }

    //         int column = (i - offset) % _columns;
    //         int row = (i - offset) / _columns;

    //         int x = (column * _frameWidth) +
    //                 BorderPadding +
    //                 (Spacing * column) +
    //                 (InnerPadding * (column + column + 1));

    //         int y = (row * _frameHeight) +
    //                 BorderPadding +
    //                 (Spacing * row) +
    //                 (InnerPadding * (row + row + 1));

    //         Rectangle bounds = new(x, y, _frameWidth, _frameHeight);
    //         TextureRegionContent region = new($"frame_{i}", bounds);
    //         regions[i] = region;
    //         originalToDuplicateLookup.Add(i, region);
    //     }
    // }

    // private void GenerateAnimationContent(ReadOnlySpan<AsepriteTag> tags, ReadOnlySpan<AsepriteFrame> frames, Span<AnimationContent> animations)
    // {
    //     for (int i = 0; i < tags.Length; i++)
    //     {
    //         AsepriteTag tag = tags[i];
    //         AnimationFrameContent[] animationFrames = new AnimationFrameContent[tag.To - tag.From + 1];
    //         for (int f = 0; f < animationFrames.Length; f++)
    //         {
    //             int index = tag.From + f;
    //             int duration = frames[index].Duration;
    //             animationFrames[f] = new AnimationFrameContent(index, TimeSpan.FromMilliseconds(duration));
    //         }

    //         byte animationFlags = 1;
    //         if (tag.Direction == AsepriteLoopDirection.Reverse)
    //         {
    //             animationFlags |= 2;
    //         }

    //         if (tag.Direction == AsepriteLoopDirection.PingPong)
    //         {
    //             animationFlags |= 4;
    //         }

    //         AnimationContent animation = new(tag.Name, animationFrames, animationFlags);
    //         animations[i] = animation;
    //     }
    // }
}
