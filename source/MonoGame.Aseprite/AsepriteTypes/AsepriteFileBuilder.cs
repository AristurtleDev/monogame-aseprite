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

using System.IO.Compression;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.AsepriteTypes;

internal class AsepriteFileBuilder
{
    private Color[] _palette = Array.Empty<Color>();
    private byte _transparentIndex = new();
    private ushort _colorDepth;
    private bool _layerOpacityValid;
    private ushort _frameWidth;
    private ushort _frameHeight;
    private List<AsepriteFrame> _frames = new();
    private List<AsepriteLayer> _layers = new();
    private List<AsepriteTag> _tags = new();
    private List<AsepriteSlice> _slices = new();
    private List<AsepriteTileset> _tilesets = new();
    private List<AsepriteCel> _nextFrameCels = new();
    private string _name;

    internal AsepriteFileBuilder(string name) => _name = name;

    internal void SetFrameWidth(ushort width) => _frameWidth = width;
    internal void SetFrameHeight(ushort height) => _frameHeight = height;
    internal void SetColorDepth(ushort depth) => _colorDepth = depth;
    internal void SetTransparentIndex(byte index)
    {
        if (_colorDepth != 8)
        {
            //  Non-zero transparent index is only valid when color depth is 8 (Indexed mode)
            _transparentIndex = 0;
        }
        else
        {
            _transparentIndex = index;
        }
    }
    internal void SetLayerOpacityValid(bool isValid) => _layerOpacityValid = isValid;

    internal void AddFrame(int duration)
    {
        AsepriteFrame frame = new(_frameWidth, _frameHeight, duration, _nextFrameCels.ToArray());
        _nextFrameCels.Clear();
        _frames.Add(frame);
    }

    internal void AddLayer(AsepriteLayerFlags flags, ushort blend, byte opacity, string name)
    {
        if (!_layerOpacityValid)
        {
            opacity = 255;
        }

        AsepriteLayer layer = new(flags, (AsepriteBlendMode)blend, opacity, name);
        _layers.Add(layer);
    }

    internal void AddTilemapLayer(uint tilesetIndex, AsepriteLayerFlags flags, ushort blend, byte opacity, string name)
    {
        AsepriteTileset tileset = _tilesets[(int)tilesetIndex];
        AsepriteTilemapLayer layer = new(tileset, /*(int)tilesetIndex,*/ flags, (AsepriteBlendMode)blend, opacity, name);
        _layers.Add(layer);
    }

    internal void AddRawImageCel(short x, short y, ushort width, ushort height, ushort layerIndex, byte opacity, ReadOnlySpan<byte> data)
    {
        Color[] pixels = new Color[width * height];
        ToColor(data, pixels);
        AsepriteLayer layer = _layers[layerIndex];
        Point position = new(x, y);
        AsepriteImageCel cel = new(width, height, pixels, layer, position, opacity);
        _nextFrameCels.Add(cel);
    }

    internal void AddLinkedCel(ushort frameIndex)
    {
        AsepriteFrame frame = _frames[frameIndex];
        AsepriteCel linkedCel = frame.Cels[_nextFrameCels.Count];
        _nextFrameCels.Add(linkedCel);
    }

    internal void AddCompressedImageCel(short x, short y, ushort width, ushort height, ushort layerIndex, byte opacity, byte[] compressedData)
    {
        Color[] pixels = new Color[width * height];
        byte[] decompressedData = Decompress(compressedData);
        AddRawImageCel(x, y, width, height, layerIndex, opacity, decompressedData);
    }

    internal void AddCompressedTilemapCel(short x, short y, ushort columns, ushort rows, ushort layerIndex, byte opacity, byte[] compressedData, ushort bitsPerTile, uint idBitmask, uint xFlipBitmask, uint yFlipBitmask, uint rotationBitmask)
    {
        Span<byte> decompressedData = Decompress(compressedData);

        int bytesPerTile = bitsPerTile / 8;
        int tileCount = decompressedData.Length / bytesPerTile;
        AsepriteTile[] tiles = new AsepriteTile[tileCount];

        for (int i = 0, b = 0; i < tileCount; i++, b += bytesPerTile)
        {
            ReadOnlySpan<byte> dword = decompressedData.Slice(b, bytesPerTile);
            uint value = BitConverter.ToUInt32(dword);
            uint id = (value & idBitmask) >> 0;
            uint xFlip = (value & xFlipBitmask);
            uint yFlip = (value & yFlipBitmask);
            uint rotation = (value & rotationBitmask);
            AsepriteTile tile = new((int)id, (int)xFlip, (int)yFlip, (int)rotation);
            tiles[i] = tile;
        }

        AsepriteLayer layer = _layers[layerIndex];
        Point position = new(x, y);
        AsepriteTilemapCel cel = new(columns, rows, tiles, layer, position, opacity);
        _nextFrameCels.Add(cel);
    }

