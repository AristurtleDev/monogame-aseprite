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

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public abstract class AsepriteChunk : IAsepriteUserData
    {
        /// <summary>
        ///     Gets the text set for this instance
        /// </summary>
        public string UserDataText { get; private set; }

        /// <summary>
        ///     Gets the color set for this instance.
        /// </summary>
        public Color UserDataColor { get; private set; }

        /// <summary>
        ///     Gets a value that indicates if this chunk has a valid value
        ///     for the <see cref="UserDataText"/> property.
        /// </summary>
        public bool HasUserDataText => string.IsNullOrEmpty(UserDataText);

        /// <summary>
        ///     Gets a value that indicates if this chunk has a valid value
        ///     for the <see cref="UserDataColor"/> property.
        /// </summary>
        public bool HasUserDataColor => UserDataColor != null;

        /// <summary>
        ///     Reads the userdata values for this chunk from the provided
        ///     reader.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        public void ReadUserData(AsepriteReader reader)
        {
            AsepriteUserDataFlags flags = (AsepriteUserDataFlags)reader.ReadDWORD();

            if ((flags & AsepriteUserDataFlags.HasText) != 0)
            {
                UserDataText = reader.ReadString();
            }

            if ((flags & AsepriteUserDataFlags.HasColor) != 0)
            {
                UserDataColor = Color.FromNonPremultiplied(
                    r: reader.ReadByte(),
                    g: reader.ReadByte(),
                    b: reader.ReadByte(),
                    a: reader.ReadByte());
            }
        }
    }
}
