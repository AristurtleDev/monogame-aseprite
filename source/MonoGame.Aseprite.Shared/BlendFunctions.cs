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
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

internal static class BlendFunctions
{
    private const byte RGBA_R_SHIFT = 0;
    private const byte RGBA_G_SHIFT = 8;
    private const byte RGBA_B_SHIFT = 16;
    private const byte RGBA_A_SHIFT = 24;
    private const uint RGBA_R_MASK = 0x000000ff;
    private const uint RGBA_G_MASK = 0x0000ff00;
    private const uint RGBA_B_MASK = 0x00ff0000;
    private const uint RGBA_RGB_MASK = 0x00ffffff;
    private const uint RGBA_A_MASK = 0xff000000;

    public static Color Blend(BlendMode mode, Color backdrop, Color source, int opacity)
    {
        if (backdrop.A == 0 && source.A == 0)
        {
            return Color.Transparent;
        }
        else if (backdrop.A == 0)
        {
            return source;
        }
        else if (source.A == 0)
        {
            return backdrop;
        }

        uint b = RGBA(backdrop.R, backdrop.G, backdrop.B, backdrop.A);
        uint s = RGBA(source.R, source.G, source.B, source.A);

        uint blended = mode switch
        {
            #pragma warning disable format
            BlendMode.Normal     => Normal(b, s, opacity),
            BlendMode.Multiply   => Multiply(b, s, opacity),
            BlendMode.Screen     => Screen(b, s, opacity),
            BlendMode.Overlay    => Overlay(b, s, opacity),
            BlendMode.Darken     => Darken(b, s, opacity),
            BlendMode.Lighten    => Lighten(b, s, opacity),
            BlendMode.ColorDodge => ColorDodge(b, s, opacity),
            BlendMode.ColorBurn  => ColorBurn(b, s, opacity),
            BlendMode.HardLight  => HardLight(b, s, opacity),
            BlendMode.SoftLight  => SoftLight(b, s, opacity),
            BlendMode.Difference => Difference(b, s, opacity),
            BlendMode.Exclusion  => Exclusion(b, s, opacity),
            BlendMode.Hue        => HslHue(b, s, opacity),
            BlendMode.Saturation => HslSaturation(b, s, opacity),
            BlendMode.Color      => HslColor(b, s, opacity),
            BlendMode.Luminosity => HslLuminosity(b, s, opacity),
            BlendMode.Addition   => Addition(b, s, opacity),
            BlendMode.Subtract   => Subtract(b, s, opacity),
            BlendMode.Divide     => Divide(b, s, opacity),
            _                    => throw new InvalidOperationException($"Unknown blend mode '{mode}'")
            #pragma warning restore format
        };

        byte red = GetR(blended);
        byte green = GetG(blended);
        byte blue = GetB(blended);
        byte alpha = GetA(blended);

        return new Color(red, green, blue, alpha);
    }

    private static uint RGBA(int r, int g, int b, int a) => (uint)r << RGBA_R_SHIFT |
                                                            (uint)g << RGBA_G_SHIFT |
                                                            (uint)b << RGBA_B_SHIFT |
                                                            (uint)a << RGBA_A_SHIFT;

    private static byte GetR(uint value) => (byte)((value >> RGBA_R_SHIFT) & 0xFF);
    private static byte GetG(uint value) => (byte)((value >> RGBA_G_SHIFT) & 0xFF);
    private static byte GetB(uint value) => (byte)((value >> RGBA_B_SHIFT) & 0xFF);
    private static byte GetA(uint value) => (byte)((value >> RGBA_A_SHIFT) & 0xFF);

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

    private static uint Normal(uint backdrop, uint src, int opacity)
    {
        if ((backdrop & RGBA_A_MASK) == 0)
        {
            int a = GetA(src);
            a = MUL_UN8(a, opacity);
            a <<= RGBA_A_SHIFT;
            return (uint)((src & RGBA_RGB_MASK) | (uint)a);
        }
        else if ((src & RGBA_A_MASK) == 0)
        {
            return backdrop;
        }

        int Br = GetR(backdrop);
        int Bg = GetG(backdrop);
        int Bb = GetB(backdrop);
        int Ba = GetA(backdrop);

        int Sr = GetR(src);
        int Sg = GetG(src);
        int Sb = GetB(src);
        int Sa = GetA(src);
        Sa = MUL_UN8(Sa, opacity);


        int Ra = Sa + Ba - MUL_UN8(Ba, Sa);

        int Rr = Br + (Sr - Br) * Sa / Ra;
        int Rg = Bg + (Sg - Bg) * Sa / Ra;
        int Rb = Bb + (Sb - Bb) * Sa / Ra;

        return RGBA(Rr, Rg, Rb, Ra);
    }

