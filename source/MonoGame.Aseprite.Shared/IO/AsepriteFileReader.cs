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
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.IO;

internal sealed class AsepriteFileReader : IDisposable
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
    private const ushort CHUNK_TYPE_TILESET = 0x2023;           //  Tileset Chunk TypeF


    //  File header values
    private string _name;               //  The name of the Aseprite file without extension
    private int _nFrames;               //  The total number of frames
    private int _width;                 //  The width, in pixels, of canvas/frame
    private int _height;                //  The height, in pixels, of canvas/frame
    private int _depth;                 //  Color depth ("bits"-per-pixel)
    private bool _layerOpacityValid;    //  Is layer opacity valid value?
    private int _transparentIndex;      //  Index of transparent color in palette for Indexed mode
    private int _nColors;               //  Total number of colors

    //  Tracking values/objects/collections
    private int _tagIterator;                               //  Tag iterator when reading tag user data
    private ushort _lastUserDataChunkType;                  //  Last chunk type read that may contain user data after
    private List<AsepriteCel> _currentFrameCels = new();    //  Cels that have been read for the current frame being read


    //  AsepriteFile being built parts
    private Color[] _palette = Array.Empty<Color>();    //  Color palette result
    private List<AsepriteFrame> _frames = new();        //  Collection of all frames that have been read
    private List<AsepriteLayer> _layers = new();        //  Collection of all layers that have been read
    private List<AsepriteTag> _tags = new();            //  Collection of all tags that have been read
    private List<AsepriteSlice> _slices = new();        //  Collection of all slices that have been read
    private List<AsepriteTileset> _tilesets = new();    //  Collection of all tilesets that have been read

    private BinaryReader _reader;
    private bool _isDisposed;

    internal AsepriteFileReader(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"No file exists at '{path}'");
        }

        _name = Path.GetFileNameWithoutExtension(path);

        Stream stream = File.OpenRead(path);
        _reader = new(stream, Encoding.UTF8, leaveOpen: false);
    }

    ~AsepriteFileReader() => Dispose(false);

    internal AsepriteFile ReadFile()
    {
        //  After reading the file header, several field values are initialized
        //  that are used throughout reading the rest of the file.
        ReadFileHeader();

        //  Read frame-by-frame until all frames are read
        for (int i = 0; i < _nFrames; i++)
        {
            ReadFrame();
        }

        Point size = new(_width, _height);
        AsepritePalette palette = new(_transparentIndex, _palette.ToImmutableArray());
        // AsepritePalette palette = new(_palette, _transparentIndex);

        AsepriteFile file = new(_name, size, palette, _frames, _layers, _tags, _slices, _tilesets);
        return file;
    }

    private void ReadFileHeader()
    {
        const int HEADER_LEN = 128;
        const ushort HEADER_MAGIC = 0xA5E0;
        const uint HEADER_FLAG_LAYER_OPACITY_VALID = 1;

        IgnoreDword();                              //  File size (don't need)
        ushort magic = ReadWord();                  //  Header magic (0xA5E0)
        _nFrames = ReadWord();                      //  Total number of frames
        _width = ReadWord();                        //  Width, in pixels, of each frame
        _height = ReadWord();                       //  Height, in pixels, of each frame
        _depth = ReadWord();                        //  Color depth (bits per pixel)
        uint flags = ReadDword();                   //  Header flags
        IgnoreWord();                               //  Speed (ms between frames) (deprecated)
        IgnoreDword();                              //  Set to 0
        IgnoreDword();                              //  Set to 0
        _transparentIndex = _reader.ReadByte();     //  Index of transparent color in palette
        IgnoreBytes(3);                             //  Ignore these bytes
        _nColors = ReadWord();                      //  Number of colors
        _reader.BaseStream.Position = HEADER_LEN;   //  Skip remainder of header, don't need

        if (magic != HEADER_MAGIC)
        {
            throw new InvalidOperationException($"Invalid header magic number (0x{magic:X4}).");
        }

        _layerOpacityValid = HasFlag(flags, HEADER_FLAG_LAYER_OPACITY_VALID);

        if (_width < 1 || _height < 1)
        {
            throw new InvalidOperationException($"Invalid canvas size ({_width}x{_height})");
        }

        if (_depth != 32 && _depth != 16 && _depth != 8)
        {
            throw new InvalidOperationException($"invalid color depth '{_depth}'");
        }

        if (_depth != 8)
        {
            //  Transparent index can only be non-zero when color depth is
            //  Indexed (8)
            _transparentIndex = 0;
        }

        //  When number of colors is zero, this means 256
        if (_nColors == 0)
        {
            _nColors = 256;
        }

        _palette = new Color[_nColors];
    }

    private void ReadFrame()
    {
        const ushort FRAME_MAGIC = 0xF1FA;

        uint len = ReadDword();         //  Length of frame, in bytes
        ushort magic = ReadWord();      //  Frame magic (0xF1FA)
        ushort nChunksA = ReadWord();   //  Old field which specifies number of chunks
        ushort duration = ReadWord();   //  Frame duration (ms)
        IgnoreBytes(2);                 //  For future (set to zero)
        uint nChunksB = ReadDword();    //  New field which specifies number of chunks

        if (magic != FRAME_MAGIC)
        {
            throw new InvalidOperationException($"Invalid frame magic number (0x{magic:X4})");
        }

        uint nChunks = nChunksA == 0xFFFF && nChunksA < nChunksB ?
                       nChunksB :
                       nChunksA;



        for (uint i = 0; i < nChunks; i++)
        {
            ReadChunk();
        }

        AsepriteFrame frame = new(new Size(_width, _height), _currentFrameCels.ToImmutableArray(), duration);
        _frames.Add(frame);
        _currentFrameCels.Clear();
    }

    private void ReadChunk()
    {
        long start = _reader.BaseStream.Position;

        uint len = ReadDword();     //  Chunk length, in bytes
        ushort type = ReadWord();   //  Chunk type

        long end = start + len;

        switch (type)
        {
            case CHUNK_TYPE_LAYER:
                ReadLayerChunk();
                _lastUserDataChunkType = type;
                break;
            case CHUNK_TYPE_CEL:
                ReadCelChunk(end);
                _lastUserDataChunkType = type;
                break;
            case CHUNK_TYPE_TAGS:
                ReadTagsChunk();
                _lastUserDataChunkType = type;
                break;
            case CHUNK_TYPE_PALETTE:
                ReadPaletteChunk();
                break;
            case CHUNK_TYPE_SLICE:
                ReadSliceChunk();
                _lastUserDataChunkType = type;
                break;
            case CHUNK_TYPE_TILESET:
                ReadTilesetChunk();
                break;
            case CHUNK_TYPE_USER_DATA:
                ReadUserDataChunk();
                break;
            case CHUNK_TYPE_OLD_PALETTE1:   //  Only exists for backwards compatibility with v1.1
            case CHUNK_TYPE_OLD_PALETTE2:   //  Only exists for backwards compatibility with v1.1
            case CHUNK_TYPE_CEL_EXTRA:      //  Used by Aseprite UI, don't need here
            case CHUNK_TYPE_COLOR_PROFILE:  //  Don't need, probably. Someone may ask for it one day
            case CHUNK_TYPE_EXTERNAL_FILES: //  Not implemented in v1.3 yet
            case CHUNK_TYPE_MASK:           //  Deprecated
            case CHUNK_TYPE_PATH:           //  Never used
                _reader.BaseStream.Position = end;
                break;
            default:
                throw new InvalidOperationException($"Unknown chunk type (0x{type:X4})");
        }
    }

    private void ReadLayerChunk()
    {
        const ushort FLAG_IS_VISIBLE = 1;
        const ushort FLAG_IS_BACKGROUND = 8;
        const ushort FLAG_IS_REFERENCE = 64;
        const ushort TYPE_NORMAL = 0;
        const ushort TYPE_GROUP = 1;
        const ushort TYPE_TILEMAP = 2;

        ushort flags = ReadWord();          //  Layer flags
        ushort type = ReadWord();           //  Layer type
        ushort level = ReadWord();          //  Layer child level
        IgnoreWord();                       //  Default layer width
        IgnoreWord();                       //  Default layer height
        ushort blend = ReadWord();          //  Blend mode
        byte opacity = _reader.ReadByte();  //  Layer opacity
        IgnoreBytes(3);                     //  For future (set to 0)
        string name = ReadString();         //  Layer name

        if (!_layerOpacityValid)
        {
            opacity = 255;
        }

        bool isVisible = HasFlag(flags, FLAG_IS_VISIBLE);
        bool isBackground = HasFlag(flags, FLAG_IS_BACKGROUND);
        bool isReference = HasFlag(flags, FLAG_IS_REFERENCE);
        BlendMode mode = (BlendMode)blend;

        AsepriteLayer layer;

        if (type == TYPE_NORMAL || type == TYPE_GROUP)
        {
            //  Treating group layers as normal layers. No use case for actually
            //  keeping track of group layer and it's children since no there
            //  are no cels or anything on a group layer, nor is there
            //  properties that can be set for group layers in Aseprite.
            layer = new AsepriteImageLayer(isVisible, isBackground, isReference, mode, opacity, name);
        }
        else if (type == TYPE_TILEMAP)
        {
            uint index = ReadDword();   //  Index of tileset used by cels on this layer
            AsepriteTileset tileset = _tilesets[(int)index];
            layer = new AsepriteTilemapLayer(tileset, isVisible, isBackground, isReference, mode, opacity, name);
        }
        else
        {
            throw new InvalidOperationException($"Unknown layer type '{type}'");
        }

        _layers.Add(layer);
    }

    private void ReadCelChunk(long chunkEnd)
    {
        const int TYPE_RAW_IMAGE = 0;
        const int TYPE_LINKED = 1;
        const int TYPE_COMPRESSED_IMAGE = 2;
        const int TYPE_COMPRESSED_TILEMAP = 3;

        ushort index = ReadWord();          //  Index of the layer this cel is on
        short x = ReadShort();              //  Cel x-position relative to frame bounds
        short y = ReadShort();              //  Cel y-position relative to frame bounds
        byte opacity = _reader.ReadByte();  //  Cel opacity
        ushort type = ReadWord();           //  Cel type
        IgnoreBytes(7);                     //  For future (set to zero)

        AsepriteLayer layer = _layers[index];
        Point position = new(x, y);

        AsepriteCel cel = type switch
        {
            TYPE_RAW_IMAGE => ReadRawImageCel(layer, position, opacity, chunkEnd),
            TYPE_LINKED => ReadLinkedCel(),
            TYPE_COMPRESSED_IMAGE => ReadCompressedImageCel(layer, position, opacity, chunkEnd),
            TYPE_COMPRESSED_TILEMAP => ReadCompressedTilemapCel(layer, position, opacity, chunkEnd),
            _ => throw new InvalidOperationException($"Unknown cel type '{type}'")
        };

        _currentFrameCels.Add(cel);
    }

    private AsepriteImageCel ReadRawImageCel(AsepriteLayer layer, Point position, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord();              //  Width of cel, in pixels
        ushort height = ReadWord();             //  Height of cel, in pixels

        int len = (int)(chunkEnd - _reader.BaseStream.Position);
        byte[] data = _reader.ReadBytes(len);   //  Raw Image Data


        Size size = new(width, height);
        Color[] pixels = ToColor(data);

        return new(size, pixels.ToImmutableArray(), layer, position, opacity);
    }

    private AsepriteCel ReadLinkedCel()
    {
        ushort frameIndex = ReadWord(); //  Frame position to link with

        return _frames[frameIndex].Cels[_currentFrameCels.Count];
    }

    private AsepriteImageCel ReadCompressedImageCel(AsepriteLayer layer, Point position, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord();  //  Width of cel, in pixels
        ushort height = ReadWord(); //  Height of cel, in pixels

        int len = (int)(chunkEnd - _reader.BaseStream.Position);
        byte[] data = _reader.ReadBytes(len);   //  Raw image data compressed with ZLIB

        data = Decompress(data);

        Size size = new(width, height);
        Color[] pixels = ToColor(data);

        return new(size, pixels.ToImmutableArray(), layer, position, opacity);
    }

    private AsepriteTilemapCel ReadCompressedTilemapCel(AsepriteLayer layer, Point position, byte opacity, long chunkEnd)
    {
        const int TILE_ID_SHIFT = 0;

        ushort width = ReadWord();              //  Width of cel, in number of tiles
        ushort height = ReadWord();             //  Height of cel, in number of tiles
        ushort bitsPerTile = ReadWord();        //  Bits per tile (currently always 32)
        uint idBitmask = ReadDword();           //  Tile ID bitmask (e.g. 0x1FFFFFFF for 32-bit)
        uint xFlipBitmask = ReadDword();        //  X-Flip bitmask
        uint yFlipBitmask = ReadDword();        //  Y-Flip bitmask
        uint rotationBitmask = ReadDword();     //  90CW rotation bitmask
        IgnoreBytes(10);                        //  Reserved

        int len = (int)(chunkEnd - _reader.BaseStream.Position);
        byte[] data = _reader.ReadBytes(len);   //  Tile data compressed with ZLIB

        data = Decompress(data);

        Size size = new(width, height);

        int bytesPerTile = bitsPerTile / 8;
        int tileCount = data.Length / bytesPerTile;
        AsepriteTile[] tiles = new AsepriteTile[tileCount];


        for (int i = 0, b = 0; i < tileCount; i++, b += bytesPerTile)
        {
            byte[] dword = data[b..(b + bytesPerTile)];
            uint value = BitConverter.ToUInt32(dword);
            uint id = (value & idBitmask) >> TILE_ID_SHIFT;
            uint xFlip = (value & xFlipBitmask);
            uint yFlip = (value & yFlipBitmask);
            uint rotation = (value & rotationBitmask);

            tiles[i] = new((int)id, (int)xFlip, (int)yFlip, (int)rotation);
        }

        AsepriteTilemapCel cel = new(size, tiles.ToImmutableArray(), layer, position, opacity);
        return cel;
    }

    private void ReadTagsChunk()
    {
        ushort count = ReadWord();  //  Total number of tags
        IgnoreBytes(8);             //  For future (set to zero)

        for (int i = 0; i < count; i++)
        {
            ushort from = ReadWord();               //  From frame
            ushort to = ReadWord();                 //  To frame
            byte direction = _reader.ReadByte();    //  Loop animation direction
            IgnoreBytes(8);                         //  For future (set to zero)
            byte r = _reader.ReadByte();            //  RGB Red value (deprecated in 1.3)
            byte g = _reader.ReadByte();            //  RGB Green value (deprecated in 1.3)
            byte b = _reader.ReadByte();            //  RGB Blue value (deprecated in 1.3)
            IgnoreByte();                           //  Extra byte (zero)
            string name = ReadString();             //  Tag name

            LoopDirection loopDirection = (LoopDirection)direction;

            Color color = Color.FromNonPremultiplied(r, g, b, 255);

            AsepriteTag tag = new(from, to, loopDirection, color, name);
            _tags.Add(tag);
        }
    }

    private void ReadPaletteChunk()
    {
        const int FLAG_HAS_NAME = 1;

        uint nSize = ReadDword();   //  New palette size (total number of entries)
        uint from = ReadDword();    //  First color index to change
        uint to = ReadDword();      //  Last color index to change
        IgnoreBytes(8);             //  For future (set to zero)

        //  Resize current palette if needed
        if (nSize > 0)
        {
            Color[] tmp = new Color[nSize];
            Array.Copy(_palette, tmp, _palette.Length);
            _palette = tmp;
        }

        for (uint i = from; i <= to; i++)
        {
            ushort flags = ReadWord();      //  Entry flags
            byte r = _reader.ReadByte();    //  RGBA Red value (0 - 255)
            byte g = _reader.ReadByte();    //  RGBA Green value (0 - 255)
            byte b = _reader.ReadByte();    //  RGBA Blue value (0 - 255)
            byte a = _reader.ReadByte();    //  RGBA Alpha value (0 - 255)

            if (HasFlag(flags, FLAG_HAS_NAME))
            {
                IgnoreString();         //  Color name (ignored)
            }

            _palette[(int)i] = Color.FromNonPremultiplied(r, g, b, a);
        }
    }

    private void ReadSliceChunk()
    {
        const int FLAG_IS_NINE_PATCH = 1;
        const int FLAG_HAS_PIVOT = 2;

        uint count = ReadDword();   //  Number of slice "keys"
        uint flags = ReadDword();   //  Slice flags
        IgnoreDword();              //  Reserved
        string name = ReadString(); //  Slice name

        bool isNinePatch = HasFlag(flags, FLAG_IS_NINE_PATCH);
        bool hasPivot = HasFlag(flags, FLAG_HAS_PIVOT);

        AsepriteSliceKey[] keys = new AsepriteSliceKey[count];


        for (uint i = 0; i < count; i++)
        {
            uint start = ReadDword();       //  Frame index this key is valid for starting on
            int x = ReadLong();             //  Slice X origin
            int y = ReadLong();             //  Slice Y origin
            uint width = ReadDword();       //  Slice width, in pixels
            uint height = ReadDword();      //  Slice height, in pixels

            Rectangle bounds = new(x, y, (int)width, (int)height);
            Rectangle? center = default;
            Point? pivot = default;

            if (isNinePatch)
            {
                int cx = ReadLong();        //  Center X position
                int cy = ReadLong();        //  Center Y position
                uint cWidth = ReadDword();  //  Center width, in pixels
                uint cHeight = ReadDword(); //  Center height, in pixels

                center = new(cx, cy, (int)cWidth, (int)cHeight);
            }

            if (hasPivot)
            {
                int px = ReadLong();        //  Pivot x position
                int py = ReadLong();        //  Pivot y position

                pivot = new(px, py);
            }

            AsepriteSliceKey key = new((int)start, bounds, center, pivot);
            keys[i] = key;
        }

        AsepriteSlice slice = new(isNinePatch, hasPivot, name, keys.ToImmutableArray());
        _slices.Add(slice);
    }

    private void ReadTilesetChunk()
    {
        const int FLAG_EXTERNAL_FILE = 1;
        const int FLAG_EMBEDDED = 2;

        uint id = ReadDword();      //  Tileset ID
        uint flags = ReadDword();   //  Tileset flags
        uint count = ReadDword();   //  Number of tiles
        ushort width = ReadWord();  //  Tile width
        ushort height = ReadWord(); //  Tile height
        IgnoreShort();              //  Base index (ignored, UI only)
        IgnoreBytes(14);            //  Reserved
        string name = ReadString(); //  Tileset name

        if (HasFlag(flags, FLAG_EXTERNAL_FILE))
        {
            throw new InvalidOperationException($"Tileset '{name}' includes tileset in external file.  This is not supported at this time.");
        }

        if (!HasFlag(flags, FLAG_EMBEDDED))
        {
            throw new InvalidOperationException($"Tileset '{name}' does not include tileset image in file");
        }

        uint len = ReadDword();                     //  Compressed data length
        byte[] data = _reader.ReadBytes((int)len);  //  Tileset image compressed with ZLIB

        data = Decompress(data);

        Color[] pixels = ToColor(data);
        Size tileSize = new(width, height);
        Size size = new(width, (int)(height * count));

        AsepriteTileset tileset = new((int)id, (int)count, tileSize, size, name, pixels.ToImmutableArray());
        _tilesets.Add(tileset);

    }

    private void ReadUserDataChunk()
    {
        const int FLAG_HAS_TEXT = 1;
        const int FLAG_HAS_COLOR = 2;

        uint flags = ReadDword();   //  User data flags

        string? text = default;
        Color? color = default;

        if (HasFlag(flags, FLAG_HAS_TEXT))
        {
            text = ReadString();    //  User data text
        }

        if (HasFlag(flags, FLAG_HAS_COLOR))
        {
            byte r = _reader.ReadByte();    //  User data RGBA Red value (0 - 255)
            byte g = _reader.ReadByte();    //  User data RGBA Green value (0 - 255)
            byte b = _reader.ReadByte();    //  User data RGBA Blue value (0 - 255)
            byte a = _reader.ReadByte();    //  User data RGBA Alpha value (0 - 255)

            color = Color.FromNonPremultiplied(r, g, b, a);
        }

        switch (_lastUserDataChunkType)
        {
            case CHUNK_TYPE_CEL:
                SetLastCelUserData(text, color);
                break;
            case CHUNK_TYPE_LAYER:
                SetLastLayerUserData(text, color);
                break;
            case CHUNK_TYPE_SLICE:
                SetLastSliceUserData(text, color);
                break;
            case CHUNK_TYPE_TAGS:
                SetNextTagUserData(text, color);
                break;
            default:
                throw new InvalidOperationException($"Invalid chunk type (0x{_lastUserDataChunkType:X4}) for user data.");
        }
    }

    private void SetLastCelUserData(string? text, Color? color)
    {
        int index = _currentFrameCels.Count - 1;
        AsepriteCel cel = _currentFrameCels[index];
        _currentFrameCels[index] = cel with { UserData = new(text, color) };
    }

    private void SetLastLayerUserData(string? text, Color? color)
    {
        int index = _layers.Count - 1;
        AsepriteLayer layer = _layers[index];
        _layers[index] = layer with { UserData = new(text, color) };
    }

    private void SetLastSliceUserData(string? text, Color? color)
    {
        int index = _slices.Count - 1;
        AsepriteSlice slice = _slices[index];
        _slices[index] = slice with { UserData = new(text, color) };
    }

    private void SetNextTagUserData(string? text, Color? color)
    {
        //  Tags are a special case, user data for tags comes all together
        //  (one next to the other) after the tags chunk, in the same order:
        //
        //  * TAGS CHUNK (TAG1, TAG2, ..., TAGn)
        //  * USER DATA CHUNK FOR TAG1
        //  * USER DATA CHUNK FOR TAG2
        //  * ...
        //  * USER DATA CHUNK FOR TAGn
        //
        //  So here we expect that the next user data chunk will correspond to
        //  the next tag in the tags collection
        AsepriteTag tag = _tags[_tagIterator];
        _tags[_tagIterator] = tag with { UserData = new(text, color) };
        _tagIterator++;
    }



    private void IgnoreByte() => _reader.BaseStream.Position += 1;
    private void IgnoreBytes(int nBytes) => _reader.BaseStream.Position += nBytes;

    private ushort ReadWord() => _reader.ReadUInt16();
    private void IgnoreWord() => _reader.BaseStream.Position += 2;

    private short ReadShort() => _reader.ReadInt16();
    private void IgnoreShort() => _reader.BaseStream.Position += 2;

    private uint ReadDword() => _reader.ReadUInt32();
    private void IgnoreDword() => _reader.BaseStream.Position += 4;

    private int ReadLong() => _reader.ReadInt32();
    private void IgnoreLong() => _reader.BaseStream.Position += 4;

    private string ReadString()
    {
        int len = ReadWord();
        byte[] bytes = _reader.ReadBytes(len);
        return Encoding.UTF8.GetString(bytes);
    }

    private void IgnoreString()
    {
        int len = ReadWord();
        _reader.BaseStream.Position += len;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

    private byte[] Decompress(byte[] buffer)
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

    private Color[] ToColor(byte[] data)
    {
        const int COLOR_DEPTH_RGBA = 32;
        const int COLOR_DEPTH_GRAYSCALE = 16;
        const int COLOR_DEPTH_INDEXED = 8;
        int bytesPerPixel = (ushort)_depth / 8;
        int len = data.Length / bytesPerPixel;

        Color[] result = new Color[len];

        if (_depth == COLOR_DEPTH_INDEXED)
        {
            for (int i = 0; i < result.Length; i++)
            {
                int index = data[i];

                if (index == _transparentIndex)
                {
                    // result[i] = Color.Transparent;
                    result[i] = new Color(0, 0, 0, 0);
                }
                else
                {
                    result[i] = _palette[index];
                }
            }
        }
        else if (_depth == COLOR_DEPTH_GRAYSCALE)
        {
            for (int i = 0, b = 0; i < result.Length; i++, b += bytesPerPixel)
            {
                byte rgb = data[b];
                byte alpha = data[b + 1];

                result[i] = Color.FromNonPremultiplied(rgb, rgb, rgb, alpha);

            }
        }
        else if (_depth == COLOR_DEPTH_RGBA)
        {
            for (int i = 0, b = 0; i < result.Length; i++, b += bytesPerPixel)
            {
                byte red = data[b];
                byte green = data[b + 1];
                byte blue = data[b + 2];
                byte alpha = data[b + 3];

                result[i] = Color.FromNonPremultiplied(red, green, blue, alpha);

            }
        }
        else
        {
            throw new InvalidOperationException($"Invalid Color Depth '{_depth}'");
        }

        return result;
    }

    /// <summary>
    ///     Releases resources held by this instance of the
    ///     <see cref="AsepriteFileReader"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool isDisposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            _reader.Dispose();
        }

        _isDisposed = true;
    }
}
