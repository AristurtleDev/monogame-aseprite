/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */
using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;
using MonoGame.Aseprite.Compression;
using Microsoft.Xna.Framework;

namespace AsepriteDotNet.IO.Image;

//  Reference: https://www.w3.org/TR/png-3
internal static class PngWriter
{
    //  Common IDAT chunk sizes are between 8 and 32 Kib.  Opting to use
    //  8Kib for this project.
    private const int MAX_IDAT_LEN = 8192;

    /// <summary>
    ///     Saves the given <paramref name="data"/> to disk at the specified
    ///     <paramref name="path"/> as a .png image file
    /// </summary>
    /// <param name="path">
    ///     The absolute path to where the file should be saved.
    /// </param>
    /// <param name="size">
    ///     A <see cref="Size"/> value that defines the width and height
    ///     of the final image.
    /// </param>
    /// <param name="data">
    ///     The pixel data of the image.
    /// </param>
    public static void SaveTo(string path, Point size, Color[] data)
    {
        try
        {
            using FileStream fs = File.OpenWrite(path);
            using BinaryWriter writer = new(fs, Encoding.UTF8);
            WriteSignature(writer);
            WriteIHDR(writer, size);
            WriteIDAT(writer, size, data);
            WriteIEND(writer);
        }
        catch (Exception ex)
        {
            throw new Exception("An exception occurred while saving the data as a PNG file. Refer to the inner exception for exact details", ex);
        }
    }

    //  PNG Signature
    //
    //  The first eight bytes of a PNG always contains the following (decimal)
    //  values: 137, 80, 78, 71, 13, 10, 26, 10
    //
    //  Which are (in hexadecimal): 89, 50, 4E, r7, 0D, 0A, 1A, 0A
    //
    //  Reference: https://www.w3.org/TR/png-3/#5PNG-file-signature
    private static void WriteSignature(BinaryWriter writer)
    {
        ReadOnlySpan<byte> signature = stackalloc byte[8]
        {
            0x89,
            0x50,   //  P
            0x4E,   //  N
            0x47,   //  G
            0x0D,   //  Carriage Return
            0x0A,   //  Line Feed
            0x1A,   //  CTRL-Z
            0x0A    //  Carriage Return
        };

        writer.Write(signature);
    }

    //  IHDR
    //   -----------------------------------
    //  | Width                 |   4-bytes |
    //  | Height                |   4-bytes |
    //  | Bit Depth             |   1-byte  |
    //  | Color Type            |   1-byte  |
    //  | Compressions Method   |   1-byte  |
    //  | Filter Method         |   1-byte  |
    //  | Interlace Method      |   1-byte  |
    //   -----------------------------------
    //
    //  Width:
    //      4-byte integer (Network Byte Order) that defines the width, in
    //      pixels of the final image
    //
    //  Height:
    //      4-byte integer (Network Byte Order) that defines the height, in
    //      pixels of the final image
    //
    //  Bit Depth:
    //      1-byte integer that indicates the number of bits per sample. Valid
    //      values depend on the color type used
    //
    //       -----------------------------------------------
    //      | Color Type            |   Allowed bit depths  |
    //       -----------------------------------------------
    //      | Grayscale             |   1, 2, 4, 8, 16      |
    //      | Truecolor             |   8, 16               |
    //      | Index-color           |   1, 2, 4, 8          |
    //      | Grayscale with alpha  |   8, 16               |
    //      | Truecolor with alpha  |   8, 16               |
    //       -----------------------------------------------
    //
    //  Color Type:
    //      1-byte integer the indicates the color type used
    //
    //       -----------------------------------
    //      | Color Type            |   Value   |
    //       -----------------------------------
    //      | Grayscale             |   0       |
    //      | Truecolor             |   2       |
    //      | Index-color           |   3       |
    //      | Grayscale with alpha  |   4       |
    //      | Truecolor with alpha  |   6       |
    //       -----------------------------------
    //
    //  Compression Method:
    //      1-byte integer that indicates the compression method used. Only
    //      compression method 0 (deflate/inflate compression with a sliding
    //      window of 32768 bytes) is defined
    //
    //  Filter Method:
    //      1-byte integer that indicates the preprocessing method applied to
    //      the image data before compression.  Only filter method 0 (adaptive
    //      filtering with five basic filter types) is defined.
    //
    //  Interlace Method:
    //      1-byte integer that indicates the transmission order of the image
    //      data.
    //
    //       ---------------------------------------
    //      | Interlace Method          |   Value   |
    //       ---------------------------------------
    //      | Standard (no interlace)   |   0       |
    //      | Adam7                     |   1       |
    //       ---------------------------------------
    //
    //  Reference: https://www.w3.org/TR/png-3/#11IHDR
    private static void WriteIHDR(BinaryWriter writer, Point size)
    {
        Span<byte> ihdr = stackalloc byte[13];
        ihdr.Clear();
        BinaryPrimitives.WriteInt32BigEndian(ihdr.Slice(0), size.X);
        BinaryPrimitives.WriteInt32BigEndian(ihdr.Slice(4), size.Y);
        ihdr[8] = 8;    //  Bit-depth
        ihdr[9] = 6;    //  Color Type
        ihdr[10] = 0;   //  Compression Method
        ihdr[11] = 0;   //  Filter Method
        ihdr[12] = 0;   //  Interlace Method

        WriteChunk(writer, "IHDR", ihdr);
    }

