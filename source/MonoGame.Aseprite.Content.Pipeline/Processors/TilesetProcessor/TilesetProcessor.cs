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
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes a single <see cref="AsepriteTileset"/> in an
///     <see cref="AsepriteFile"/>.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Processor - MonoGame.Aseprite")]
public sealed class TilesetProcessor : CommonProcessor<AsepriteFile, TilesetProcessorResult>
{
    /// <summary>
    ///     Gets or SEts the name of the <see cref="AsepriteTileset"/> element in the <see cref="AsepriteFile"/> to
    ///     process.
    /// </summary>
    [DisplayName("Frame Index")]
    public string TilesetName { get; set; } = string.Empty;

    /// <summary>
    ///     Processes a single <see cref="AsepriteTileset"/> in an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The <see cref="ContentProcessorContext"/> that provides contextual information  about the content being
    ///     processed.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="TilesetProcessorResult"/> class that contains the result of this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the <see cref="AsepriteFile"/> does not contain a <see cref="AsepriteTileset"/> element with the
    ///     name specified in the <see cref="TilesetName"/> property.
    /// </exception>
    public override TilesetProcessorResult Process(AsepriteFile file, ContentProcessorContext context)
    {
        if (TryGetTilesetByName(file.Tilesets, TilesetName, out AsepriteTileset? tileset))
        {
            TilesetContent tilesetContent = CreateTilesetContent(tileset, file.Name, context);
            return new TilesetProcessorResult(tilesetContent);
        }

        throw NoTilesetFound(file.Tilesets);
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

    private static Exception NoTilesetFound(ReadOnlySpan<AsepriteTileset> tilesets)
    {
        string[] names = new string[tilesets.Length];
        for (int i = 0; i < tilesets.Length; i++)
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
