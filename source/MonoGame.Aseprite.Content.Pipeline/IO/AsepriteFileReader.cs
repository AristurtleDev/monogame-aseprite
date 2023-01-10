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
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.IO;

internal static class AsepriteFileReader
{
    internal static AsepriteFile ReadFile(string path)
    {
        Stream stream = File.OpenRead(path);
        BinaryReader reader = new(stream);

        string name = Path.GetFileNameWithoutExtension(path);
        AsepriteFile file = new(name);

        ReadFileHeader(reader, file);
        ReadFrames(reader, file);

        return file;
    }

    private static void ReadFileHeader(BinaryReader reader, AsepriteFile file)
    {
        IgnoreDword(reader);
        ushort magic = ReadWord(reader);

        if (magic != 0xA5E0)
        {
            throw new InvalidOperationException($"Invalid header magic number (0x{magic:X4}).");
        }

        file.FrameCount = ReadWord(reader);
        ushort width = ReadWord(reader);
        ushort height = ReadWord(reader);

        file.FrameSize = new(width, height);

        file.ColorDepth = ReadWord(reader);
        uint flags = ReadDword(reader);

        file.LayerOpacityValid = HasFlag(flags, 1);

        IgnoreWord(reader);
        IgnoreDword(reader);
        IgnoreDword(reader);

        file.TransparentIndex = ReadByte(reader);
        IgnoreBytes(reader, 3);
        ushort nColors = ReadWord(reader);

        file.ResizePalette(nColors);

        reader.BaseStream.Position = 128;
    }

    private static void ReadFrames(BinaryReader reader, AsepriteFile file)
    {
        for (int frameNum = 0; frameNum < file.FrameCount; frameNum++)
        {
            IgnoreDword(reader);
            ushort magic = ReadWord(reader);

            if (magic != 0xF1fA)
            {
                throw new InvalidOperationException($"Invalid frame magic in frame number {frameNum}");
            }

            ushort nChunksA = ReadWord(reader);
            ushort duration = ReadWord(reader);
            IgnoreBytes(reader, 2);
            uint nChunksB = ReadDword(reader);

            uint nChunks = nChunksA == 0xFFFF && nChunksA < nChunksB ?
                           nChunksB :
                           nChunksA;

            Frame frame = new(file.FrameSize, duration);
            file.Frames.Add(frame);

            //  Start iterator at -1 so after tags chunk is read, it'll
            //  increment to 0 to kick off the user data read.
            int tagIterator = -1;
            ushort lastChunkType = default;

            for (uint chunkNum = 0; chunkNum < nChunks; chunkNum++)
            {
                lastChunkType = ReadChunk(reader, file, lastChunkType, tagIterator);
                if (lastChunkType == 0x2018)
                {
                    tagIterator++;
                }
            }
        }
    }

    private static ushort ReadChunk(BinaryReader reader, AsepriteFile file, ushort lastChunkType, int tagIterator)
    {
        long start = reader.BaseStream.Position;

        uint len = ReadDword(reader);
        ushort type = ReadWord(reader);

        long end = start + len;

        switch (type)
        {
            case 0x2004:
                ReadLayerChunk(reader, file);
                break;
            case 0x2005:
                ReadCelChunk(reader, file, end);
                break;
            case 0x2018:
                ReadTagsChunk(reader, file);
                break;
            case 0x2019:
                ReadPaletteChunk(reader, file);
                break;
            case 0x2022:
                ReadSliceChunk(reader, file);
                break;
            case 0x2023:
                ReadTilesetChunk(reader, file);
                break;
            case 0x2020:
                ReadUserDataChunk(reader, file, lastChunkType, tagIterator);
                break;
            case 0x0004:    //  Old Palette 1
            case 0x0011:    //  Old Palette 2
            case 0x2006:    //  Cel Extra
            case 0x2007:    //  Color Profile
            case 0x2008:    //  External Files
            case 0x2016:    //  Mask
            case 0x2017:    //  Path
                reader.BaseStream.Position = end;
                break;
            default:
                throw new InvalidOperationException($"Unknown chunk type (0x{type:X4})");
        }

        if(type == 0x2020)
        {
            return lastChunkType;
        }

        return type;
    }

