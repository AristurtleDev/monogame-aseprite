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
///     Defines a reader that reads a <see cref="RawTextureAtlas"/> from a file.
/// </summary>
public static class RawTextureAtlasReader
{
    /// <summary>
    ///     Reads the <see cref="RawTextureAtlas"/> from the file at the specified path.
    /// </summary>
    /// <param name="path">
    ///     The path and name of the file that contains the <see cref="RawTextureAtlas"/> to read.
    /// </param>
    /// <returns>
    ///     The <see cref="RawTextureAtlas"/> that was read.
    /// </returns>
    public static RawTextureAtlas Read(string path)
    {
        Stream stream = File.OpenRead(path);
        BinaryReader reader = new(stream);
        return Read(reader);
    }

    internal static RawTextureAtlas Read(BinaryReader reader)
    {
        reader.ReadMagic();
        return reader.ReadRawTextureAtlas();
    }
}
