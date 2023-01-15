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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Content.Pipeline.Readers;

/// <summary>
///     Provides method for reading a <see cref="SpriteSheet"/> from an xnb file
///     that was generated using the MonoGame.Aseprite library.
/// </summary>
public sealed class SpriteSheetReader : ContentTypeReader<SpriteSheet>
{
    protected override SpriteSheet Read(ContentReader input, SpriteSheet? existingInstance)
    {
        if (existingInstance is not null)
        {
            return existingInstance;
        }

        string name = input.ReadString();

        //  Texture Content
        int textureWidth = input.ReadInt32();
        int textureHeight = input.ReadInt32();
        int pixelCount = input.ReadInt32();
        Color[] pixels = new Color[pixelCount];
        for (int i = 0; i < pixelCount; i++)
        {
            pixels[i] = input.ReadColor();
        }

        //  Create texture
        Texture2D texture = new(input.GetGraphicsDevice(), textureWidth, textureHeight, false, SurfaceFormat.Color);
        texture.SetData<Color>(pixels);

        SpriteSheet spriteSheet = new(name, texture);

        //  Texture Region Content
        int regionCount = input.ReadInt32();
        for (int i = 0; i < regionCount; i++)
        {
            string regionName = input.ReadString();
            int x = input.ReadInt32();
            int y = input.ReadInt32();
            int w = input.ReadInt32();
            int h = input.ReadInt32();

            _ = spriteSheet.CreateRegion(regionName, x, y, w, h);
        }

        //  Animation Content
        int animationCount = input.ReadInt32();
        for (int i = 0; i < animationCount; i++)
        {
            string animationName = input.ReadString();
            byte flags = input.ReadByte();

            bool isLooping = (flags & 1) != 0;
            bool isReversed = (flags & 2) != 0;
            bool isPingPong = (flags & 4) != 0;

            int frameCount = input.ReadInt32();

            AnimationCycleBuilder builder = new(animationName, spriteSheet);
            builder.IsLooping(isLooping)
                   .IsReversed(isReversed)
                   .IsPingPong(isPingPong);

            for (int j = 0; j < frameCount; j++)
            {
                int index = input.ReadInt32();
                long ticks = input.ReadInt64();
                TimeSpan duration = TimeSpan.FromTicks(ticks);

                builder.AddFrame(index, duration);
            }

            AnimationCycle cycle = builder.Build();
            spriteSheet.AddAnimationCycle(cycle);
        }

        return spriteSheet;
    }
}
