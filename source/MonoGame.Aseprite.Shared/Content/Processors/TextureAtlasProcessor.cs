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
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Processors;

/// <inheritdoc/>
public static partial class TextureAtlasProcessor
{
    /// <summary>
    /// Processes a <see cref="RawTextureAtlas"/> from the given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> to process the <see cref="RawTextureAtlas"/> from.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates if only <see cref="AsepriteCel"/> elements on visible <see cref="AsepriteLayer"/> elements should 
    ///     be included.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates if <see cref="AsepriteCel"/> elements on the <see cref="AsepriteLayer"/> marked as the background 
    ///     layer should be included.
    /// </param>
    /// <param name="includeTilemapLayers">
    ///     Indicates if <see cref="AsepriteCel"/> elements on a <see cref="AsepriteTilemapLayer"/> should be included.
    /// </param>
    /// <param name="mergeDuplicates">
    ///     Indicates if duplicate <see cref="AsepriteFrame"/> elements should be merged into one.
    /// </param>
    /// <param name="borderPadding">
    ///     The amount of transparent pixels to add between the edge of the generated image
    /// </param>
    /// <param name="spacing">
    ///     The amount of transparent pixels to add between each texture region in the generated image.
    /// </param>
    /// <param name="innerPadding">
    ///     The amount of transparent pixels to add around the edge of each texture region in the generated image.
    /// </param>
    /// <returns>The <see cref="RawTextureAtlas"/> created by this method.</returns>
    public static RawTextureAtlas ProcessRaw(AsepriteFile aseFile, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true, bool mergeDuplicates = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
    {
        int frameWidth = aseFile.CanvasWidth;
        int frameHeight = aseFile.CanvasHeight;
        int frameCount = aseFile.Frames.Length;

        Color[][] flattenedFrames = new Color[frameCount][];

        for (int i = 0; i < frameCount; i++)
        {
            flattenedFrames[i] = aseFile.Frames[i].FlattenFrame(onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);
        }

        Dictionary<int, int> duplicateMap = new();
        Dictionary<int, RawTextureRegion> originalToDuplicateLookup = new();

        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            for (int d = 0; d < i; d++)
            {
                if (flattenedFrames[i].SequenceEqual(flattenedFrames[d]))
                {
                    duplicateMap.Add(i, d);
                    break;
                }
            }
        }

        if (mergeDuplicates)
        {
            frameCount -= duplicateMap.Count;
        }

        double sqrt = Math.Sqrt(frameCount);
        int columns = (int)Math.Ceiling(sqrt);
        int rows = (frameCount + columns - 1) / columns;

        int imageWidth = (columns * frameWidth)
                         + (borderPadding * 2)
                         + (spacing * (columns - 1))
                         + (innerPadding * 2 * columns);

        int imageHeight = (rows * frameHeight)
                          + (borderPadding * 2)
                          + (spacing * (rows - 1))
                          + (innerPadding * 2 * rows);

        Color[] imagePixels = new Color[imageWidth * imageHeight];
        RawTextureRegion[] regions = new RawTextureRegion[aseFile.Frames.Length];

        int offset = 0;

        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            if (mergeDuplicates && duplicateMap.ContainsKey(i))
            {
                RawTextureRegion original = originalToDuplicateLookup[duplicateMap[i]];
                RawTextureRegion duplicate = new($"{aseFile.Name} {i}", original.Bounds, GetSlicesForFrame(i, aseFile.Slices));
                regions[i] = duplicate;
                offset++;
                continue;
            }

            int column = (i - offset) % columns;
            int row = (i - offset) / columns;
            Color[] frame = flattenedFrames[i];

            int x = (column * frameWidth)
                    + borderPadding
                    + (spacing * column)
                    + (innerPadding * (column + column + 1));

            int y = (row * frameHeight)
                     + borderPadding
                     + (spacing * row)
                     + (innerPadding * (row + row + 1));

            for (int p = 0; p < frame.Length; p++)
            {
                int px = (p % frameWidth) + x;
                int py = (p / frameWidth) + y;

                int index = py * imageWidth + px;
                imagePixels[index] = frame[p];

            }

            Rectangle bounds = new(x, y, frameWidth, frameHeight);
            RawTextureRegion rawTextureRegion = new($"{aseFile.Name} {i}", bounds, GetSlicesForFrame(i, aseFile.Slices));
            regions[i] = rawTextureRegion;
            originalToDuplicateLookup.Add(i, rawTextureRegion);
        }

        RawTexture rawTexture = new(aseFile.Name, imagePixels, imageWidth, imageHeight);
        return new(aseFile.Name, rawTexture, regions);
    }

    private static RawSlice[] GetSlicesForFrame(int frameIndex, ReadOnlySpan<AsepriteSlice> slices)
    {
        List<RawSlice> result = new();
        HashSet<string> sliceNameCheck = new();

        for (int s = 0; s < slices.Length; s++)
        {
            AsepriteSlice slice = slices[s];
            ReadOnlySpan<AsepriteSliceKey> keys = slice.Keys;

            //  Traverse keys backwards until we find a match for the frame index
            for (int k = keys.Length - 1; k >= 0; k--)
            {
                AsepriteSliceKey key = keys[k];

                if (key.FrameIndex > frameIndex)
                {
                    continue;
                }

                string name = slice.Name;

                if (sliceNameCheck.Contains(name))
                {
                    throw new InvalidOperationException($"Duplicate slice name '{name}' found. Slices must have unique names");
                }

                Rectangle bounds = key.Bounds;
                Color color = slice.UserData.Color.GetValueOrDefault();
                Vector2 origin = key.Pivot.GetValueOrDefault().ToVector2();

                RawSlice rawSlice;

                if (key.IsNinePatch)
                {
                    rawSlice = new RawNinePatchSlice(name, bounds, key.CenterBounds.GetValueOrDefault(), origin, color);
                }
                else
                {
                    rawSlice = new RawSlice(name, bounds, origin, color);
                }

                result.Add(rawSlice);
                sliceNameCheck.Add(name);
                break;
            }
        }

        return result.ToArray();
    }
}
