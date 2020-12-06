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

using System.Collections.Generic;
using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteDocument
    {
        public AsepriteHeader Header { get; internal set; }
        public List<AsepriteFrame> Frames { get; internal set; }
        public List<AsepriteSliceChunk> Slices { get; internal set; }
        public List<AsepriteTagChunk> Tags { get; internal set; }
        public List<AsepriteLayerChunk> Layers { get; internal set; }
        public AsepritePaletteChunk Palette { get; internal set; }

        public AsepriteDocument(AsepriteReader reader)
        {
            Frames = new List<AsepriteFrame>();
            Slices = new List<AsepriteSliceChunk>();
            Tags = new List<AsepriteTagChunk>();
            Layers = new List<AsepriteLayerChunk>();

            Header = new AsepriteHeader(reader);

            for (int i = 0; i < Header.FrameCount; i++)
            {
                Frames.Add(new AsepriteFrame(this, reader));
            }

            ////////  Flatten all cels in each frame so that they provide
            ////////  a single array of pixel data for the image the contain.
            //////for (int i = 0; i < Frames.Count; i++)
            //////{
            //////    Frames[i].FlattenCels();
            //////}
        }
    }
}
