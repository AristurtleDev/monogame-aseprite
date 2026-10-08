// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
/// Processes the <see cref="AsepriteFileImportResult"/>
/// </summary>
[ContentProcessor(DisplayName = "Aseprite File Processor - MonoGame.Aseprite")]
public sealed class AsepriteFileContentProcessor : ContentProcessor<AsepriteFileImportResult, AsepriteFileProcessResult>
{
    /// <summary>
    /// Gets or sets whether alpha is premultiplied when processing.
    /// </summary>
    [DisplayName("Premultiply Alpha")]
    public bool PremultiplyAlpha { get; set; } = true;

    //Processes the specified input data and returns the result.

    /// <inheritdoc/>
    /// <param name="content">The result from the importer.</param>
    /// <param name="context">Contains any required custom process parameters.</param>
    public override AsepriteFileProcessResult Process(AsepriteFileImportResult content, ContentProcessorContext context)
    {
        if (content == null)
            throw new InvalidContentException("The content to process is null");

        string name = Path.GetFileNameWithoutExtension(content.FilePath);
        byte[] data = File.ReadAllBytes(content.FilePath);
        AsepriteFileProcessResult result = new AsepriteFileProcessResult(name, PremultiplyAlpha, data);
        return result;
    }
}
