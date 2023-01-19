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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Processors;

/// <summary>
///     Defines a processor to generate a <see cref="SpriteSheet"/> from the contents of an <see cref="AsepriteFile"/>.
/// </summary>
public static class SpriteSheetProcessor
{
    private record GenerateImageResult(Color[] Pixels, int Width, int Height, int FrameWidth, int FrameHeight, int Columns, int Rows, Dictionary<int, int> DuplicateMap);

    /// <summary>
    ///     Processes the contents of an <see cref="AsepriteFile"/> as a <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="GraphicsDevice"/> used to create the resources.
    /// </param>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="configuration">
    ///     An instance of the <see cref="SpriteSheetProcessorConfiguration"/> class that defines configurations for
    ///     this process.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="SpriteSheet"/> class that is created by this method.
    /// </returns>
    public static SpriteSheet Process(GraphicsDevice device, AsepriteFile file, SpriteSheetProcessorConfiguration configuration)
    {
        SpriteSheetProcessorResult result = Process(file, configuration);
        Texture2D texture = new(device, result.Width, result.Height, mipmap: false, SurfaceFormat.Color);
        texture.SetData<Color>(result.Pixels);

        SpriteSheet spriteSheet = new(result.Name, texture);

        for (int i = 0; i < result.Regions.Count; i++)
        {
            //  Use the same naming convention as Aseprite when you do export spritesheet
            //  which is "{fileName} {frameNumber}.aseprite"
            spriteSheet.CreateRegion($"{result.Name} {i}.aseprite", result.Regions[i]);
        }

        foreach (var entry in result.Animations)
        {
            spriteSheet.CreateAnimationCycle(entry.Key, builder =>
            {
                for (int i = 0; i < entry.Value.Item1.Length; i++)
                {
                    int index = entry.Value.Item1[i];
                    TimeSpan duration = TimeSpan.FromMilliseconds(entry.Value.Item2[i]);
                    builder.AddFrame(index, duration);
                }

                builder.IsLooping(entry.Value.Item3)
                       .IsReversed(entry.Value.Item4)
                       .IsPingPong(entry.Value.Item5);
            });
        }

        return spriteSheet;
    }

    /// <summary>
    ///     Processes the contents of an <see cref="AsepriteFile"/> as a <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="GraphicsDevice"/> used to create the resources.
    /// </param>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="configure">
    ///     An instance of the <see cref="SpriteSheetProcessorConfiguration"/> class that defines configurations for
    ///     this process.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="SpriteSheet"/> class that is created by this method.
    /// </returns>
    public static SpriteSheet Process(GraphicsDevice device, AsepriteFile file, Action<SpriteSheetProcessorConfiguration> configure)
    {
        SpriteSheetProcessorConfiguration configuration = new();
        configure(configuration);
        return Process(device, file, configuration);
    }

    internal static SpriteSheetProcessorResult Process(AsepriteFile file, SpriteSheetProcessorConfiguration configuration)
    {
        GenerateImageResult image = GenerateImage(file, configuration);
        List<Rectangle> regions = GenerateTextureRegionBounds(file.Frames, image, configuration);
        Dictionary<string, (int[], int[], bool, bool, bool)> animations = GenerateAnimationCycles(file.Tags, file.Frames);

        return new(file.Name, image.Pixels, image.Width, image.Height, regions, animations);
    }

    private static GenerateImageResult GenerateImage(AsepriteFile file, SpriteSheetProcessorConfiguration configuration)
    {
        int frameWidth = file.CanvasWidth;
        int frameHeight = file.CanvasHeight;
        int frameCount = file.Frames.Length;

        Color[][] flattenedFrames = FlattenFrames(file.Frames, configuration);
        Dictionary<int, int> duplicateMap = BuildDuplicateMap(flattenedFrames);

        if (configuration.MergeDuplicateFrames)
        {
            frameCount -= duplicateMap.Count;
        }

        double sqrt = Math.Sqrt(frameCount);
        int columns = (int)Math.Ceiling(sqrt);
        int rows = (frameCount + columns - 1) / columns;

        int imageWidth = (columns * frameWidth)
                         + (configuration.BorderPadding * 2)
                         + (configuration.Spacing * (columns - 1))
                         + (configuration.InnerPadding * 2 * columns);

        int imageHeight = (rows * frameHeight)
                          + (configuration.BorderPadding * 2)
                          + (configuration.Spacing * (rows - 1))
                          + (configuration.InnerPadding * 2 * rows);

        Color[] image = new Color[imageWidth * imageHeight];

        int offset = 0;
        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            if (configuration.MergeDuplicateFrames && duplicateMap.ContainsKey(i))
            {
                offset++;
                continue;
            }

            int column = (i - offset) % columns;
            int row = (i - offset) / columns;
            Color[] frame = flattenedFrames[i];
            CopyFrameToImage(frame, image, column, row, frameWidth, frameHeight, imageWidth, configuration);
        }

