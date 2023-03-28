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

using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.Tilemaps;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
/// Defines a processor that processes a <see cref="Tileset"/> from an <see cref="AsepriteFile"/>.
/// </summary>
/// <seealso cref="MonoGame.Aseprite.Content.Processors"/>
public static partial class TilesetProcessor
{
    /// <summary>
    ///     Processes a <see cref="Tileset"/> from the <see cref="AsepriteTile"/> element at the specified index in the
    ///     given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> used to create graphical resources.
    ///  </param>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> that contains the <see cref="AsepriteTileset"/> to processes.
    /// </param>
    /// <param name="tilesetIndex">
    ///     The index of the <see cref="AsepriteTile"/> element in the <see cref="AsepriteFile"/> to processes.
    /// </param>
    /// <returns>
    ///     The <see cref="Tileset"/> created by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than or equal to the total number of
    ///     <see cref="AsepriteTileset"/> elements in the given <see cref="AsepriteFile"/>.
    /// </exception>
    /// <seealso cref="AsepriteFile"/>
    /// <seealso cref="Tileset"/>
    public static Tileset Process(GraphicsDevice device, AsepriteFile aseFile, int tilesetIndex)
    {
        AsepriteTileset aseTileset = aseFile.GetTileset(tilesetIndex);
        return Process(device, aseTileset);
    }

    /// <summary>
    ///     Processes a <see cref="Tileset"/> from the <see cref="AsepriteTileset"/> with the specified name in the 
    ///     given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> used to create graphical resources.
    ///  </param>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> that contains the <see cref="AsepriteTileset"/> to process.
    /// </param>
    /// <param name="tilesetName">
    ///     The name of the <see cref="AsepriteTileset"/> element in the <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <returns>
    ///     The <see cref="Tileset"/> created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the given <see cref="AsepriteFile"/> does not contain an <see cref="AsepriteTileset"/> element 
    ///     with the specified name.
    /// </exception>
    /// <seealso cref="AsepriteFile"/>
    /// <seealso cref="Tileset"/>
    public static Tileset Process(GraphicsDevice device, AsepriteFile aseFile, string tilesetName)
    {
        AsepriteTileset aseTileset = aseFile.GetTileset(tilesetName);
        return Process(device, aseTileset);
    }

    /// <summary>
    ///     Processes a <see cref="Tileset"/> from an <see cref="AsepriteTileset"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> used to create graphical resources.
    ///  </param>
    /// <param name="aseTileset">
    ///     The <see cref="AsepriteTileset"/> to process.
    /// </param>
    /// <returns>
    ///     The <see cref="Tileset"/> created by this method.
    /// </returns>
    /// <seealso cref="AsepriteFile"/>
    /// <seealso cref="AsepriteTileset"/>
    /// <seealso cref="Tileset"/>
    public static Tileset Process(GraphicsDevice device, AsepriteTileset aseTileset)
    {
        RawTileset rawTileset = ProcessRaw(aseTileset);
        return Tileset.FromRaw(device, rawTileset);
    }
}
