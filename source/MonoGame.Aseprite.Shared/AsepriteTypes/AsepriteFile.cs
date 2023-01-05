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
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.IO;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     A read-only representation of an Aseprite file that was read.
/// </summary>
public sealed class AsepriteFile
{
    private List<AsepriteFrame> _frames;
    private List<AsepriteLayer> _layers;
    private List<AsepriteTag> _tags;
    private List<AsepriteSlice> _slices;
    private List<AsepriteTileset> _tilesets;

    /// <summary>
    ///     The width and height extents, in pixels of each frame in this
    ///     Aseprite file.
    /// </summary>
    public Point FrameSize { get; }

    /// <summary>
    ///     THe width, in pixels of each frame in this Aseprite file.
    /// </summary>
    public int FrameWidth => FrameSize.X;

    /// <summary>
    ///     The height, in pixels, of each frame in this Aseprite file.
    /// </summary>
    public int FrameHeight => FrameSize.Y;

    /// <summary>
    ///     The collection of color values that were used by this Aseprite file.
    /// </summary>
    public AsepritePalette Palette { get; }

    /// <summary>
    ///     A read-only collection of all frames in this Aseprite file.
    /// </summary>
    public ReadOnlyCollection<AsepriteFrame> Frames => _frames.AsReadOnly();

    /// <summary>
    ///     A read-only collection of all layers in this Aseprite file.
    /// </summary>
    public ReadOnlyCollection<AsepriteLayer> Layers => _layers.AsReadOnly();

    /// <summary>
    ///     A read-only collection of all tags in this Aseprite file.
    /// </summary>
    public ReadOnlyCollection<AsepriteTag> Tags => _tags.AsReadOnly();

    /// <summary>
    ///     A read-only collection of all slices in this Aseprite file.
    /// </summary>
    public ReadOnlyCollection<AsepriteSlice> Slices => _slices.AsReadOnly();

    /// <summary>
    ///     A read-only collection of all tilesets in this Aseprite file.
    /// </summary>
    public ReadOnlyCollection<AsepriteTileset> Tilesets => _tilesets.AsReadOnly();

    internal AsepriteFile(Point frameSize, AsepritePalette palette, List<AsepriteFrame> frames, List<AsepriteLayer> layers, List<AsepriteTag> tags, List<AsepriteSlice> slices, List<AsepriteTileset> tilesets)
    {
        FrameSize = frameSize;
        Palette = palette;
        _frames = frames;
        _layers = layers;
        _tags = tags;
        _slices = slices;
        _tilesets = tilesets;
    }

    public static AsepriteFile Load(string path)
    {
        if(!File.Exists(path))
        {
            throw new FileNotFoundException($"No file exists at the path '{path}'");
        }

        using(AsepriteFileReader reader = new(path))
        {
            return reader.ReadFile();
        }
    }
}
