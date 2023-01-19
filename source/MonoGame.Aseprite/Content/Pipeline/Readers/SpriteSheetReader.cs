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
    protected override SpriteSheet Read(ContentReader reader, SpriteSheet? existingInstance)
    {
        if (existingInstance is not null)
        {
            return existingInstance;
        }

        string name = reader.ReadString();
        Texture2D texture = reader.ReadTexture2D(existingInstance: null);

        SpriteSheet spriteSheet = new(name, texture);

        int regionCount = reader.ReadInt32();
        for (int i = 0; i < regionCount; i++)
        {
            Rectangle bounds = reader.ReadRectangle();
            spriteSheet.CreateRegion($"{name} {i}", bounds);
        }

        int cycleCount = reader.ReadInt32();
        for (int i = 0; i < cycleCount; i++)
        {
            string cycleName = reader.ReadString();
            AnimationCycleBuilder builder = new(cycleName, spriteSheet);

            int frameCount = reader.ReadInt32();

            for (int j = 0; j < frameCount; j++)
            {
                int index = reader.ReadInt32();
                int ms = reader.ReadInt32();
                builder.AddFrame(index, TimeSpan.FromMilliseconds(ms));
            }

            bool isLooping = reader.ReadBoolean();
            bool isReversed = reader.ReadBoolean();
            bool isPingPong = reader.ReadBoolean();

            builder.IsLooping(isLooping)
                   .IsReversed(isReversed)
                   .IsPingPong(isPingPong);

            spriteSheet.AddAnimationCycle(builder.Build());
        }

        return spriteSheet;
    }
}