    private static uint Multiply(uint backdrop, uint source, int opacity)
    {
        int r = MUL_UN8(GetR(backdrop), GetR(source));
        int g = MUL_UN8(GetG(backdrop), GetG(source));
        int b = MUL_UN8(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Screen(uint backdrop, uint source, int opacity)
    {
        int r = GetR(backdrop) + GetR(source) - MUL_UN8(GetR(backdrop), GetR(source));
        int g = GetG(backdrop) + GetG(source) - MUL_UN8(GetG(backdrop), GetG(source));
        int b = GetB(backdrop) + GetB(source) - MUL_UN8(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Overlay(uint backdrop, uint source, int opacity)
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

        int r = overlay(GetR(backdrop), GetR(source));
        int g = overlay(GetG(backdrop), GetG(source));
        int b = overlay(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Darken(uint backdrop, uint source, int opacity)
    {
        int blend(int b, int s) => Math.Min(b, s);

        int r = blend(GetR(backdrop), GetR(source));
        int g = blend(GetG(backdrop), GetG(source));
        int b = blend(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Lighten(uint backdrop, uint source, int opacity)
    {
        int lighten(int b, int s) => Math.Max(b, s);

        int r = lighten(GetR(backdrop), GetR(source));
        int g = lighten(GetG(backdrop), GetG(source));
        int b = lighten(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint ColorDodge(uint backdrop, uint source, int opacity)
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

        int r = dodge(GetR(backdrop), GetR(source));
        int g = dodge(GetG(backdrop), GetG(source));
        int b = dodge(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint ColorBurn(uint backdrop, uint source, int opacity)
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

        int r = burn(GetR(backdrop), GetR(source));
        int g = burn(GetG(backdrop), GetG(source));
        int b = burn(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    //  Not working
    private static uint HardLight(uint backdrop, uint source, int opacity)
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

        int r = hardlight(GetR(backdrop), GetR(source));
        int g = hardlight(GetG(backdrop), GetG(source));
        int b = hardlight(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint SoftLight(uint backdrop, uint source, int opacity)
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

        int r = softlight(GetR(backdrop), GetR(source));
        int g = softlight(GetG(backdrop), GetG(source));
        int b = softlight(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Difference(uint backdrop, uint source, int opacity)
    {
        int difference(int b, int s)
        {
            return Math.Abs(b - s);
        }

        int r = difference(GetR(backdrop), GetR(source));
        int g = difference(GetG(backdrop), GetG(source));
        int b = difference(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Exclusion(uint backdrop, uint source, int opacity)
    {
        int exclusion(int b, int s)
        {
            return b + s - 2 * MUL_UN8(b, s);
        }

        int r = exclusion(GetR(backdrop), GetR(source));
        int g = exclusion(GetG(backdrop), GetG(source));
        int b = exclusion(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint HslHue(uint backdrop, uint source, int opacity)
    {
        double r = GetR(backdrop) / 255.0;
        double g = GetG(backdrop) / 255.0;
        double b = GetB(backdrop) / 255.0;
        double s = Sat(r, g, b);
        double l = Lum(r, g, b);

        r = GetR(source) / 255.0;
        g = GetG(source) / 255.0;
        b = GetB(source) / 255.0;

        SetSat(ref r, ref g, ref b, s);
        SetLum(ref r, ref g, ref b, l);

        uint src = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint HslSaturation(uint backdrop, uint source, int opacity)
    {
        double r = GetR(source) / 255.0;
        double g = GetG(source) / 255.0;
        double b = GetB(source) / 255.0;
        double s = Sat(r, g, b);

        r = GetR(backdrop) / 255.0;
        g = GetG(backdrop) / 255.0;
        b = GetB(backdrop) / 255.0;
        double l = Lum(r, g, b);

        SetSat(ref r, ref g, ref b, s);
        SetLum(ref r, ref g, ref b, l);

        uint src = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint HslColor(uint backdrop, uint source, int opacity)
    {
        double r = GetR(backdrop) / 255.0;
        double g = GetG(backdrop) / 255.0;
        double b = GetB(backdrop) / 255.0;
        double l = Lum(r, g, b);

        r = GetR(source) / 255.0;
        g = GetG(source) / 255.0;
        b = GetB(source) / 255.0;

        SetLum(ref r, ref g, ref b, l);

        uint src = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint HslLuminosity(uint backdrop, uint source, int opacity)
    {
        double r = GetR(source) / 255.0;
        double g = GetG(source) / 255.0;
        double b = GetB(source) / 255.0;
        double l = Lum(r, g, b);

        r = GetR(backdrop) / 255.0;
        g = GetG(backdrop) / 255.0;
        b = GetB(backdrop) / 255.0;

        SetLum(ref r, ref g, ref b, l);

        uint src = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Addition(uint backdrop, uint source, int opacity)
    {
        int r = GetR(backdrop) + GetR(source);
        int g = GetG(backdrop) + GetG(source);
        int b = GetB(backdrop) + GetB(source);
        uint src = RGBA(Math.Min(r, 255),
                        Math.Min(g, 255),
                        Math.Min(b, 255), 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Subtract(uint backdrop, uint source, int opacity)
    {
        int r = GetR(backdrop) - GetR(source);
        int g = GetG(backdrop) - GetG(source);
        int b = GetB(backdrop) - GetB(source);
        uint src = RGBA(Math.Max(r, 0), Math.Max(g, 0), Math.Max(b, 0), 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }

    private static uint Divide(uint backdrop, uint source, int opacity)
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
        int r = divide(GetR(backdrop), GetR(source));
        int g = divide(GetG(backdrop), GetG(source));
        int b = divide(GetB(backdrop), GetB(source));
        uint src = RGBA(r, g, b, 0) | (source & RGBA_A_MASK);
        return Normal(backdrop, src, opacity);
    }
}
