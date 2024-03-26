// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

[ContentProcessor(DisplayName = "Aseprite File Processor - MonoGame.Aseprite")]
internal sealed class AsepriteFileContentProcessor : ContentProcessor<AsepriteFileImportResult, AsepriteFileProcessResult>
{
    [DisplayName("Premultiply Alpha")]
    public bool PremultiplyAlpha { get; set; } = true;

    public override AsepriteFileProcessResult Process(AsepriteFileImportResult content, ContentProcessorContext context)
    {
        string name = Path.GetFileNameWithoutExtension(content.FilePath);
        byte[] data = File.ReadAllBytes(content.FilePath);
        AsepriteFileProcessResult result = new AsepriteFileProcessResult(name, PremultiplyAlpha, data);
        return result;
    }
}