    //  IDAT
    //
    //  The IDAT chunk contains the actual image data, which is the output of
    //  the compression stream.
    //
    //  The compression stream is a deflate stream including the Adler-32
    //  trailer.
    //
    //  Each scanline of the image begins with a single byte that defines the
    //  filter used on that scanline.
    //
    //  The IDAT can be split over multiple chunks.  If so, they must appear
    //  consecutively with no other intervening chunks.
    //
    //  If the IDAT is split over multiple chunks, the compression stream is
    //  the concatenation of the contents of the data fields of all IDAT chunks.
    //
    //  Reference: https://www.w3.org/TR/png-3/#11IDAT
    //             https://www.w3.org/TR/png-3/#10Compression
    //             https://www.w3.org/TR/png-3/#7Scanline
    //             https://www.w3.org/TR/png-3/#7Filtering
    private static void WriteIDAT(BinaryWriter writer, Point size, Color[] data)
    {
        void Flush(MemoryStream stream)
        {
            Span<byte> output = stream.GetBuffer();
            int remainder = (int)stream.Length;
            int offset = 0;

            while (remainder >= MAX_IDAT_LEN)
            {
                int len = Math.Min(remainder, MAX_IDAT_LEN);
                WriteChunk(writer, "IDAT", output.Slice(offset, len));
                offset += len;
                remainder -= len;
            }

            if (remainder > 0)
            {
                output.Slice(offset).CopyTo(output);
                stream.Position = remainder;
                stream.SetLength(remainder);
            }
        }

        void FlushAll(MemoryStream stream)
        {
            Span<byte> output = stream.GetBuffer();
            int remainder = (int)stream.Length;
            int offset = 0;

            while (remainder > 0)
            {
                int len = Math.Min(remainder, MAX_IDAT_LEN);
                WriteChunk(writer, "IDAT", output.Slice(offset, len));
                offset += len;
                remainder -= len;
            }
        }

        using MemoryStream ms = new();

        //  Zlib deflate header for Default Compression
        ms.WriteByte(0x78);
        ms.WriteByte(0x9C);

        Adler32 adler = new();

        using (DeflateStream deflate = new DeflateStream(ms, CompressionMode.Compress, leaveOpen: true))
        {
            ReadOnlySpan<byte> filter = stackalloc byte[1] { 0 };   //  Filter mode 0
            for (int i = 0; i < data.Length; i += size.X)
            {
                deflate.Write(filter);
                adler.Update(filter);

                Color[] scanline = data[(i)..(i + size.X)];
                for (int c = 0; c < scanline.Length; c++)
                {
                    ReadOnlySpan<byte> pixel = new byte[4]
                    {
                        scanline[c].R,
                        scanline[c].G,
                        scanline[c].B,
                        scanline[c].A
                    };
                    deflate.Write(pixel);
                    adler.Update(pixel);


                    if (ms.Length >= MAX_IDAT_LEN)
                    {
                        Flush(ms);
                    }
                }
            }
        }

        using (BinaryWriter adlerWriter = new(ms, Encoding.UTF8, leaveOpen: true))
        {
            adlerWriter.Write(ToBigEndian((int)adler.CurrentValue));
        }

        FlushAll(ms);
    }

