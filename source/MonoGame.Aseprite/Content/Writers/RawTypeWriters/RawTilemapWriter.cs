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

using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Content.Writers.RawTypeWriters;

/// <summary>
/// Defines a writer that writes the contents of a raw tilemap to a file.
/// </summary>
public static class RawTilemapWriter
{
    /// <summary>
    /// Writes the contents of the raw tilemap given to the file at the path specified.
    /// </summary>
    /// <param name="path">
    /// The path and name of the file to write the contents of the raw tilemap to.  If no file exists at this
    /// path, one will be created.  If a file already exists, it will be overwritten.
    /// </param>
    /// <param name="rawTilemap">The raw tilemap to write.</param>
    public static void Write(string path, RawTilemap rawTilemap)
    {
        Stream stream = File.Create(path);
        BinaryWriter writer = new(stream);
        Write(writer, rawTilemap);
    }

    internal static void Write(BinaryWriter writer, RawTilemap rawTilemap)
    {
        writer.WriteMagic();
        writer.Write(rawTilemap.Name);
        writer.Write(rawTilemap.RawTilesets.Length);

        for (int i = 0; i < rawTilemap.RawTilesets.Length; i++)
        {
            writer.Write(rawTilemap.RawTilesets[i]);
        }

        writer.Write(rawTilemap.RawLayers.Length);

        for (int i = 0; i < rawTilemap.RawLayers.Length; i++)
        {
            writer.Write(rawTilemap.RawLayers[i]);
        }
    }
}