    private static void ReadLayerChunk(BinaryReader reader, AsepriteFile file)
    {
        ushort flags = ReadWord(reader);
        ushort type = ReadWord(reader);
        IgnoreWord(reader);
        IgnoreWord(reader);
        IgnoreWord(reader);
        ushort blend = ReadWord(reader);
        byte opacity = ReadByte(reader);
        IgnoreBytes(reader, 3);
        string name = ReadString(reader);

        if (!file.LayerOpacityValid)
        {
            opacity = 255;
        }

        bool isVisible = HasFlag(flags, 1);
        bool isBackground = HasFlag(flags, 8);
        bool isReference = HasFlag(flags, 64);

        Layer layer = type switch
        {
            0 or 1 => new Layer(isVisible, isBackground, isReference, blend, opacity, name),
            2 => ReadTilemapLayerChunk(reader, file, isVisible, isBackground, isReference, blend, opacity, name),
            _ => throw new InvalidOperationException($"Unknown layer type '{type}'")
        };

        file.Layers.Add(layer);
    }

    private static TilemapLayer ReadTilemapLayerChunk(BinaryReader reader, AsepriteFile file, bool isVisible, bool isBackground, bool isReference, ushort blend, byte opacity, string name)
    {
        uint index = ReadDword(reader);
        Tileset tileset = file.Tilesets[(int)index];
        return new(tileset, isVisible, isBackground, isReference, blend, opacity, name);
    }

    private static void ReadCelChunk(BinaryReader reader, AsepriteFile file, long chunkEnd)
    {
        ushort index = ReadWord(reader);
        short x = ReadShort(reader);
        short y = ReadShort(reader);
        byte opacity = ReadByte(reader);
        ushort type = ReadWord(reader);
        IgnoreBytes(reader, 7);

        Frame frame = file.Frames[file.Frames.Count - 1];
        Layer layer = file.Layers[index];
        Point position = new(x, y);

        Cel cel = type switch
        {
            0 => ReadRawImageCel(reader, file, layer, position, opacity, chunkEnd),
            1 => ReadLinkedCel(reader, file, frame),
            2 => ReadCompressedImageCel(reader, file, layer, position, opacity, chunkEnd),
            3 => ReadCompressedTilemapCel(reader, layer, position, opacity, chunkEnd),
            _ => throw new InvalidOperationException($"Unknown cel type '{type}'")
        };

        frame.Cels.Add(cel);
    }

    private static ImageCel ReadRawImageCel(BinaryReader reader, AsepriteFile file, Layer layer, Point position, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord(reader);
        ushort height = ReadWord(reader);

        int len = (int)(chunkEnd - reader.BaseStream.Position);
        byte[] data = ReadBytes(reader, len);

        Point size = new(width, height);
        Color[] pixels = ToColor(data, file.ColorDepth, file.TransparentIndex, file.Palette);

        return new(size, pixels, layer, position, opacity);
    }

    private static Cel ReadLinkedCel(BinaryReader reader, AsepriteFile file, Frame frame)
    {
        ushort frameIndex = ReadWord(reader);
        return file.Frames[frameIndex].Cels[frame.Cels.Count];
    }

    private static ImageCel ReadCompressedImageCel(BinaryReader reader, AsepriteFile file, Layer layer, Point position, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord(reader);
        ushort height = ReadWord(reader);

        int len = (int)(chunkEnd - reader.BaseStream.Position);
        byte[] data = ReadBytes(reader, len);

        data = Decompress(data);

        Point size = new(width, height);
        Color[] pixels = ToColor(data, file.ColorDepth, file.TransparentIndex, file.Palette);

        return new(size, pixels, layer, position, opacity);
    }

    private static TilemapCel ReadCompressedTilemapCel(BinaryReader reader, Layer layer, Point position, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord(reader);
        ushort height = ReadWord(reader);
        ushort bitsPerTile = ReadWord(reader);
        uint idBitmask = ReadDword(reader);
        uint xFlipBitmask = ReadDword(reader);
        uint yFlipBitmask = ReadDword(reader);
        uint rotationBitmask = ReadDword(reader);
        IgnoreBytes(reader, 10);

        int len = (int)(chunkEnd - reader.BaseStream.Position);
        byte[] data = ReadBytes(reader, len);

        data = Decompress(data);

        Point size = new(width, height);

        int bytesPerTile = bitsPerTile / 8;
        int tileCount = data.Length / bytesPerTile;

        TilemapCel cel = new(size, layer, position, opacity);

        for (int i = 0, b = 0; i < tileCount; i++, b += bytesPerTile)
        {
            byte[] dword = data[b..(b + bytesPerTile)];
            uint value = BitConverter.ToUInt32(dword);
            uint id = (value & idBitmask) >> 0;
            uint xFlip = (value & xFlipBitmask);
            uint yFlip = (value & yFlipBitmask);
            uint rotation = (value & rotationBitmask);

            Tile tile = new((int)id, (int)xFlip, (int)yFlip, (int)rotation);
            cel.Tiles.Add(tile);
        }

        return cel;
    }

