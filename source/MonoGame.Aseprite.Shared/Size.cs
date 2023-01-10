// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2018-2023 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */

// using System.Diagnostics.CodeAnalysis;

// namespace MonoGame.Aseprite;

// /// <summary>
// ///     Represents the width and height of something.
// /// </summary>
// public struct Size : IEquatable<Size>
// {
//     /// <summary>
//     ///     Represents a <see cref="Size"/> value who's width and height
//     ///     elements are initialized to zero.
//     /// </summary>
//     public static readonly Size Empty = new Size(0, 0);

//     private int _w;
//     private int _h;

//     /// <summary>
//     ///     Gets the width element of this <see cref="Size"/>.
//     /// </summary>
//     public int Width
//     {
//         readonly get => _w;
//         set => _w = value;
//     }

//     /// <summary>
//     ///     Gets the height element of this <see cref="Size"/>.
//     /// </summary>
//     public int Height
//     {
//         readonly get => _h;
//         set => _h = value;
//     }

//     /// <summary>
//     ///     Gets a value that indicates whether this <see cref="Size"/> is
//     ///     empty, meaning that its width and height elements are set to zero.
//     /// </summary>
//     public readonly bool IsEmpty => _w == 0 && _h == 0;

//     /// <summary>
//     ///     Initializes a new <see cref="Size"/> value.
//     /// </summary>
//     /// <param name="w">
//     ///     The width element of this <see cref="Size"/>.
//     /// </param>
//     /// <param name="h">
//     ///     The height element of this <see cref="Size"/>.
//     /// </param>
//     public Size(int w, int h) => (_w, _h) = (w, h);

//     /// <summary>
//     ///     Returns a value that indicates whether the specified
//     ///     <see cref="object"/> is equal to this <see cref="Size"/>.
//     /// </summary>
//     /// <param name="obj">
//     ///     The <see cref="object"/> to check for equality with this
//     ///     <see cref="Size"/>.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the specified <see cref="object"/> is
//     ///     equal to this <see cref="Size"/>; otherwise,
//     ///     <see langword="false"/>.
//     /// </returns>
//     public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Size other && Equals(other);

//     /// <summary>
//     ///     Returns a value that indicates whether the specified
//     ///     <see cref="Size"/> is equal to this <see cref="Size"/>.
//     /// </summary>
//     /// <param name="other">
//     ///     The other <see cref="Size"/> to check for equality
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the specified <see cref="Size"/> value
//     ///     is equal to this <see cref="Size"/> value; otherwise,
//     ///     <see langword="false"/>.
//     /// </returns>
//     public readonly bool Equals(Size other) => this == other;

//     /// <summary>
//     ///     Returns the hash code for this <see cref="Size"/> value.
//     /// </summary>
//     /// <returns>
//     ///     A 32-bit signed integer that is the hash code for this
//     ///     <see cref="Size"/> value.
//     /// </returns>
//     public override readonly int GetHashCode() => HashCode.Combine(_w, _h);

//     /// <summary>
//     ///     Adds the width and height elements of two <see cref="Size"/>
//     ///     values.
//     /// </summary>
//     /// <param name="left">
//     ///     The <see cref="Size"/> value on the left side of the addition
//     ///     operator.
//     /// </param>
//     /// <param name="right">
//     ///     The <see cref="Size"/> value on the right side fo the addition
//     ///     operator.
//     /// </param>
//     /// <returns>
//     ///     A new <see cref="Size"/> value who's width and height elements are
//     ///     the sum of the two <see cref="Size"/> values given.
//     /// </returns>
//     public static Size operator +(Size left, Size right) => Add(left, right);

//     /// <summary>
//     ///     Subtracts the width and height elements of one <see cref="Size"/>
//     ///     value from another.
//     /// </summary>
//     /// <param name="left">
//     ///     The <see cref="Size"/> value on the left side of the subtraction
//     ///     operator.
//     /// </param>
//     /// <param name="right">
//     ///     The <see cref="Size"/> value on the right side fo the subtraction
//     ///     operator.
//     /// </param>
//     /// <returns>
//     ///     A new <see cref="Size"/> value who's width and height  elements are
//     ///     the result of subtracting the width and height elements of the
//     ///     <paramref name="right"/> <see cref="Size"/> from the width and
//     ///     height elements of the <paramref name="left"/> <see cref="Size"/>.
//     /// </returns>
//     public static Size operator -(Size left, Size right) => Subtract(left, right);

