/* ------------------------------------------------------------------------------
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

using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values found inside a Slice chunk in an Aseprite file.
    /// </summary>
    /// <remarks>
    ///     A slice chunk is made up of a collection of <see cref="AsepriteSliceKey"/>
    ///     instances that describe the bounds of the slice on a specific frame.
    ///     <para>
    ///         Aseprite Slice Chunk documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#slice-chunk-0x2022">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepriteSliceChunk : AsepriteChunk
    {
        /// <summary>
        ///     Gets the total number of keys contained within this slice.
        /// </summary>
        public int TotalKeys { get; private set; }

        /// <summary>
        ///     Gets the <see cref="AsepriteSliceFlags"/> value that indicates if
        ///     this slice has 9-patch and/or pivot data in its keys.
        /// </summary>
        public AsepriteSliceFlags Flags { get; private set; }

        /// <summary>
        ///     Gets the name of this slice.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the <see cref="AsepriteSliceKey"/> instances that belong to this slice.
        /// </summary>
        public AsepriteSliceKey[] Keys { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteSliceChunk"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        internal AsepriteSliceChunk(AsepriteReader reader)
        {
            TotalKeys = (int)reader.ReadDWORD();
            Flags = (AsepriteSliceFlags)reader.ReadDWORD();

            //  Per ase file spec, ignore the next DWORD, it's "reserved"
            _ = reader.ReadDWORD();

            Name = reader.ReadString();

            Keys = new AsepriteSliceKey[TotalKeys];
            for (int i = 0; i < TotalKeys; i++)
            {
                Keys[i] = new AsepriteSliceKey(reader, Flags);
            }
        }
    }
}
