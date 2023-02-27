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
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.Processors;
using MonoGame.Aseprite.Content.Readers;

namespace MonoGame.Aseprite.Content.Pipeline.Importers;

[ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite File Importer - MonoGame.Aseprite", DefaultProcessor = nameof(AsepriteFileContentProcessor))]
internal class AsepriteFileContentImporter : ContentImporter<AsepriteFileImportResult>
{
    public override AsepriteFileImportResult Import(string path, ContentImporterContext context)
    {
        byte[] data = File.ReadAllBytes(path);
        AsepriteFile aseFile;

        try
        {
            aseFile = AsepriteFile.Load(path);
        }
        catch (InvalidOperationException ex)
        {
            throw new ContentImportException(ex.Message, path);
        }

        return new(data, aseFile);
    }
}
