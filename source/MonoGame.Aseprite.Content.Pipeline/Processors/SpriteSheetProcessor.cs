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
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Provides method for processing all <see cref="Frame"/>,
///     <see cref="Tag"/>, and <see cref="Slice"/> elements in an
///     <see cref="AsepriteFile"/> as an instance of the
///     <see cref="SpriteSheetContent"/> class.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Spritesheet Processor - MonoGame.Aseprite")]
public sealed class SpritesheetProcessor : ContentProcessor<AsepriteFile, SpriteSheetContent>
{
    /// <summary>
    ///     Indicates whether only <see cref="Cel"/> elements that are on
    ///     <see cref="Layer"/> elements that are visible should be used.
    /// </summary>
    /// <remarks>
    ///     This value is set in the property window of the mgcb-editor
    /// </remarks>
    [DisplayName("Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Indicates whether <see cref="Cel"/> elements that are on the
    ///     <see cref="Layer"/> element that is set as the background should be
    ///     included.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A <see cref="Layer"/> has to be set as the background in
    ///         Aseprite.  Being the bottom layer does not automatically mean it
    ///         is a background layer.
    ///     </para>
    ///     <para>
    ///         This value is set in the property window of the mgcb-editor
    ///     </para>
    /// </remarks>
    [DisplayName("Include Background Layer")]
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    ///     Indicates whether <see cref="Frame"/> elements that are detected
    ///     as being duplicates should be merged into a single element.
    /// </summary>
    /// <remarks>
    ///     This value is set in the property window of the mgcb-editor
    /// </remarks>
    [DisplayName("Merge Duplicate Frames")]
    public bool MergeDuplicateFrames { get; set; } = true;

    /// <summary>
    ///     The amount of transparent pixels to add between the edge of the
    ///     generated texture for the spritesheet and the frame regions within
    ///     it.
    /// </summary>
    /// <remarks>
    ///     This value is set in the property window of the mgcb-editor
    /// </remarks>
    [DisplayName("Border Padding")]
    public int BorderPadding { get; set; } = 0;

    /// <summary>
    ///     The amount of transparent pixels to add between each frame region
    ///     in the generated texture for the spritesheet.
    /// </summary>
    /// <remarks>
    ///     This value is set in the property window of the mgcb-editor
    /// </remarks>
    [DisplayName("Spacing")]
    public int Spacing { get; set; } = 0;

    /// <summary>
    ///     The amount of transparent pixels to add around the edge of each
    ///     frame region in the generated texture for the spritesheet.
    /// </summary>
    /// <remarks>
    ///     This value is set in the property window of the mgcb-editor
    /// </remarks>
    [DisplayName("Inner Padding")]
    public int InnerPadding { get; set; } = 0;

    /// <summary>
    ///     Processes all <see cref="Frame"/>, <see cref="Tag"/>, and
    ///     <see cref="Slice"/> elements in an <see cref="AsepriteFile"/>.  The
    ///     result is a new instance of the <see cref="SpriteSheetContent"/>
    ///     class containing the texture content and data generated to be
    ///     written to the xnb file.
    /// </summary>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The context of the content processor.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SpriteSheetContent"/> class
    ///     containing the texture content and data generated to be written to
    ///     the xnb file.
    /// </returns>
    public override SpriteSheetContent Process(AsepriteFile input, ContentProcessorContext context)
    {

        CreateTextureContentResult textureContentResult = CreateTextureContent(input);
        List<TextureRegionContent> regions = CreateSpriteSheetRegionContent(input, textureContentResult.DuplicateMap, textureContentResult.Columns);
        List<AnimationContent> tags = CreateAnimationContent(input);
        // GenerateFrameRegionData(input, regions);

        return new SpriteSheetContent(input.Name, textureContentResult.Content, regions, tags);
    }

    private CreateTextureContentResult CreateTextureContent(AsepriteFile input)
    {
        int frameCount = input.FrameCount;
        Color[][] frames = FlattenFrames(input.Frames);
        Dictionary<int, int> duplicateMap = BuildDuplicateMap(frames);

        if (MergeDuplicateFrames)
        {
            frameCount -= duplicateMap.Count;
        }

        (int Columns, int Rows) grid = CalculateGridSize(frameCount);
        (int Width, int Height) imageSize = CalculateImageSize(input.FrameWidth, input.FrameHeight, grid.Columns, grid.Rows);
        Color[] pixels = GenerateImage(frames, input.FrameWidth, input.FrameHeight, grid.Columns, grid.Rows, imageSize.Width, imageSize.Height, duplicateMap);

        TextureContent textureContent = new(imageSize.Width, imageSize.Height, pixels);
        return new CreateTextureContentResult(textureContent, duplicateMap, grid.Columns);
    }

    private Color[][] FlattenFrames(List<Frame> frames)
    {
        Color[][] result = new Color[frames.Count][];

        for (int i = 0; i < frames.Count; i++)
        {
            Frame frame = frames[i];
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

    private (int Columns, int Rows) CalculateGridSize(int frameCount)
    {
        double sqrt = Math.Sqrt(frameCount);
        int columns = (int)Math.Ceiling(sqrt);
        int rows = (frameCount + columns - 1) / columns;
        return (columns, rows);
    }

    private (int Width, int Height) CalculateImageSize(int frameWidth, int frameHeight, int columns, int rows)
    {
        int width = (columns * frameWidth) +
                    (BorderPadding * 2) +
                    (Spacing * (columns - 1)) +
                    (InnerPadding * 2 * rows);

        int height = (rows * frameHeight) +
                     (BorderPadding * 2) +
                     (Spacing * (rows - 1)) +
                     (InnerPadding * 2 * rows);

        return (width, height);
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

    private List<TextureRegionContent> CreateSpriteSheetRegionContent(AsepriteFile input, Dictionary<int, int> duplicateMap, int columns)
    {
        List<TextureRegionContent> frames = new();
        Dictionary<int, TextureRegionContent> originalToDuplicateLookup = new();

        int offset = 0;

        for (int fNum = 0; fNum < input.Frames.Count; fNum++)
        {
            if (MergeDuplicateFrames && duplicateMap.ContainsKey(fNum))
            {
                TextureRegionContent original = originalToDuplicateLookup[duplicateMap[fNum]];
                TextureRegionContent duplicate = new($"frame_{fNum}", original.Bounds);
                frames.Add(duplicate);
                offset++;
                continue;
            }

            int column = (fNum - offset) % columns;
            int row = (fNum - offset) / columns;

            int x = (column * input.FrameWidth) +
                    BorderPadding +
                    (Spacing * column) +
                    (InnerPadding * (column + column + 1));

            int y = (row * input.FrameHeight) +
                    BorderPadding +
                    (Spacing * row) +
                    (InnerPadding * (row + row + 1));

            Rectangle bounds = new(x, y, input.FrameWidth, input.FrameHeight);
            TextureRegionContent frame = new($"frame_{fNum}", bounds);
            frames.Add(frame);
            originalToDuplicateLookup.Add(fNum, frame);
        }

        return frames;
    }

    private List<AnimationContent> CreateAnimationContent(AsepriteFile file)
    {
        List<AnimationContent> animations = new();

        for (int i = 0; i < file.Tags.Count; i++)
        {
            Tag aseTag = file.Tags[i];
            AnimationFrameContent[] frames = new AnimationFrameContent[aseTag.To - aseTag.From + 1];
            for (int f = 0; f < frames.Length; f++)
            {
                int index = aseTag.From + f;
                int duration = file.Frames[index].Duration;
                frames[f] = new AnimationFrameContent(index, TimeSpan.FromMilliseconds(duration));
            }

            byte loopReversePingPongMask = 1;
            if (aseTag.Direction == 1)
            {
                loopReversePingPongMask |= 2;
            }

            if (aseTag.Direction == 2)
            {
                loopReversePingPongMask |= 4;
            }

            AnimationContent animation = new(aseTag.Name, frames, loopReversePingPongMask);

            animations.Add(animation);
        }

        return animations;
    }

    // private void GenerateFrameRegionData(AsepriteFile file, List<TextureRegionContent> frames)
    // {
    //     List<SpriteSheetFrameRegionContent> regions = new();

    //     //  Slice keys in Aseprite are defined with a frame index that indicates
    //     //  the frame that the key starts on, but doesn't give a value for what
    //     //  frame it ends on or may be transformed on.  So we'll interpolate the
    //     //  keys to create the Slice elements
    //     for (int i = 0; i < file.Slices.Count; i++)
    //     {
    //         Slice aseSlice = file.Slices[i];
    //         string name = aseSlice.Name;

    //         //  If no color defined in user data, use Aseprite default for slice
    //         //  color, which is just blue
    //         Color color = aseSlice.UserData?.Color ?? new Color(0, 0, 255, 255);

    //         SliceKey? lastKey = default;

    //         for (int k = 0; k < aseSlice.Keys.Count; k++)
    //         {
    //             SliceKey key = aseSlice.Keys[k];

    //             SpriteSheetFrameRegionContent region = new(name, key.Bounds, color, key.CenterBounds, key.Pivot);
    //             frames[key.FrameIndex].Regions.Add(name, region);

    //             //  Perform interpolation before caching last key
    //             if (lastKey is not null && lastKey.FrameIndex < key.FrameIndex)
    //             {
    //                 for (int offset = 1; offset < key.FrameIndex - lastKey.FrameIndex; offset++)
    //                 {
    //                     SpriteSheetFrameRegionContent interpolatedRegion = new(name, lastKey.Bounds, color, lastKey.CenterBounds, lastKey.Pivot);
    //                     frames[lastKey.FrameIndex + offset].Regions.Add(name, interpolatedRegion);
    //                 }
    //             }

    //             lastKey = key;
    //         }

    //         //  Do we need to interpolate the last key given up to the final
    //         //  frame?
    //         if (lastKey?.FrameIndex < file.Frames.Count)
    //         {
    //             for (int offset = 1; offset < file.Frames.Count - lastKey.FrameIndex; offset++)
    //             {
    //                 SpriteSheetFrameRegionContent interpolatedRegion = new(name, lastKey.Bounds, color, lastKey.CenterBounds, lastKey.Pivot);
    //                 frames[lastKey.FrameIndex + offset].Regions.Add(name, interpolatedRegion);
    //             }
    //         }
    //     }
    // }

    private record CreateTextureContentResult(TextureContent Content, Dictionary<int, int> DuplicateMap, int Columns);
}
