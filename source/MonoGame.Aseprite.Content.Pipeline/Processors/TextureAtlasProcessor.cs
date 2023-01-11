// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2018-2023 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */

// using System.ComponentModel;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Content.Pipeline;
// using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

// namespace MonoGame.Aseprite.Content.Pipeline.Processors;

// /// <summary>
// ///     Provides method for processing all <see cref="Frame"/>, in an
// ///     <see cref="AsepriteFile"/> as an instance of the
// ///     <see cref="TextureAtlasContent"/> class.
// /// </summary>
// [ContentProcessor(DisplayName = "Aseprite Texture Atlas Processor - MonoGame.Aseprite")]
// public sealed class TextureAtlasProcessor : ContentProcessor<AsepriteFile, TextureAtlasContent>
// {
//     /// <summary>
//     ///     Indicates whether only <see cref="Cel"/> elements that are on
//     ///     <see cref="Layer"/> elements that are visible should be used.
//     /// </summary>
//     /// <remarks>
//     ///     This value is set in the property window of the mgcb-editor
//     /// </remarks>
//     [DisplayName("Only Visible Layers")]
//     public bool OnlyVisibleLayers { get; set; } = true;

//     /// <summary>
//     ///     Indicates whether <see cref="Cel"/> elements that are on the
//     ///     <see cref="Layer"/> element that is set as the background should be
//     ///     included.
//     /// </summary>
//     /// <remarks>
//     ///     <para>
//     ///         A <see cref="Layer"/> has to be set as the background in
//     ///         Aseprite.  Being the bottom layer does not automatically mean it
//     ///         is a background layer.
//     ///     </para>
//     ///     <para>
//     ///         This value is set in the property window of the mgcb-editor
//     ///     </para>
//     /// </remarks>
//     [DisplayName("Include Background Layer")]
//     public bool IncludeBackgroundLayer { get; set; } = false;

//     /// <summary>
//     ///     Indicates whether <see cref="Frame"/> elements that are detected
//     ///     as being duplicates should be merged into a single element.
//     /// </summary>
//     /// <remarks>
//     ///     This value is set in the property window of the mgcb-editor
//     /// </remarks>
//     [DisplayName("Merge Duplicate Frames")]
//     public bool MergeDuplicateFrames { get; set; } = true;

//     /// <summary>
//     ///     The amount of transparent pixels to add between the edge of the
//     ///     generated texture for the spritesheet and the frame regions within
//     ///     it.
//     /// </summary>
//     /// <remarks>
//     ///     This value is set in the property window of the mgcb-editor
//     /// </remarks>
//     [DisplayName("Border Padding")]
//     public int BorderPadding { get; set; } = 0;

//     /// <summary>
//     ///     The amount of transparent pixels to add between each frame region
//     ///     in the generated texture for the spritesheet.
//     /// </summary>
//     /// <remarks>
//     ///     This value is set in the property window of the mgcb-editor
//     /// </remarks>
//     [DisplayName("Spacing")]
//     public int Spacing { get; set; } = 0;

//     /// <summary>
//     ///     The amount of transparent pixels to add around the edge of each
//     ///     frame region in the generated texture for the spritesheet.
//     /// </summary>
//     /// <remarks>
//     ///     This value is set in the property window of the mgcb-editor
//     /// </remarks>
//     [DisplayName("Inner Padding")]
//     public int InnerPadding { get; set; } = 0;

//     /// <summary>
//     ///     Processes all <see cref="Frame"/>, elements in an
//     ///     <see cref="AsepriteFile"/>.  The result is a new instance of the
//     ///     <see cref="TextureAtlasContent"/> class containing the texture
//     ///     content and data generated to be written to the xnb file.
//     /// </summary>
//     /// <param name="file">
//     ///     The <see cref="AsepriteFile"/> to process.
//     /// </param>
//     /// <param name="context">
//     ///     The context of the content processor.
//     /// </param>
//     /// <returns>
//     ///     A new instance of the <see cref="TextureAtlasContent"/> class
//     ///     containing the texture content and data generated to be written to
//     ///     the xnb file.
//     /// </returns>
//     public override TextureAtlasContent Process(AsepriteFile input, ContentProcessorContext context) =>
//         GenerateTextureAtlasContent(input);

