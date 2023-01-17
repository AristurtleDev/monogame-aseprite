/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Diagnostics.CodeAnalysis;

namespace MonoGame.Aseprite;

/// <summary>
///     Represents a 32-bit packed RGBA (red, green, blue, alpha) color value.
/// </summary>
/// <remarks>
///     This is modeled after the way color is implemented in Aseprite.  Unlike
///     System.Drawing.Color, this stores the red color component as the most
///     significant octet and alpha as the least significant octet.
/// </remarks>
public struct Rgba32 : IEquatable<Rgba32>
{
    internal const byte RGBA_R_SHIFT = 0;
    internal const byte RGBA_G_SHIFT = 8;
    internal const byte RGBA_B_SHIFT = 16;
    internal const byte RGBA_A_SHIFT = 24;
    internal const uint RGBA_R_MASK = 0x000000ff;
    internal const uint RGBA_G_MASK = 0x0000ff00;
    internal const uint RGBA_B_MASK = 0x00ff0000;
    internal const uint RGBA_RGB_MASK = 0x00ffffff;
    internal const uint RGBA_A_MASK = 0xff000000;

    /// <summary>
    ///     Represents a <see cref="Rgba32"/> value who's red, green, blue, and
    ///     alpha components are all set to zero.
    /// </summary>
    public static readonly Rgba32 Transparent = Rgba32.FromRGBA(0, 0, 0, 0);

    /// <summary>
    ///     Represents a <see cref="Rgba32"/> values who's red and alpha
    ///     components are set to 255 and the green and blue components set to
    ///     zero.
    /// </summary>
    public static readonly Rgba32 Red = Rgba32.FromRGBA(255, 0, 0, 255);

    /// <summary>
    ///     Represents a <see cref="Rgba32"/> values who's green and alpha
    ///     components are set to 255 and the red and blue components set to
    ///     zero.
    /// </summary>
    public static readonly Rgba32 Green = Rgba32.FromRGBA(0, 255, 0, 255);

    /// <summary>
    ///     Represents a <see cref="Rgba32"/> values who's blue and alpha
    ///     components are set to 255 and the red and green components set to
    ///     zero.
    /// </summary>
    public static readonly Rgba32 Blue = Rgba32.FromRGBA(0, 0, 255, 255);

    public static readonly Rgba32 Yellow = Rgba32.FromRGBA(255, 255, 0, 255);
    public static readonly Rgba32 Magenta = Rgba32.FromRGBA(255, 0, 255, 255);
    public static readonly Rgba32 Aqua = Rgba32.FromRGBA(0, 255, 255, 255);
    public static readonly Rgba32 White = Rgba32.FromRGBA(255, 255, 255, 255);
    public static readonly Rgba32 Black = Rgba32.FromRGBA(0, 0, 0, 255);

    private uint _value;

    /// <summary>
    ///     Gets the red component value of this <see cref="Rgba32"/>.
    /// </summary>
    public readonly byte R => unchecked((byte)((_value >> RGBA_R_SHIFT) & 0xFF));

    /// <summary>
    ///     Gets the green component value of this <see cref="Rgba32"/>.
    /// </summary>
    public readonly byte G => unchecked((byte)((_value >> RGBA_G_SHIFT) & 0xFF));

    /// <summary>
    ///     Gets the blue component value of this <see cref="Rgba32"/>.
    /// </summary>
    public readonly byte B => unchecked((byte)((_value >> RGBA_B_SHIFT) & 0xFF));

    /// <summary>
    ///     Gets the alpha component value of this <see cref="Rgba32"/>.
    /// </summary>
    public readonly byte A => unchecked((byte)((_value >> RGBA_A_SHIFT) & 0xFF));

    /// <summary>
    ///     Gets a 32-bit unsigned integer that represents the packed RGBA
    ///     value for this <see cref="Rgba32"/>, with the red component value
    ///     being the most most significant octet and the alpha component being
    ///     the least significant octet.
    /// </summary>
    public readonly uint Value => _value;