//     /// <summary>
//     ///     Compares two <see cref="Size"/> values for equality.
//     /// </summary>
//     /// <param name="left">
//     ///     The <see cref="Size"/> value on the left side of the equality
//     ///     operator.
//     /// </param>
//     /// <param name="right">
//     ///     The <see cref="Size"/> value on the right side of the equality
//     ///     operator.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the two <see cref="Size"/> values are
//     ///     equal; otherwise, <see langword="false"/>.
//     /// </returns>
//     public static bool operator ==(Size left, Size right) => left._w == right._w && left._h == right._h;

//     /// <summary>
//     ///     Compares two <see cref="Size"/> values for inequality.
//     /// </summary>
//     /// <param name="left">
//     ///     The <see cref="Size"/> value on the left side of the inequality
//     ///     operator.
//     /// </param>
//     /// <param name="right">
//     ///     The <see cref="Size"/> value on the right side of the inequality
//     ///     operator.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the two <see cref="Size"/> values are
//     ///     unequal; otherwise, <see langword="false"/>.
//     /// </returns>
//     public static bool operator !=(Size left, Size right) => !(left == right);

//     /// <summary>
//     ///     Adds the width and height elements of <paramref name="size2"/> to
//     ///     the width and height elements of <paramref name="size1"/>. The
//     ///     result is a new <see cref="Size"/> value where the width and height
//     ///     elements are the result of the addition.
//     ///     (size1 + size2)
//     /// </summary>
//     /// <param name="size1">
//     ///     The <see cref="Size"/> value that will have the width and height
//     ///     elements of <paramref name="size2"/> added to its width and height
//     ///     elements.
//     /// </param>
//     /// <param name="size2">
//     ///     The <see cref="Size"/> value who's width and height elements will
//     ///     be added to the width and height elements of
//     ///     <paramref name="size1"/>.
//     /// </param>
//     /// <returns>
//     ///     A new <see cref="Size"/> value who's width and height elements are
//     ///     the result of adding the width and height elements of
//     ///     <paramref name="size2"/> to the width and height elements of
//     ///     <paramref name="size1"/>.
//     /// </returns>
//     public static Size Add(Size size1, Size size2) => new Size(unchecked(size1._w + size2._w), unchecked(size1._h + size2._h));

//     /// <summary>
//     ///     Subtracts the width and height elements of <paramref name="size2"/>
//     ///     from the width and height elements of <paramref name="size1"/>.  The
//     ///     result is a new <see cref="Size"/> value where the width and height
//     ///     elements are the result of the subtraction.
//     ///     (size1 - size2)
//     /// </summary>
//     /// <param name="size1">
//     ///     The <see cref="Size"/> value that will have the width and height
//     ///     elements of <paramref name="size2"/> subtracted from it's width and
//     ///     height elements.
//     /// </param>
//     /// <param name="size2">
//     ///     The <see cref="Size"/> value who's width and height elements will
//     ///     be subtracted from the width and height elements of
//     ///     <paramref name="size1"/>.
//     /// </param>
//     /// <returns>
//     ///     A new <see cref="Size"/> value who's width and height elements are
//     ///     the result of subtracting the width and height elements of
//     ///     <paramref name="size2"/> from the width and height elements of
//     ///     <paramref name="size1"/>.
//     /// </returns>
//     public static Size Subtract(Size size1, Size size2) => new Size(unchecked(size1._w - size2._w), unchecked(size1._h - size2._h));

//     /// <summary>
//     ///     Returns a string representation of this <see cref="Size"/>.
//     /// </summary>
//     /// <returns>
//     ///     A new string representation of this <see cref="Size"/>.
//     /// </returns>
//     public override readonly string ToString() => $"{{Width={Width}, Height={Height}}}";
// }
