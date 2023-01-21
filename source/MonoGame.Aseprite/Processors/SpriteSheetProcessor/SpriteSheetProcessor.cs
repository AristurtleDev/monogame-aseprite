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
    private record SpriteSheetData(RawTexture RawTexture, int FrameWidth, int FrameHeight, int Columns, int Rows, Dictionary<int, int> DuplicateMap);

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
        RawSpriteSheet rawSpriteSheet = GetRawSpriteSheet(file, configuration);
        RawTexture rawTexture = rawSpriteSheet.Texture;

        Texture2D texture = new(device, rawTexture.Width, rawTexture.Height, mipmap: false, SurfaceFormat.Color);
        texture.SetData<Color>(rawTexture.Pixels);

        SpriteSheet spriteSheet = new(rawTexture.Name, texture, rawSpriteSheet.Regions, rawSpriteSheet.Cycles);
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

    internal static RawSpriteSheet GetRawSpriteSheet(AsepriteFile file, SpriteSheetProcessorConfiguration configuration)
    {
        SpriteSheetData spriteSheetData = GenerateSpriteSheetData(file, configuration);
        Rectangle[] regions = GenerateRawTextureRegions(file.Frames, spriteSheetData, configuration);
        Dictionary<string, RawAnimationCycle> animations = GenerateRawAnimationCycles(file.Tags, file.Frames);

        return new(file.Name, spriteSheetData.RawTexture, regions, animations);
    }

    private static SpriteSheetData GenerateSpriteSheetData(AsepriteFile file, SpriteSheetProcessorConfiguration configuration)
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

        int rawImageWidth = (columns * frameWidth)
                            + (configuration.BorderPadding * 2)
                            + (configuration.Spacing * (columns - 1))
                            + (configuration.InnerPadding * 2 * columns);

        int rawImageHeight = (rows * frameHeight)
                             + (configuration.BorderPadding * 2)
                             + (configuration.Spacing * (rows - 1))
                             + (configuration.InnerPadding * 2 * rows);

        Color[] rawImagePixels = new Color[rawImageWidth * rawImageHeight];

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
            CopyFrameToRawImage(frame, rawImagePixels, column, row, frameWidth, frameHeight, rawImageWidth, configuration);
        }

        RawTexture rawTexture = new(file.Name, rawImagePixels, rawImageWidth, rawImageHeight);
        return new(rawTexture, frameWidth, frameHeight, columns, rows, duplicateMap);
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

    private static void CopyFrameToRawImage(ReadOnlySpan<Color> frame, Span<Color> image, int column, int row, int frameWidth, int frameHeight, int imageWidth, SpriteSheetProcessorConfiguration configuration)
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

    private static Rectangle[] GenerateRawTextureRegions(ReadOnlySpan<AsepriteFrame> frames, SpriteSheetData image, SpriteSheetProcessorConfiguration configuration)
    {
        Rectangle[] rawRegions = new Rectangle[frames.Length];
        Dictionary<int, Rectangle> originalToDuplicateLookup = new();
        int offset = 0;

        for (int i = 0; i < frames.Length; i++)
        {
            if (configuration.MergeDuplicateFrames & image.DuplicateMap.ContainsKey(i))
            {
                Rectangle original = originalToDuplicateLookup[image.DuplicateMap[i]];
                Rectangle duplicate = original;
                rawRegions[i] = duplicate;
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

            Rectangle rawRegion = new(x, y, image.FrameWidth, image.FrameHeight);
            rawRegions[i] = rawRegion;
            originalToDuplicateLookup.Add(i, rawRegion);
        }

        return rawRegions;
    }

    private static Dictionary<string, RawAnimationCycle> GenerateRawAnimationCycles(ReadOnlySpan<AsepriteTag> tags, ReadOnlySpan<AsepriteFrame> frames)
    {
        Dictionary<string, RawAnimationCycle> rawAnimations = new();


        for (int i = 0; i < tags.Length; i++)
        {
            AsepriteTag tag = tags[i];
            int frameCount = tag.To - tag.From + 1;
            int[] rawAnimationFrames = new int[frameCount];
            int[] rawAnimationFrameDurations = new int[frameCount];

            for (int j = 0; j < frameCount; j++)
            {
                int index = tag.From + j;
                rawAnimationFrames[j] = index;
                rawAnimationFrameDurations[j] = frames[index].Duration;
            }

            //  All aseprite animations are looping by default
            bool isLooping = true;
            bool isReversed = tag.Direction == AsepriteLoopDirection.Reverse;
            bool isPingPong = tag.Direction == AsepriteLoopDirection.PingPong;

            RawAnimationCycle rawAnimation = new(rawAnimationFrames, rawAnimationFrameDurations, isLooping, isReversed, isPingPong);
            if (!rawAnimations.TryAdd(tag.Name, rawAnimation))
            {
                throw new InvalidOperationException($"{nameof(SpriteSheet)} animations must have a unique name. Please check that you do not have duplicate Tag names in your Aseprite file");
            }
        }

        return rawAnimations;
    }
}

