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

namespace MonoGame.Aseprite;

internal sealed class AsepriteFileReader
{
    private enum ChunkTypes : ushort
    {
        OldPalette1 = 0x0004,
        OldPalette2 = 0x0011,
        Layer = 0x2004,
        Cel = 0x2005,
        CelExtra = 0x2006,
        ColorProfile = 0x2007,
        ExternalFiles = 0x2008,
        Mask = 0x2016,
        Path = 0x2017,
        Tags = 0x2018,
        Palette = 0x2019,
        UserData = 0x2020,
        Slice = 0x2022,
        Tileset = 0x2023
    }

    private Stream _stream;
    private BinaryReader _reader;
    private bool _isDisposed;

    private int _nFrames;                           //  Total number of frames to read
    private int _width;                             //  Width, in pixels, of each frame
    private int _height;                            //  Height, in pixels, of each frame
    private int _depth;                             //  Color depth (bits per pixels)
    private bool _layerOpacityValid;                //  Is layer opacity valid?
    private int _transparentIndex;                  //  Index of transparent color in palette
    private int _nColors;                           //  Total number of colors

    private int _tagIterator;                       //  Iterator used when reading user data for tags
    private ushort _lastUserDataChunkType;          //  The last chunk type that was read that can contain user data
    private AsepriteGroupLayer? _lastGroupLayer;    //  The last group layer that was read in

    private Color[] _palette;                      //  The palette that is read in
    private List<AsepriteFrame> _frames;            //  Frames that have been read in
    private List<AsepriteLayer> _layers;            //  Layers that have been read in
    private List<AsepriteTag> _tags;                //  Tags that have been read in
    private List<AsepriteSlice> _slices;            //  Slices that have been read in
    private List<AsepriteTileset> _tilesets;        //  Tilesets that have been read in

    internal AsepriteFileReader(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"No file exists at '{path}'");
        }

        _stream = File.OpenRead(path);
        _reader = new BinaryReader(_stream);

        _lastGroupLayer = default;

        _tagIterator = 0;
        _palette = Array.Empty<Color>();
        _frames = new();
        _layers = new();
        _tags = new();
        _slices = new();
        _tilesets = new();
    }

    ~AsepriteFileReader() => Dispose(false);

    internal void ReadFile()

}
