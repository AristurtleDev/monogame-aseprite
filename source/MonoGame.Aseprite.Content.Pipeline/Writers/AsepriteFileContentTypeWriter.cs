// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

/// <summary>
/// Writes the contents of the <see cref="AsepriteFileProcessResult"/> to an XNB file.
/// </summary>
[ContentTypeWriter]
public sealed class AsepriteFileContentTypeWriter : ContentTypeWriter<AsepriteFileProcessResult>
{
        /// <inheritdoc/>
        /// <param name="output">The content writer serializing the value.</param>
        /// <param name="content">The content from the processor to write.</param>
        protected override void Write(ContentWriter output, AsepriteFileProcessResult content)
        {
                if (output == null)
                        throw new ArgumentNullException(nameof(output), "The content writer cannot be null.");

                if (content == null)
                        throw new ArgumentNullException(nameof(content), "The content to write cannot be null");

                output.Write(content.Name);
                output.Write(content.PremultiplyAlpha);
                output.Write(content.Data.Length);
                output.Write(content.Data);
        }

        /// <inheritdoc/>
        public override string GetRuntimeType(TargetPlatform targetPlatform) =>
#if KNI
            "MonoGame.Aseprite.AsepriteFile, KNI.Aseprite";
#else
        "MonoGame.Aseprite.AsepriteFile, MonoGame.Aseprite";
#endif

        /// <inheritdoc/>
        public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
#if KNI
            "MonoGame.Aseprite.Content.Pipeline.Readers.AsepriteFileContentTypeReader, KNI.Aseprite";
#else
        "MonoGame.Aseprite.Content.Pipeline.Readers.AsepriteFileContentTypeReader, MonoGame.Aseprite";
#endif
}