//     private TextureAtlasContent GenerateTextureAtlasContent(AsepriteFile input)
//     {
//         List<TextureAtlasRegionContent> regions = new();
//         Dictionary<int, int> duplicateMap = new();
//         Dictionary<int, TextureAtlasRegionContent> originalToDuplicateLookup = new();

//         Color[][] flattenedFrames = FlattenFrames(input.Frames);

//         int nFrames = flattenedFrames.GetLength(0);

//         if (MergeDuplicateFrames)
//         {
//             BuildDuplicateMap(duplicateMap, flattenedFrames, ref nFrames);
//         }

//         CalculateColumnAndRowCount(nFrames, out int columns, out int rows);

//         int width = (columns * input.FrameSize.X) +
//                    (BorderPadding * 2) +
//                    (Spacing * (columns - 1)) +
//                    (InnerPadding * 2 * columns);

//         int height = (rows * input.FrameSize.Y) +
//                      (BorderPadding * 2) +
//                      (Spacing * (columns - 1)) +
//                      (InnerPadding * 2 * columns);

//         Color[] pixels = new Color[width * height];

//         int fOffset = 0;
//         for (int fNum = 0; fNum < flattenedFrames.GetLength(0); fNum++)
//         {
//             if (MergeDuplicateFrames && duplicateMap.ContainsKey(fNum))
//             {
//                 TextureAtlasRegionContent originalRegion = originalToDuplicateLookup[duplicateMap[fNum]];
//                 TextureAtlasRegionContent duplicate = new($"frame_{fNum}", originalRegion.Bounds);
//                 regions.Add(duplicate);
//                 fOffset++;
//                 continue;
//             }

//             int fColumn = (fNum - fOffset) % columns;
//             int fRow = (fNum - fOffset) / columns;

//             Color[] fPixels = flattenedFrames[fNum];

//             for (int p = 0; p < fPixels.Length; p++)
//             {
//                 int x = (p % input.FrameSize.X) + (fColumn * input.FrameSize.X);
//                 int y = (p / input.FrameSize.X) + (fRow * input.FrameSize.Y);

//                 x += BorderPadding +
//                      (Spacing * fColumn) +
//                      (InnerPadding * (fColumn + 1 + fColumn));

//                 y += BorderPadding +
//                      (Spacing * fRow) +
//                      (InnerPadding * (fRow + 1 + fRow));

//                 int index = y * width * x;
//                 pixels[index] = fPixels[p];
//             }

//             Rectangle bounds = new
//             (
//                 x: fColumn * input.FrameSize.X,
//                 y: fRow * input.FrameSize.Y,
//                 width: input.FrameSize.X,
//                 height: input.FrameSize.Y
//             );

//             bounds.X += BorderPadding +
//                         (Spacing * fColumn) +
//                         (InnerPadding * (fColumn + 1 + fColumn));

//             bounds.Y += BorderPadding +
//                         (Spacing * fRow) +
//                         (InnerPadding * (fRow + 1 + fRow));

//             TextureAtlasRegionContent regionContent = new($"frame_{fNum}", bounds);
//             regions.Add(regionContent);
//             originalToDuplicateLookup.Add(fNum, regionContent);
//         }

//         TextureContent textureContent = new(new Point(width, height), pixels);
//         TextureAtlasContent atlasContent = new(input.Name, textureContent, regions);
//         return atlasContent;
//     }

//     private Color[][] FlattenFrames(List<Frame> frames)
//     {
//         int count = frames.Count;
//         Color[][] flattenedFrames = new Color[count][];

//         for (int i = 0; i < count; i++)
//         {
//             Frame frame = frames[i];
//             flattenedFrames[i] = frame.FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer);
//         }

//         return flattenedFrames;
//     }

//     private void BuildDuplicateMap(Dictionary<int, int> map, Color[][] frames, ref int nFrames)
//     {
//         for (int i = 0; i < nFrames; i++)
//         {
//             for (int d = 0; d < i; d++)
//             {
//                 if (frames[i].SequenceEqual(frames[d]))
//                 {
//                     map.Add(i, d);
//                     nFrames--;
//                     break;
//                 }
//             }
//         }
//     }

//     private void CalculateColumnAndRowCount(int nFrames, out int columns, out int rows)
//     {
//         CalculateTotalColumns(nFrames, out columns);
//         CalculateTotalRows(nFrames, columns, out rows);
//     }

