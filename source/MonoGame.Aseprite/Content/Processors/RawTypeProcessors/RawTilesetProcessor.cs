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
using MonoGame.Aseprite.Content.RawTypes;
using MonoGame.Aseprite.Tilemaps;

namespace MonoGame.Aseprite.Content.Processors.RawProcessors;

/// <summary>
/// Defines a processor that processes a raw tileset record from an aseprite file.
/// </summary>
public static class RawTilesetProcessor
{
    /// <summary>
    /// Processes a raw tileset record from the aseprite tileset at the specified index in the given aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file that contains the aseprite tileset to process.</param>
    /// <param name="tilesetIndex">The index of the aseprite tileset in the aseprite file to process.</param>
    /// <returns>The raw tileset record created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the tileset index specified is less than zero or is greater than or equal to the total number of
    /// aseprite tilesets in the given aseprite file.
    /// </exception>
    public static RawTileset Process(AsepriteFile aseFile, int tilesetIndex)
    {
        AsepriteTileset aseTileset = aseFile.GetTileset(tilesetIndex);
        return Process(aseTileset);
    }

    /// <summary>
    /// Processes a raw tileset record from the aseprite tileset with the specified name in the given aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file that contains the aseprite tileset to process.</param>
    /// <param name="tilesetName">The name of the aseprite tileset in the aseprite file to process.</param>
    /// <returns>The raw tileset created by this method.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given aseprite file does not contain an aseprite tileset with the specified name.
    /// </exception>
    public static RawTileset Process(AsepriteFile aseFile, string tilesetName)
    {
        AsepriteTileset aseTileset = aseFile.GetTileset(tilesetName);
        return Process(aseTileset);
    }

    /// <summary>
    /// Processes a raw tileset record from the given aseprite tileset.
    /// </summary>
    /// <param name="aseTileset">The aseprite tileset to process.</param>
    /// <returns>The raw tileset created by this method.</returns>
    public static RawTileset Process(AsepriteTileset aseTileset)
    {
        RawTexture texture = new(aseTileset.Name, aseTileset.Pixels.ToArray(), aseTileset.Width, aseTileset.Height);
        return new(aseTileset.ID, aseTileset.Name, texture, aseTileset.TileWidth, aseTileset.TileHeight);
    }
}
