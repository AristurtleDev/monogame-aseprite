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

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes the contents of an aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite File Processor - MonoGame.Aseprite")]
internal sealed class SpriteContentProcessor : ContentProcessor<AsepriteFile, ContentProcessorResult<SpriteContent>>
{
    [DisplayName("Configuration Path")]
    public string? ConfigurationPath { get; set; } = default;

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
    public override ContentProcessorResult<SpriteContent> Process(AsepriteFile aseFile, ContentProcessorContext context)
    {
        SpriteContentProcessorConfiguration? config = default;

        if (!string.IsNullOrEmpty(ConfigurationPath))
        {
            string json = File.ReadAllText(ConfigurationPath);
            config = JsonSerializer.Deserialize<SpriteContentProcessorConfiguration>(json);

            if (config is null)
            {
                throw new InvalidContentException($"The configuration file at that path '{ConfigurationPath}' could not be deserialized. Please ensure it is in the correct format");
            }
        }

        if (config is null)
        {
            config = SpriteContentProcessorConfiguration.Default;
        }

        RawSprite rawSprite = SpriteProcessor.ProcessRaw(aseFile, 0, config.OnlyVisibleLayers, config.IncludeBackgroundLayer, config.IncludeTilemapLayers);
        
        Texture2DContent textureContent = CreateTextureContent(rawSprite.RawTexture, rawSprite.Name);
        SpriteContent spriteContent = new(rawSprite.Name, textureContent);
        return new(spriteContent);
    }

    private Texture2DContent CreateTextureContent(RawTexture raw, string sourceFileName)
    {
        PixelBitmapContent<Color> face = new(raw.Width, raw.Height);

        for (int i = 0; i < raw.Pixels.Length; i++)
        {
            int x = i / raw.Width;
            int y = i % raw.Width;
            face.SetPixel(x, y, raw.Pixels[i]);
        }

        Texture2DContent textureContent = new();
        textureContent.Identity = new ContentIdentity(sourceFileName);
        textureContent.Faces[0].Add(face);
        return textureContent;
    }
}
