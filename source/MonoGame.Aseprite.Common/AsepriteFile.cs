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
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.IO;

namespace MonoGame.Aseprite;

/// <summary>
/// Defines the contents of an imported aseprite file.
/// </summary>
public sealed class AsepriteFile
{
    private Color[] _palette = Array.Empty<Color>();
    private AsepriteFrame[] _frames;
    private AsepriteLayer[] _layers;
    private AsepriteTag[] _tags;
    private AsepriteSlice[] _slices;
    private AsepriteTileset[] _tilesets;

    /// <summary>
    /// Gets a read-only span of all frames in this aseprite file.
    /// </summary>
    public ReadOnlySpan<AsepriteFrame> Frames => _frames;

    /// <summary>
    /// Gets the total number of frames in this aseprite file.
    /// </summary>
    public int FrameCount => _frames.Length;

    /// <summary>
    /// Gets a read-only span of all layers in this aseprite file.  Order of layer elements if from bottom-to-top.
    /// </summary>
    public ReadOnlySpan<AsepriteLayer> Layers => _layers;

    /// <summary>
    /// Gets the total number of layers in this aseprite file.
    /// </summary>
    public int LayerCount => _layers.Length;

    /// <summary>
    /// Gets a read-only span of all tags in this aseprite file.
    /// </summary>
    public ReadOnlySpan<AsepriteTag> Tags => _tags;

    /// <summary>
    /// Gets the total number of tags in this aseprite file.
    /// </summary>
    public int TagCount => _tags.Length;

    /// <summary>
    /// Gets a read-only span of all slices in this aseprite file.
    /// </summary>
    public ReadOnlySpan<AsepriteSlice> Slices => _slices;

    /// <summary>
    /// Gets the total number of slices in this aseprite file.
    /// </summary>
    public int SliceCount => _slices.Length;

    /// <summary>
    /// Gets a read-only span of all tilesets in this aseprite file.
    /// </summary>
    public ReadOnlySpan<AsepriteTileset> Tilesets => _tilesets;

    /// <summary>
    /// Gets the total number of tilesets in this aseprite file.
    /// </summary>
    public int TilesetCount => _tilesets.Length;

    /// <summary>
    /// Gets a read-only span of the color values that represent the palette of this aseprite file.
    /// </summary>
    public ReadOnlySpan<Color> Palette => _palette;

    /// <summary>
    /// Gets the total number of color values in this palette of this aseprite file.
    /// </summary>
    public int PaletteCount => _palette.Length;

    /// <summary>
    /// Gets the name of this aseprite file.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the width, in pixels, of the canvas.
    /// </summary>
    public int CanvasWidth { get; }

    /// <summary>
    /// Gets the height, in pixels, of the canvas.
    /// </summary>
    public int CanvasHeight { get; }

    /// <summary>
    /// Gets the custom user data that was set for the sprite element in aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; }

    internal AsepriteFile(string name, int width, int height, Color[] palette, AsepriteFrame[] frames, AsepriteLayer[] layers, AsepriteTag[] tags, AsepriteSlice[] slices, AsepriteTileset[] tilesets, AsepriteUserData userData)
    {
        Name = name;
        CanvasWidth = width;
        CanvasHeight = height;
        _palette = palette;
        _frames = frames;
        _layers = layers;
        _tags = tags;
        _slices = slices;
        _tilesets = tilesets;
        UserData = userData;
    }
    /// <summary>
    /// Gets the frame at the specified index in this aseprite file.
    /// </summary>
    /// <param name="frameIndex">The index of the frame to locate.</param>
    /// <returns>The frame located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the frame index specified is less than zero or is greater than or equal to the total number of frames
    /// in this aseprite file.
    /// </exception>
    public AsepriteFrame GetFrame(int frameIndex)
    {
        if (frameIndex < 0 || frameIndex >= _frames.Length)
        {
            ArgumentOutOfRangeException ex = new(nameof(frameIndex), $"{nameof(frameIndex)} cannot be less than zero or greater than or equal to the total number of frames in this aseprite file.");
            ex.Data.Add("TotalFrames", _frames.Length);
            throw ex;
        }

        return _frames[frameIndex];
    }

    /// <summary>
    /// Gets the frame at the specified index from this aseprite file.
    /// </summary>
    /// <param name="frameIndex">The index of the frame to locate</param>
    /// <param name="located">When this method returns true, contains the frame located; otherwise, false.</param>
    /// <returns>
    /// true if the frame was located; otherwise, false.  This method returns false if this frame index specified is
    /// less than zero or is greater than or equal to the total number of frames in this aseprite file.
    /// </returns>
    public bool TryGetFrame(int frameIndex, [NotNullWhen(true)] out AsepriteFrame? located)
    {
        located = default;

        if (frameIndex < 0 || frameIndex >= _frames.Length)
        {
            return false;
        }

        located = _frames[frameIndex];
        return located is not null;
    }

