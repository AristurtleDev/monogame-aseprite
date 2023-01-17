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

namespace MonoGame.Aseprite.AsepriteTypes;

public sealed class AsepriteFile
{
    private Color[] _palette = Array.Empty<Color>();

    internal List<AsepriteFrame> Frames { get; } = new();
    internal List<AsepriteLayer> Layers { get; } = new();
    internal List<AsepriteTag> Tags { get; } = new();
    internal List<AsepriteSlice> Slices { get; } = new();
    internal List<AsepriteTileset> Tilesets { get; } = new();
    internal Color[] Palette => _palette;
    internal int TransparentIndex { get; set; }
    internal string Name { get; set; }
    internal int FrameWidth { get; set; }
    internal int FrameHeight { get; set; }
    internal ushort ColorDepth { get; set; }
    internal int FrameCount { get; set; }
    internal bool LayerOpacityValid { get; set; }

    internal AsepriteFile(string name) => Name = name;

    internal void ResizePalette(int newSize)
    {
        if (newSize > 0 && newSize > _palette.Length)
        {
            Color[] tmp = new Color[newSize];
            Array.Copy(_palette, tmp, _palette.Length);
            _palette = tmp;
        }
    }
}
