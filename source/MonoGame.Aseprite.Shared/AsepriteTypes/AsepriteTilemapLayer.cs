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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
/// Defines a layer that <see cref="AsepriteTilemapCel"/> elements are on.
/// </summary>
public sealed class AsepriteTilemapLayer : AsepriteLayer
{
    /// <summary>
    ///     Gets a reference to the <see cref="AsepriteTileset"/> used by the 
    ///     <see cref="AsepriteTilemapCel"/> elements on this tilemap layer.
    /// </summary>
    public AsepriteTileset Tileset { get; }

    internal AsepriteTilemapLayer(AsepriteTileset tileset, /*int tilesetID,*/ AsepriteLayerFlags flags, AsepriteBlendMode blend, byte opacity, string name)
        : base(flags, blend, opacity, name) => (Tileset/*, TilesetID*/) = (tileset/*, tilesetID*/);
}
