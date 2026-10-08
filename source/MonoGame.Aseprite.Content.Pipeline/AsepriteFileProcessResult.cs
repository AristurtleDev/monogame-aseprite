// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Aseprite.Content.Pipeline;

/// <summary>
/// Represents the result of processing an aseprite file after importing.
/// </summary>
/// <param name="Name">The name of the Aseprite file.</param>
/// <param name="PremultiplyAlpha"><see langword="true"/> if alpha was premultiplied; otherwise, <see langword="false"/>.</param>
/// <param name="Data">The data result from processing.</param>
public record AsepriteFileProcessResult(string Name, bool PremultiplyAlpha, byte[] Data);