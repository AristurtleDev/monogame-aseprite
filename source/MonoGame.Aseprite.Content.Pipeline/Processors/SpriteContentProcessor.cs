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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.ContentTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

[ContentProcessor(DisplayName = "Aseprite Sprite Processor - MonoGame.Aseprite")]
internal sealed class SpriteContentProcessor : ContentProcessor<AsepriteFileImportResult, SpriteContent>
{
    [DisplayName("Frame Index")]
    public int FrameIndex { get; set; } = 0;

    [DisplayName("Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    [DisplayName("Include Background Layer")]
    public bool IncludeBackgroundLayer { get; set; } = false;

    [DisplayName("Include Tilemap Layers")]
    public bool IncludeTilemapLayers { get; set; } = true;

    [DisplayName("Generate Mipmaps")]
    public bool GenerateMipmaps { get; set; } = false;

    public override SpriteContent Process(AsepriteFileImportResult content, ContentProcessorContext context)
    {
        if (FrameIndex < 0 || FrameIndex >= content.AsepriteFile.FrameCount)
        {
            throw new ProcessorParameterException($"The 'Frame Index' parameter cannot be less than zero or greater than or equal to the total number of frames in the Aseprite file", nameof(SpriteContentProcessor), nameof(FrameIndex));
        }

        AsepriteFrame aseFrame = content.AsepriteFile.Frames[FrameIndex];
        Color[] pixels = aseFrame.FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer, IncludeTilemapLayers);
        Texture2DContent texture2DContent = ProcessorHelpers.CreateTexture2DContent(pixels, aseFrame.Width, aseFrame.Height);

        if (GenerateMipmaps)
        {
            texture2DContent.GenerateMipmaps(true);
        }

        return new(aseFrame.Name, texture2DContent);
    }
}