    internal Rgba32(uint value) => _value = value;

    /// <summary>
    ///     Creates a new <see cref="Rgba32"/> value from the specified 8-bit
    ///     <paramref name="red"/>, <paramref name="green"/>,
    ///     <paramref name="blue"/>, and <paramref name="alpha"/> component
    ///     values.
    /// </summary>
    /// <remarks>
    ///     This method allows a 32-bit value to be passed for each component
    ///     value, but the value of each component is limited to 8-bits.
    /// </remarks>
    /// <param name="red">
    ///     The red component value for the new <see cref="Rgba32"/>.  Valid
    ///     values are 0 through 255
    /// </param>
    /// <param name="green">
    ///     The green component value for the new <see cref="Rgba32"/>.  Valid
    ///     values are 0 through 255
    /// </param>
    /// <param name="blue">
    ///     The blue component value for the new <see cref="Rgba32"/>.  Valid
    ///     values are 0 through 255
    /// </param>
    /// <param name="alpha">
    ///     The alpha component value for the new <see cref="Rgba32"/>.  Valid
    ///     values are 0 through 255
    /// </param>
    /// <returns>
    ///     The new <see cref="Rgba32"/> value created by this method.
    /// </returns>
    public static Rgba32 FromRGBA(int red, int green, int blue, int alpha)
    {
        CheckByte(red, nameof(red));
        CheckByte(green, nameof(green));
        CheckByte(blue, nameof(blue));
        CheckByte(alpha, nameof(alpha));

        return new Rgba32(RGBA(red, green, blue, alpha));
    }

    private static void CheckByte(int value, string name)
    {
        if (unchecked((uint)value) > byte.MaxValue)
        {
            throw new ArgumentOutOfRangeException(name);
        }
    }

    private static uint RGBA(int r, int g, int b, int a) => (uint)r << RGBA_R_SHIFT |
                                                            (uint)g << RGBA_G_SHIFT |
                                                            (uint)b << RGBA_B_SHIFT |
                                                            (uint)a << RGBA_A_SHIFT;
    /// <summary>
    ///     Returns a value that indicates whether the specified
    ///     <see cref="object"/> is equal to this <see cref="Rgba32"/>.
    /// </summary>
    /// <param name="obj">
    ///     The <see cref="object"/> to check for equality with this
    ///     <see cref="Rgba32"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified <see cref="object"/> is
    ///     equal to this <see cref="Rgba32"/>; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Rgba32 color && Equals(color);

    /// <summary>
    ///     Returns a value that indicates whether the specified
    ///     <see cref="Rgba32"/> is equal to this <see cref="Rgba32"/>.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="Rgba32"/> to check for equality
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified <see cref="Rgba32"/> value
    ///     is equal to this <see cref="Rgba32"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public readonly bool Equals(Rgba32 other) => this == other;

    /// <summary>
    ///     Returns the hash code for this <see cref="Rgba32"/> value.
    /// </summary>
    /// <returns>
    ///     A 32-bit signed integer that is the hash code for this
    ///     <see cref="Rgba32"/> value.
    /// </returns>
    public override readonly int GetHashCode() => _value.GetHashCode();

    /// <summary>
    ///     Compares two <see cref="Rgba32"/> values for equality.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rgba32"/> value on the left side of the equality
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Rgba32"/> value on the right side of the equality
    ///     operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Rgba32"/> values are
    ///     equal; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Rgba32 left, Rgba32 right) => left._value == right._value;

    /// <summary>
    ///     Compares two <see cref="Rgba32"/> values for inequality.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rgba32"/> value on the left side of the inequality
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Rgba32"/> value on the right side of the inequality
    ///     operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Rgba32"/> values are
    ///     unequal; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Rgba32 left, Rgba32 right) => !(left == right);

