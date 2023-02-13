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

namespace MonoGame.Aseprite.Content.Writers;

/// <summary>
///     Defines a writer that writes the contents of a <see cref="RawSprite"/> to a file.
/// </summary>
public static class RawSpriteWriter
{
    /// <summary>
    ///     Writes the contents of the <see cref="RawSprite"/> given to the file at the path specified.
    /// </summary>
    /// <param name="path">
    ///     The path and name of the file to write the contents of the <see cref="RawSprite"/> to.  If no file exists at
    ///     this path, one will be created.  If a file already exists, it will be overwritten.
    /// </param>
    /// <param name="rawSprite">The <see cref="RawSprite"/> to write.</param>
    public static void Write(string path, RawSprite rawSprite)
    {
        Stream stream = File.Create(path);
        BinaryWriter writer = new(stream);
        Write(writer, rawSprite);
    }

    internal static void Write(BinaryWriter writer, RawSprite rawSprite)
    {
        writer.WriteMagic();
        writer.Write(rawSprite.Name);
        writer.Write(rawSprite.RawTexture);
        writer.Write(rawSprite.Slices.Length);

        for (int i = 0; i < rawSprite.Slices.Length; i++)
        {
            writer.Write(rawSprite.Slices[i]);
        }
    }
}
