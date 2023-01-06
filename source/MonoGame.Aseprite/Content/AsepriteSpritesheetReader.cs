/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

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

namespace MonoGame.Aseprite.Content;

public sealed class SpriteSheetReader : ContentTypeReader<SpriteSheet>
{
    protected override SpriteSheet Read(ContentReader input, SpriteSheet? existingInstance)
    {
        if (existingInstance is not null)
        {
            return existingInstance;
        }

        ReadInput(input, out string name, out int width, out int height, out Color[] pixels, out Frame[] frames, out Tag[] tags, out Slice[] slices);
        Texture2D texture = CreateTexture(input.GetGraphicsDevice(), pixels, width, height);
        SpriteSheet spriteSheet = CreateSpriteSheet(name, texture, frames, tags, slices);
        return spriteSheet;
    }

    private void ReadInput(ContentReader input, out string name, out int width, out int height, out Color[] pixels, out Frame[] frames, out Tag[] tags, out Slice[] slices)
    {
        name = input.ReadString();
        width = input.ReadInt32();
        height = input.ReadInt32();

        int nPixels = input.ReadInt32();
        pixels = new Color[nPixels];
        for (int i = 0; i < nPixels; i++)
        {
            pixels[i] = input.ReadColor();
        }

        int nFrames = input.ReadInt32();
        frames = new Frame[nFrames];
        for (int i = 0; i < nFrames; i++)
        {
            int x = input.ReadInt32();
            int y = input.ReadInt32();
            int w = input.ReadInt32();
            int h = input.ReadInt32();
            double ms = input.ReadDouble();

            Rectangle bounds = new(x, y, w, h);

            TimeSpan duration = TimeSpan.FromMilliseconds(ms);

            Frame frame = new(bounds, duration);
            frames[i] = frame;
        }

        int nTags = input.ReadInt32();
        tags = new Tag[nTags];
        for (int i = 0; i < nTags; i++)
        {
            string tagName = input.ReadString();
            Color color = input.ReadColor();
            int from = input.ReadInt32();
            int to = input.ReadInt32();
            LoopDirection direction = (LoopDirection)input.ReadInt32();

            Tag tag = new(tagName, from, to, direction, color);
            tags[i] = tag;
        }

        int nSlices = input.ReadInt32();
        slices = new Slice[nSlices];
        for (int i = 0; i < nSlices; i++)
        {
            string sliceName = input.ReadString();
            Color color = input.ReadColor();
            int frame = input.ReadInt32();

            int x = input.ReadInt32();
            int y = input.ReadInt32();
            int w = input.ReadInt32();
            int h = input.ReadInt32();
            Rectangle bounds = new(x, y, w, h);

            Rectangle? center = default;
            bool isNinePatch = input.ReadBoolean();
            if (isNinePatch)
            {
                int cx = input.ReadInt32();
                int cy = input.ReadInt32();
                int cw = input.ReadInt32();
                int ch = input.ReadInt32();
                center = new(cx, cy, cw, ch);
            }

            Point? pivot = default;
            bool hasPivot = input.ReadBoolean();
            if (hasPivot)
            {
                int px = input.ReadInt32();
                int py = input.ReadInt32();
                pivot = new(px, py);
            }

            Slice slice = new(sliceName, color, frame, bounds, center, pivot);
            slices[i] = slice;
        }
    }

    private Texture2D CreateTexture(GraphicsDevice device, Color[] pixels, int width, int height)
    {
        Texture2D texture = new(device, width, height, false, SurfaceFormat.Color);
        texture.SetData<Color>(pixels);
        return texture;
    }

    private SpriteSheet CreateSpriteSheet(string name, Texture2D texture, Frame[] frames, Tag[] tags, Slice[] slices)
    {
        SpriteSheet spriteSheet = new(name, texture);

        for (int i = 0; i < frames.Length; i++)
        {
            Frame frame = frames[i];
            _ = spriteSheet.CreateFrame($"frame_{i}", frame.SourceRectangle, frame.Duration);
        }

        //  2 .. 5
        //  2, 3, 4, 5 = 4 frames
        //  5 - 2 = 3 + 1 = 4

        //  3 .. 10
        //  3, 4, 5, 6, 7, 8, 9, 10 = 8 frames
        //   10 - 3 = 7 + 1 = 8

        //  1 .. 5
        //  1, 2, 3, 4, 5

        for (int i = 0; i < tags.Length; i++)
        {
            Tag tag = tags[i];
            int[] indexes = new int[tag.To - tag.From + 1];
            for (int f = 0; f < indexes.Length; f++)
            {
                indexes[f] = tag.From + f;
            }

            spriteSheet.AddAnimationDefinition(tag.Name, true, tag.Direction == LoopDirection.Reverse, tag.Direction == LoopDirection.PingPong, indexes);
        }

        return spriteSheet;

    }
}
