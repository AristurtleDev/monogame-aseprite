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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Provides method for processing a single <see cref="Tileset"/> in an
///     <see cref="AsepriteFile"/> as an instance of the
///     <see cref="TilesetContent"/> class.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Processor - MonoGame.Aseprite")]
public sealed class TilesetProcessor : ContentProcessor<AsepriteFile, TilesetContent>
{
    /// <summary>
    ///     The name of the <see cref="Tileset"/> to process.
    /// </summary>
    /// <remarks>
    ///     This value is set in the property window of the mgcb-editor
    /// </remarks>
    [DisplayName("Frame Index")]
    public string TilesetName { get; set; } = string.Empty;

    /// <summary>
    ///     Processes a single <see cref="Tileset"/> in an
    ///     <see cref="AsepriteFile"/>.  The result is a new instance of the
    ///     <see cref="TilesetContent"/> class containing the content of the
    ///     <see cref="Tileset"/> to be written to the xnb file.
    /// </summary>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The context of the content processor.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="TilesetContent"/> class containing
    ///     the content of the <see cref="Tileset"/> to be written to the
    ///     xnb file.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the given <see cref="AsepriteFile"/> does not have a
    ///     <see cref="Tileset"/> element that has a name equal to
    ///     <see cref="TilesetName"/>.
    /// </exception>
    public override TilesetContent Process(AsepriteFile file, ContentProcessorContext context)
    {
        if (TryGetTilesetByName(file.Tilesets, TilesetName, out Tileset? tileset))
        {
            TextureContent textureContent = new(tileset.Width, tileset.Height, tileset.Pixels);
            return new(tileset.Name, tileset.TileCount, tileset.TileWidth, tileset.TileHeight, textureContent);
        }

        throw NoTilesetFound(file.Tilesets);
    }

    private static bool TryGetTilesetByName(List<Tileset> tilesets, string name, [NotNullWhen(true)] out Tileset? tileset)
    {
        tileset = default;

        for (int i = 0; i < tilesets.Count; i++)
        {

            if (tilesets[i].Name == name)
            {
                tileset = tilesets[i];
                break;
            }
        }

        return tileset is not null;
    }

    private static Exception NoTilesetFound(List<Tileset> tilesets)
    {
        string[] names = new string[tilesets.Count];
        for (int i = 0; i < tilesets.Count; i++)
        {
            names[i] = tilesets[i].Name;
        }

        string[] message = new string[]
        {
            "The Aseprite file does not contain a tileset with the name '{0}'\n",
            "The following tilesets were found: ",
            string.Join(", ", names)
        };

        return new InvalidOperationException(string.Join("", message));
    }
}
