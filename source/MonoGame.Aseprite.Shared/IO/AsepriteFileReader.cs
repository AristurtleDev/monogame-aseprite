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

    private bool _isDisposed;

    private int _nFrames;
    private int _width;
    private int _height;
    private int _depth;
    private bool _layerOpacityValid;
    private int _transparentIndex;
    private int _nColors;

    private int _tagIterator;
    private ushort _lastUserDataChunkType;
    private AsepriteGroupLayer? _lastGroupLayer = default;

    private Color[] _palette = Array.Empty<Color>();
    private List<AsepriteFrame> _frames = new();
    private List<AsepriteLayer> _layers = new();
    private List<AsepriteTag> _tags = new();
    private List<AsepriteSlice> _slices = new();
    private List<AsepriteTileset> _tilesets = new();

    private BinaryReader _reader;

    internal AsepriteFileReader(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"No file exists at '{path}'");
        }

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
        AsepritePalette palette = new(_palette, _transparentIndex);

        AsepriteFile file = new(size, palette, _frames, _layers, _tags, _slices, _tilesets);
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

        AsepriteFrame frame = new(new Point(_width, _height), duration);
        _frames.Add(frame);

        for (uint i = 0; i < nChunks; i++)
        {
            ReadChunk();
        }
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

        if (type == TYPE_NORMAL)
        {
            layer = new AsepriteImageLayer(isVisible, isBackground, isReference, mode, opacity, name);
        }
        else if (type == TYPE_GROUP)
        {
            layer = new AsepriteGroupLayer(isVisible, isBackground, isReference, mode, opacity, name);
        }
        else if (type == TYPE_TILEMAP)
        {
            uint index = ReadDword();   //  Index of tileset used by cels on this layer
            AsepriteTileset tileset = _tilesets[(int)index];
            layer = new AsepriteTilemapLayer(isVisible, isBackground, isReference, mode, opacity, name, tileset);
        }
        else
        {
            throw new InvalidOperationException($"Unknown layer type '{type}'");
        }

        if (level != 0 && _lastGroupLayer is not null)
        {
            _lastGroupLayer.AddChild(layer);
        }

        if (layer is AsepriteGroupLayer gLayer)
        {
            _lastGroupLayer = gLayer;
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

        AsepriteFrame frame = _frames[_frames.Count - 1];
        AsepriteLayer layer = _layers[index];
        Point position = new(x, y);

        AsepriteCel cel = type switch
        {
            TYPE_RAW_IMAGE => ReadRawImageCel(layer, position, opacity, chunkEnd),
            TYPE_LINKED => ReadLinkedCel(frame),
            TYPE_COMPRESSED_IMAGE => ReadCompressedImageCel(layer, position, opacity, chunkEnd),
            TYPE_COMPRESSED_TILEMAP => ReadCompressedTilemapCel(layer, position, opacity, chunkEnd),
            _ => throw new InvalidOperationException($"Unknown cel type '{type}'")
        };

        frame.AddCel(cel);
    }

    private AsepriteImageCel ReadRawImageCel(AsepriteLayer layer, Point position, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord();              //  Width of cel, in pixels
        ushort height = ReadWord();             //  Height of cel, in pixels

        int len = (int)(chunkEnd - _reader.BaseStream.Position);
        byte[] data = _reader.ReadBytes(len);   //  Raw Image Data


        Point size = new(width, height);
        Color[] pixels = ToColor(data);

        return new(size, pixels, layer, position, opacity);
    }

    private AsepriteCel ReadLinkedCel(AsepriteFrame frame)
    {
        ushort frameIndex = ReadWord(); //  Frame position to link with

        return _frames[frameIndex].Cels[frame.Cels.Count];
    }

    private AsepriteImageCel ReadCompressedImageCel(AsepriteLayer layer, Point position, byte opacity, long chunkEnd)
    {
        ushort width = ReadWord();  //  Width of cel, in pixels
        ushort height = ReadWord(); //  Height of cel, in pixels

        int len = (int)(chunkEnd - _reader.BaseStream.Position);
        byte[] data = _reader.ReadBytes(len);   //  Raw image data compressed with ZLIB

        data = Decompress(data);

        Point size = new(width, height);
        Color[] pixels = ToColor(data);

        return new(size, pixels, layer, position, opacity);
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

        Point size = new(width, height);

        int bytesPerTile = bitsPerTile / 8;
        int tileCount = data.Length / bytesPerTile;
        List<AsepriteTile> tiles = new(tileCount);

        AsepriteTilemapCel cel = new(size, layer, position, opacity);

        for (int i = 0, b = 0; i < tileCount; i++, b += bytesPerTile)
        {
            byte[] dword = data[b..(b + bytesPerTile)];
            uint value = BitConverter.ToUInt32(dword);
            uint id = (value & idBitmask) >> TILE_ID_SHIFT;
            uint xFlip = (value & xFlipBitmask);
            uint yFlip = (value & yFlipBitmask);
            uint rotation = (value & rotationBitmask);

            AsepriteTile tile = new((int)id, (int)xFlip, (int)yFlip, (int)rotation);
            cel.AddTile(tile);
        }

        Debug.Assert(cel.TileCount == tileCount);

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

        List<AsepriteSliceKey> keys = new();

        AsepriteSlice slice = new(name, isNinePatch, hasPivot);

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
            slice.AddKey(key);
        }

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
        Point size = new(width, height);

        AsepriteTileset tileset = new((int)id, (int)count, size, name, pixels);
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

        AsepriteUserData userData;

        switch (_lastUserDataChunkType)
        {
            case CHUNK_TYPE_CEL:
                AsepriteFrame frame = _frames[_frames.Count - 1];
                AsepriteCel cel = frame.Cels[frame.Cels.Count - 1];
                userData = cel.UserData;
                break;
            case CHUNK_TYPE_LAYER:
                AsepriteLayer layer = _layers[_layers.Count - 1];
                userData = layer.UserData;
                break;
            case CHUNK_TYPE_SLICE:
                AsepriteSlice slice = _slices[_slices.Count - 1];
                userData = slice.UserData;
                break;
            case CHUNK_TYPE_TAGS:
                //  Tags are a special case, user data for tags comes all
                //  together (one next to the other) after the tags chunk,
                //  in the same order:
                //
                //  * TAGS CHUNK (TAG1, TAG2, ..., TAGn)
                //  * USER DATA CHUNK FOR TAG1
                //  * USER DATA CHUNK FOR TAG2
                //  * ...
                //  * USER DATA CHUNK FOR TAGn
                //
                //  So here we expect that the next user data chunk will
                //  correspond to the next tag in the tags collection
                AsepriteTag tag = _tags[_tagIterator];
                _tagIterator++;
                userData = tag.UserData;
                break;
            default:
                throw new InvalidOperationException($"Invalid chunk type (0x{_lastUserDataChunkType:X4}) for user data.");
        }

        userData.Text = text;
        userData.Color = color;
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
                    result[i] = Color.Transparent;
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