    internal void AddTag(ushort from, ushort to, byte direction, ReadOnlySpan<byte> rgb, string name)
    {
        Color color = new Color(rgb[0], rgb[1], rgb[2], (byte)255);
        AsepriteTag tag = new(from, to, (AsepriteLoopDirection)direction, color, name);
        _tags.Add(tag);
    }

    internal void ResizePalette(uint newSize)
    {
        if (newSize > 0 && newSize > _palette.Length)
        {
            Color[] tmp = new Color[newSize];
            Array.Copy(_palette, tmp, _palette.Length);
            _palette = tmp;
        }
    }

    internal void AddPaletteEntry(uint index, ReadOnlySpan<byte> rgba)
    {
        _palette[index] = new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
    }

    internal void AddSlice(string name, bool isNinePatch, bool hasPivot, AsepriteSliceKey[] keys)
    {
        AsepriteSlice slice = new(name, isNinePatch, hasPivot, keys);
        _slices.Add(slice);
    }

    internal void AddTileset(uint id, uint count, ushort tileWidth, ushort tileHeight, string name, byte[] compressedData)
    {
        byte[] decompressedData = Decompress(compressedData);
        Color[] pixels = new Color[tileWidth * (tileHeight * count)];
        ToColor(decompressedData, pixels);
        AsepriteTileset tileset = new((int)id, (int)count, tileWidth, tileHeight, name, pixels);
        _tilesets.Add(tileset);
    }

    internal void SetLastCelUserData(string? text, Color? color)
    {
        AsepriteCel cel = _nextFrameCels[_nextFrameCels.Count - 1];
        cel.UserData.Text = text;
        cel.UserData.Color = color;
    }

    internal void SetLastLayerUserData(string? text, Color? color)
    {
        AsepriteLayer layer = _layers[_layers.Count - 1];
        layer.UserData.Text = text;
        layer.UserData.Color = color;
    }

    internal void SetLastSliceUserData(string? text, Color? color)
    {
        AsepriteSlice slice = _slices[_slices.Count - 1];
        slice.UserData.Text = text;
        slice.UserData.Color = color;
    }

    internal void SetTagUserData(int index, string? text, Color? color)
    {
        AsepriteTag tag = _tags[index];
        tag.UserData.Text = text;
        tag.UserData.Color = color;
    }

    internal AsepriteFile Build() =>
        new(_name, _frameWidth, _frameHeight, _palette, _frames.ToArray(), _layers.ToArray(), _tags.ToArray(), _slices.ToArray(), _tilesets.ToArray());

    private static byte[] Decompress(byte[] buffer)
    {
        using MemoryStream compressedStream = new(buffer);

        //  First 2 bytes are the zlib header information, skip past them.
        _ = compressedStream.ReadByte();
        _ = compressedStream.ReadByte();

        using MemoryStream decompressedStream = new();
        using DeflateStream deflateStream = new(compressedStream, CompressionMode.Decompress);
        deflateStream.CopyTo(decompressedStream);
        return decompressedStream.ToArray();
    }

    private void ToColor(ReadOnlySpan<byte> source, Span<Color> dest)
    {
        const ushort INDEXED_COLOR_DEPTH = 8;
        const ushort GRAYSCALE_COLOR_DEPTH = 16;
        const ushort RGBA_COLOR_DEPTH = 32;

        int bytesPerPixel = _colorDepth / 8;

        switch (_colorDepth)
        {
            case INDEXED_COLOR_DEPTH:
                IndexedToColor(source, dest);
                break;
            case GRAYSCALE_COLOR_DEPTH:
                GrayscaleToColor(source, dest);
                break;
            case RGBA_COLOR_DEPTH:
                RgbaToColor(source, dest);
                break;
            default:
                throw new InvalidOperationException($"Invalid Color Depth '{_colorDepth}'");
        }
    }

    private void IndexedToColor(ReadOnlySpan<byte> source, Span<Color> dest)
    {
        for (int i = 0; i < source.Length; i++)
        {
            int paletteIndex = source[i];

            if (paletteIndex == _transparentIndex)
            {
                dest[i] = new Color(0, 0, 0, 0);
            }
            else
            {
                dest[i] = _palette[paletteIndex];
            }
        }
    }

    private void GrayscaleToColor(ReadOnlySpan<byte> source, Span<Color> dest)
    {
        for (int i = 0, b = 0; i < dest.Length; i++, b += 2)
        {
            byte rgb = source[b];
            byte a = source[b + 1];
            dest[i] = new Color(rgb, rgb, rgb, a);
        }
    }

    private void RgbaToColor(ReadOnlySpan<byte> source, Span<Color> dest)
    {
        for (int i = 0, b = 0; i < dest.Length; i++, b += 4)
        {
            byte red = source[b];
            byte green = source[b + 1];
            byte blue = source[b + 2];
            byte alpha = source[b + 3];
            dest[i] = new Color(red, green, blue, alpha);
        }
    }
}

