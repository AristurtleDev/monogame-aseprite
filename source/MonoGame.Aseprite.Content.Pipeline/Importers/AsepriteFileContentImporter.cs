// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.Content.Pipeline.Processors;


namespace MonoGame.Aseprite.Content.Pipeline.Importers;

/// <summary>
/// Imports the contents of an Aseprite file for processing.
/// </summary>
[ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite File Importer - MonoGame.Aseprite", DefaultProcessor = nameof(AsepriteFileContentProcessor))]
public class AsepriteFileContentImporter : ContentImporter<AsepriteFileImportResult>
{
    /// <inheritdoc/>
    /// <param name="filename">The path to the Aseprite file to import</param>
    /// <param name="context">Contains information for importing a game asset, such as a logger interface</param>
    public override AsepriteFileImportResult Import(string filename, ContentImporterContext context)
    {
        return new AsepriteFileImportResult(filename);
    }
}
