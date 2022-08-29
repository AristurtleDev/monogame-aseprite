/* ------------------------------------------------------------------------------
    Copyright(c) 2018 - 2020 Igara Studio S.A.
    Copyright (c) 2001 - 2018 David Capello
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



// ------------------------------------------------------------------------
//
//  This is a straight port of the Aseprite blend_funcs.cpp
//  https://github.com/aseprite/aseprite/blob/master/src/doc/blend_funcs.cpp
//  For the most part this is a copy/paste of the code with a few adjustments
//  made for cpp-to-c# conventions.
// ------------------------------------------------------------------------
using System;
using static MonoGame.Aseprite.ContentPipeline.ThirdParty.Aseprite.DocColor;
using static MonoGame.Aseprite.ContentPipeline.ThirdParty.Pixman.Combine32;

//  To make things simpler to port the code over, we'll use the typedefs as
//  defined and used in aseprite
//  Color is defined as a uint32_t
//  https://github.com/aseprite/aseprite/blob/master/src/doc/color.h#L18
using color_t = System.UInt32;
using uint32_t = System.UInt32;
using uint8_t = System.Byte;

namespace MonoGame.Aseprite.ContentPipeline.ThirdParty.Aseprite
{
    public static class BlendFuncs
    {
        public static uint8_t blend_multiply(uint8_t b, uint8_t s) => MUL_UN8(b, s);
        public static uint8_t blend_screen(uint8_t b, uint8_t s) => (uint8_t)(b + s - MUL_UN8(b, s));
        public static uint8_t blend_overlay(uint8_t b, uint8_t s) => blend_hard_light(s, b);
        public static uint8_t blend_darken(uint8_t b, uint8_t s) => Math.Min(b, s);
        public static uint8_t blend_lighten(uint8_t b, uint8_t s) => Math.Max(b, s);

        public static uint8_t blend_hard_light(uint8_t b, uint8_t s)
        {
            return s < 128 ? blend_multiply(b, (uint8_t)(s << 1)) : blend_screen(b, (uint8_t)((s << 1) - 255));
        }

        public static uint8_t blend_difference(uint8_t b, uint8_t s) => (uint8_t)Math.Abs(b - s);

        public static uint8_t blend_exclusion(uint8_t b, uint8_t s)
        {
            int t = MUL_UN8(b, s);
            return (uint8_t)(b + s - 2 * t);
        }

        public static uint8_t blend_divide(uint8_t b, uint8_t s)
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
                return DIV_UN8(b, s); // return b / s
            }
        }

        public static uint8_t blend_color_dodge(uint8_t b, uint8_t s)
        {
            if (b == 0)
            {
                return 0;
            }

            s = (uint8_t)(255 - s);

            if (b >= s)
            {
                return 255;
            }
            else
            {
                return DIV_UN8(b, s); // return b / (1-s)
            }
        }

        public static uint8_t blend_color_burn(uint32_t b, uint32_t s)
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
                return (uint8_t)(255 - DIV_UN8((uint8_t)b, (uint8_t)s)); // return 1 - ((1-b)/s)
            }
        }

        public static uint8_t blend_soft_light(uint32_t _b, uint32_t _s)
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

            return (uint8_t)(r * 255 + 0.5);
        }

        #region RGBA Blender Functions
        public static color_t rgba_blender_normal(color_t backdrop, color_t src, int opacity)
        {
            if ((backdrop & rgba_a_mask) == 0)
            {
                uint32_t a = rgba_geta(src);
                a = MUL_UN8((uint8_t)a, (uint8_t)opacity);
                a <<= (int)rgba_a_shift;
                return (src & rgba_rgb_mask) | a;
            }
            else if ((src & rgba_a_mask) == 0)
            {
                return backdrop;
            }

            int Br = rgba_getr(backdrop);
            int Bg = rgba_getg(backdrop);
            int Bb = rgba_getb(backdrop);
            int Ba = rgba_geta(backdrop);

            int Sr = rgba_getr(src);
            int Sg = rgba_getg(src);
            int Sb = rgba_getb(src);
            int Sa = rgba_geta(src);
            Sa = MUL_UN8((byte)Sa, (byte)opacity);

            // Ra = Sa + Ba*(1-Sa)
            //    = Sa + Ba - Ba*Sa
            int Ra = Sa + Ba - MUL_UN8((byte)Ba, (byte)Sa);

            // Ra = Sa + Ba*(1-Sa)
            // Ba = (Ra-Sa) / (1-Sa)
            // Rc = (Sc*Sa + Bc*Ba*(1-Sa)) / Ra                Replacing Ba with (Ra-Sa) / (1-Sa)...
            //    = (Sc*Sa + Bc*(Ra-Sa)/(1-Sa)*(1-Sa)) / Ra
            //    = (Sc*Sa + Bc*(Ra-Sa)) / Ra
            //    = Sc*Sa/Ra + Bc*Ra/Ra - Bc*Sa/Ra
            //    = Sc*Sa/Ra + Bc - Bc*Sa/Ra
            //    = Bc + (Sc-Bc)*Sa/Ra
            int Rr = Br + (Sr - Br) * Sa / Ra;
            int Rg = Bg + (Sg - Bg) * Sa / Ra;
            int Rb = Bb + (Sb - Bb) * Sa / Ra;

            return rgba((uint32_t)Rr, (uint32_t)Rg, (uint32_t)Rb, (uint32_t)Ra);
        }

        public static color_t rgba_blender_multiply(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_multiply(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_multiply(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_multiply(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_screen(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_screen(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_screen(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_screen(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_overlay(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_overlay(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_overlay(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_overlay(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_darken(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_darken(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_darken(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_darken(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_lighten(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_lighten(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_lighten(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_lighten(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_color_dodge(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_color_dodge(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_color_dodge(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_color_dodge(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_color_burn(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_color_burn(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_color_burn(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_color_burn(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_hard_light(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_hard_light(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_hard_light(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_hard_light(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_soft_light(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_soft_light(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_soft_light(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_soft_light(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_difference(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_difference(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_difference(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_difference(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_exclusion(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_exclusion(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_exclusion(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_exclusion(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }
        #endregion RGBA Blender Functions

        #region HSV Blender Functions
        private static double lum(double r, double g, double b)
        {
            return (0.3 * r) + (0.59 * g) + (0.11 * b);
        }

        private static double sat(double r, double g, double b)
        {
            return Math.Max(r, Math.Max(g, b)) - Math.Min(r, Math.Min(g, b));
        }

        private static void clip_color(ref double r, ref double g, ref double b)
        {
            double l = lum(r, g, b);
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

        private static void set_lum(ref double r, ref double g, ref double b, double l)
        {
            double d = l - lum(r, g, b);
#pragma warning disable IDE0054 // Use compound assignment
            r = r + d;
            g = g + d;
            b = b + d;
#pragma warning restore IDE0054 // Use compound assignment
            clip_color(ref r, ref g, ref b);
        }

        //  This is ugly, and i hate it, but it works
        static void set_sat(ref double r, ref double g, ref double b, double s)
        {
            ref double MIN(ref double x, ref double y) => ref (x < y ? ref x : ref y);
            ref double MAX(ref double x, ref double y) => ref (x > y ? ref x : ref y);
            ref double MID(ref double x, ref double y, ref double z) =>
                ref (x > y ? ref (y > z ? ref y : ref (x > z ?
                    ref z : ref x)) : ref (y > z ? ref (z > x ? ref z :
                    ref x) : ref y));


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

        public static color_t rgba_blender_hsl_hue(color_t backdrop, color_t src, int opacity)
        {
            double r = rgba_getr(backdrop) / 255.0;
            double g = rgba_getg(backdrop) / 255.0;
            double b = rgba_getb(backdrop) / 255.0;
            double s = sat(r, g, b);
            double l = lum(r, g, b);

            r = rgba_getr(src) / 255.0;
            g = rgba_getg(src) / 255.0;
            b = rgba_getb(src) / 255.0;

            set_sat(ref r, ref g, ref b, s);
            set_lum(ref r, ref g, ref b, l);

            src = rgba((uint32_t)(255.0 * r), (uint32_t)(255.0 * g), (uint32_t)(255.0 * b), 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_hsl_saturation(color_t backdrop, color_t src, int opacity)
        {
            double r = rgba_getr(src) / 255.0;
            double g = rgba_getg(src) / 255.0;
            double b = rgba_getb(src) / 255.0;
            double s = sat(r, g, b);

            r = rgba_getr(backdrop) / 255.0;
            g = rgba_getg(backdrop) / 255.0;
            b = rgba_getb(backdrop) / 255.0;
            double l = lum(r, g, b);

            set_sat(ref r, ref g, ref b, s);
            set_lum(ref r, ref g, ref b, l);

            src = rgba((uint32_t)(255.0 * r), (uint32_t)(255.0 * g), (uint32_t)(255.0 * b), 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_hsl_color(color_t backdrop, color_t src, int opacity)
        {
            double r = rgba_getr(backdrop) / 255.0;
            double g = rgba_getg(backdrop) / 255.0;
            double b = rgba_getb(backdrop) / 255.0;
            double l = lum(r, g, b);

            r = rgba_getr(src) / 255.0;
            g = rgba_getg(src) / 255.0;
            b = rgba_getb(src) / 255.0;

            set_lum(ref r, ref g, ref b, l);

            src = rgba((uint32_t)(255.0 * r), (uint32_t)(255.0 * g), (uint32_t)(255.0 * b), 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_hsl_luminosity(color_t backdrop, color_t src, int opacity)
        {
            double r = rgba_getr(src) / 255.0;
            double g = rgba_getg(src) / 255.0;
            double b = rgba_getb(src) / 255.0;
            double l = lum(r, g, b);

            r = rgba_getr(backdrop) / 255.0;
            g = rgba_getg(backdrop) / 255.0;
            b = rgba_getb(backdrop) / 255.0;

            set_lum(ref r, ref g, ref b, l);

            src = rgba((uint32_t)(255.0 * r), (uint32_t)(255.0 * g), (uint32_t)(255.0 * b), 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_addition(color_t backdrop, color_t src, int opacity)
        {
            int r = rgba_getr(backdrop) + rgba_getr(src);
            int g = rgba_getg(backdrop) + rgba_getg(src);
            int b = rgba_getb(backdrop) + rgba_getb(src);
            src = rgba((uint8_t)Math.Min(r, 255), (uint8_t)Math.Min(g, 255), (uint8_t)Math.Min(b, 255), 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_subtract(color_t backdrop, color_t src, int opacity)
        {
            int r = rgba_getr(backdrop) - rgba_getr(src);
            int g = rgba_getg(backdrop) - rgba_getg(src);
            int b = rgba_getb(backdrop) - rgba_getb(src);
            src = rgba((uint8_t)Math.Max(r, 0), (uint8_t)Math.Max(g, 0), (uint8_t)Math.Max(b, 0), 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }

        public static color_t rgba_blender_divide(color_t backdrop, color_t src, int opacity)
        {
            uint8_t r = blend_divide(rgba_getr(backdrop), rgba_getr(src));
            uint8_t g = blend_divide(rgba_getg(backdrop), rgba_getg(src));
            uint8_t b = blend_divide(rgba_getb(backdrop), rgba_getb(src));
            src = rgba(r, g, b, 0) | (src & rgba_a_mask);
            return rgba_blender_normal(backdrop, src, opacity);
        }
        #endregion HSV BLender Functions
    }
}
