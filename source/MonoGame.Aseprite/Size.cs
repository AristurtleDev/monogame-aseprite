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

    public static bool operator ==(Size first, Size second) => first.Equals(second);
    public static bool operator !=(Size first, Size second) => !(first.Equals(second));
    public static Size operator +(Size first, Size second) => Add(first, second);
    public static Size operator -(Size first, Size second) => Subtract(first, second);
    public static Size operator *(Size size, int value) => Multiply(size, value);
    public static Size operator /(Size size, int value) => Divide(size, value);

    public static Size Add(Size first, Size second)
    {
        Size newSize;
        newSize.Width = first.Width + second.Width;
        newSize.Height = first.Height + second.Height;
        return newSize;
    }

    public static Size Subtract(Size first, Size second)
    {
        Size newSize;
        newSize.Width = first.Width - second.Width;
        newSize.Height = first.Height - second.Height;
        return newSize;
    }

    public static Size Multiply(Size size, int value)
    {
        Size newSize;
        newSize.Width = size.Width * value;
        newSize.Height = size.Height * value;
        return newSize;
    }

    public static Size Divide(Size size, int value)
    {
        Size newSize;
        newSize.Width = size.Width / value;
        newSize.Height = size.Height / value;
        return newSize;
    }
}