        return new(image, imageWidth, imageHeight, frameWidth, frameHeight, columns, rows, duplicateMap);
    }

    private static Color[][] FlattenFrames(ReadOnlySpan<AsepriteFrame> frames, SpriteSheetProcessorConfiguration configuration)
    {
        Color[][] result = new Color[frames.Length][];

        for (int i = 0; i < frames.Length; i++)
        {
            result[i] = frames[i].FlattenFrame(configuration.OnlyVisibleLayers, configuration.IncludeBackgroundLayer);
        }

        return result;
    }

    private static Dictionary<int, int> BuildDuplicateMap(Color[][] frames)
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

    private static void CopyFrameToImage(ReadOnlySpan<Color> frame, Span<Color> image, int column, int row, int frameWidth, int frameHeight, int imageWidth, SpriteSheetProcessorConfiguration configuration)
    {
        for (int i = 0; i < frame.Length; i++)
        {
            int x = (i % frameWidth)
                    + (column * frameWidth)
                    + configuration.BorderPadding
                    + (configuration.Spacing * column)
                    + (configuration.InnerPadding * (column + column + 1));

            int y = (i / frameWidth)
                    + (row * frameHeight)
                    + configuration.BorderPadding
                    + (configuration.Spacing * row)
                    + (configuration.InnerPadding * (row + row + 1));

            int index = y * imageWidth + x;
            image[index] = frame[i];

        }
    }

    private static List<Rectangle> GenerateTextureRegionBounds(ReadOnlySpan<AsepriteFrame> frames, GenerateImageResult image, SpriteSheetProcessorConfiguration configuration)
    {
        List<Rectangle> regionBounds = new();
        Dictionary<int, Rectangle> originalToDuplicateLookup = new();
        int offset = 0;

        for (int i = 0; i < frames.Length; i++)
        {
            if (configuration.MergeDuplicateFrames & image.DuplicateMap.ContainsKey(i))
            {
                Rectangle original = originalToDuplicateLookup[image.DuplicateMap[i]];
                Rectangle duplicate = original;
                regionBounds.Add(duplicate);
                offset++;
                continue;
            }

            int column = (i - offset) % image.Columns;
            int row = (i - offset) / image.Columns;

            int x = (column * image.FrameWidth)
                    + configuration.BorderPadding
                    + (configuration.Spacing * column)
                    + (configuration.InnerPadding * (column + column + 1));

            int y = (row * image.FrameHeight)
                    + configuration.BorderPadding
                    + (configuration.Spacing * row)
                    + (configuration.InnerPadding * (row * row + 1));

            Rectangle bounds = new(x, y, image.FrameWidth, image.FrameHeight);
            regionBounds.Add(bounds);
            originalToDuplicateLookup.Add(i, bounds);
        }

        return regionBounds;
    }

    private static Dictionary<string, (int[], int[], bool, bool, bool)> GenerateAnimationCycles(ReadOnlySpan<AsepriteTag> tags, ReadOnlySpan<AsepriteFrame> frames)
    {
        Dictionary<string, (int[], int[], bool, bool, bool)> animations = new();


        for (int i = 0; i < tags.Length; i++)
        {
            AsepriteTag tag = tags[i];
            int frameCount = tag.To - tag.From + 1;
            int[] animationFrames = new int[frameCount];
            int[] frameDurations = new int[frameCount];

            for (int j = 0; j < frameCount; j++)
            {
                int index = tag.From + j;
                animationFrames[j] = index;
                frameDurations[j] = frames[index].Duration;
            }

            //  All aseprite animations are looping by default
            bool isLooping = true;
            bool isReversed = tag.Direction == AsepriteLoopDirection.Reverse;
            bool isPingPong = tag.Direction == AsepriteLoopDirection.PingPong;

            animations.Add(tag.Name, (animationFrames, frameDurations, isLooping, isReversed, isPingPong));
        }

        return animations;
    }
}

