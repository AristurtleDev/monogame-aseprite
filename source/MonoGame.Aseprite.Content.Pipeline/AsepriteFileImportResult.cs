// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Aseprite.Content.Pipeline;

/// <summary>
/// The result from the aseprite file content importer.
/// </summary>
/// <param name="FilePath">The file path to the aseprite file.</param>
public record AsepriteFileImportResult(string FilePath);