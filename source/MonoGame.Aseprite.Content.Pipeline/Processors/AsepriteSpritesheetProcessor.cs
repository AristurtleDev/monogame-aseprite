/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

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
using MonoGame.Aseprite.AsepriteTypes;

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
        List<Frame> frames = GetFrames(input, out int width, out int height, out Color[] pixels);
        List<Tag> tags = GetTags(input);
        List<Slice> slices = GetSlices(input);

        return new AsepriteSpritesheetProcessorResult(input.Name, width, height, pixels, frames, tags, slices);
    }

    private List<Frame> GetFrames(AsepriteFile file, out int width, out int height, out Color[] pixels)
    {
        List<Color[]> flattenedFrames = new();

        for (int i = 0; i < file.Frames.Count; i++)
        {
            flattenedFrames.Add(file.Frames[i].FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer));
        }

        List<Frame> frames = new();
        int nFrames = flattenedFrames.Count;
        Dictionary<int, Frame> originalToDuplicateLookup = new();
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
        width = (columns * file.FrameWidth) +
                (BorderPadding * 2) +
                (Spacing * (columns - 1)) +
                (InnerPadding * 2 * columns);

        height = (rows * file.FrameHeight) +
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
                    int x = (p % file.FrameWidth) + (frameColumn * file.FrameWidth);
                    int y = (p / file.FrameWidth) + (frameRow * file.FrameHeight);

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
                    X = frameColumn * file.FrameWidth,
                    Y = frameRow * file.FrameHeight,
                    Width = file.FrameWidth,
                    Height = file.FrameHeight
                };

                sourceRectangle.X += BorderPadding +
                                     (Spacing * frameColumn) +
                                     (InnerPadding * (frameColumn + 1 + frameColumn));

                sourceRectangle.Y += BorderPadding +
                                     (Spacing * frameRow) +
                                     (InnerPadding * (frameRow + 1 + frameRow));

                Frame frame = new(sourceRectangle, TimeSpan.FromMilliseconds(file.Frames[fNum].Duration));
                frames.Add(frame);
                originalToDuplicateLookup.Add(fNum, frame);
            }
            else
            {
                //  We are merging duplicates and it was detected that the
                //  current frame to process is a duplicate.  so we still need
                //  to create the frame data
                Frame original = originalToDuplicateLookup[duplicateMap[fNum]];
                Frame frame = original;
                frames.Add(frame);
                fOffset++;
            }
        }

        return frames;
    }

    private List<Tag> GetTags(AsepriteFile file)
    {
        List<Tag> tags = new();

        for (int i = 0; i < file.Tags.Count; i++)
        {
            AsepriteTag aseTag = file.Tags[i];
            Tag tag = new(aseTag.Name, aseTag.From, aseTag.To, aseTag.Direction, aseTag.Color);
            tags.Add(tag);
        }

        return tags;
    }

    private List<Slice> GetSlices(AsepriteFile file)
    {
        List<Slice> slices = new();

        //  Slice keys in Aseprite are defined with a frame index that indicates
        //  the frame that the key starts on, but doesn't give a value for what
        //  frame it ends on or may be transformed on.  So we'll interpolate the
        //  keys to create the Slice elements
        for (int i = 0; i < file.Slices.Count; i++)
        {
            AsepriteSlice aseSlice = file.Slices[i];
            string name = aseSlice.Name;

            //  If no color defined in user data, use Aseprite default for slice
            //  color, which is just blue
            Color color = aseSlice.UserData.Color ?? new Color(0, 0, 255, 255);

            AsepriteSliceKey? lastKey = default;

            for (int k = 0; k < aseSlice.KeyCount; k++)
            {
                AsepriteSliceKey key = aseSlice[k];

                Slice slice = new(name: name,
                                  color: color,
                                  frame: key.FrameIndex,
                                  bounds: key.Bounds,
                                  center: key.CenterBounds,
                                  pivot: key.Pivot);

                //  Perform interpolation before adding the slice
                if (lastKey is not null && lastKey.FrameIndex < key.FrameIndex)
                {
                    for (int offset = 1; offset < key.FrameIndex - lastKey.FrameIndex; offset++)
                    {
                        Slice interpolatedSlice = new(name: name,
                                                      color: color,
                                                      frame: lastKey.FrameIndex,
                                                      bounds: lastKey.Bounds,
                                                      center: lastKey.CenterBounds,
                                                      pivot: lastKey.Pivot);
                        slices.Add(interpolatedSlice);
                    }
                }

                slices.Add(slice);
                lastKey = key;
            }

            //  Do we need to interpolate the last key given up to the final
            //  frame?
            if (lastKey?.FrameIndex < file.Frames.Count)
            {
                for (int offset = 1; offset < file.Frames.Count - lastKey.FrameIndex; offset++)
                {
                    Slice interpolatedSlice = new(name: name,
                                                  color: color,
                                                  frame: lastKey.FrameIndex,
                                                  bounds: lastKey.Bounds,
                                                  center: lastKey.CenterBounds,
                                                  pivot: lastKey.Pivot);

                    slices.Add(interpolatedSlice);
                }
            }
        }

        return slices;
    }
}
