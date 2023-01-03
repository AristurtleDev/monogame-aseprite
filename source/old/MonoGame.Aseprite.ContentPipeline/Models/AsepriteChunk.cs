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
    ///     A base class to use for all Aseprite chunk types.
    /// </summary>
    /// <remarks>
    ///     Not all chunks will have an associated UserData chunk to read for. To
    ///     determine if an instance of this will have UserData, the <see cref="HasUserDataColor"/>
    ///     and <see cref="HasUserDataText"/> properties are supplied.
    ///     <para>
    ///         Aseprite User Data Chunk documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#user-data-chunk-0x2020">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public abstract class AsepriteChunk : IAsepriteUserData
    {
        /// <summary>
        ///     Gets the text set for this chunk in the user data.
        /// </summary>
        public string UserDataText { get; private set; }

        /// <summary>
        ///     Gets the color set for this chunk in the user data.
        /// </summary>
        public byte[] UserDataColor { get; private set; }

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
        internal void ReadUserData(AsepriteReader reader)
        {
            AsepriteUserDataFlags flags = (AsepriteUserDataFlags)reader.ReadDWORD();

            if ((flags & AsepriteUserDataFlags.HasText) != 0)
            {
                UserDataText = reader.ReadString();
            }

            if ((flags & AsepriteUserDataFlags.HasColor) != 0)
            {
                UserDataColor = new byte[4];
                UserDataColor[0] = reader.ReadByte();
                UserDataColor[1] = reader.ReadByte();
                UserDataColor[2] = reader.ReadByte();
                UserDataColor[3] = reader.ReadByte();
            }
        }
    }
}
