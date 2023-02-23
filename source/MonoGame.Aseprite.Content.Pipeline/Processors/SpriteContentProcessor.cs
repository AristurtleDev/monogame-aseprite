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
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using MonoGame.Aseprite.Content.Pipeline.ContentTypes;
using MonoGame.Aseprite.Content.Pipeline.Processors.Configuration;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.Content.Pipeline.Importers;

using TInput = MonoGame.Aseprite.AsepriteFile;
using TOutput = MonoGame.Aseprite.Content.Pipeline.ContentProcessorResult<MonoGame.Aseprite.Content.Pipeline.ContentTypes.SpriteContent>;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes the contents of an aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Sprite Processor - MonoGame.Aseprite")]
internal sealed class SpriteContentProcessor : ContentProcessor<ContentImporterResult<AsepriteFile>, ContentProcessorResult<SpriteContent>>
{
    [DisplayName("Frame Index")]
    public int FrameIndex { get; set; } = 0;

    [Browsable(false)]
    public bool SingleFrame { get; set; } = false;

    /// <summary>
    ///     Processes an aseprite file.
    /// </summary>
    /// <param name="content">
    ///     The <see cref="ContentImporterResult"/> from the import process.
    /// </param>
    /// <param name="context">
    ///     The content processor context that provides contextual information about the content being processed.
    /// </param>
    /// <returns>
    ///     A new <see cref="ContentProcessorResult{T}"/> containing the contents of the aseprite file created by this
    ///     method.
    /// </returns>
    public override ContentProcessorResult<SpriteContent> Process(ContentImporterResult<AsepriteFile> content, ContentProcessorContext context)
    {
        if (!SingleFrame)
        {
            ExternalReference<AsepriteFile> externalReference = new(content.Path);
            OpaqueDataDictionary dict = new();
            dict.Add(nameof(FrameIndex), 1);
            dict.Add(nameof(SingleFrame), true);

            context.BuildAsset<AsepriteFile, SpriteContent>
            (
                sourceAsset: externalReference,
                processorName: nameof(SpriteContentProcessor),
                processorParameters: dict,
                importerName: nameof(AsepriteFileContentImporter),
                assetName: content.Content.Frames[1].Name
            );
        }


        SpriteContentProcessorConfiguration? config = SpriteContentProcessorConfiguration.Default;


        RawSprite rawSprite = SpriteProcessor.ProcessRaw(content.Content, FrameIndex, config.OnlyVisibleLayers, config.IncludeBackgroundLayer, config.IncludeTilemapLayers);

        Texture2DContent textureContent = CreateTextureContent(rawSprite.RawTexture, rawSprite.Name);
        textureContent.GenerateMipmaps(false);
        SpriteContent spriteContent = new(rawSprite.Name, textureContent);
        return new(spriteContent);
    }

    private Texture2DContent CreateTextureContent(RawTexture raw, string sourceFileName)
    {
        PixelBitmapContent<Color> face = new(raw.Width, raw.Height);

        for (int i = 0; i < raw.Pixels.Length; i++)
        {
            int x = i % raw.Width;
            int y = i / raw.Width;

            face.SetPixel(x, y, raw.Pixels[i]);
        }

        Texture2DContent textureContent = new();
        textureContent.Identity = new ContentIdentity(sourceFileName);
        textureContent.Faces[0].Add(face);
        return textureContent;
    }
}
