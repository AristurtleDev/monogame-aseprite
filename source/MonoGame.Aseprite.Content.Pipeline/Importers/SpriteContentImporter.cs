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
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Importers;

/// <summary>
///     Defines a content pipeline importer for importing the contents of an aseprite file.
/// </summary>
[ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite Sprite Importer - MonoGame.Aseprite", DefaultProcessor = nameof(SpriteContentProcessor))]
public class SpriteContentImporter : ContentImporter<AsepriteFrame[]>
{
    [DisplayName("Frame Index")]
    public bool FrameIndex { get; set; }

    /// <summary>
    ///     Imports the contents of the aseprite file at the specified path.
    /// </summary>
    /// <param name="path">
    ///     The path and name of the aseprite file to import.
    /// </param>
    /// <param name="context">
    ///     The content importer context that provides contextual information about the importer.
    /// </param>
    /// <returns>
    ///     A new <see cref="ContentImporterResult"/> containing the result of the import.
    /// </returns>
    public override AsepriteFrame[] Import(string path, ContentImporterContext context)
    {
        AsepriteFile aseFile;

        try
        {
            aseFile = AsepriteFile.Load(path);
        }
        catch (InvalidOperationException ex)
        {
            ContentImportException contentImportException = new("An exception occurred while attempting to import the Aseprite file.  See inner exception for details", path, ex);
            throw contentImportException;
        }

        if (aseFile.FrameCount == 0)
        {
            throw new ContentImportException("No frames found in the Aseprite file imported", path);
        }

        return aseFile.Frames.ToArray();
    }
}
