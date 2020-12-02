/* ------------------------------------------------------------------------------
    Copyright (c) 2020 Christopher Whitley

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
using MonoGame.Aseprite.ContentPipeline.Models;

namespace MonoGame.Aseprite.ContentPipeline
{
    [ContentProcessor(DisplayName = "Aseprite Processor")]
    class AsepriteProcessor : ContentProcessor<AsepriteImporterResult, AsepriteProcessorResult>
    {
        public override AsepriteProcessorResult Process(AsepriteImporterResult input, ContentProcessorContext context)
        {
            //  Read the aseprite document from the stream.
            AsepriteDocument doc;
            using (MemoryStream stream = new MemoryStream(input.Data))
            {
                using (AsepriteReader reader = new AsepriteReader(stream))
                {
                    doc = new AsepriteDocument(reader);;
                }
            }


            //  Convert the document into a format thats easier to ingest and return it.
            AsepriteProcessorResult result = new AsepriteProcessorResult(doc);
            return result;
        }
    }
}
