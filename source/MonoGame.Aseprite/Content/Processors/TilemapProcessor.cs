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
using MonoGame.Aseprite.Content.Processors.RawProcessors;
using MonoGame.Aseprite.Content.RawTypes;
using MonoGame.Aseprite.Tilemaps;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
/// Defines a processor that processes a tilemap from an aseprite file.
/// </summary>
public static class TilemapProcessor
{
    /// <summary>
    /// Processes a tilemap from the aseprite frame at the specified index in the given aseprite file.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="aseFile">The aseprite file that contains the animated tilemap to process.</param>
    /// <param name="frameIndex">The index of the aseprite frame in the aseprite file to process.</param>
    /// <param name="onlyVisibleLayers">Indicates if only aseprite layers that are visible should be processed.</param>
    /// <returns>The tilemap created by this method. </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the frame index specified is less than zero or is greater than or equal to the total number of
    /// aseprite frames in the given aseprite file.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if aseprite layers are found in the aseprite file with duplicate names.  Tilemaps must contain layers
    /// with unique names even though Aseprite does not enforce unique names for layers.
    /// </exception>
    public static Tilemap Process(GraphicsDevice device, AsepriteFile aseFile, int frameIndex, bool onlyVisibleLayers = true)
    {
        RawTilemap rawTilemap = RawTilemapProcessor.Process(aseFile, frameIndex, onlyVisibleLayers);
        return Tilemap.FromRaw(device, rawTilemap);
    }
}