    /// <summary>
    ///     Returns a string representation of this <see cref="Rgba32"/>.
    /// </summary>
    /// <returns>
    ///     A new string representation of this <see cref="Rgba32"/>.
    /// </returns>
    public override readonly string ToString() => $"{{Red={R}, Green={G}, Blue={B}, Alpha={A}}}";

    //  Blend Functions

    /// <summary>
    ///     Blends two <see cref="Rgba32"/> values using a specified
    ///     <see cref="BlendMode"/>.
    /// </summary>
    /// <param name="mode">
    ///     The <see cref="BlendMode"/> to use for blending the two colors.
    /// </param>
    /// <param name="backdrop">
    ///     The <see cref="Rgba32"/> value that is behind the
    ///     <paramref name="source"/>.
    /// </param>
    /// <param name="source">
    ///     The <see cref="Rgba32"/> value that is on top of the
    ///     <paramref name="backdrop"/>.
    /// </param>
    /// <param name="opacity">
    ///     The opacity of the container (e.g. layer) that the
    ///     <paramref name="source"/> exists on.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rgba32"/> value that is the result of blending the
    ///     <paramref name="source"/> with the <paramref name="backdrop"/>
    ///     using the specified <see cref="BlendMode"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Rgba32 Blend(BlendMode mode, Rgba32 backdrop, Rgba32 source, int opacity)
    {
        if (backdrop.A == 0 && source.A == 0)
        {
            return Rgba32.Transparent;
        }
        else if (backdrop.A == 0)
        {
            return source;
        }
        else if (source.A == 0)
        {
            return backdrop;
        }

        uint blended = mode switch
        {
            #pragma warning disable format
            BlendMode.Normal     => Normal(backdrop, source, opacity),
            BlendMode.Multiply   => Multiply(backdrop, source, opacity),
            BlendMode.Screen     => Screen(backdrop, source, opacity),
            BlendMode.Overlay    => Overlay(backdrop, source, opacity),
            BlendMode.Darken     => Darken(backdrop, source, opacity),
            BlendMode.Lighten    => Lighten(backdrop, source, opacity),
            BlendMode.ColorDodge => ColorDodge(backdrop, source, opacity),
            BlendMode.ColorBurn  => ColorBurn(backdrop, source, opacity),
            BlendMode.HardLight  => HardLight(backdrop, source, opacity),
            BlendMode.SoftLight  => SoftLight(backdrop, source, opacity),
            BlendMode.Difference => Difference(backdrop, source, opacity),
            BlendMode.Exclusion  => Exclusion(backdrop, source, opacity),
            BlendMode.Hue        => HslHue(backdrop, source, opacity),
            BlendMode.Saturation => HslSaturation(backdrop, source, opacity),
            BlendMode.Color      => HslColor(backdrop, source, opacity),
            BlendMode.Luminosity => HslLuminosity(backdrop, source, opacity),
            BlendMode.Addition   => Addition(backdrop, source, opacity),
            BlendMode.Subtract   => Subtract(backdrop, source, opacity),
            BlendMode.Divide     => Divide(backdrop, source, opacity),
            _                    => throw new InvalidOperationException($"Unknown blend mode '{mode}'")
            #pragma warning restore format
        };

        return new Rgba32(blended);
    }

    private static double Sat(double r, double g, double b) => Math.Max(r, Math.Max(g, b)) - Math.Min(r, Math.Min(g, b));

    private static double Lum(double r, double g, double b) => 0.3 * r + 0.59 * g + 0.11 * b;

    private static void SetSat(ref double r, ref double g, ref double b, double s)
    {
        ref double MIN(ref double x, ref double y) => ref (x < y ? ref x : ref y);
        ref double MAX(ref double x, ref double y) => ref (x > y ? ref x : ref y);
        ref double MID(ref double x, ref double y, ref double z) =>
            ref (x > y ? ref (y > z ? ref y : ref (x > z ? ref z : ref x)) : ref (y > z ? ref (z > x ? ref z : ref x) : ref y));

        ref double min = ref MIN(ref r, ref MIN(ref g, ref b));
        ref double mid = ref MID(ref r, ref g, ref b);
        ref double max = ref MAX(ref r, ref MAX(ref g, ref b));

        if (max > min)
        {
            mid = ((mid - min) * s) / (max - min);
            max = s;
        }
        else
        {
            mid = max = 0;
        }

        min = 0;
    }

