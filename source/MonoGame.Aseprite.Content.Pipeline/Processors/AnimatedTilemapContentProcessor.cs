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

[ContentProcessor(DisplayName = "Aseprite Animated Tilemap Processor - MonoGame.Aseprite")]
internal sealed class AnimatedTilemapContentProcessor : ContentProcessor<AsepriteFile, AnimatedTilemapContent>
{
    [DisplayName("Only Visible Layers")]
    public bool OnlyVisibleLayer { get; set; } = true;

    [DisplayName("Generate Mipmaps")]
    public bool GenerateMipmaps { get; set; } = false;

    public override AnimatedTilemapContent Process(AsepriteFile aseFile, ContentProcessorContext context)
    {
        RawAnimatedTilemap rawAnimatedTilemap = AnimatedTilemapProcessor.ProcessRaw(aseFile, OnlyVisibleLayer);

        Texture2DContent[] texture2DContents = ProcessTilesetTexture(rawAnimatedTilemap.RawTilesets);
        return new(rawAnimatedTilemap, texture2DContents);
    }

    private Texture2DContent[] ProcessTilesetTexture(ReadOnlySpan<RawTileset> rawTilesets)
    {
        Texture2DContent[] texture2DContents = new Texture2DContent[rawTilesets.Length];

        for (int i = 0; i < rawTilesets.Length; i++)
        {
            Texture2DContent texture2DContent = ProcessorHelpers.CreateTextureContent(rawTilesets[i].RawTexture, rawTilesets[i].Name);
            if (GenerateMipmaps)
            {
                texture2DContent.GenerateMipmaps(true);
            }
            texture2DContents[i] = texture2DContent;
        }

        return texture2DContents;
    }
}
