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
using System.Text;
using MonoGame.Aseprite.ContentPipeline.Models;

namespace MonoGame.Aseprite.ContentPipeline
{
    /// <summary>
    ///     A <see cref="BinaryReader"/> implementations used for reading the contents
    ///     of a .ase/.aseprite file.
    /// </summary>
    public class AsepriteReader : BinaryReader
    {
        /// <summary>
        ///     Gets or Sets the last <see cref="AsepriteChunk"/> that was read.
        /// </summary>
        public AsepriteChunk LastChunkRead { get; set; }

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        /// <param name="input">
        ///     The stream to read from.
        /// </param>
        public AsepriteReader(Stream input) : base(input) { }

        /// <summary>
        ///     Reads a 2-byte (16-bit) unsigned integer from the current stream using little-endian encoding
        ///     and advances the position of the stream by two bytes.
        /// </summary>
        /// <returns>
        ///     A (2-byte) (16-bit) unsigned integer read from the current stream.
        /// </returns>
        public ushort ReadWORD() => base.ReadUInt16();

        /// <summary>
        ///     Reads a 2-byte (16-bit) signed integer from the current stream and advances the current
        ///     position of the stream by two bytes.
        /// </summary>
        /// <returns>
        ///     A 2-byte (16-bit) signed integer read from the current stream.
        /// </returns>
        public short ReadSHORT() => base.ReadInt16();

        /// <summary>
        ///     Reads a 4-byte (32-bit) unsigned integer from the current stream and advances the position
        ///     of the stream by four bytes.
        /// </summary>
        /// <returns>
        ///     A 4-byte (32-bit) unsigned integer read from this stream.
        /// </returns>
        public uint ReadDWORD() => base.ReadUInt32();

        /// <summary>
        ///     Reads a 4-byte (32-bit) signed integer from the current stream and advances the position
        ///     of the stream by four bytes.
        /// </summary>
        /// <returns>
        ///     A 4-byte (32-bit) signed integer read from this stream.
        /// </returns>
        public int ReadLONG() => base.ReadInt32();

        /// <summary>
        ///     Reads a string from the current stream and advances the position of the stream by the
        ///     total number of bytes in the string read.
        /// </summary>
        /// <returns>
        ///     A string read from this stream.
        /// </returns>
        public override string ReadString() => Encoding.UTF8.GetString(base.ReadBytes(ReadWORD()));

        /// <summary>
        ///     Advances the position of the stream by the total number of bytes given, ignoring the
        ///     data contined within the skiped part of the stream.
        /// </summary>
        /// <param name="totalBytes">
        ///     THe total number of bytes to skip over in the stream.
        /// </param>
        public void Ignore(int totalBytes) => BaseStream.Position += totalBytes;
    }
}
