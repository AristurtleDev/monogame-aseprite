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
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.IO;

namespace MonoGame.Aseprite;

/// <summary>
///     Defines the contents of an Aseprite file that has been loaded from disk.
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
    ///     Gets a <see cref="ReadOnlySpan{T}"/> of the <see cref="AsepriteFrame"/> elements in this
    ///     <see cref="AsepriteFile"/>
    /// </summary>
    public ReadOnlySpan<AsepriteFrame> Frames => _frames;

    /// <summary>
    ///     Gets a <see cref="ReadOnlySpan{T}"/> of the <see cref="AsepriteLayer"/> elements in this
    ///     <see cref="AsepriteFile"/>
    /// </summary>
    public ReadOnlySpan<AsepriteLayer> Layers => _layers;

    /// <summary>
    ///     Gets a <see cref="ReadOnlySpan{T}"/> of the <see cref="AsepriteTag"/> elements in this
    ///     <see cref="AsepriteFile"/>
    /// </summary>
    public ReadOnlySpan<AsepriteTag> Tags => _tags;

    /// <summary>
    ///     Gets a <see cref="ReadOnlySpan{T}"/> of the <see cref="AsepriteSlice"/> elements in this
    ///     <see cref="AsepriteFile"/>
    /// </summary>
    public ReadOnlySpan<AsepriteSlice> Slices => _slices;

    /// <summary>
    ///     Gets a <see cref="ReadOnlySpan{T}"/> of the <see cref="AsepriteTileset"/> elements in this
    ///     <see cref="AsepriteFile"/>
    /// </summary>
    public ReadOnlySpan<AsepriteTileset> Tilesets => _tilesets;

    /// <summary>
    ///     Gets a <see cref="ReadOnlySpan{T}"/> of the <see cref="Microsoft.Xna.Framework.Color"/> values that
    ///     represent the color palette of this <see cref="AsepriteFile"/>.
    ///     <see cref="AsepriteFile"/>
    /// </summary>
    public ReadOnlySpan<Color> Palette => _palette;

    /// <summary>
    ///     Gets the name of this <see cref="AsepriteFile"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the width, in pixels, of the canvas of this <see cref="AsepriteFile"/>.
    /// </summary>
    public int CanvasWidth { get; }

    /// <summary>
    ///     Gets the height, in pixels, fo the canvas of this <see cref="AsepriteFile"/>.
    /// </summary>
    public int CanvasHeight { get; }

    internal AsepriteFile(string name, int width, int height, Color[] palette, AsepriteFrame[] frames, AsepriteLayer[] layers, AsepriteTag[] tags, AsepriteSlice[] slices, AsepriteTileset[] tilesets)
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
    }

    /// <summary>
    ///     Loads the Aseprite file that is located at the given file path.  The result is a new instance of the
    ///     <see cref="AsepriteFile"/> class that contains the data loaded from the file.
    /// </summary>
    /// <param name="path">
    ///     The absolute file path to the Aseprite file to load.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="AsepriteFile"/> class that is created by this method.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    ///     Thrown if there is not file located at the path specified.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if an error occurs during the reading of the Aseprite file. See the exception message for details on
    ///     why the error occurred.
    /// </exception>
    public static AsepriteFile Load(string path) => AsepriteFileReader.ReadFile(path);

}
