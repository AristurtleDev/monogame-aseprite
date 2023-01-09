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

using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Importers;

/// <summary>
///     The content pipeline importer for importing the contents of an
///     Aseprite file.
/// </summary>
[ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite File Importer - MonoGame.Aseprite", DefaultProcessor = "AsepriteSpritesheetProcessor")]
public class AsepriteFileImporter : ContentImporter<AsepriteFile>
{
    private const ushort CHUNK_TYPE_OLD_PALETTE1 = 0x0004;      //  Old Palette Chunk Type
    private const ushort CHUNK_TYPE_OLD_PALETTE2 = 0x0011;      //  Old Palette Chunk Type
    private const ushort CHUNK_TYPE_LAYER = 0x2004;             //  Layer Chunk Type
    private const ushort CHUNK_TYPE_CEL = 0x2005;               //  Cel Chunk Type
    private const ushort CHUNK_TYPE_CEL_EXTRA = 0x2006;         //  Cel Extra Chunk Type
    private const ushort CHUNK_TYPE_COLOR_PROFILE = 0x2007;     //  Color Profile Chunk Type
    private const ushort CHUNK_TYPE_EXTERNAL_FILES = 0x2008;    //  External Files Chunk Type
    private const ushort CHUNK_TYPE_MASK = 0x2016;              //  Mask Chunk Type
    private const ushort CHUNK_TYPE_PATH = 0x2017;              //  Path Chunk Type
    private const ushort CHUNK_TYPE_TAGS = 0x2018;              //  Tags Chunk Type
    private const ushort CHUNK_TYPE_PALETTE = 0x2019;           //  Palette Chunk Type
    private const ushort CHUNK_TYPE_USER_DATA = 0x2020;         //  User Data Chunk Type
    private const ushort CHUNK_TYPE_SLICE = 0x2022;             //  Slice Chunk Type
    private const ushort CHUNK_TYPE_TILESET = 0x2023;           //  Tileset Chunk Type

    private const int HEADER_LEN = 128;
    private const ushort HEADER_MAGIC = 0xA5E0;
    private const uint HEADER_FLAG_LAYER_OPACITY_VALID = 1;

    private const ushort LAYER_FLAG_IS_VISIBLE = 1;
    private const ushort LAYER_FLAG_IS_BACKGROUND = 8;
    private const ushort LAYER_FLAG_IS_REFERENCE = 64;
    private const ushort LAYER_TYPE_NORMAL = 0;
    private const ushort LAYER_TYPE_GROUP = 1;
    private const ushort LAYER_TYPE_TILEMAP = 2;

    private const int CEL_TYPE_RAW_IMAGE = 0;
    private const int CEL_TYPE_LINKED = 1;
    private const int CEL_TYPE_COMPRESSED_IMAGE = 2;
    private const int CEL_TYPE_COMPRESSED_TILEMAP = 3;

    private const int TILE_ID_SHIFT = 0;

    private const ushort FRAME_MAGIC = 0xF1FA;

    /// <summary>
    ///     Imports the Aseprite file at the specified file path.
    /// </summary>
    /// <param name="filePath">
    ///     The absolute path, including extension, to the Aseprite file to
    ///     import.
    /// </param>
    /// <param name="context">
    ///     The importer context. This is provided by the MonoGame framework
    ///     when called from the mgcb-editor.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="AsepriteFile"/> class containing
    ///     the data imported from the Aseprite file.
    /// </returns>
    public override AsepriteFile Import(string filePath, ContentImporterContext context)
    {
        return AsepriteFile.Load(filePath);
    }

    private AsepriteFileContent ReadFileContent(string path)
    {
        using Stream stream = File.OpenRead(path);
        using BinaryReader reader = new BinaryReader(stream);

        AsepriteFileContent content = new();
        content.Name = Path.GetFileNameWithoutExtension(path);

        IgnoreDword(reader);                        //  File size (don't need)
        ushort hMagic = ReadWord(reader);           //  Header magic number
        ushort nFrames = ReadWord(reader);          //  Total number of frames
        ushort frameWidth = ReadWord(reader);       //  Frame width, in pixels
        ushort frameHeight = ReadWord(reader);      //  Frame height, in pixels
        ushort depth = ReadWord(reader);            //  Color depth (bits-per-pixel)
        uint hFlags = ReadDword(reader);            //  Header flags
        IgnoreWord(reader);                         //  Speed (ms between frames) (deprecated)
        IgnoreDword(reader);                        //  Set to 0
        IgnoreDword(reader);                        //  Set to 0
        byte transIndex = ReadByte(reader);         //  Index of transparent color in palette
        IgnoreBytes(reader, 3);                     //  Ignore these bytes
        ushort nColors = ReadWord(reader);          //  Total number of colors
        reader.BaseStream.Position = HEADER_LEN;    //  Skip to end of header, don't need the rest

        if (hMagic != HEADER_MAGIC)
        {
            throw new InvalidContentException($"Invalid header magic number (0x{hMagic:X4}). This does not appear to be a valid Aseprite file.");
        }

        bool layerOpacityValid = HasFlag(hFlags, HEADER_FLAG_LAYER_OPACITY_VALID);

        if (frameWidth < 1 || frameHeight < 1)
        {
            throw new InvalidContentException($"Invalid canvas size ({frameWidth}x{frameHeight}). Width and Height must be greater than zero");
        }

        if (depth != 32 && depth != 16 && depth != 8)
        {
            throw new InvalidContentException($"Invalid color depth '{depth}'");
        }

        if (depth != 8)
        {
            //  Transparent index can only be non-zero when the color depth mode
            //  is indexed mode (8)
            transIndex = 0;
        }

        //  When number of colors is zero, this means 256 (old sprites)
        if (nColors == 0)
        {
            nColors = 256;
        }

        Size frameSize = new(frameWidth, frameHeight);

        for (int frameNum = 0; frameNum < nFrames; frameNum++)
        {
            uint frameLen = ReadDword(reader);  //  Frame length, in bytes
            ushort frameMagic = ReadWord(reader);   //  Frame magic 0xF1FA
            ushort nChunksA = ReadWord(reader); //  Old field which specifies number of chunks
            ushort duration = ReadWord(reader); //  Frame duration (in milliseconds)
            IgnoreBytes(reader, 2);             //  For future (set to 0)
            uint nChunksB = ReadDword(reader);  //  New field which specifies number of chunks

            if (frameMagic != FRAME_MAGIC)
            {
                throw new InvalidContentException($"Invalid frame magic number (0x{frameMagic:X4}) in frame number {frameNum}");
            }

            uint nChunks = nChunksA == 0xFFFF && nChunksA < nChunksB ?
                           nChunksB :
                           nChunksA;

            AsepriteFrame frame = new(frameSize, duration);

            for (uint chunkNum = 0; chunkNum < nChunks; chunkNum++)
            {
                long chunkStart = reader.BaseStream.Position;

                uint chunkLen = ReadDword(reader);      //  Chunk length, in bytes
                ushort chunkType = ReadWord(reader);    //  Chunk type

                long chunkEnd = chunkStart + chunkLen;

                if (chunkType == CHUNK_TYPE_LAYER)
                {
                    ushort layerFlags = ReadWord(reader);   //  Layer flags
                    ushort layerType = ReadWord(reader);    //  Layer type
                    IgnoreWord(reader);                     //  Layer child level (don't need)
                    IgnoreWord(reader);                     //  Default layer width (deprecated)
                    IgnoreWord(reader);                     //  Default layer height (deprecated)
                    ushort blend = ReadWord(reader);        //  Blend mode
                    byte layerOpacity = ReadByte(reader);   //  Layer opacity
                    IgnoreBytes(reader, 3);                 //  For future (set to 0)
                    string layerName = ReadString(reader);  //  Layer name

                    if (!layerOpacityValid)
                    {
                        layerOpacity = 255;
                    }

                    bool isVisible = HasFlag(layerFlags, LAYER_FLAG_IS_VISIBLE);
                    bool isBackground = HasFlag(layerFlags, LAYER_FLAG_IS_BACKGROUND);
                    bool isReference = HasFlag(layerFlags, LAYER_FLAG_IS_REFERENCE);
                    BlendMode mode = (BlendMode)blend;

                    AsepriteLayer layer;

                    if (layerType == LAYER_TYPE_NORMAL || layerType == LAYER_TYPE_GROUP)
                    {
                        layer = new AsepriteLayer(isVisible, isBackground, isReference, mode, layerOpacity, layerName);
                    }
                    else if (layerType == LAYER_TYPE_TILEMAP)
                    {
                        uint index = ReadDword(reader); //  Index of tileset used by cels on this layer
                        AsepriteTileset tileset = content.Tilesets[(int)index];
                        layer = new AsepriteTilemapLayer(tileset, isVisible, isBackground, isReference, mode, layerOpacity, layerName);
                    }
                    else
                    {
                        throw new InvalidContentException($"Unknown layer type '{layerType}' in frame {frameNum}");
                    }

                    content.Layers.Add(layer);
                }
                else if (chunkType == CHUNK_TYPE_CEL)
                {
                    ushort layerIndex = ReadWord(reader);   //  Index of layer this cel is on
                    short celXPosition = ReadShort(reader); //  Cel x-position in frame
                    short celYPosition = ReadShort(reader); //  Cel y-position in frame
                    byte celOpacity = ReadByte(reader);     //  Cel opacity
                    ushort celType = ReadWord(reader);      //  Cel type
                    IgnoreBytes(reader, 7);                 //  For future (set to 0)

                    AsepriteLayer celLayer = content.Layers[layerIndex];
                    Point celPosition = new(celXPosition, celYPosition);

                    if (celType == CEL_TYPE_RAW_IMAGE)
                    {
                        ushort celWidth = ReadWord(reader);     //  Cel width, in pixels
                        ushort celHeight = ReadWord(reader);    //  Cel height, in pixels

                        int len = (int)(chunkEnd - reader.BaseStream.Position);

                        byte[] celData = ReadBytes(reader, len);    //  Raw cel image data

                        Size size = new(celWidth, celHeight);
                        Color[] pixels = ToColor(celData);

                        AsepriteImageCel cel = new(size, pixels, celLayer, celPosition, celOpacity);
                        frame.Cels.Add(cel);

                    }
                    else if (celType == CEL_TYPE_LINKED)
                    {
                        ushort frameIndex = ReadWord(reader);   //  Frame index to link with
                        AsepriteCel link = content.Frames[frameIndex].Cels[frame.Cels.Count];
                        frame.Cels.Add(link);
                    }
                    else if (celType == CEL_TYPE_COMPRESSED_IMAGE)
                    {
                        ushort celWidth = ReadWord(reader);     //  Cel width, in pixels
                        ushort celHeight = ReadWord(reader);    //  Cel height, in pixels

                        int len = (int)(chunkEnd - reader.BaseStream.Position);

                        byte[] celData = ReadBytes(reader, len);    //  Cel image data compressed with ZLIB

                        celData = Decompress(celData);

                        Size size = new(celWidth, celHeight);
                        Color[] pixels = ToColor(celData);

                        AsepriteImageCel cel = new(size, pixels, celLayer, celPosition, celOpacity);
                        frame.Cels.Add(cel);
                    }
                    else if (celType == CEL_TYPE_COMPRESSED_TILEMAP)
                    {
                        ushort celWidth = ReadWord(reader);         //  Cel width, in number of tiles
                        ushort celHeight = ReadWord(reader);        //  Cel height, in number of tiles
                        ushort bitsPerTile = ReadWord(reader);      //  Bits per tile (currently always 32)
                        uint idBitmask = ReadDword(reader);         //  Tile ID bitmask (e.g. 0x1FFFFFFF for 32-bit)
                        uint xFlipBitmask = ReadDword(reader);      //  X-flip bitmask
                        uint yFlipBitmask = ReadDword(reader);      //  Y-flip bitmask
                        uint rotationBitmask = ReadDword(reader);   //  90CW rotation bitmask
                        IgnoreBytes(reader, 10);                    //  Reserved

                        int len = (int)(chunkEnd - reader.BaseStream.Position);

                        byte[] celData = ReadBytes(reader, len);    //  Tile data compressed with ZLIB

                        celData = Decompress(celData);

                        Size size = new(celWidth, celHeight);

                        int bytesPerTile = bitsPerTile / 8;
                        int tileCount = celData.Length / bytesPerTile;

                        AsepriteTilemapCel cel = new(size, celLayer, celPosition, celOpacity);

                        for (int i = 0, b = 0; i < tileCount; i++, b += bytesPerTile)
                        {
                            byte[] dword = celData[b..(b + bytesPerTile)];
                            uint value = BitConverter.ToUInt32(dword);
                            uint id = (value & idBitmask) >> TILE_ID_SHIFT;
                            uint xFlip = (value & xFlipBitmask);
                            uint yFlip = (value & yFlipBitmask);
                            uint rotation = (value & rotationBitmask);

                            AsepriteTile tile = new((int)id, (int)xFlip, (int)yFlip, (int)rotation);
                            cel.Tiles.Add(tile);
                        }

                        Debug.Assert(cel.Tiles.Count == tileCount);

                        frame.Cels.Add(cel);
                    }
                    else
                    {
                        throw new InvalidContentException($"Invalid cel type '{celType}' in frame {frameNum}");
                    }
                }
                else if (chunkType == CHUNK_TYPE_TAGS)
                {
                    ushort tagCount = ReadWord(reader); //  Total number of tags
                    IgnoreBytes(reader, 8);             //  For future (set to 0)

                    for (int tagNum = 0; tagNum < tagCount; tagNum++)
                    {
                        ushort from = ReadWord(reader);         //  From frame
                        ushort to = ReadWord(reader);           //  To frame
                        byte direction = ReadByte(reader);      //  Animation loop direction
                        IgnoreBytes(reader, 8);                 //  For future (set to 0)
                        byte tagColorR = ReadByte(reader);      //  RGB Red value (deprecated)
                        byte tagColorG = ReadByte(reader);      //  RGB Green value (deprecated)
                        byte tagColorB = ReadByte(reader);      //  RGB Blue value (deprecated)
                        IgnoreByte(reader);                     //  Extra byte
                        string tagName = ReadString(reader);    //  Tag name

                        LoopDirection loopDirection = (LoopDirection)direction;

                        Color tagColor = Color.FromNonPremultiplied(tagColorR, tagColorG, tagColorB, 255);

                        AsepriteTag tag = new(from, to, loopDirection, tagColor, tagName);
                        content.Tags.Add(tag);
                    }
                }


            }
        }
    }

    private byte ReadByte(BinaryReader reader) => reader.ReadByte();
    private void IgnoreByte(BinaryReader reader) => reader.BaseStream.Position += 1;
    private byte[] ReadBytes(BinaryReader reader, int len) => reader.ReadBytes(len);
    private void IgnoreBytes(BinaryReader reader, int len) => reader.BaseStream.Position += len;
    private ushort ReadWord(BinaryReader reader) => reader.ReadUInt16();
    private void IgnoreWord(BinaryReader reader) => reader.BaseStream.Position += 2;
    private short ReadShort(BinaryReader reader) => reader.ReadInt16();
    private void IgnoreShort(BinaryReader reader) => reader.BaseStream.Position += 2;
    private uint ReadDword(BinaryReader reader) => reader.ReadUInt32();
    private void IgnoreDword(BinaryReader reader) => reader.BaseStream.Position += 4;
    private int ReadLong(BinaryReader reader) => reader.ReadInt32();
    private void IgnoreLong(BinaryReader reader) => reader.BaseStream.Position += 4;
    private string ReadString(BinaryReader reader) => Encoding.UTF8.GetString(ReadBytes(reader, ReadWord(reader)));
    private void IgnoreString(BinaryReader reader) => IgnoreBytes(reader, ReadWord(reader));
}
