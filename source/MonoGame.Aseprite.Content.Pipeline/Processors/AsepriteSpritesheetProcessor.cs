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

using System.Collections.Immutable;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

[ContentProcessor(DisplayName = "Aseprite Spritesheet Processor - MonoGame.Aseprite")]
public sealed class AsepriteSpritesheetProcessor : ContentProcessor<AsepriteFile, AsepriteSpritesheetProcessorResult>
{
    /// <summary>
    ///     Indicates whether only cels that are on layers that are visible
    ///     should be used.
    /// </summary>
    [DisplayName("Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Indicates whether the layer marked as a background layer in
    ///     Aseprite should be included when generating image data.
    /// </summary>
    [DisplayName("Include Background Layer")]
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    ///     Indicates whether frames that are detected as being duplicates
    ///     should be merged into a single frame.
    /// </summary>
    [DisplayName("Merge Duplicate Frames")]
    public bool MergeDuplicateFrames { get; set; } = true;

    /// <summary>
    ///     The amount of transparent pixels to add between the edge of the
    ///     spritesheet and the frames.
    /// </summary>
    [DisplayName("Border Padding")]
    public int BorderPadding { get; set; } = 0;

    /// <summary>
    ///     The amount of transparent pixels to add between each frame.
    /// </summary>
    [DisplayName("Spacing")]
    public int Spacing { get; set; } = 0;

    /// <summary>
    ///     The amount of transparent pixels to add around the edge of each
    ///     frame.
    /// </summary>
    [DisplayName("Inner Padding")]
    public int InnerPadding { get; set; } = 0;

    public override AsepriteSpritesheetProcessorResult Process(AsepriteFile input, ContentProcessorContext context)
    {
        List<SpriteSheetFrameContent> frames = GenerateFrameAndImageData(input, out int width, out int height, out Color[] pixels);
        List<SpriteSheetAnimationDefinition> tags = GenerateAnimationDefinitionData(input);
        GenerateFrameRegionData(input, frames);

        return new AsepriteSpritesheetProcessorResult(input.Name, new Point(width, height), pixels, frames, tags);
    }

    private List<SpriteSheetFrameContent> GenerateFrameAndImageData(AsepriteFile file, out int width, out int height, out Color[] pixels)
    {
        List<Color[]> flattenedFrames = new();

        for (int i = 0; i < file.Frames.Count; i++)
        {
            flattenedFrames.Add(file.Frames[i].FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer));
        }

        List<SpriteSheetFrameContent> frames = new();
        int nFrames = flattenedFrames.Count;
        Dictionary<int, SpriteSheetFrameContent> originalToDuplicateLookup = new();
        Dictionary<int, int> duplicateMap = new();

        if (MergeDuplicateFrames)
        {
            for (int i = 0; i < flattenedFrames.Count; i++)
            {
                for (int d = 0; d < i; d++)
                {
                    if (flattenedFrames[i].SequenceEqual(flattenedFrames[d]))
                    {
                        duplicateMap.Add(i, d);
                        nFrames--;
                        break;
                    }
                }
            }
        }

        //  Determine the number of columns and rows needed to pack frames into
        //  a spritesheet image
        double sqrt = Math.Sqrt(nFrames);
        int columns = (int)Math.Floor(sqrt);
        if (Math.Abs(sqrt % 1) >= double.Epsilon)
        {
            columns++;
        }

        int rows = nFrames / columns;
        if (nFrames % columns != 0)
        {
            rows++;
        }

        //  Determine the final width and height of the spritesheet image based
        //  on the number of columns and rows, adjusting for padding and spacing
        width = (columns * file.FrameSize.X) +
                (BorderPadding * 2) +
                (Spacing * (columns - 1)) +
                (InnerPadding * 2 * columns);

        height = (rows * file.FrameSize.Y) +
                 (BorderPadding * 2) +
                 (Spacing * (rows - 1)) +
                 (InnerPadding * 2 * rows);

        pixels = new Color[width * height];

        //  Offset for when we detect merged frames
        int fOffset = 0;

        for (int fNum = 0; fNum < flattenedFrames.Count; fNum++)
        {
            if (!MergeDuplicateFrames || !duplicateMap.ContainsKey(fNum))
            {
                //  Calculate the x- and y-coordinate position of the frame's
                //  top-left pixel relative to the top-left of the final
                //  spritesheet image
                int frameColumn = (fNum - fOffset) % columns;
                int frameRow = (fNum - fOffset) / columns;

                //  Inject the pixel color data from the frame into the final
                //  spritesheet image
                Color[] framePixels = flattenedFrames[fNum];

                for (int p = 0; p < framePixels.Length; p++)
                {
                    int x = (p % file.FrameSize.X) + (frameColumn * file.FrameSize.X);
                    int y = (p / file.FrameSize.X) + (frameRow * file.FrameSize.Y);

                    //  Adjust x- and y-coordinate for any padding and/or
                    //  spacing.
                    x += BorderPadding +
                         (Spacing * frameColumn) +
                         (InnerPadding * (frameColumn + 1 + frameColumn));

                    y += BorderPadding +
                         (Spacing * frameRow) +
                         (InnerPadding * (frameRow + 1 + frameRow));

                    int index = y * width + x;
                    pixels[index] = framePixels[p];
                }

                //  Now create the frame data
                Rectangle sourceRectangle = new Rectangle
                {
                    X = frameColumn * file.FrameSize.X,
                    Y = frameRow * file.FrameSize.Y,
                    Width = file.FrameSize.X,
                    Height = file.FrameSize.Y
                };

                sourceRectangle.X += BorderPadding +
                                     (Spacing * frameColumn) +
                                     (InnerPadding * (frameColumn + 1 + frameColumn));

                sourceRectangle.Y += BorderPadding +
                                     (Spacing * frameRow) +
                                     (InnerPadding * (frameRow + 1 + frameRow));

                SpriteSheetFrameContent frame = new($"frame_{fNum}", sourceRectangle, TimeSpan.FromMilliseconds(file.Frames[fNum].Duration));
                // Frame frame = new(sourceRectangle, TimeSpan.FromMilliseconds(file.Frames[fNum].Duration));
                frames.Add(frame);
                originalToDuplicateLookup.Add(fNum, frame);
            }
            else
            {
                //  We are merging duplicates and it was detected that the
                //  current frame to process is a duplicate.  so we still need
                //  to create the frame data
                SpriteSheetFrameContent original = originalToDuplicateLookup[duplicateMap[fNum]];
                SpriteSheetFrameContent duplicate = new($"frame_{fNum}", original.Bounds, original.Duration);
                frames.Add(duplicate);
                fOffset++;
            }
        }

        return frames;
    }

    private List<SpriteSheetAnimationDefinition> GenerateAnimationDefinitionData(AsepriteFile file)
    {
        List<SpriteSheetAnimationDefinition> definitions = new();

        for (int i = 0; i < file.Tags.Count; i++)
        {
            Tag aseTag = file.Tags[i];
            int[] frameIndexes = new int[aseTag.To - aseTag.From + 1];
            for (int f = 0; f < frameIndexes.Length; f++)
            {
                frameIndexes[f] = aseTag.From + f;
            }
            SpriteSheetAnimationDefinition definition = new(frameIndexes, aseTag.Name, true, aseTag.Direction == LoopDirection.Reverse, aseTag.Direction == LoopDirection.PingPong);
            definitions.Add(definition);
        }

        return definitions;
    }

    private void GenerateFrameRegionData(AsepriteFile file, List<SpriteSheetFrameContent> frames)
    {
        List<SpriteSheetFrameRegion> regions = new();

        //  Slice keys in Aseprite are defined with a frame index that indicates
        //  the frame that the key starts on, but doesn't give a value for what
        //  frame it ends on or may be transformed on.  So we'll interpolate the
        //  keys to create the Slice elements
        for (int i = 0; i < file.Slices.Count; i++)
        {
            Slice aseSlice = file.Slices[i];
            string name = aseSlice.Name;

            //  If no color defined in user data, use Aseprite default for slice
            //  color, which is just blue
            Color color = aseSlice.UserData?.Color ?? new Color(0, 0, 255, 255);

            SliceKey? lastKey = default;

            for (int k = 0; k < aseSlice.Keys.Count; k++)
            {
                SliceKey key = aseSlice.Keys[k];

                SpriteSheetFrameRegion region = new(name, key.Bounds, color, key.CenterBounds, key.Pivot);
                frames[key.FrameIndex].Regions.Add(name, region);

                //  Perform interpolation before caching last key
                if (lastKey is not null && lastKey.FrameIndex < key.FrameIndex)
                {
                    for (int offset = 1; offset < key.FrameIndex - lastKey.FrameIndex; offset++)
                    {
                        SpriteSheetFrameRegion interpolatedRegion = new(name, lastKey.Bounds, color, lastKey.CenterBounds, lastKey.Pivot);
                        frames[lastKey.FrameIndex + offset].Regions.Add(name, interpolatedRegion);
                    }
                }

                lastKey = key;
            }

            //  Do we need to interpolate the last key given up to the final
            //  frame?
            if (lastKey?.FrameIndex < file.Frames.Count)
            {
                for (int offset = 1; offset < file.Frames.Count - lastKey.FrameIndex; offset++)
                {
                    SpriteSheetFrameRegion interpolatedRegion = new(name, lastKey.Bounds, color, lastKey.CenterBounds, lastKey.Pivot);
                    frames[lastKey.FrameIndex + offset].Regions.Add(name, interpolatedRegion);
                }
            }
        }
    }
}