    private static void ReadTagsChunk(BinaryReader reader, AsepriteFile file)
    {
        ushort count = ReadWord(reader);
        IgnoreBytes(reader, 8);

        for (int tagNum = 0; tagNum < count; tagNum++)
        {
            ushort from = ReadWord(reader);
            ushort to = ReadWord(reader);
            byte direction = ReadByte(reader);
            IgnoreBytes(reader, 8);
            byte r = ReadByte(reader);
            byte g = ReadByte(reader);
            byte b = ReadByte(reader);
            IgnoreByte(reader);
            string name = ReadString(reader);

            Color color = Color.FromNonPremultiplied(r, g, b, 255);

            Tag tag = new(from, to, direction, color, name);
            file.Tags.Add(tag);
        }
    }

    private static void ReadPaletteChunk(BinaryReader reader, AsepriteFile file)
    {
        uint newSize = ReadDword(reader);
        uint from = ReadDword(reader);
        uint to = ReadDword(reader);
        IgnoreBytes(reader, 8);

        file.ResizePalette((int)newSize);

        for (uint entry = from; entry <= to; entry++)
        {
            ushort flags = ReadWord(reader);
            byte r = ReadByte(reader);
            byte g = ReadByte(reader);
            byte b = ReadByte(reader);
            byte a = ReadByte(reader);

            if (HasFlag(flags, 1))
            {
                IgnoreString(reader);
            }

            file.Palette[(int)entry] = Color.FromNonPremultiplied(r, g, b, a);
        }
    }

    private static void ReadSliceChunk(BinaryReader reader, AsepriteFile file)
    {
        uint count = ReadDword(reader);
        uint flags = ReadDword(reader);
        IgnoreDword(reader);
        string name = ReadString(reader);

        bool isNinePatch = HasFlag(flags, 1);
        bool hasPivot = HasFlag(flags, 2);

        Slice slice = new(name, isNinePatch, hasPivot);

        for (uint keyNum = 0; keyNum < count; keyNum++)
        {
            uint start = ReadDword(reader);
            int x = ReadLong(reader);
            int y = ReadLong(reader);
            uint width = ReadDword(reader);
            uint height = ReadDword(reader);

            Rectangle bounds = new(x, y, (int)width, (int)height);
            Rectangle? center = default;
            Point? pivot = default;

            if (isNinePatch)
            {
                int cx = ReadLong(reader);
                int cy = ReadLong(reader);
                uint cw = ReadDword(reader);
                uint ch = ReadDword(reader);

                center = new(cx, cy, (int)cw, (int)ch);
            }

            if (hasPivot)
            {
                int px = ReadLong(reader);
                int py = ReadLong(reader);

                pivot = new(px, py);
            }

            SliceKey key = new((int)start, bounds, center, pivot);
            slice.Keys.Add(key);
        }

        file.Slices.Add(slice);
    }

    private static void ReadTilesetChunk(BinaryReader reader, AsepriteFile file)
    {
        uint id = ReadDword(reader);
        uint flags = ReadDword(reader);
        uint count = ReadDword(reader);
        ushort width = ReadWord(reader);
        ushort height = ReadWord(reader);
        IgnoreShort(reader);
        IgnoreBytes(reader, 14);
        string name = ReadString(reader);

        if (HasFlag(flags, 1))
        {
            throw new InvalidOperationException($"Tileset '{name}' includes tileset in external file.  This is not supported at this time");
        }

        if (!HasFlag(flags, 2))
        {
            throw new InvalidOperationException($"Tileset '{name}' does not include tileset image in file");
        }

        uint len = ReadDword(reader);
        byte[] data = ReadBytes(reader, (int)len);

        data = Decompress(data);

        Color[] pixels = ToColor(data, file.ColorDepth, file.TransparentIndex, file.Palette);
        Point size = new(width, height);

        Tileset tileset = new((int)id, (int)count, size, name, pixels);
        file.Tilesets.Add(tileset);
    }

    private static void ReadUserDataChunk(BinaryReader reader, AsepriteFile file, ushort lastChunkType, int tagIterator)
    {
        uint flags = ReadDword(reader);

        string? text = default;
        Color? color = default;

        if (HasFlag(flags, 1))
        {
            text = ReadString(reader);
        }

        if (HasFlag(flags, 2))
        {
            byte r = ReadByte(reader);
            byte g = ReadByte(reader);
            byte b = ReadByte(reader);
            byte a = ReadByte(reader);

            color = Color.FromNonPremultiplied(r, g, b, a);
        }

        switch (lastChunkType)
        {
            case 0x2005:
                SetLastCelUserData(file, text, color);
                break;
            case 0x2004:
                SetLastLayerUserData(file, text, color);
                break;
            case 0x2022:
                SetLastSliceUserData(file, text, color);
                break;
            case 0x2018:
                SetNextTagUserData(file, tagIterator, text, color);
                break;
            default:
                throw new InvalidOperationException($"Invalid chunk type (0x{lastChunkType:X4} for user data");
        }
    }

