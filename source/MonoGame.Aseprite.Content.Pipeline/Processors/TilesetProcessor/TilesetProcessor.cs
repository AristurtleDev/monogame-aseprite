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
///     Defines the method for processing a single tileset in an Aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Processor - MonoGame.Aseprite")]
public sealed class TilesetProcessor : CommonProcessor<AsepriteFile, TilesetProcessorResult>
{
    /// <summary>
    ///     Gets or Sets the name of the tileset in the Aseprite file to
    ///     process.
    /// </summary>
    [DisplayName("Frame Index")]
    public string TilesetName { get; set; } = string.Empty;

    /// <summary>
    ///     Processes a single tileset from the Aseprite file.
    /// </summary>
    /// <param name="file">
    ///     The Aseprite file to process.
    /// </param>
    /// <param name="context">
    ///     The content processor context that contains contextual information
    ///     about content being processed.
    /// </param>
    /// <returns>
    ///     A new instance of the TilesetProcessorResult class that contains the
    ///     content fo the tileset processed.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the Aseprite file does not contain a tileset that has a
    ///     name that equal to the value set for the TilesetName property.
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

    private static bool TryGetTilesetByName(List<AsepriteTileset> tilesets, string name, [NotNullWhen(true)] out AsepriteTileset? tileset)
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

    private static Exception NoTilesetFound(List<AsepriteTileset> tilesets)
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
