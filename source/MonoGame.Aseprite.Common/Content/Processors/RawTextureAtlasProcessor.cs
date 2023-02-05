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
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
/// Defines a processor that processes a raw texture atlas from an aseprite file.
/// </summary>
public static class RawTextureAtlasProcessor
{
    /// <summary>
    /// Processes a raw texture atlas from the given aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file to process the raw texture atlas from.</param>
    /// <param name="onlyVisibleLayers">Indicates if only cels on visible layers should be included. </param>
    /// <param name="includeBackgroundLayer">
    /// Indicates if cels on the layer marked as the background layer should be included.
    /// </param>
    /// <param name="includeTilemapLayers">Indicates if cels on a tilemap layer should be included.</param>
    /// <param name="mergeDuplicates">Indicates if duplicate frames should be merged into one.</param>
    /// <param name="borderPadding">
    /// The amount of transparent pixels to add between the edge of the generated image
    /// </param>
    /// <param name="spacing">
    /// The amount of transparent pixels to add between each texture region in the generated image.
    /// </param>
    /// <param name="innerPadding">
    /// The amount of transparent pixels to add around the edge of each texture region in the generated image.
    /// </param>
    /// <returns>The raw texture atlas created by this method.</returns>
    public static TextureAtlasContent Process(AsepriteFile aseFile, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true, bool mergeDuplicates = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
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
        Dictionary<int, TextureRegionContent> originalToDuplicateLookup = new();

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
        TextureRegionContent[] regions = new TextureRegionContent[aseFile.Frames.Length];

        int offset = 0;

        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            if (mergeDuplicates && duplicateMap.ContainsKey(i))
            {
                TextureRegionContent original = originalToDuplicateLookup[duplicateMap[i]];
                TextureRegionContent duplicate = new($"{aseFile.Name} {i}", original.Bounds);
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

            int y =  (row * frameHeight)
                     + borderPadding
                     + (spacing * row)
                     + (innerPadding * (row + row + 1));

            for (int p = 0; p < frame.Length; p++)
            {
                int px = (p % frameWidth) + x;
                        // + (column * frameWidth)
                        // + borderPadding
                        // + (spacing * column)
                        // + (innerPadding * (column + column + 1));

                int py = (p / frameWidth) + y;
                        // + (row * frameHeight)
                        // + borderPadding
                        // + (spacing * row)
                        // + (innerPadding * (row + row + 1));

                int index = py * imageWidth + px;
                imagePixels[index] = frame[p];

            }

            Rectangle bounds = new(x, y, frameWidth, frameHeight);
            TextureRegionContent rawTextureRegion = new($"{aseFile.Name} {i}", bounds);
            regions[i] = rawTextureRegion;
            originalToDuplicateLookup.Add(i, rawTextureRegion);
        }

        TextureContent rawTexture = new(aseFile.Name, imagePixels, imageWidth, imageHeight);
        return new(aseFile.Name, rawTexture, regions);
    }
}
