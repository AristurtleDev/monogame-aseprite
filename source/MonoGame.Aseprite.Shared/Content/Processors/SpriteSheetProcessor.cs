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

using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Processors;

/// <inheritdoc/>
public static partial class SpriteSheetProcessor
{
    /// <summary>
    ///     Processes a <see cref="RawSpriteSheet"/> from the given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> to process the <see cref="RawSpriteSheet"/> from.
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
    /// <returns>
    ///     The <see cref="RawSpriteSheet"/> created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if <see cref="AsepriteTag"/> elements are found in the <see cref="AsepriteFile"/> with duplicate 
    ///     names.  Spritesheets must contain <see cref="AsepriteTag"/> elements with unique names even though aseprite
    ///     does not enforce unique names for <see cref="AsepriteTag"/> elements.
    /// </exception>
    public static RawSpriteSheet ProcessRaw(AsepriteFile aseFile, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true, bool mergeDuplicates = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
    {
        RawTextureAtlas rawAtlas = TextureAtlasProcessor.ProcessRaw(aseFile, onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers, mergeDuplicates, borderPadding, spacing, innerPadding);

        RawAnimationTag[] rawTags = new RawAnimationTag[aseFile.Tags.Length];
        HashSet<string> tagNameCheck = new();

        for (int i = 0; i < aseFile.Tags.Length; i++)
        {
            AsepriteTag aseTag = aseFile.GetTag(i);

            if (!tagNameCheck.Add(aseTag.Name))
            {
                throw new InvalidOperationException($"Duplicate tag name '{aseTag.Name}' found.  Tags must have unique names for a spritesheet.");
            }

            rawTags[i] = ProcessRawTag(aseTag, aseFile.Frames);
        }

        return new(aseFile.Name, rawAtlas, rawTags);

    }

    private static RawAnimationTag ProcessRawTag(AsepriteTag aseTag, ReadOnlySpan<AsepriteFrame> aseFrames)
    {
        int frameCount = aseTag.To - aseTag.From + 1;
        RawAnimationFrame[] rawAnimationFrames = new RawAnimationFrame[frameCount];
        int[] frames = new int[frameCount];
        int[] durations = new int[frameCount];

        for (int i = 0; i < frameCount; i++)
        {
            int index = aseTag.From + i;
            rawAnimationFrames[i] = new(index, aseFrames[index].DurationInMilliseconds);
        }

        // In aseprite, all tags are looping
        int loopCount = aseTag.Repeat;
        bool isReversed = aseTag.Direction == AsepriteLoopDirection.Reverse || aseTag.Direction == AsepriteLoopDirection.PingPongReverse;
        bool isPingPong = aseTag.Direction == AsepriteLoopDirection.PingPong || aseTag.Direction == AsepriteLoopDirection.PingPongReverse;

        return new(aseTag.Name, rawAnimationFrames, loopCount, isReversed, isPingPong);
    }
}

