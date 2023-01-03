﻿/* ------------------------------------------------------------------------------
    Copyright (c) 2022 Christopher Whitley

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
------------------------------------------------------------------------------ */

using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MonoGame.Aseprite.ContentPipeline.Importers
{
    [ContentImporter(".ase", ".aseprite", DisplayName = "Aseprite Document Importer", DefaultProcessor = "AsepriteDocumentProcessor")]
    public class AsepriteDocumentImporter : ContentImporter<AsepriteImporterResult>
    {
        /// <summary>
        ///     Import method that can be used without the content pipeline.
        /// </summary>
        /// <remarks>
        ///     This overload should only be used when importing an Aseprite file outside
        ///     of the Content Pipeline Tool.
        /// </remarks>
        /// <param name="filename">
        ///     The fully qualified path to the file in which to import.
        /// </param>
        /// <returns>
        ///     A new <see cref="AsepriteImporterResult"/> instance containing the
        ///     results of the import.
        /// </returns>
        public AsepriteImporterResult Import(string filename) => Import(filename, null);


        /// <summary>
        ///     Import method that is called by the content pipeline.
        /// </summary>
        /// <remarks>
        ///     If you are using the importer without the assistance of the Content Pipeline Tool,
        ///     then the other Import(string) overload should be used instead.
        /// </remarks>
        /// <param name="filename">
        ///     The fully qualifie dpath to the file in which to import.
        /// </param>
        /// <param name="context">
        ///     A <see cref="ContentImporterContext"/> instance provided by the content pipeline.
        /// </param>
        /// <returns></returns>
        public override AsepriteImporterResult Import(string filename, ContentImporterContext context)
        {

            AsepriteImporterResult result = new AsepriteImporterResult
            {
                Data = File.ReadAllBytes(filename)
            };
            return result;
        }
    }
}
