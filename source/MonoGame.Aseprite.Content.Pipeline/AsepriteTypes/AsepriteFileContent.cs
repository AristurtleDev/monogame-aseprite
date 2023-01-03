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

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

public sealed class AsepriteFileContent
{
    internal Point FrameSize { get; }
    internal int FrameWidth => FrameSize.X;
    internal int FrameHeight => FrameSize.Y;
    internal AsepritePalette Palette { get; }
    internal List<AsepriteFrame> Frames { get; }
    internal List<AsepriteLayer> Layers { get; }
    internal List<AsepriteTag> Tags { get; }
    internal List<AsepriteSlice> Slices { get; }
    internal List<AsepriteTileset> Tilesets { get; }

    internal AsepriteFileContent(Point frameSize, AsepritePalette palette, List<AsepriteFrame> frames, List<AsepriteLayer> layers, List<AsepriteTag> tags, List<AsepriteSlice> slices, List<AsepriteTileset> tilesets)
    {
        FrameSize = frameSize;
        Palette = palette;
        Frames = frames;
        Layers = layers;
        Tags = tags;
        Slices = slices;
        Tilesets = tilesets;
    }
}
