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
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Processors;

/// <summary>
///     Defines a processor that processes a tileset from an Aseprite file.
/// </summary>
public static class TilesetProcessor
{
    /// <summary>
    ///     Processes a single tileset in an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="GraphicsDevice"/> used to create the resources.
    /// </param>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="tilesetName">
    ///     The name of the tileset in the <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="Tileset"/> class created by this method.
    /// </returns>
    public static Tileset Process(GraphicsDevice device, AsepriteFile file, string tilesetName)
    {
        TilesetProcessorResult result = Process(file, tilesetName);
        return CreateTileset(device, result);
    }

    /// <summary>
    ///     Processes a <see cref="Tileset"/> from the given <see cref="AsepriteTileset"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="GraphicsDevice"/> used to create the resources.
    /// </param>
    /// <param name="tileset">
    ///     The <see cref="AsepriteTileset"/> to process.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="Tileset"/> class created by this method.
    /// </returns>
    public static Tileset Process(GraphicsDevice device, AsepriteTileset tileset)
    {
        TilesetProcessorResult result = Process(tileset);
        return CreateTileset(device, result);
    }

    internal static TilesetProcessorResult Process(AsepriteFile file, string tilesetName)
    {
        if (TryGetTilesetByName(file.Tilesets, tilesetName, out AsepriteTileset? tileset))
        {
            return Process(tileset);
        }

        throw NoTilesetFound(file.Tilesets, tilesetName);
    }

    internal static TilesetProcessorResult Process(AsepriteTileset tileset) =>
        new(tileset.Name, tileset.Pixels.ToArray(), tileset.Width, tileset.Height, tileset.TileWidth, tileset.TileHeight);

    internal static Tileset CreateTileset(GraphicsDevice device, TilesetProcessorResult result)
    {
        Texture2D texture = new(device, result.Width, result.Height, mipmap: false, SurfaceFormat.Color);
        texture.SetData<Color>(result.Pixels);
        texture.Name = result.Name;
        return new(result.Name, texture, result.TileWidth, result.TileHeight);
    }

    private static bool TryGetTilesetByName(ReadOnlySpan<AsepriteTileset> tilesets, string name, [NotNullWhen(true)] out AsepriteTileset? tileset)
    {
        tileset = default;

        for (int i = 0; i < tilesets.Length; i++)
        {
            if (tilesets[i].Name == name)
            {
                tileset = tilesets[i];
                break;
            }
        }

        return tileset is not null;
    }

    private static Exception NoTilesetFound(ReadOnlySpan<AsepriteTileset> tilesets, string name)
    {
        string[] names = new string[tilesets.Length];
        for (int i = 0; i < tilesets.Length; i++)
        {
            names[i] = tilesets[i].Name;
        }

        string[] message = new string[]
        {
            $"The Aseprite file does not contain a tileset with the name '{name}'\n",
            "The following tilesets were found: ",
            string.Join(", ", names)
        };

        return new InvalidOperationException(string.Join("", message));
    }
}