    /// <summary>
    /// Gets the tag at the specified index in this aseprite file.
    /// </summary>
    /// <param name="tagIndex">The index of the tag to locate.</param>
    /// <returns>The tag located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the tag index specified is less than zero or is greater than or equal to the total number of tags in
    /// this aseprite file.
    /// </exception>
    public AsepriteTag GetTag(int tagIndex)
    {
        if (tagIndex < 0 || tagIndex >= _tags.Length)
        {
            ArgumentOutOfRangeException ex = new(nameof(tagIndex), $"{nameof(tagIndex)} cannot be less than zero or greater than or equal to the total number of tags in this aseprite file.");
            ex.Data.Add("TotalTags", _tags.Length);
            throw ex;
        }

        return _tags[tagIndex];
    }

    /// <summary>
    /// Gets the tag with the specified name from this aseprite file.
    /// </summary>
    /// <param name="tagName">The name of the tag to locate.</param>
    /// <returns>The tag located.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this aseprite file does not contain a tag with the specified name.
    /// </exception>
    public AsepriteTag GetTag(string tagName)
    {
        List<string> tagsFound = new();

        for (int i = 0; i < _tags.Length; i++)
        {
            AsepriteTag aseTag = _tags[i];
            tagsFound.Add($"'{aseTag.Name}'");

            if (aseTag.Name == tagName)
            {
                return aseTag;
            }
        }

        InvalidOperationException ex = new($"No tag found with the name '{tagName}' in this aseprite file.");
        ex.Data.Add(nameof(tagsFound), tagsFound);
        throw ex;
    }

    /// <summary>
    /// Gets the tag at the specified index from this aseprite file.
    /// </summary>
    /// <param name="tagIndex">The index of the tag to locate</param>
    /// <param name="located">When this method returns true, contains the tag located; otherwise, false.</param>
    /// <returns>
    /// true if the tag was located; otherwise, false.  This method returns false if this tag index specified is less
    /// than zero or is greater than or equal to the total number of tags in this aseprite file.
    /// </returns>
    public bool TryGetTag(int tagIndex, [NotNullWhen(true)] out AsepriteTag? located)
    {
        located = default;

        if (tagIndex < 0 || tagIndex >= _tags.Length)
        {
            return false;
        }

        located = _tags[tagIndex];
        return located is not null;
    }

    /// <summary>
    /// Gets the tag with the specified name from this aseprite file.
    /// </summary>
    /// <param name="tagName">The name of the tag to locate.</param>
    /// <param name="located">When this method returns true, contains the tag located; otherwise, false.</param>
    /// <returns>
    /// true if the tag was located; otherwise, false.  This method returns false if this aseprite file does not contain
    /// a  tag with the specified name.
    /// </returns>
    public bool TryGetTag(string tagName, [NotNullWhen(true)] out AsepriteTag? located)
    {
        located = default;

        for (int i = 0; i < _tags.Length; i++)
        {
            if (_tags[i].Name == tagName)
            {
                located = _tags[i];
                break;
            }
        }

        return located is not null;
    }

    /// <summary>
    /// Gets the slice at the specified index in this aseprite file.
    /// </summary>
    /// <param name="sliceIndex">The index of the slice to locate.</param>
    /// <returns>The slice located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the slice index specified is less than zero or is greater than or equal to the total number of slices
    /// in this aseprite file.
    /// </exception>
    public AsepriteSlice GetSlice(int sliceIndex)
    {
        if (sliceIndex < 0 || sliceIndex >= _slices.Length)
        {
            ArgumentOutOfRangeException ex = new(nameof(sliceIndex), $"{nameof(sliceIndex)} cannot be less than zero or greater than or equal to the total number of slices in this aseprite file.");
            ex.Data.Add(nameof(sliceIndex), sliceIndex);
            ex.Data.Add(nameof(SliceCount), SliceCount);
            throw ex;
        }

        return _slices[sliceIndex];
    }

    /// <summary>
    /// Gets the slice with the specified name from this aseprite file.
    /// </summary>
    /// <param name="sliceName">The name of the slice to locate.</param>
    /// <returns>The slice located.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this aseprite file does not contain a slice with the specified name.
    /// </exception>
    public AsepriteSlice GetSlice(string sliceName)
    {
        List<string> slicesFound = new();

        for (int i = 0; i < _slices.Length; i++)
        {
            AsepriteSlice aseSlice = _slices[i];
            slicesFound.Add($"'{aseSlice.Name}'");

            if (aseSlice.Name == sliceName)
            {
                return aseSlice;
            }
        }

        InvalidOperationException ex = new($"This aseprite file does not contain a slice with the name '{sliceName}'.");
        ex.Data.Add(nameof(sliceName), sliceName);
        ex.Data.Add(nameof(slicesFound), slicesFound);
        throw ex;
    }

    /// <summary>
    /// Gets the slice at the specified index from this aseprite file.
    /// </summary>
    /// <param name="sliceIndex">The index of the slice to locate</param>
    /// <param name="located">When this method returns true, contains the slice located; otherwise, false.</param>
    /// <returns>
    /// true if the slice was located; otherwise, false.  This method returns false if this slice index specified is
    /// less than zero or is greater than or equal to the total number of slices in this aseprite file.
    /// </returns>
    public bool TryGetSlice(int sliceIndex, [NotNullWhen(true)] out AsepriteSlice? located)
    {
        located = default;

        if (sliceIndex < 0 || sliceIndex >= _slices.Length)
        {
            return false;
        }

        located = _slices[sliceIndex];
        return located is not null;
    }