//     private void CalculateTotalColumns(int nFrames, out int columns)
//     {
//         double sqrt = Math.Sqrt(nFrames);
//         columns = (int)Math.Floor(sqrt);
//         if (Math.Abs(sqrt % 1) >= double.Epsilon)
//         {
//             columns++;
//         }
//     }

//     private void CalculateTotalRows(int nFrames, int columns, out int rows)
//     {
//         rows = nFrames / columns;
//         if (nFrames % columns != 0)
//         {
//             rows++;
//         }
//     }
























//     private List<SpriteSheetFrameContent> GenerateFrameAndImageData(AsepriteFile file, out TextureContent textureContent)
//     {
//         List<Color[]> flattenedFrames = new();

//         for (int i = 0; i < file.Frames.Count; i++)
//         {
//             flattenedFrames.Add(file.Frames[i].FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer));
//         }

//         List<SpriteSheetFrameContent> frames = new();
//         int nFrames = flattenedFrames.Count;
//         Dictionary<int, SpriteSheetFrameContent> originalToDuplicateLookup = new();
//         Dictionary<int, int> duplicateMap = new();

//         if (MergeDuplicateFrames)
//         {
//             for (int i = 0; i < flattenedFrames.Count; i++)
//             {
//                 for (int d = 0; d < i; d++)
//                 {
//                     if (flattenedFrames[i].SequenceEqual(flattenedFrames[d]))
//                     {
//                         duplicateMap.Add(i, d);
//                         nFrames--;
//                         break;
//                     }
//                 }
//             }
//         }

//         //  Determine the number of columns and rows needed to pack frames into
//         //  a spritesheet image
//         double sqrt = Math.Sqrt(nFrames);
//         int columns = (int)Math.Floor(sqrt);
//         if (Math.Abs(sqrt % 1) >= double.Epsilon)
//         {
//             columns++;
//         }

//         int rows = nFrames / columns;
//         if (nFrames % columns != 0)
//         {
//             rows++;
//         }

//         //  Determine the final width and height of the spritesheet image based
//         //  on the number of columns and rows, adjusting for padding and spacing
//         int width = (columns * file.FrameSize.X) +
//                     (BorderPadding * 2) +
//                     (Spacing * (columns - 1)) +
//                     (InnerPadding * 2 * columns);

//         int height = (rows * file.FrameSize.Y) +
//                      (BorderPadding * 2) +
//                      (Spacing * (rows - 1)) +
//                      (InnerPadding * 2 * rows);

//         Color[] pixels = new Color[width * height];

//         //  Offset for when we detect merged frames
//         int fOffset = 0;

//         for (int fNum = 0; fNum < flattenedFrames.Count; fNum++)
//         {
//             if (!MergeDuplicateFrames || !duplicateMap.ContainsKey(fNum))
//             {
//                 //  Calculate the x- and y-coordinate position of the frame's
//                 //  top-left pixel relative to the top-left of the final
//                 //  spritesheet image
//                 int frameColumn = (fNum - fOffset) % columns;
//                 int frameRow = (fNum - fOffset) / columns;

//                 //  Inject the pixel color data from the frame into the final
//                 //  spritesheet image
//                 Color[] framePixels = flattenedFrames[fNum];

//                 for (int p = 0; p < framePixels.Length; p++)
//                 {
//                     int x = (p % file.FrameSize.X) + (frameColumn * file.FrameSize.X);
//                     int y = (p / file.FrameSize.X) + (frameRow * file.FrameSize.Y);

//                     //  Adjust x- and y-coordinate for any padding and/or
//                     //  spacing.
//                     x += BorderPadding +
//                          (Spacing * frameColumn) +
//                          (InnerPadding * (frameColumn + 1 + frameColumn));

//                     y += BorderPadding +
//                          (Spacing * frameRow) +
//                          (InnerPadding * (frameRow + 1 + frameRow));

//                     int index = y * width + x;
//                     pixels[index] = framePixels[p];
//                 }

//                 //  Now create the frame data
//                 Rectangle sourceRectangle = new Rectangle
//                 {
//                     X = frameColumn * file.FrameSize.X,
//                     Y = frameRow * file.FrameSize.Y,
//                     Width = file.FrameSize.X,
//                     Height = file.FrameSize.Y
//                 };

//                 sourceRectangle.X += BorderPadding +
//                                      (Spacing * frameColumn) +
//                                      (InnerPadding * (frameColumn + 1 + frameColumn));

