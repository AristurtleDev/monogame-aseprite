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

namespace MonoGame.Aseprite.Content.Pipeline.Importers;

/// <summary>
///     Defines the content pipeline importer for importing the contents of an Aseprite file.
/// </summary>
[ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite File Importer - MonoGame.Aseprite", DefaultProcessor = "AsepriteSpritesheetProcessor")]
public class AsepriteFileImporter : ContentImporter<ContentImporterResult<AsepriteFile>>
{
    /// <summary>
    ///     Imports the Aseprite file at the specified file path.
    /// </summary>
    /// <param name="filePath">
    ///     The absolute path, including extension, to the Aseprite file to
    ///     import.
    /// </param>
    /// <param name="context">
    ///     The <see cref="ContentImporterContext"/> that provides contexture information about the file being imported.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="ContentImporterResult{T}"/> class containing the <see cref="AsepriteFile"/>
    ///     data that was imported.
    /// </returns>
    public override ContentImporterResult<AsepriteFile> Import(string filePath, ContentImporterContext context)
    {
        AsepriteFile file = AsepriteFile.Load(filePath);
        return new(filePath, file);
    }
}
