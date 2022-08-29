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
//  This is a straight port of the Aseprite color.h
//  https://github.com/aseprite/aseprite/blob/master/src/doc/color.h
//  For the most part this is a copy/paste of the code with a few adjustments
//  made for cpp-to-c# conventions.
// ------------------------------------------------------------------------


//  To make things simpler to port the code over, we'll use the typedefs as
//  defined and used in aseprite
using uint16_t = System.UInt16;
using uint32_t = System.UInt32;
using uint8_t = System.Byte;

namespace MonoGame.Aseprite.ContentPipeline.ThirdParty.Aseprite
{
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable IDE0051 // Remove unused private members
    public static class DocColor
    {
        public const uint32_t rgba_r_shift = 0;

        public const uint32_t rgba_g_shift = 8;
        public const uint32_t rgba_b_shift = 16;
        public const uint32_t rgba_a_shift = 24;

        public const uint32_t rgba_r_mask = 0x000000ff;
        public const uint32_t rgba_g_mask = 0x0000ff00;
        public const uint32_t rgba_b_mask = 0x00ff0000;
        public const uint32_t rgba_rgb_mask = 0x00ffffff;
        public const uint32_t rgba_a_mask = 0xff000000;

        #region RGBA
        public static uint8_t rgba_getr(uint32_t c)
        {
            return (uint8_t)((c >> (int)(rgba_r_shift)) & 0xff);
        }

        public static uint8_t rgba_getg(uint32_t c)
        {
            return (uint8_t)((c >> (int)rgba_g_shift) & 0xff);
        }

        public static uint8_t rgba_getb(uint32_t c)
        {
            return (uint8_t)((c >> (int)rgba_b_shift) & 0xff);
        }

        public static uint8_t rgba_geta(uint32_t c)
        {
            return (uint8_t)((c >> (int)rgba_a_shift) & 0xff);
        }

        public static uint32_t rgba(uint32_t r, uint32_t g, uint32_t b, uint32_t a)
        {
            return ((r << (int)rgba_r_shift) |
                    (g << (int)rgba_g_shift) |
                    (b << (int)rgba_b_shift) |
                    (a << (int)rgba_a_shift));
        }

        public static int rgb_luma(int r, int g, int b)
        {
            return (r * 2126 + g * 7152 + b * 722) / 10000;
        }

        public static uint8_t rgba_luma(uint32_t c)
        {
            return (uint8_t)rgb_luma(rgba_getr(c), rgba_getg(c), rgba_getb(c));
        }
        #endregion RGBA

        #region Grayscale
        const uint16_t graya_v_shift = 0;
        const uint16_t graya_a_shift = 8;

        const uint16_t graya_v_mask = 0x00ff;

        const uint16_t graya_a_mask = 0xff00;


        public static uint8_t graya_getv(uint16_t c)
        {
            return (uint8_t)((c >> graya_v_shift) & 0xff);
        }

        public static uint8_t graya_geta(uint16_t c)
        {
            return (uint8_t)((c >> graya_a_shift) & 0xff);
        }

        public static uint16_t graya(uint8_t v, uint8_t a)
        {
            return (uint16_t)((v << graya_v_shift) | (a << graya_a_shift));
        }

        public static uint16_t gray(uint8_t v)
        {
            return graya(v, 255);
        }
        #endregion Grayscale
    }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE1006 // Naming Styles
}
