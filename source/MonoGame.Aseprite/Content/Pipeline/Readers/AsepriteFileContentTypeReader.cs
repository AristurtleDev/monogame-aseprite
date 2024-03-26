// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework.Content;

namespace MonoGame.Aseprite.Content.Pipeline.Readers;

internal sealed class AsepriteFileContentTypeReader : ContentTypeReader<AsepriteFile>
{
    protected override AsepriteFile Read(ContentReader reader, AsepriteFile? existingInstance)
    {

        if (existingInstance is not null)
        {
            return existingInstance;
        }

        string name = reader.ReadString();
        bool premultiplyAlpha = reader.ReadBoolean();
        int len = reader.ReadInt32();
        byte[] data = reader.ReadBytes(len);

        using MemoryStream stream = new(data);
        return AsepriteFileLoader.FromStream(name, stream, premultiplyAlpha);
    }
}