//                 sourceRectangle.Y += BorderPadding +
//                                      (Spacing * frameRow) +
//                                      (InnerPadding * (frameRow + 1 + frameRow));

//                 SpriteSheetFrameContent frame = new($"frame_{fNum}", sourceRectangle, file.Frames[fNum].Duration);
//                 // Frame frame = new(sourceRectangle, TimeSpan.FromMilliseconds(file.Frames[fNum].Duration));
//                 frames.Add(frame);
//                 originalToDuplicateLookup.Add(fNum, frame);
//             }
//             else
//             {
//                 //  We are merging duplicates and it was detected that the
//                 //  current frame to process is a duplicate.  so we still need
//                 //  to create the frame data
//                 SpriteSheetFrameContent original = originalToDuplicateLookup[duplicateMap[fNum]];
//                 SpriteSheetFrameContent duplicate = new($"frame_{fNum}", original.Bounds, original.Duration);
//                 frames.Add(duplicate);
//                 fOffset++;
//             }
//         }

//         textureContent = new(new Point(width, height), pixels);

//         return frames;
//     }

//     private List<SpriteSheetAnimationDefinitionContent> GenerateAnimationDefinitionData(AsepriteFile file)
//     {
//         List<SpriteSheetAnimationDefinitionContent> definitions = new();

//         for (int i = 0; i < file.Tags.Count; i++)
//         {
//             Tag aseTag = file.Tags[i];
//             int[] frameIndexes = new int[aseTag.To - aseTag.From + 1];
//             for (int f = 0; f < frameIndexes.Length; f++)
//             {
//                 frameIndexes[f] = aseTag.From + f;
//             }

//             byte loopReversePingPongMask = 1;
//             if (aseTag.Direction == 1)
//             {
//                 loopReversePingPongMask |= 2;
//             }

//             if (aseTag.Direction == 2)
//             {
//                 loopReversePingPongMask |= 4;
//             }

//             SpriteSheetAnimationDefinitionContent definition = new(frameIndexes, aseTag.Name, loopReversePingPongMask);
//             definitions.Add(definition);
//         }

//         return definitions;
//     }

//     private void GenerateFrameRegionData(AsepriteFile file, List<SpriteSheetFrameContent> frames)
//     {
//         List<SpriteSheetFrameRegionContent> regions = new();

//         //  Slice keys in Aseprite are defined with a frame index that indicates
//         //  the frame that the key starts on, but doesn't give a value for what
//         //  frame it ends on or may be transformed on.  So we'll interpolate the
//         //  keys to create the Slice elements
//         for (int i = 0; i < file.Slices.Count; i++)
//         {
//             Slice aseSlice = file.Slices[i];
//             string name = aseSlice.Name;

//             //  If no color defined in user data, use Aseprite default for slice
//             //  color, which is just blue
//             Color color = aseSlice.UserData?.Color ?? new Color(0, 0, 255, 255);

//             SliceKey? lastKey = default;

//             for (int k = 0; k < aseSlice.Keys.Count; k++)
//             {
//                 SliceKey key = aseSlice.Keys[k];

//                 SpriteSheetFrameRegionContent region = new(name, key.Bounds, color, key.CenterBounds, key.Pivot);
//                 frames[key.FrameIndex].Regions.Add(name, region);

//                 //  Perform interpolation before caching last key
//                 if (lastKey is not null && lastKey.FrameIndex < key.FrameIndex)
//                 {
//                     for (int offset = 1; offset < key.FrameIndex - lastKey.FrameIndex; offset++)
//                     {
//                         SpriteSheetFrameRegionContent interpolatedRegion = new(name, lastKey.Bounds, color, lastKey.CenterBounds, lastKey.Pivot);
//                         frames[lastKey.FrameIndex + offset].Regions.Add(name, interpolatedRegion);
//                     }
//                 }

//                 lastKey = key;
//             }

//             //  Do we need to interpolate the last key given up to the final
//             //  frame?
//             if (lastKey?.FrameIndex < file.Frames.Count)
//             {
//                 for (int offset = 1; offset < file.Frames.Count - lastKey.FrameIndex; offset++)
//                 {
//                     SpriteSheetFrameRegionContent interpolatedRegion = new(name, lastKey.Bounds, color, lastKey.CenterBounds, lastKey.Pivot);
//                     frames[lastKey.FrameIndex + offset].Regions.Add(name, interpolatedRegion);
//                 }
//             }
//         }
//     }
// }
