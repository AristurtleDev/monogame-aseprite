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
namespace MonoGame.Aseprite.Compression;

/// <summary>
///     Utility class for calculating a CRC checksum.
/// </summary>
internal class CRC
{
    /// <summary>
    ///     The default initial CRC value.
    /// </summary>
    public const uint DEFAULT = 0xFFFFFFFF;

    private static readonly uint[] _crcTable;

    private uint _value;

    /// <summary>
    ///     Gets the current checksum value.
    /// </summary>
    public uint CurrentValue => _value ^ 0xFFFFFFFF;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CRC"/> class with the
    ///     default value of 0.
    /// </summary>
    public CRC() => _value = DEFAULT;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CRC"/> class.
    /// </summary>
    /// <param name="initial">
    ///     The initial checksum value to start with.
    /// </param>
    public CRC(uint initial) => _value = initial;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CRC"/> class.
    /// </summary>
    /// <param name="initial">
    ///     A <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> elements that
    ///     the initial checksum value will be calculated from.
    /// </param>
    public CRC(ReadOnlySpan<byte> initial) : this() => _ = Update(initial);

    static CRC()
    {

        //  Make the table for fast crc
        //  https://www.w3.org/TR/2003/REC-PNG-20031110/#D-CRCAppendix
        _crcTable = new uint[256];

        uint c;
        for (uint n = 0; n < 256; n++)
        {
            c = n;
            for (int k = 0; k < 8; k++)
            {
                if ((c & 1) != 0)
                {
                    c = 0xEDB88320 ^ (c >> 1);
                }
                else
                {
                    c >>= 1;
                }
            }

            _crcTable[n] = c;
        }
    }

    /// <summary>
    ///     Resets the underlying checksum value of this instance of the
    ///     <see cref="CRC"/> class to <see cref="CRC.DEFAULT"/>.
    /// </summary>
    public void Reset() => _value = DEFAULT;

    /// <summary>
    ///     Updates ane returns the underlying checksum value using the
    ///     specified <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">
    ///     A <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> elements that
    ///     will be added to the underlying checksum value.
    /// </param>
    /// <returns>
    ///     The updated checksum value.
    /// </returns>
    public uint Update(ReadOnlySpan<byte> buffer)
    {
        for (int n = 0; n < buffer.Length; n++)
        {
            _value = _crcTable[(_value ^ buffer[n]) & 0xFF] ^ (_value >> 8);
        }

        return _value ^ 0xFFFFFFFF;
    }

    public static uint Calculate(ReadOnlySpan<byte> buffer) => new CRC().Update(buffer);
}
