// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

[ContentTypeWriter]
internal sealed class AsepriteFileContentTypeWriter : ContentTypeWriter<AsepriteFileProcessResult>
{
    protected override void Write(ContentWriter writer, AsepriteFileProcessResult content)
    {
        writer.Write(content.Name);
        writer.Write(content.PremultiplyAlpha);
        writer.Write(content.Data.Length);
        writer.Write(content.Data);
    }

    public override string GetRuntimeType(TargetPlatform targetPlatform) =>
        "MonoGame.Aseprite.AsepriteFile, MonoGame.Aseprite";

    public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
        "MonoGame.Aseprite.Content.Pipeline.Readers.AsepriteFileContentTypeReader, MonoGame.Aseprite";
}
