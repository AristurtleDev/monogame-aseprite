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

public sealed class AsepriteSheetReader : ContentTypeReader<AsepriteSheet>
{
    protected override AsepriteSheet Read(ContentReader input, AsepriteSheet existingInstance)
    {
        int width = input.ReadInt32();
        int height = input.ReadInt32();

        int nPixels = input.ReadInt32();
        Color[] pixels = new Color[nPixels];
        for (int i = 0; i < nPixels; i++)
        {
            pixels[i] = input.ReadColor();
        }

        int nFrames = input.ReadInt32();
        List<Frame> frames = new();
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
            frames.Add(frame);
        }

        int nTags = input.ReadInt32();
        List<Tag> tags = new();
        for (int i = 0; i < nTags; i++)
        {
            string name = input.ReadString();
            Color color = input.ReadColor();
            int from = input.ReadInt32();
            int to = input.ReadInt32();
            LoopDirection direction = (LoopDirection)input.ReadInt32();

            Tag tag = new(name, from, to, direction, color);
            tags.Add(tag);
        }

        int nSlices = input.ReadInt32();
        List<Slice> slices = new();
        for (int i = 0; i < nSlices; i++)
        {
            string name = input.ReadString();
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

            Slice slice = new(name, color, frame, bounds, center, pivot);
            slices.Add(slice);
        }

        Texture2D texture = new(input.GetGraphicsDevice(), width, height, false, SurfaceFormat.Color);
        texture.SetData<Color>(pixels);

        return new(texture, frames, tags, slices);
    }
}
