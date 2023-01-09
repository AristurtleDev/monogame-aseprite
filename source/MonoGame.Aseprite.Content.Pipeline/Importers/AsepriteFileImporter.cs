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

using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.IO;

namespace MonoGame.Aseprite.Content.Pipeline.Importers;

/// <summary>
///     The content pipeline importer for importing the contents of an
///     Aseprite file.
/// </summary>
[ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite File Importer - MonoGame.Aseprite", DefaultProcessor = "AsepriteSpritesheetProcessor")]
public class AsepriteFileImporter : ContentImporter<AsepriteFile>
{
    /// <summary>
    ///     Imports the Aseprite file at the specified file path.
    /// </summary>
    /// <param name="filePath">
    ///     The absolute path, including extension, to the Aseprite file to
    ///     import.
    /// </param>
    /// <param name="context">
    ///     The importer context. This is provided by the MonoGame framework
    ///     when called from the mgcb-editor.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="AsepriteFile"/> class containing
    ///     the data imported from the Aseprite file.
    /// </returns>
    public override AsepriteFile Import(string filePath, ContentImporterContext context)
    {
        return StaticAsepriteFileReader.ReadFile(filePath);
    }
}
