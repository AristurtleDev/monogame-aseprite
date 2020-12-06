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

using Microsoft.Xna.Framework;
using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteTagChunk : AsepriteChunk
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

        ///////////// <summary>
        /////////////     Gets the color value for this tag.
        ///////////// </summary>
        //////////public Color Color { get; private set; }

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
        public AsepriteTagChunk(AsepriteReader reader)
        {
            From = reader.ReadWORD();
            To = reader.ReadWORD();
            Direction = (AsepriteLoopDirection)reader.ReadByte();

            //  Per ase file spec, ignore next 8 bytes, they are reserved for future use
            reader.Ignore(8);

            ColorR = reader.ReadByte();
            ColorG = reader.ReadByte();
            ColorB = reader.ReadByte();
            ////////////byte[] colorData = reader.ReadBytes(3);
            ////////////Color = new Color(colorData[0], colorData[1], colorData[2]);

            //  Per ase file spec, ignore next byte, it's just an extra one set to zero
            reader.Ignore(1);

            Name = reader.ReadString();
        }
    }
}
