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

using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
/// Defines a processor that processes a raw spritesheet from an aseprite file.
/// </summary>
public static class RawSpriteSheetProcessor
{
    /// <summary>
    /// Processes a raw spritesheet from the given aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file to process the raw spritesheet from.</param>
    /// <param name="onlyVisibleLayers">Indicates if only cels on visible layers should be included.</param>
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
    /// <returns>The raw sprite sheet created by this method.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if tags are found in the aseprite file with duplicate names.  Spritesheets must contain tags with unique
    /// names even though aseprite does not enforce unique names for tags.
    /// </exception>
    public static SpriteSheetContent Process(AsepriteFile aseFile, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true, bool mergeDuplicates = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
    {
        TextureAtlasContent rawAtlas = RawTextureAtlasProcessor.Process(aseFile, onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers, mergeDuplicates, borderPadding, spacing, innerPadding);

        AnimationTagContent[] rawTags = new AnimationTagContent[aseFile.Tags.Length];
        HashSet<string> tagNameCheck = new();

        for (int i = 0; i < aseFile.Tags.Length; i++)
        {
            AsepriteTag aseTag = aseFile.GetTag(i);

            if (!tagNameCheck.Add(aseTag.Name))
            {
                throw new InvalidOperationException($"Duplicate tag name '{aseTag.Name}' found.  Tags must have unique names for a spritesheet.");
            }

            rawTags[i] = RawAnimationTagProcessor.Process(aseTag, aseFile.Frames);
        }

        return new(aseFile.Name, rawAtlas, rawTags);

    }
}

