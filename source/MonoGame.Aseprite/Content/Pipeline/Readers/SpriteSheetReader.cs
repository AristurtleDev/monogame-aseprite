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

public sealed class SpriteSheetReader : ContentTypeReader<SpriteSheet>
{
    protected override SpriteSheet Read(ContentReader input, SpriteSheet? existingInstance)
    {
        if (existingInstance is not null)
        {
            return existingInstance;
        }

        string spriteSheetName = input.ReadString();
        int textureWidth = input.ReadInt32();
        int textureHeight = input.ReadInt32();

        int pixelCount = input.ReadInt32();
        Color[] pixels = new Color[textureWidth * textureHeight];

        if (pixelCount != pixels.Length)
        {
            throw new InvalidOperationException($"Pixel count does not match expected number of pixels to read");
        }

        for (int i = 0; i < pixelCount; i++)
        {
            pixels[i] = input.ReadColor();
        }

        Texture2D texture = new(input.GetGraphicsDevice(), textureWidth, textureHeight, false, SurfaceFormat.Color);
        texture.SetData<Color>(pixels);

        SpriteSheet spriteSheet = new(spriteSheetName, texture);

        int frameCount = input.ReadInt32();

        for (int i = 0; i < frameCount; i++)
        {
            string frameName = input.ReadString();
            int frameX = input.ReadInt32();
            int frameY = input.ReadInt32();
            int frameWidth = input.ReadInt32();
            int frameHeight = input.ReadInt32();

            Rectangle frameBounds = new(frameX, frameY, frameWidth, frameHeight);

            long frameDurationInTicks = input.ReadInt64();

            TimeSpan duration = TimeSpan.FromTicks(frameDurationInTicks);

            SpriteSheetFrame frame = spriteSheet.CreateFrame(frameName, frameBounds, duration);

            int regionCount = input.ReadInt32();

            for (int j = 0; j < regionCount; j++)
            {
                string regionName = input.ReadString();
                Color regionColor = input.ReadColor();

                Rectangle regionBounds = new
                (
                    x: input.ReadInt32(),
                    y: input.ReadInt32(),
                    width: input.ReadInt32(),
                    height: input.ReadInt32()
                );

                Rectangle? regionCenter = default;

                bool isNine = input.ReadBoolean();
                if (isNine)
                {
                    regionCenter = new
                    (
                        x: input.ReadInt32(),
                        y: input.ReadInt32(),
                        width: input.ReadInt32(),
                        height: input.ReadInt32()
                    );
                }

                Point? regionPivot = default;

                bool hasPivot = input.ReadBoolean();
                if (hasPivot)
                {
                    regionPivot = new
                    (
                        x: input.ReadInt32(),
                        y: input.ReadInt32()
                    );
                }

                frame.AddRegion(regionName, regionBounds, regionColor, regionCenter, regionPivot);
            }
        }

        int animationCount = input.ReadInt32();

        for (int i = 0; i < animationCount; i++)
        {
            string animationName = input.ReadString();

            int animationFrameIndexCount = input.ReadInt32();

            int[] animationFrameIndexes = new int[animationFrameIndexCount];

            for (int j = 0; j < animationFrameIndexCount; j++)
            {
                animationFrameIndexes[j] = input.ReadInt32();
            }

            bool isLooping = input.ReadBoolean();
            bool isReversed = input.ReadBoolean();
            bool isPingPong = input.ReadBoolean();

            spriteSheet.AddAnimationDefinition(animationName, isLooping, isReversed, isPingPong, animationFrameIndexes);
        }


        return spriteSheet;
    }
}
