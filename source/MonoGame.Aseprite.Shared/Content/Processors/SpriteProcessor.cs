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
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Processors;

/// <inheritdoc/>
public static partial class SpriteProcessor
{
    /// <summary>
    ///     Processes a <see cref="RawSprite"/> from the <see cref="AsepriteFrame"/> at the specified index in the 
    ///     given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> that contains the <see cref="AsepriteFrame"/> to processes.
    /// </param>
    /// <param name="aseFrameIndex">
    ///     The index of the <see cref="AsepriteFrame"/> in the <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates if only <see cref="AsepriteCel"/> elements on visible <see cref="AsepriteLayer"/> elements should 
    ///     be included.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates if <see cref="AsepriteCel"/> elements on an <see cref="AsepriteLayer"/> marked as the background 
    ///     layer should be included.
    /// </param>
    /// <param name="includeTilemapLayers">
    ///     Indicates if <see cref="AsepriteCel"/> elements on a <see cref="AsepriteTilemapLayer"/> should be included.
    /// </param>
    /// <returns>
    ///     The <see cref="RawSprite"/> created by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified  <see cref="AsepriteFrame"/> index is less than zero or is greater than or equal to 
    ///     the total number of  <see cref="AsepriteFrame"/> elements in the given  <see cref="AsepriteFile"/>.
    /// </exception>
    public static RawSprite ProcessRaw(AsepriteFile aseFile, int aseFrameIndex, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true)
    {
        AsepriteFrame aseFrame = aseFile.GetFrame(aseFrameIndex);

        Color[] pixels = aseFrame.FlattenFrame(onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);
        RawTexture rawTexture = new(aseFrame.Name, pixels, aseFrame.Width, aseFrame.Height);

        RawSlice[] slices = ProcessSlices(aseFrameIndex, aseFile.Slices);

        return new(aseFrame.Name, rawTexture, slices);
    }

    private static RawSlice[] ProcessSlices(int frameIndex, ReadOnlySpan<AsepriteSlice> slices)
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
