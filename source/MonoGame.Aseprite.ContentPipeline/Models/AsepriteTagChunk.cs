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
    ///     Provides the values found inside a Tag chunk in an Aseprite file.
    /// </summary>
    /// <remarks>
    ///     A Tag chunk describes the starting and ending frame of a tagged animation
    ///     in the Aseprite file, and the direction in which the animation playes.
    ///     The color of the tag is also made available for use.
    ///     <para>
    ///         Aseprite Tag Chunk documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#tags-chunk-0x2018">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepriteTagChunk : AsepriteChunk
    {
        /// <summary>
        ///     Gets the index of the frame this tag starts on.
        /// </summary>
        public int From { get; private set; }

        /// <summary>
        ///     Gets the index of the frame this tag ends on.
        /// </summary>
        public int To { get; private set; }

        /// <summary>
        ///     Gets the <see cref="AsepriteLoopDirection"/> value that indicates
        ///     the direction in which the animation should go when looping.
        /// </summary>
        public AsepriteLoopDirection Direction { get; private set; }

        /// <summary>
        ///     Gets the Red color value for this tag.
        /// </summary>
        public byte ColorR { get; private set; }

        /// <summary>
        ///     Gets the Green color value for this tag.
        /// </summary>
        public byte ColorG { get; private set; }

        /// <summary>
        ///     Gets the Blue color value for this tag.
        /// </summary>
        public byte ColorB { get; private set; }

        /// <summary>
        ///     Gets the name of this tag.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteTagChunk"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        internal AsepriteTagChunk(AsepriteReader reader)
        {
            From = reader.ReadWORD();
            To = reader.ReadWORD();
            Direction = (AsepriteLoopDirection)reader.ReadByte();

            //  Per ase file spec, ignore next 8 bytes, they are reserved for future use
            reader.Ignore(8);

            ColorR = reader.ReadByte();
            ColorG = reader.ReadByte();
            ColorB = reader.ReadByte();

            //  Per ase file spec, ignore next byte, it's just an extra one set to zero
            reader.Ignore(1);

            Name = reader.ReadString();
        }
    }
}
