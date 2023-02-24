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

using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Readers;

/// <summary>
/// Defines a reader that reads an <see cref="AsepriteFile"/>.
/// </summary>
public static class AsepriteFileReader
{
    const ushort CHUNK_TYPE_OLD_PALETTE_1 = 0x0004;
    const ushort CHUNK_TYPE_OLD_PALETTE_2 = 0x0011;
    const ushort CHUNK_TYPE_LAYER = 0x2004;
    const ushort CHUNK_TYPE_CEL = 0x2005;
    const ushort CHUNK_TYPE_CEL_EXTRA = 0x2006;
    const ushort CHUNK_TYPE_COLOR_PROFILE = 0x2007;
    const ushort CHUNK_TYPE_EXTERNAL_FILES = 0x2008;
    const ushort CHUNK_TYPE_MASK = 0x2016;
    const ushort CHUNK_TYPE_PATH = 0x2017;
    const ushort CHUNK_TYPE_TAGS = 0x2018;
    const ushort CHUNK_TYPE_PALETTE = 0x2019;
    const ushort CHUNK_TYPE_USER_DATA = 0x2020;
    const ushort CHUNK_TYPE_SLICE = 0x2022;
    const ushort CHUNK_TYPE_TILESET = 0x2023;

    /// <summary>
    ///     Reads the <see cref="AsepriteFile"/> at the given path.
    /// </summary>
    /// <param name="path">
    ///     The path and name of the aseprite file to read.
    /// </param>
    /// <returns>
    ///     The <see cref="AsepriteFile"/> created by this method.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    ///     Thrown if no file is located at the specified path.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if an error occurs during the reading of the aseprite file.  The exception message will contain the
    ///     cause of the exception.
    /// </exception>
    public static AsepriteFile ReadFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Unable to locate a file at the path '{path}'");
        }

        using Stream stream = File.OpenRead(path);
        using BinaryReader reader = new(stream);

        string name = Path.GetFileNameWithoutExtension(path);
        return Read(name, reader);
    }

    /// <summary>
    ///     Reads the <see cref="AsepriteFile"/> using the provided <see cref="Stream"/>.
    ///     <br />
    ///     Use this method with <see cref="TitleContainer.OpenStream(string)"/> to load raw .aseprite files on Android 
    ///     or other platforms.
    /// </summary>
    /// <param name="name">
    ///     The name of the Aseprite file.
    /// </param>
    /// <param name="fileStream">
    ///     A file stream, preferably instantiated from calling <see cref="TitleContainer.OpenStream(string)"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="AsepriteFile"/> created by this method.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    ///     Thrown if no file is located at the specified path.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if an error occurs during the reading of the aseprite file.  The exception message will contain the
    ///     cause of the exception.
    /// </exception>
    public static AsepriteFile ReadStream(string name, [NotNull] Stream fileStream)
    {
        using BinaryReader reader = new(fileStream);
        return Read(name, reader);
    }

    internal static AsepriteFile Read(string name, BinaryReader reader)
    {
        AsepriteFileBuilder fileBuilder = new(name);

        ReadFileHeader(reader, fileBuilder, out ushort nFrames);
        ReadFrames(reader, fileBuilder, nFrames);

        AsepriteFile file = fileBuilder.Build();

        return file;
    }

    private static void ReadFileHeader(BinaryReader reader, AsepriteFileBuilder builder, out ushort nFrames)
    {
        const uint LAYER_OPACITY_VALID_FLAG = 1;

        IgnoreDword(reader);
        ushort magic = ReadWord(reader);

        if (magic != 0xA5E0)
        {
            throw new InvalidOperationException($"Invalid header magic number (0x{magic:X4}).");
        }

        nFrames = ReadWord(reader);
        ushort frameWidth = ReadWord(reader);
        ushort frameHeight = ReadWord(reader);

        ushort colorDepth = ReadWord(reader);
        uint flags = ReadDword(reader);

        bool layerOpacityValid = (flags & LAYER_OPACITY_VALID_FLAG) != 0;

        IgnoreWord(reader);
        IgnoreDword(reader);
        IgnoreDword(reader);

        byte transparentIndex = ReadByte(reader);
        IgnoreBytes(reader, 3);
        ushort nColors = ReadWord(reader);

        reader.BaseStream.Position = 128;

        builder.SetFrameWidth(frameWidth);
        builder.SetFrameHeight(frameHeight);
        builder.SetColorDepth(colorDepth);
        builder.SetTransparentIndex(transparentIndex);
        builder.SetLayerOpacityValid(layerOpacityValid);
    }

    private static void ReadFrames(BinaryReader reader, AsepriteFileBuilder builder, ushort nFrames)
    {
        for (int frameNum = 0; frameNum < nFrames; frameNum++)
        {
            IgnoreDword(reader);
            ushort magic = ReadWord(reader);

            if (magic != 0xF1FA)
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


            //  Start iterator at -1 so after tags chunk is read, it'll
            //  increment to 0 to kick off the user data read.
            int tagIterator = -1;
            ushort lastChunkType = default;

            List<AsepriteCel> cels = new();

            for (uint chunkNum = 0; chunkNum < nChunks; chunkNum++)
            {
                lastChunkType = ReadChunk(reader, builder, lastChunkType, tagIterator, cels);
                if (lastChunkType == CHUNK_TYPE_TAGS)
                {
                    tagIterator++;
                }
            }

            builder.AddFrame(duration);
        }

    }

    private static ushort ReadChunk(BinaryReader reader, AsepriteFileBuilder builder, ushort lastChunkType, int tagIterator, List<AsepriteCel> cels)
    {
        long start = reader.BaseStream.Position;

        uint len = ReadDword(reader);
        ushort type = ReadWord(reader);

        long end = start + len;

        switch (type)
        {
            case CHUNK_TYPE_LAYER:
                ReadLayerChunk(reader, builder);
                break;
            case CHUNK_TYPE_CEL:
                ReadCelChunk(reader, builder, end);
                break;
            case CHUNK_TYPE_TAGS:
                ReadTagsChunk(reader, builder);
                break;
            case CHUNK_TYPE_PALETTE:
                ReadPaletteChunk(reader, builder);
                break;
            case CHUNK_TYPE_SLICE:
                ReadSliceChunk(reader, builder);
                break;
            case CHUNK_TYPE_TILESET:
                ReadTilesetChunk(reader, builder);
                break;
            case CHUNK_TYPE_USER_DATA:
                ReadUserDataChunk(reader, builder, lastChunkType, tagIterator);
                break;
            case CHUNK_TYPE_OLD_PALETTE_1:
            case CHUNK_TYPE_OLD_PALETTE_2:
            case CHUNK_TYPE_CEL_EXTRA:
            case CHUNK_TYPE_COLOR_PROFILE:
            case CHUNK_TYPE_EXTERNAL_FILES:
            case CHUNK_TYPE_MASK:
            case CHUNK_TYPE_PATH:
                reader.BaseStream.Position = end;
                break;
            default:
                throw new InvalidOperationException($"Unknown chunk type (0x{type:X4})");
        }

        if (type == CHUNK_TYPE_USER_DATA)
        {
            return lastChunkType;
        }

        return type;
    }

    private static void ReadLayerChunk(BinaryReader reader, AsepriteFileBuilder builder)
    {
        const int NORMAL_LAYER_TYPE = 0;
        const int GROUP_LAYER_TYPE = 1;
        const int TILEMAP_LAYER_TYPE = 2;

        ushort flagsValue = ReadWord(reader);
        ushort type = ReadWord(reader);
        IgnoreWord(reader);
        IgnoreWord(reader);
        IgnoreWord(reader);
        ushort blend = ReadWord(reader);
        byte opacity = ReadByte(reader);
        IgnoreBytes(reader, 3);
        string name = ReadString(reader);

        AsepriteLayerFlags flags = (AsepriteLayerFlags)flagsValue;

        switch (type)
        {
            case NORMAL_LAYER_TYPE:
            case GROUP_LAYER_TYPE:
                builder.AddLayer(flags, blend, opacity, name);
                break;
            case TILEMAP_LAYER_TYPE:
                ReadTilemapLayerChunk(reader, builder, flags, blend, opacity, name);
                break;
            default:
                throw new InvalidOperationException($"Unknown layer type '{type}'");
        }
    }

    private static void ReadTilemapLayerChunk(BinaryReader reader, AsepriteFileBuilder builder, AsepriteLayerFlags flags, ushort blend, byte opacity, string name)
    {
        uint index = ReadDword(reader);
        builder.AddTilemapLayer(index, flags, blend, opacity, name);
    }

    private static void ReadCelChunk(BinaryReader reader, AsepriteFileBuilder builder, long chunkEnd)
    {
        const ushort RAW_IMAGE_TYPE = 0;
        const ushort LINKED_CEL_TYPE = 1;
        const ushort COMPRESSED_IMAGE_TYPE = 2;
        const ushort COMPRESSED_TILEMAP_TYPE = 3;

        ushort layerIndex = ReadWord(reader);
        short x = ReadShort(reader);
        short y = ReadShort(reader);
        byte opacity = ReadByte(reader);
        ushort type = ReadWord(reader);
        IgnoreBytes(reader, 7);

        Point position = new(x, y);

        switch (type)
        {
            case RAW_IMAGE_TYPE:
                ReadRawImageCel(reader, builder, x, y, layerIndex, opacity, chunkEnd);
                break;
            case LINKED_CEL_TYPE:
                ReadLinkedCel(reader, builder);
                break;
            case COMPRESSED_IMAGE_TYPE:
                ReadCompressedImageCel(reader, builder, x, y, layerIndex, opacity, chunkEnd);
                break;
            case COMPRESSED_TILEMAP_TYPE:
                ReadCompressedTilemapCel(reader, builder, x, y, layerIndex, opacity, chunkEnd);
                break;
            default:
                throw new InvalidOperationException($"Unknown cel type '{type}'");
        }
    }

    private static void ReadRawImageCel(BinaryReader reader, AsepriteFileBuilder builder, short x, short y, ushort layerIndex, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord(reader);
        ushort height = ReadWord(reader);

        int len = (int)(chunkEnd - reader.BaseStream.Position);
        byte[] data = ReadBytes(reader, len);

        builder.AddRawImageCel(x, y, width, height, layerIndex, opacity, data);
    }

    private static void ReadLinkedCel(BinaryReader reader, AsepriteFileBuilder builder)
    {
        ushort frameIndex = ReadWord(reader);
        builder.AddLinkedCel(frameIndex);
    }

    private static void ReadCompressedImageCel(BinaryReader reader, AsepriteFileBuilder builder, short x, short y, ushort layerIndex, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord(reader);
        ushort height = ReadWord(reader);

        int len = (int)(chunkEnd - reader.BaseStream.Position);
        byte[] compressedData = ReadBytes(reader, len);
        builder.AddCompressedImageCel(x, y, width, height, layerIndex, opacity, compressedData);
    }

    private static void ReadCompressedTilemapCel(BinaryReader reader, AsepriteFileBuilder builder, short x, short y, ushort layerIndex, byte opacity, long chunkEnd)
    {
        ushort columns = ReadWord(reader);
        ushort rows = ReadWord(reader);
        ushort bitsPerTile = ReadWord(reader);
        uint idBitmask = ReadDword(reader);
        uint xFlipBitmask = ReadDword(reader);
        uint yFlipBitmask = ReadDword(reader);
        uint rotationBitmask = ReadDword(reader);
        IgnoreBytes(reader, 10);

        int len = (int)(chunkEnd - reader.BaseStream.Position);
        byte[] compressedData = ReadBytes(reader, len);
        builder.AddCompressedTilemapCel(x, y, columns, rows, layerIndex, opacity, compressedData, bitsPerTile, idBitmask, xFlipBitmask, yFlipBitmask, rotationBitmask);
    }

    private static void ReadTagsChunk(BinaryReader reader, AsepriteFileBuilder builder)
    {
        ushort count = ReadWord(reader);
        IgnoreBytes(reader, 8);

        for (int tagNum = 0; tagNum < count; tagNum++)
        {
            ushort from = ReadWord(reader);
            ushort to = ReadWord(reader);
            byte direction = ReadByte(reader);
            IgnoreBytes(reader, 8);
            ReadOnlySpan<byte> rgb = ReadBytes(reader, 3);
            IgnoreByte(reader);
            string name = ReadString(reader);

            builder.AddTag(from, to, direction, rgb, name);
        }
    }

    private static void ReadPaletteChunk(BinaryReader reader, AsepriteFileBuilder builder)
    {
        const ushort HAS_NAME_FLAG = 1;

        uint newSize = ReadDword(reader);
        uint from = ReadDword(reader);
        uint to = ReadDword(reader);
        IgnoreBytes(reader, 8);

        builder.ResizePalette(newSize);


        for (uint entry = from; entry <= to; entry++)
        {
            ushort flags = ReadWord(reader);
            ReadOnlySpan<byte> rgba = ReadBytes(reader, 4);

            if ((flags & HAS_NAME_FLAG) != 0)
            {
                IgnoreString(reader);
            }

            builder.AddPaletteEntry(entry, rgba);
        }
    }

    private static void ReadSliceChunk(BinaryReader reader, AsepriteFileBuilder builder)
    {
        const uint NINE_PATCH_FLAG = 1;
        const uint HAS_PIVOT_FLAG = 2;

        uint count = ReadDword(reader);
        uint flags = ReadDword(reader);
        IgnoreDword(reader);
        string name = ReadString(reader);

        bool isNinePatch = (flags & NINE_PATCH_FLAG) != 0;
        bool hasPivot = (flags & HAS_PIVOT_FLAG) != 0;

        AsepriteSliceKey[] keys = new AsepriteSliceKey[count];

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

            AsepriteSliceKey key = new((int)start, bounds, center, pivot);
            keys[keyNum] = key;
        }

        builder.AddSlice(name, isNinePatch, hasPivot, keys);
    }

    private static void ReadTilesetChunk(BinaryReader reader, AsepriteFileBuilder builder)
    {
        const uint EXTERNAL_FILE_FLAG = 1;
        const uint EMBEDDED_FLAG = 2;

        uint id = ReadDword(reader);
        uint flags = ReadDword(reader);
        uint count = ReadDword(reader);
        ushort tileWidth = ReadWord(reader);
        ushort tileHeight = ReadWord(reader);
        IgnoreShort(reader);
        IgnoreBytes(reader, 14);
        string name = ReadString(reader);

        if ((flags & EXTERNAL_FILE_FLAG) != 0)
        {
            throw new InvalidOperationException($"Tileset '{name}' includes tileset in external file.  This is not supported at this time");
        }

        if ((flags & EMBEDDED_FLAG) == 0)
        {
            throw new InvalidOperationException($"Tileset '{name}' does not include tileset image in file");
        }

        uint len = ReadDword(reader);
        byte[] compressedData = ReadBytes(reader, (int)len);

        builder.AddTileset(id, count, tileWidth, tileHeight, name, compressedData);
    }

    private static void ReadUserDataChunk(BinaryReader reader, AsepriteFileBuilder builder, ushort lastChunkType, int tagIterator)
    {
        const uint HAS_TEXT_FLAG = 1;
        const uint HAS_COLOR_FLAG = 2;
        uint flags = ReadDword(reader);

        string? text = default;
        Color? color = default;

        if ((flags & HAS_TEXT_FLAG) != 0)
        {
            text = ReadString(reader);
        }

        if ((flags & HAS_COLOR_FLAG) != 0)
        {
            ReadOnlySpan<byte> rgba = ReadBytes(reader, 4);
            color = new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
        }

        switch (lastChunkType)
        {
            case CHUNK_TYPE_CEL:
                builder.SetLastCelUserData(text, color);
                break;
            case CHUNK_TYPE_LAYER:
                builder.SetLastLayerUserData(text, color);
                break;
            case CHUNK_TYPE_SLICE:
                builder.SetLastSliceUserData(text, color);
                break;
            case CHUNK_TYPE_TAGS:
                builder.SetTagUserData(tagIterator, text, color);
                break;
            case CHUNK_TYPE_OLD_PALETTE_1:
                //  Starting in Aseprite 1.3-beta21, after the first palette chunk in the first frame, if user data is
                //  detected, then that is user data for the "sprite" itself
                builder.SetSpriteUserData(text, color);
                break;
            default:
                throw new InvalidOperationException($"Invalid chunk type (0x{lastChunkType:X4}) for user data");
        }
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
}
