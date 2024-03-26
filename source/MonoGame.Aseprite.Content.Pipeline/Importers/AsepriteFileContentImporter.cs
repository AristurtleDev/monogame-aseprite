// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.Content.Pipeline.Processors;


namespace MonoGame.Aseprite.Content.Pipeline.Importers;

[ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite File Importer - MonoGame.Aseprite", DefaultProcessor = nameof(AsepriteFileContentProcessor))]
internal class AsepriteFileContentImporter : ContentImporter<AsepriteFileImportResult>
{
    public override AsepriteFileImportResult Import(string path, ContentImporterContext context)
    {
        return new AsepriteFileImportResult(path);
    }
}
