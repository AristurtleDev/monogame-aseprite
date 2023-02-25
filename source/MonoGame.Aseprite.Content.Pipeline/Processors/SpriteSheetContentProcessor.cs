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

using System.ComponentModel;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using MonoGame.Aseprite.Content.Pipeline.ContentTypes;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

[ContentProcessor(DisplayName = "Aseprite SpriteSheet Processor - MonoGame.Aseprite")]
internal sealed class SpriteSheetContentProcessor : ContentProcessor<AsepriteFile, SpriteSheetContent>
{
    [DisplayName("Frame Index")]
    public int FrameIndex { get; set; } = 0;

    [DisplayName("Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    [DisplayName("Include Background Layer")]
    public bool IncludeBackgroundLayer { get; set; } = false;

    [DisplayName("Include Tilemap Layers")]
    public bool IncludeTilemapLayers { get; set; } = true;

    [DisplayName("Merge Duplicate Frames")]
    public bool MergeDuplicateFrames { get; set; } = true;

    [DisplayName("Border Padding")]
    public int BorderPadding { get; set; } = 0;

    [DisplayName("Spacing")]
    public int Spacing { get; set; } = 0;

    [DisplayName("Inner Padding")]
    public int InnerPadding { get; set; } = 0;

    [DisplayName("Generate Mipmaps")]
    public bool GenerateMipmaps { get; set; } = false;

    public override SpriteSheetContent Process(AsepriteFile aseFile, ContentProcessorContext context)
    {
        RawSpriteSheet rawSpriteSheet = SpriteSheetProcessor.ProcessRaw(aseFile, OnlyVisibleLayers, IncludeBackgroundLayer, IncludeTilemapLayers, MergeDuplicateFrames, BorderPadding, Spacing, InnerPadding);
        Texture2DContent texture2DContent = ProcessorHelpers.CreateTextureContent(rawSpriteSheet.RawTextureAtlas.RawTexture, rawSpriteSheet.Name);

        if (GenerateMipmaps)
        {
            texture2DContent.GenerateMipmaps(true);
        }

        return new(rawSpriteSheet, texture2DContent);
    }
}
