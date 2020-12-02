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

using System;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.ContentPipeline.Models;
using MonoGame.Aseprite.ContentPipeline.ThirdParty.Aseprite;

namespace MonoGame.Aseprite.ContentPipeline
{
    public static class Utils
    {
        public static uint ColorToUINT(Color color)
        {
            return DocColor.rgba(color.R, color.G, color.B, color.A);
        }

        public static Color UINTToColor(uint color)
        {
            byte r = DocColor.rgba_getr(color);
            byte g = DocColor.rgba_getg(color);
            byte b = DocColor.rgba_getb(color);
            byte a = DocColor.rgba_geta(color);

            return new Color(r, g, b, a);
        }

        public static uint BytesToPacked(byte r, byte g, byte b, byte a)
        {
            return DocColor.rgba(r, g, b, a);
        }


        public static Func<uint, uint, int, uint> GetBlendFunction(AsepriteBlendMode mode)
        {
            switch (mode)
            {
                case AsepriteBlendMode.Normal:
                    return BlendFuncs.rgba_blender_normal;
                case AsepriteBlendMode.Multiply:
                    return BlendFuncs.rgba_blender_multiply;
                case AsepriteBlendMode.Screen:
                    return BlendFuncs.rgba_blender_screen;
                case AsepriteBlendMode.Overlay:
                    return BlendFuncs.rgba_blender_overlay;
                case AsepriteBlendMode.Darken:
                    return BlendFuncs.rgba_blender_darken;
                case AsepriteBlendMode.Lighten:
                    return BlendFuncs.rgba_blender_lighten;
                case AsepriteBlendMode.ColorDodge:
                    return BlendFuncs.rgba_blender_color_dodge;
                case AsepriteBlendMode.ColorBurn:
                    return BlendFuncs.rgba_blender_color_burn;
                case AsepriteBlendMode.HardLight:
                    return BlendFuncs.rgba_blender_hard_light;
                case AsepriteBlendMode.SoftLight:
                    return BlendFuncs.rgba_blender_soft_light;
                case AsepriteBlendMode.Difference:
                    return BlendFuncs.rgba_blender_difference;
                case AsepriteBlendMode.Exclusion:
                    return BlendFuncs.rgba_blender_exclusion;
                case AsepriteBlendMode.Hue:
                    return BlendFuncs.rgba_blender_hsl_hue;
                case AsepriteBlendMode.Saturation:
                    return BlendFuncs.rgba_blender_hsl_saturation;
                case AsepriteBlendMode.Color:
                    return BlendFuncs.rgba_blender_hsl_color;
                case AsepriteBlendMode.Luminosity:
                    return BlendFuncs.rgba_blender_hsl_luminosity;
                case AsepriteBlendMode.Addition:
                    return BlendFuncs.rgba_blender_addition;
                case AsepriteBlendMode.Subtract:
                    return BlendFuncs.rgba_blender_subtract;
                case AsepriteBlendMode.Divide:
                    return BlendFuncs.rgba_blender_divide;
                default:
                    throw new Exception("Unknown blend mode");
            }
        }
    }
}
