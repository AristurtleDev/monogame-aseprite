/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.Sprites;

namespace MonoGame.Aseprite.Content.Pipeline.Readers;

internal sealed class SpriteSheetContentTypeReader : ContentTypeReader<SpriteSheet>
{
    protected override SpriteSheet Read(ContentReader reader, SpriteSheet? existingInstance)
    {
        if (existingInstance is not null)
        {
            return existingInstance;
        }

        string spriteSheetName = reader.ReadString();
        string textureAtlasName = reader.ReadString();

        Texture2D texture = reader.ReadObject<Texture2D>();
        TextureAtlas textureAtlas = new(textureAtlasName, texture);

        RawTextureRegion[] rawTextureRegions = reader.ReadRawTextureRegions();

        for (int i = 0; i < rawTextureRegions.Length; i++)
        {
            RawTextureRegion rawTextureRegion = rawTextureRegions[i];

            TextureRegion textureRegion = textureAtlas.CreateRegion(rawTextureRegion.Name, rawTextureRegion.Bounds);

            foreach (RawSlice rawSlice in rawTextureRegion.Slices)
            {
                if (rawSlice is RawNinePatchSlice rawNinePatchSlice)
                {
                    textureRegion.CreateNinePatchSlice(rawNinePatchSlice.Name, rawNinePatchSlice.Bounds, rawNinePatchSlice.CenterBounds, rawNinePatchSlice.Origin, rawNinePatchSlice.Color);
                }
                else
                {
                    textureRegion.CreateSlice(rawSlice.Name, rawSlice.Bounds, rawSlice.Origin, rawSlice.Color);
                }
            }
        }

        SpriteSheet spriteSheet = new(spriteSheetName, textureAtlas);
        RawAnimationTag[] tags = reader.ReadRawAnimationTags();
        for (int i = 0; i < tags.Length; i++)
        {
            RawAnimationTag tag = tags[i];

            spriteSheet.CreateAnimationTag(tag.Name, builder =>
            {
                builder.IsLooping(tag.IsLooping)
                       .IsReversed(tag.IsReversed)
                       .IsPingPong(tag.IsPingPong);

                for (int j = 0; j < tag.RawAnimationFrames.Length; j++)
                {
                    RawAnimationFrame rawAnimationFrame = tag.RawAnimationFrames[j];
                    TimeSpan duration = TimeSpan.FromMilliseconds(rawAnimationFrame.DurationInMilliseconds);
                    builder.AddFrame(rawAnimationFrame.FrameIndex, duration);
                }
            });
        }

        return spriteSheet;
    }
}
