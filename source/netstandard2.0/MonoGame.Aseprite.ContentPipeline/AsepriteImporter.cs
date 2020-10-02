﻿//--------------------------------------------------------------------------------
//  Importer
//  Imports the .json file produced by Aseprite for the Content Pipeline
//--------------------------------------------------------------------------------
//
//                              License
//  
//    Copyright(c) 2018 Chris Whitley
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in
//    all copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//    THE SOFTWARE.
//--------------------------------------------------------------------------------
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using TInput = System.String;

namespace MonoGame.Aseprite.ContentPipeline
{
    [ContentImporter(".json", DisplayName = "Aseprite Animation Importer", DefaultProcessor = "AsepriteProcessor")]
    public class AsepriteImporter : ContentImporter<TInput>
    {

        public override TInput Import(string filename, ContentImporterContext context)
        {
            //  Read the json from the file and return it
            return File.ReadAllText(filename);

        }

    }

}
