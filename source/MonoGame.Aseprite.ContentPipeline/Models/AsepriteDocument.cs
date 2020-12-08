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
    /// <summary>
    ///     A class that describes an Aseprite file document.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Aseprite File Spec documentation: 
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepriteDocument
    {
        /// <summary>
        ///     Gets the <see cref="AsepriteHeader"/> instance of the
        ///     document.
        /// </summary>
        public AsepriteHeader Header { get; private set; }

        /// <summary>
        ///     Gets a collection of all <see cref="AsepriteFrame"/> instances
        ///     found within the document.
        /// </summary>
        public List<AsepriteFrame> Frames { get; private set; }

        /// <summary>
        ///     Gets a collection of all <see cref="AsepriteSliceChunk"/> instances
        ///     found within the document.
        /// </summary>
        public List<AsepriteSliceChunk> Slices { get; private set; }

        /// <summary>
        ///     Gets a collection of all <see cref="AsepriteTagChunk"/> instances
        ///     found within the doucment.
        /// </summary>
        public List<AsepriteTagChunk> Tags { get; private set; }

        /// <summary>
        ///     Gets a collectin of all <see cref="AsepriteLayerChunk"/> instances
        ///     found within the document.
        /// </summary>
        public List<AsepriteLayerChunk> Layers { get; private set; }

        /// <summary>
        ///     Gets the <see cref="AsepritePaletteChunk"/> instance found within
        ///     the document.
        /// </summary>
        public AsepritePaletteChunk Palette { get; internal set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteDocument"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to
        ///     read the Aseprite document file.
        /// </param>
        internal AsepriteDocument(AsepriteReader reader)
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
        }
    }
}
