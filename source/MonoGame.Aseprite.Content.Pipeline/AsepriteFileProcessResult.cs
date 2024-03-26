// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Aseprite.Content.Pipeline;

internal record AsepriteFileProcessResult(string Name, bool PremultiplyAlpha, byte[] Data);