    //  IEND
    //
    //  The IEND chunk marks the end of the PNG datastream.
    //  The chunk data field for IEND is empty
    //
    //  Reference: https://www.w3.org/TR/png-3/#11IEND
    private static void WriteIEND(BinaryWriter writer)
    {
        WriteChunk(writer, "IEND", ReadOnlySpan<byte>.Empty);
    }

    //  Chunks consist of three or four parts
    //
    //   ------------    ----------------    ----------------    ---------
    //  |   Length   |  |   Chunk Type   |  |   Chunk Data   |  |   CRC   |
    //   ------------    ----------------    ----------------    ---------
    //
    //  If there is no data (Length = 0), then the data chunk is not written
    //
    //  Length:
    //      A 4-byte unsigned integer giving the number of bytes in the chunk's
    //      data field. Only the data field. Do not include the length field
    //      itself, the chunk type field, or the crc field
    //
    //  Chunk Type:
    //      A sequence of 4-bytes that represent the lowercase and/or uppercase
    //      ISO-646 letters (A-Z and a-z) of the chunk type (e.g. IHDR, IDAT)
    //
    //  Chunk Data:
    //      The data bytes of the chunk. Can be zero length (empty)
    //
    //  CRC:
    //      A 4-byte CRC calculated on the preceding bytes in the chunk
    //      including the chunk type field and the chunk data field, but not the
    //      chunk length field. CRC is always present, even if the chunk data
    //      field is excluded.
    //
    //  Reference: https://www.w3.org/TR/png-3/#5Chunk-layout
    private static void WriteChunk(BinaryWriter writer, string chunkType, ReadOnlySpan<byte> data)
    {
        //  Write the length
        writer.Write(ToBigEndian(data.Length));

        //  Create a byte array that will contain the chunk type and the data
        byte[] typeAndData = new byte[4 + data.Length];

        //  First insert the chunk type bytes
        typeAndData[0] = (byte)chunkType[0];
        typeAndData[1] = (byte)chunkType[1];
        typeAndData[2] = (byte)chunkType[2];
        typeAndData[3] = (byte)chunkType[3];

        //  Copy the chunk data into it
        for (int i = 0; i < data.Length; i++)
        {
            typeAndData[4 + i] = data[i];
        }

        //  Write the type and data
        writer.Write(typeAndData);

        //  Calculate the CRC of the type and data
        int crc = (int)CRC.Calculate(typeAndData);

        //  Write the crc
        writer.Write(ToBigEndian(crc));
    }


    //  Per https://www.w3.org/TR/png-3/#7Integers-and-byte-order
    //
    //      "All integers that require more than one byte shall be in network
    //      byte order"
    //
    //  Basically, we have to ensure that all integer type values are in
    //  BigEndian.
    //
    //  This method will check for endianess and convert to BigEndian if needed.
    private static int ToBigEndian(int value)
    {
        //  https://stackoverflow.com/a/19276259

        //  If already BigEndian just return the value back
        if (!BitConverter.IsLittleEndian) { return value; }

        uint b = (uint)value;
        uint b0 = (b & 0x000000FF) << 24;   //  Least significant to most significant
        uint b1 = (b & 0x0000FF00) << 8;    //  2nd least significant to 2nd most significant
        uint b2 = (b & 0x00FF0000) >> 8;    //  2nd most significant to 2nd least significant
        uint b3 = (b & 0xFF000000) >> 24;   //  Most significant to least significant.

        return (int)(b0 | b1 | b2 | b3);
    }
}