    private static void SetLastCelUserData(AsepriteFile file, string? text, Color? color)
    {
        Frame frame = file.Frames[file.Frames.Count - 1];
        Cel cel = frame.Cels[frame.Cels.Count - 1];
        cel.UserData.Text = text;
        cel.UserData.Color = color;
    }

    private static void SetLastLayerUserData(AsepriteFile file, string? text, Color? color)
    {
        Layer layer = file.Layers[file.Layers.Count - 1];
        layer.UserData.Text = text;
        layer.UserData.Color = color;
    }

    private static void SetLastSliceUserData(AsepriteFile file, string? text, Color? color)
    {
        Slice slice = file.Slices[file.Slices.Count - 1];
        slice.UserData.Text = text;
        slice.UserData.Color = color;
    }

    private static void SetNextTagUserData(AsepriteFile file, int tagIterator, string? text, Color? color)
    {
        Tag tag = file.Tags[tagIterator];
        tag.UserData.Text = text;
        tag.UserData.Color = color;
    }

    private static byte ReadByte(BinaryReader reader) => reader.ReadByte();
    private static byte[] ReadBytes(BinaryReader reader, int len) => reader.ReadBytes(len);
    private static ushort ReadWord(BinaryReader reader) => reader.ReadUInt16();
    private static short ReadShort(BinaryReader reader) => reader.ReadInt16();
    private static uint ReadDword(BinaryReader reader) => reader.ReadUInt32();
    private static int ReadLong(BinaryReader reader) => reader.ReadInt32();
    private static string ReadString(BinaryReader reader) => Encoding.UTF8.GetString(ReadBytes(reader, ReadWord(reader)));

    private static void IgnoreBytes(BinaryReader reader, int len) => reader.BaseStream.Position += len;
    private static void IgnoreByte(BinaryReader reader) => IgnoreBytes(reader, 1);
    private static void IgnoreWord(BinaryReader reader) => IgnoreBytes(reader, 2);
    private static void IgnoreShort(BinaryReader reader) => IgnoreBytes(reader, 2);
    private static void IgnoreDword(BinaryReader reader) => IgnoreBytes(reader, 4);
    private static void IgnoreLong(BinaryReader reader) => IgnoreBytes(reader, 4);
    private static void IgnoreString(BinaryReader reader) => IgnoreBytes(reader, ReadWord(reader));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

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

    private static Color[] ToColor(byte[] data, ushort depth, int transparentIndex, Color[] palette)
    {
        int bytesPerPixel = depth / 8;
        int len = data.Length / bytesPerPixel;

        Color[] result = depth switch
        {
            8 => IndexedToColor(data, transparentIndex, palette),
            16 => GrayscaleToColor(data),
            32 => RgbaToColor(data),
            _ => throw new InvalidOperationException($"Invalid Color Depth '{depth}'")
        };

        return result;
    }

    private static Color[] IndexedToColor(byte[] data, int transparentIndex, Color[] palette)
    {
        Color[] result = new Color[data.Length];

        for (int i = 0; i < result.Length; i++)
        {
            int index = data[i];

            if (index == transparentIndex)
            {
                result[i] = new Color(0, 0, 0, 0);
            }
            else
            {
                result[i] = palette[index];
            }
        }

        return result;
    }

    private static Color[] GrayscaleToColor(byte[] data)
    {
        Color[] result = new Color[data.Length / 2];

        for (int i = 0, b = 0; i < result.Length; i++, b += 2)
        {
            byte rgb = data[b];
            byte alpha = data[b + 1];

            result[i] = Color.FromNonPremultiplied(rgb, rgb, rgb, alpha);
        }

        return result;
    }

    private static Color[] RgbaToColor(byte[] data)
    {
        Color[] result = new Color[data.Length / 4];

        for (int i = 0, b = 0; i < result.Length; i++, b += 4)
        {
            byte red = data[b];
            byte green = data[b + 1];
            byte blue = data[b + 2];
            byte alpha = data[b + 3];

            result[i] = Color.FromNonPremultiplied(red, green, blue, alpha);
        }

        return result;
    }
}
