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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content process that processes all <see cref="AsepriteFrame"/>, <see cref="AsepriteTag"/>, and
///     <see cref="AsepriteSlice"/> elements in a <see cref="AsepriteFile"/> and generates a spritesheet.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Spritesheet Processor - MonoGame.Aseprite")]
public sealed class SpritesheetProcessor : CommonProcessor<AsepriteFile, SpriteSheetProcessorResult>
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
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The <see cref="ContentProcessorContext"/> that provides contextual information  about the content being
    ///     processed.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SpriteSheetProcessorResult"/> class that contains the results of this
    ///     method.
    /// </returns>
    public override SpriteSheetProcessorResult Process(AsepriteFile file, ContentProcessorContext context)
    {
        int frameCount = file.FrameCount;
        Color[][] frames = FlattenFrames(file.Frames);
        Dictionary<int, int> duplicateMap = BuildDuplicateMap(frames);

        if (MergeDuplicateFrames)
        {
            frameCount -= duplicateMap.Count;
        }

        // *********************************************************************
        //  Generate the sprite sheet image and create texture content
        // *********************************************************************
        double sqrt = Math.Sqrt(frameCount);
        int columns = (int)Math.Ceiling(sqrt);
        int rows = (frameCount + columns - 1) / columns;

        int width = (columns * file.CanvasWidth) +
                    (BorderPadding * 2) +
                    (Spacing * (columns - 1)) +
                    (InnerPadding * 2 * columns);

        int height = (rows * file.CanvasHeight) +
                     (BorderPadding * 2) +
                     (Spacing * (rows - 1)) +
                     (InnerPadding * 2 * rows);

        Color[] pixels = GenerateImage(frames, file.CanvasWidth, file.CanvasHeight, columns, rows, width, height, duplicateMap);
        TextureContent textureContent = CreateTextureContent(file.Name, pixels, width, height, context);

        SpriteSheetProcessorResult content = new(file.Name, textureContent);

        // *********************************************************************
        //  Generate the texture region content
        // *********************************************************************
        Dictionary<int, TextureRegionContent> originalToDuplicateLookup = new();
        int offset = 0;

        for (int fNum = 0; fNum < file.FrameCount; fNum++)
        {
            if (MergeDuplicateFrames && duplicateMap.ContainsKey(fNum))
            {
                TextureRegionContent original = originalToDuplicateLookup[duplicateMap[fNum]];
                TextureRegionContent duplicate = new($"frame_{fNum}", original.Bounds);
                content.Regions.Add(duplicate);
                offset++;
                continue;
            }

            int column = (fNum - offset) % columns;
            int row = (fNum - offset) / columns;

            int x = (column * file.CanvasWidth) +
                    BorderPadding +
                    (Spacing * column) +
                    (InnerPadding * (column + column + 1));

            int y = (row * file.CanvasHeight) +
                    BorderPadding +
                    (Spacing * row) +
                    (InnerPadding * (row + row + 1));

            Rectangle bounds = new(x, y, file.CanvasWidth, file.CanvasHeight);
            TextureRegionContent region = new($"frame_{fNum}", bounds);
            content.Regions.Add(region);
            originalToDuplicateLookup.Add(fNum, region);
        }

        // *********************************************************************
        //  Generate the animation content
        // *********************************************************************
        for (int i = 0; i < file.TagCount; i++)
        {
            AsepriteTag aseTag = file.Tags[i];
            AnimationFrameContent[] animationFrames = new AnimationFrameContent[aseTag.To - aseTag.From + 1];
            for (int f = 0; f < animationFrames.Length; f++)
            {
                int index = aseTag.From + f;
                int duration = file.Frames[index].Duration;
                animationFrames[f] = new AnimationFrameContent(index, TimeSpan.FromMilliseconds(duration));
            }

            byte animationFlags = 1;
            if (aseTag.Direction == AsepriteLoopDirection.Reverse)
            {
                animationFlags |= 2;
            }

            if (aseTag.Direction == AsepriteLoopDirection.PingPong)
            {
                animationFlags |= 4;
            }

            AnimationContent animation = new(aseTag.Name, animationFrames, animationFlags);

            content.Animations.Add(animation);
        }

        return content;
    }

    private Color[][] FlattenFrames(ReadOnlySpan<AsepriteFrame> frames)
    {
        Color[][] result = new Color[frames.Length][];

        for (int i = 0; i < frames.Length; i++)
        {
            AsepriteFrame frame = frames[i];
            result[i] = frame.FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer);
        }

        return result;
    }

    private Dictionary<int, int> BuildDuplicateMap(Color[][] frames)
    {
        Dictionary<int, int> map = new();
        Dictionary<Color[], int> checkedFrames = new();

        for (int i = 0; i < frames.GetLength(0); i++)
        {
            if (!checkedFrames.ContainsKey(frames[i]))
            {
                checkedFrames.Add(frames[i], i);
            }
            else
            {
                map.Add(i, checkedFrames[frames[i]]);
            }
        }

        return map;
    }

    private Color[] GenerateImage(Color[][] frames, int frameWidth, int frameHeight, int columns, int rows, int imageWidth, int imageHeight, Dictionary<int, int> duplicateMap)
    {
        Color[] image = new Color[imageWidth * imageHeight];

        int offset = 0;

        for (int fNum = 0; fNum < frames.GetLength(0); fNum++)
        {
            if (MergeDuplicateFrames && duplicateMap.ContainsKey(fNum))
            {
                offset++;
                continue;
            }

            int column = (fNum - offset) % columns;
            int row = (fNum - offset) / columns;

            Color[] frame = frames[fNum];
            CopyFrameToImage(frame, image, frameWidth, frameHeight, imageWidth, imageHeight, column, row);
        }

        return image;
    }

    private void CopyFrameToImage(Color[] frame, Color[] image, int frameWidth, int frameHeight, int imageWidth, int imageHeight, int column, int row)
    {
        for (int i = 0; i < frame.Length; i++)
        {
            int x = (i % frameWidth) + (column * frameWidth) +
                    BorderPadding +
                    (Spacing * column) +
                    (InnerPadding * (column + column + 1));

            int y = (i / frameWidth) + (row * frameHeight) +
                    BorderPadding +
                    (Spacing * row) +
                    (InnerPadding * (row + row + 1));

            int index = y * imageWidth + x;
            image[index] = frame[i];
        }
    }
}
