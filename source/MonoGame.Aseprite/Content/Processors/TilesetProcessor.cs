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
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Content.Processors.RawProcessors;
using MonoGame.Aseprite.Content.RawTypes;
using MonoGame.Aseprite.Tilemaps;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
/// Defines a processor that processes a tileset from an aseprite file.
/// </summary>
public static class TilesetProcessor
{
    /// <summary>
    /// Processes a tileset from the aseprite tileset at the specified index in the given aseprite file.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="aseFile">The aseprite file that contains the aseprite tileset to processes.</param>
    /// <param name="tilesetIndex">The index of the aseprite tileset in the aseprite file to processes.</param>
    /// <returns>The tileset created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the tileset index specified is less than zero or is greater than or equal to the total number of
    /// aseprite tilesets in the given aseprite file.
    /// </exception>
    public static Tileset Process(GraphicsDevice device, AsepriteFile aseFile, int tilesetIndex)
    {
        AsepriteTileset aseTileset = aseFile.GetTileset(tilesetIndex);
        return Process(device, aseTileset);
    }

    /// <summary>
    /// Processes a tileset from the aseprite tileset with the specified name int he given aseprite file.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="aseFile">The aseprite file that contains the aseprite tileset to process.</param>
    /// <param name="tilesetName">The name of the aseprite tileset in the Aseprite file to process.</param>
    /// <returns>The tileset created by this method.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given aseprite file does not contain an aseprite tileset with the specified name.
    /// </exception>
    public static Tileset Process(GraphicsDevice device, AsepriteFile aseFile, string tilesetName)
    {
        AsepriteTileset aseTileset = aseFile.GetTileset(tilesetName);
        return Process(device, aseTileset);
    }

    /// <summary>
    /// Processes a tileset from an aseprite tileset.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="aseTileset">The aseprite tileset to process.</param>
    /// <returns>The tileset created by this method.</returns>
    public static Tileset Process(GraphicsDevice device, AsepriteTileset aseTileset)
    {
        RawTileset rawTileset = RawTilesetProcessor.Process(aseTileset);
        return Tileset.FromRaw(device, rawTileset);
    }
}
