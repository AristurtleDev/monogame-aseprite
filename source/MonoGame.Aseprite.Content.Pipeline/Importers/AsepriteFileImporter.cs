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

namespace MonoGame.Aseprite.Content.Pipeline.Importers;

/// <summary>
///     Defines a content pipeline importer for importing the contents of an aseprite file.
/// </summary>
[ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite File Importer - MonoGame.Aseprite", DefaultProcessor = nameof(AsepriteFileContentProcessor))]
public class AsepriteFileContentImporter : ContentImporter<AsepriteFile>
{
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
    ///     The <see cref="AsepriteFile"/> that is created as a result of the import.
    /// </returns>
    public override AsepriteFile Import(string path, ContentImporterContext context)
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

        ThrowIfNoFrames(aseFile, path);
        ThrowIfDuplicateLayerNames(aseFile.Layers, path);
        ThrowIfDuplicateTagNames(aseFile.Tags, path);
        ThrowIfDuplicateSliceNames(aseFile.Slices, path);
        ThrowIfDuplicateTilesetNames(aseFile.Tilesets, path);

        return aseFile;
    }

    private void ThrowIfNoFrames(AsepriteFile aseFile, string path)
    {
        if (aseFile.FrameCount <= 0)
        {
            ContentImportException ex = new("The Aseprite file imported contains no frames", path);
            throw ex;
        }
    }

    private void ThrowIfDuplicateLayerNames(ReadOnlySpan<AsepriteLayer> layers, string path)
    {
        HashSet<string> nameHash = new();
        foreach (AsepriteLayer layer in layers)
        {
            if (!nameHash.Add(layer.Name))
            {
                throw new ContentImportException($"Duplicate layer name '{layer.Name}' found in Aseprite file.  Layer names must be unique.");
            }
        }
    }

    private void ThrowIfDuplicateSliceNames(ReadOnlySpan<AsepriteSlice> slices, string path)
    {
        HashSet<string> nameHash = new();
        foreach (AsepriteSlice slice in slices)
        {
            if (!nameHash.Add(slice.Name))
            {
                throw new ContentImportException($"Duplicate slice name '{slice.Name}' found in Aseprite file.  Slice names must be unique.", path);
            }
        }
    }

    private void ThrowIfDuplicateTagNames(ReadOnlySpan<AsepriteTag> tags, string path)
    {
        HashSet<string> nameHash = new();
        foreach (AsepriteTag tag in tags)
        {
            if (!nameHash.Add(tag.Name))
            {
                throw new ContentImportException($"Duplicate tag name '{tag.Name}' found in Aseprite file.  Tag names must be unique", path);
            }
        }
    }

    private void ThrowIfDuplicateTilesetNames(ReadOnlySpan<AsepriteTileset> tilesets, string path)
    {
        HashSet<string> nameHash = new();
        foreach (AsepriteTileset tileset in tilesets)
        {
            if (!nameHash.Add(tileset.Name))
            {
                throw new ContentImportException($"Duplicate tileset name '{tileset.Name}' found in Aseprite file.  Tileset names must be unique", path);
            }
        }
    }

}