    /// <summary>
    /// Gets the slice with the specified name from this aseprite file.
    /// </summary>
    /// <param name="sliceName">The name of the slice to locate.</param>
    /// <param name="located">When this method returns true, contains the slice located; otherwise, false.</param>
    /// <returns>
    /// true if the slice  was located; otherwise, false.  This method returns false if this aseprite file does not
    /// contain an slice with the specified name.
    /// </returns>
    public bool TryGetSlice(string sliceName, [NotNullWhen(true)] out AsepriteSlice? located)
    {
        located = default;

        for (int i = 0; i < _slices.Length; i++)
        {
            if (_tags[i].Name == sliceName)
            {
                located = _slices[i];
                break;
            }
        }

        return located is not null;
    }

    /// <summary>
    /// Gets the tileset at the specified index in this aseprite file.
    /// </summary>
    /// <param name="tilesetIndex">The index of the tileset to locate.</param>
    /// <returns>The tileset located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the tileset index specified is less than zero or is greater than or equal to the total number of
    /// tilesets in this aseprite file.
    /// </exception>
    public AsepriteTileset GetTileset(int tilesetIndex)
    {
        if (tilesetIndex < 0 || tilesetIndex >= _tilesets.Length)
        {
            ArgumentOutOfRangeException ex = new(nameof(tilesetIndex), $"{nameof(tilesetIndex)} cannot be less than zero or greater than or equal to the total number of tilesets in this aseprite file.");
            ex.Data.Add("TotalTilesets", _tilesets.Length);
            throw ex;
        }

        return _tilesets[tilesetIndex];
    }

    /// <summary>
    /// Gets the tileset with the specified name from this aseprite file.
    /// </summary>
    /// <param name="tilesetName">The name of the tileset to locate.</param>
    /// <returns>The tileset located.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this aseprite file does not contain a tileset with the specified name.
    /// </exception>
    public AsepriteTileset GetTileset(string tilesetName)
    {
        List<string> tilesetsFound = new();

        for (int i = 0; i < _tilesets.Length; i++)
        {
            AsepriteTileset aseTileset = _tilesets[i];
            tilesetsFound.Add($"'{aseTileset.Name}'");

            if (aseTileset.Name == tilesetName)
            {
                return aseTileset;
            }
        }

        InvalidOperationException ex = new($"Unable to locate a tileset with the name '{tilesetName}' in this aseprite file.");
        ex.Data.Add(nameof(tilesetsFound), tilesetsFound);
        throw ex;
    }

    /// <summary>
    /// Gets the tileset at the specified index from this aseprite file.
    /// </summary>
    /// <param name="tilesetIndex">The index of the tileset to locate</param>
    /// <param name="located">When this method returns true, contains the tileset located; otherwise, false.</param>
    /// <returns>
    /// true if the tileset was located; otherwise, false.  This method returns false if this tileset index specified is
    /// ess than zero or is greater than or equal to the total number of tilesets in this aseprite file.
    /// </returns>
    public bool TryGetTileset(int tilesetIndex, [NotNullWhen(true)] out AsepriteTileset? located)
    {
        located = default;

        if (tilesetIndex < 0 || tilesetIndex >= _tilesets.Length)
        {
            return false;
        }

        located = _tilesets[tilesetIndex];
        return located is not null;
    }

    /// <summary>
    /// Gets the tileset with the specified name from this aseprite file.
    /// </summary>
    /// <param name="tilesetName">The name of the tileset to locate.</param>
    /// <param name="located">When this method returns true, contains the tileset located; otherwise, false.</param>
    /// <returns>
    /// true if the tileset was located; otherwise, false.  This method returns false if this aseprite file does not
    /// contain an tileset with the specified name.
    /// </returns>
    public bool TryGetTileset(string tilesetName, [NotNullWhen(true)] out AsepriteTileset? located)
    {
        located = default;

        for (int i = 0; i < _tilesets.Length; i++)
        {
            if (_tilesets[i].Name == tilesetName)
            {
                located = _tilesets[i];
                break;
            }
        }

        return located is not null;
    }

    /// <summary>
    /// Loads the aseprite file at the specified path.  The result is a new instance of the aseprite file class that
    /// contains the contents of the aseprite file loaded.
    /// </summary>
    /// <param name="path">The absolute file path to the aseprite file to load.</param>
    /// <returns> A new instance of the aseprite file class that is create by this method.</returns>
    /// <exception cref="FileNotFoundException">Thrown if no file is located at the specified path.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an error occurs during the reading of the aseprite file.  The exception message will contain the
    /// cause of the exception.
    /// </exception>
    public static AsepriteFile Load(string path) => AsepriteFileReader.ReadFile(path);

}