    private static void SetLum(ref double r, ref double g, ref double b, double l)
    {
        double d = l - Lum(r, g, b);
        r += d;
        g += d;
        b += d;
        ClipColor(ref r, ref g, ref b);
    }

    private static void ClipColor(ref double r, ref double g, ref double b)
    {
        double l = Lum(r, g, b);
        double n = Math.Min(r, Math.Min(g, b));
        double x = Math.Max(r, Math.Max(g, b));

        if (n < 0)
        {
            r = l + (((r - l) * l) / (l - n));
            g = l + (((g - l) * l) / (l - n));
            b = l + (((b - l) * l) / (l - n));
        }

        if (x > 1)
        {
            r = l + (((r - l) * (1 - l)) / (x - l));
            g = l + (((g - l) * (1 - l)) / (x - l));
            b = l + (((b - l) * (1 - l)) / (x - l));
        }
    }

    internal static byte MUL_UN8(int a, int b)
    {
        int t = (a * b) + 0x80;
        return (byte)(((t >> 8) + t) >> 8);
    }

    internal static byte DIV_UN8(int a, int b)
    {
        return (byte)(((ushort)a * 0xFF + (b / 2)) / b);
    }

    private static uint Normal(Rgba32 backdrop, Rgba32 src, int opacity)
    {
        if ((backdrop._value & RGBA_A_MASK) == 0)
        {
            int a = src.A;
            a = MUL_UN8(a, opacity);
            a <<= RGBA_A_SHIFT;
            return (uint)((src._value & RGBA_RGB_MASK) | (uint)a);
        }
        else if ((src._value & RGBA_A_MASK) == 0)
        {
            return backdrop._value;
        }

        int Br = backdrop.R;
        int Bg = backdrop.G;
        int Bb = backdrop.B;
        int Ba = backdrop.A;

        int Sr = src.R;
        int Sg = src.G;
        int Sb = src.B;
        int Sa = src.A;
        Sa = MUL_UN8(Sa, opacity);


        int Ra = Sa + Ba - MUL_UN8(Ba, Sa);

        int Rr = Br + (Sr - Br) * Sa / Ra;
        int Rg = Bg + (Sg - Bg) * Sa / Ra;
        int Rb = Bb + (Sb - Bb) * Sa / Ra;

        return RGBA(Rr, Rg, Rb, Ra);
    }

