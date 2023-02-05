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
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.Tilemaps;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
/// Defines a processor that processes an animated tilemap from an aseprite file.
/// </summary>
public static class AnimatedTilemapProcessor
{
    /// <summary>
    /// Processes an animated tilemap from the given aseprite file.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="aseFile">The aseprite file to processed the animated tilemap from.</param>
    /// <param name="onlyVisibleLayers">Indicates if only layers that are visible should be processed.</param>
    /// <returns>The animated tilemap created by this method. </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if layers are found in the aseprite file with duplicate names.  Tilemaps must contain layers with unique
    /// names even though Aseprite does not enforce unique names for layers.
    /// </exception>
    public static AnimatedTilemap Process(GraphicsDevice device, AsepriteFile aseFile, bool onlyVisibleLayers = true)
    {
        RawAnimatedTilemap rawTilemap = RawAnimatedTilemapProcessor.Process(aseFile, onlyVisibleLayers);
        return AnimatedTilemap.FromRaw(device, rawTilemap);
    }
}
