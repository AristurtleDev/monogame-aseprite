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

using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Readers;

/// <summary>
/// Defines a reader that reads a raw animated tilemap from a file.
/// </summary>
public static class RawAnimatedTilemapReader
{
    /// <summary>
    /// Reads the raw animated tilemap from the file at the specified path.
    /// </summary>
    /// <param name="path">The path to the file that contains the raw animated tilemap to read.</param>
    /// <returns>The raw animated tilemap that was read.</returns>
    public static RawAnimatedTilemap Read(string path)
    {
        Stream stream = File.OpenRead(path);
        BinaryReader reader = new(stream);
        return Read(reader);
    }

    internal static RawAnimatedTilemap Read(BinaryReader reader)
    {
        reader.ReadMagic();
        string name = reader.ReadString();
        int tilesetCount = reader.ReadInt32();

        RawTileset[] tilesets = new RawTileset[tilesetCount];

        for (int i = 0; i < tilesetCount; i++)
        {
            tilesets[i] = reader.ReadRawTileset();
        }

        int frameCount = reader.ReadInt32();

        RawTilemapFrame[] frames = new RawTilemapFrame[frameCount];

        for (int i = 0; i < frameCount; i++)
        {
            int duration = reader.ReadInt32();
            int layerCount = reader.ReadInt32();

            RawTilemapLayer[] layers = new RawTilemapLayer[layerCount];

            for (int j = 0; j < layerCount; j++)
            {
                layers[j] = reader.ReadRawTilemapLayer();
            }

            frames[i] = new(duration, layers);
        }

        return new(name, tilesets, frames);
    }
}