    private static uint Multiply(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int r = MUL_UN8(backdrop.R, source.R);
        int g = MUL_UN8(backdrop.G, source.G);
        int b = MUL_UN8(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Screen(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int r = backdrop.R + source.R - MUL_UN8(backdrop.R, source.R);
        int g = backdrop.G + source.G - MUL_UN8(backdrop.G, source.G);
        int b = backdrop.B + source.B - MUL_UN8(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Overlay(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int overlay(int b, int s)
        {
            if (b < 128)
            {
                b <<= 1;
                return MUL_UN8(s, b);
            }
            else
            {
                b = (b << 1) - 255;
                return s + b - MUL_UN8(s, b);
            }
        }

        int r = overlay(backdrop.R, source.R);
        int g = overlay(backdrop.G, source.G);
        int b = overlay(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Darken(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int blend(int b, int s) => Math.Min(b, s);

        int r = blend(backdrop.R, source.R);
        int g = blend(backdrop.G, source.G);
        int b = blend(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Lighten(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int lighten(int b, int s) => Math.Max(b, s);

        int r = lighten(backdrop.R, source.R);
        int g = lighten(backdrop.G, source.G);
        int b = lighten(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint ColorDodge(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int dodge(int b, int s)
        {
            if (b == 0)
            {
                return 0;
            }

            s = 255 - s;

            if (b >= s)
            {
                return 255;
            }
            else
            {
                return DIV_UN8(b, s);
            }
        }

        int r = dodge(backdrop.R, source.R);
        int g = dodge(backdrop.G, source.G);
        int b = dodge(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint ColorBurn(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int burn(int b, int s)
        {
            if (b == 255)
            {
                return 255;
            }

            b = (255 - b);

            if (b >= s)
            {
                return 0;
            }
            else
            {
                return 255 - DIV_UN8(b, s);
            }
        }

        int r = burn(backdrop.R, source.R);
        int g = burn(backdrop.G, source.G);
        int b = burn(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    //  Not working
    private static uint HardLight(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int hardlight(int b, int s)
        {
            if (s < 128)
            {
                s <<= 1;
                return MUL_UN8(b, s);
            }
            else
            {
                s = (s << 1) - 255;
                return b + s - MUL_UN8(b, s);
            }
        }

        int r = hardlight(backdrop.R, source.R);
        int g = hardlight(backdrop.G, source.G);
        int b = hardlight(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint SoftLight(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int softlight(int _b, int _s)
        {
            double b = _b / 255.0;
            double s = _s / 255.0;
            double r, d;

            if (b <= 0.25)
            {
                d = ((16 * b - 12) * b + 4) * b;
            }
            else
            {
                d = Math.Sqrt(b);
            }

            if (s <= 0.5)
            {
                r = b - (1.0 - 2.0 * s) * b * (1.0 - b);
            }
            else
            {
                r = b + (2.0 * s - 1.0) * (d - b);
            }

            return (int)(r * 255 + 0.5);
        }

        int r = softlight(backdrop.R, source.R);
        int g = softlight(backdrop.G, source.G);
        int b = softlight(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Difference(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int difference(int b, int s)
        {
            return Math.Abs(b - s);
        }

        int r = difference(backdrop.R, source.R);
        int g = difference(backdrop.G, source.G);
        int b = difference(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Exclusion(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int exclusion(int b, int s)
        {
            return b + s - 2 * MUL_UN8(b, s);
        }

        int r = exclusion(backdrop.R, source.R);
        int g = exclusion(backdrop.G, source.G);
        int b = exclusion(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint HslHue(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double s = Sat(r, g, b);
        double l = Lum(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        SetSat(ref r, ref g, ref b, s);
        SetLum(ref r, ref g, ref b, l);

        uint src = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint HslSaturation(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double s = Sat(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;
        double l = Lum(r, g, b);

        SetSat(ref r, ref g, ref b, s);
        SetLum(ref r, ref g, ref b, l);

        uint src = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint HslColor(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double l = Lum(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        SetLum(ref r, ref g, ref b, l);

        uint src = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint HslLuminosity(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double l = Lum(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;

        SetLum(ref r, ref g, ref b, l);

        uint src = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Addition(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int r = backdrop.R + source.R;
        int g = backdrop.G + source.G;
        int b = backdrop.B + source.B;
        uint src = RGBA(Math.Min(r, 255),
                        Math.Min(g, 255),
                        Math.Min(b, 255), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Subtract(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int r = backdrop.R - source.R;
        int g = backdrop.G - source.G;
        int b = backdrop.B - source.B;
        uint src = RGBA(Math.Max(r, 0), Math.Max(g, 0), Math.Max(b, 0), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }

    private static uint Divide(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        int divide(int b, int s)
        {
            if (b == 0)
            {
                return 0;
            }
            else if (b >= s)
            {
                return 255;
            }
            else
            {
                return DIV_UN8(b, s);
            }
        }
        int r = divide(backdrop.R, source.R);
        int g = divide(backdrop.G, source.G);
        int b = divide(backdrop.B, source.B);
        uint src = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Rgba32(src), opacity);
    }
}
