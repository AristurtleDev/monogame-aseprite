/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

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

using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

public struct Size : IEquatable<Size>
{
    public static readonly Size Empty = new Size();

    public int Width;
    public int Height;
    public bool IsEmpty => Width == 0 && Height == 0;

    public Size(int width, int height) => (Width, Height) = (width, height);


    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Size size && Equals(size);
    public bool Equals(Size other) => Equals(other);
    public override int GetHashCode() => HashCode.Combine(Width, Height);
    public override string ToString() => $"(Width={Width}, Height={Height})";

    public static bool operator ==(Size size1, Size size2) => size1.Equals(size2);
    public static bool operator !=(Size size1, Size size2) => !(size1.Equals(size2));
    public static Size operator +(Size size1, Size size2) => Add(size1, size2);
    public static Size operator -(Size size1, Size size2) => Subtract(size1, size2);
    public static Size operator *(Size size, int multiplier) => Multiply(size, multiplier);
    public static Size operator /(Size size, int divisor) => Divide(size, divisor);

    public static implicit operator Point(Size size) => new Point(size.Width, size.Height);
    public static implicit operator Size(Point point) => new Size(point.X, point.Y);

    public static Size Add(Size size1, Size size2)
    {
        Size newSize;
        newSize.Width = size1.Width + size2.Width;
        newSize.Height = size1.Height + size2.Height;
        return newSize;
    }

    public static Size Subtract(Size size1, Size size2)
    {
        Size newSize;
        newSize.Width = size1.Width - size2.Width;
        newSize.Height = size1.Height - size2.Height;
        return newSize;
    }

    public static Size Multiply(Size size, int multiplier)
    {
        Size newSize;
        newSize.Width = size.Width * multiplier;
        newSize.Height = size.Height * multiplier;
        return newSize;
    }

    public static Size Divide(Size size, int divisor)
    {
        Size newSize;
        newSize.Width = size.Width / divisor;
        newSize.Height = size.Height / divisor;
        return newSize;
    }
}
