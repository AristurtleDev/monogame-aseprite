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

using Microsoft.Xna.Framework;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Utilities;

namespace MonoGame.Aseprite.Tests;

/*
    In order to test the blend modes, I needed to have hard factual values to assert as the expected values after
    performing a blend.  Since the blend functions were ported form Aseprite, we'll use Aseprite itself as the source
    of truth for what the values should be.

    To get these values, a 4x4 sprite was created with the following colors

    * orange      = argb(255, 223, 113, 38)
    * green       = argb(255, 106, 190, 48)
    * purple      = argb(255, 63, 63, 116)
    * pink        = argb(255, 215, 123, 186)
    * red         = argb(255, 172, 50, 50)
    * transparent = argb(0, 0, 0, 0)

    Next two layers were added to the sprite. The bottom layer consists of the following pixels, in order from
    top-to-bottom, read left-to-right

    [Layer 1]
    green, green, purple, purple,
    green, green, purple, purple,
    pink,  pink,  red,    red,
    pink,  pink,  red,    red,

    The top layers consists fo the following pixels, in order from top-to-bottom, read left-to-right

    [Layer 2]
    transparent, transparent, transparent, transparent,
    transparent, orange,      orange,      transparent,
    transparent, orange,      orange,      transparent,
    transparent, transparent, transparent, transparent,

    There is much simpler ways of doing this (like making it a 2x2 image and trimming off the transparent border), but
    this is what i did. Anyway, the next steps was to set the Blend Mode used by the top layer, then copy the RGBA value
     of each pixel where it overlapped one of the base layer colors into the test for that blend mode.

    It was a manual process, but the point of the tests is to ensure that the ported code blends colors the same way
    that Aseprite does to match it 1:1.
*/
public sealed class BlendFunctionsTests
{
    private static readonly Color _green = new Color(106, 190, 48, 255);
    private static readonly Color _purple = new Color(63, 63, 116, 255);
    private static readonly Color _pink = new Color(215, 123, 186, 255);
    private static readonly Color _red = new Color(172, 50, 50, 255);
    private static readonly Color _orange = new Color(223, 113, 38, 255);
    private static readonly Color _transparent = new Color(0, 0, 0, 0);

    [Fact]
    public void BlendFunctions_Blend_Normal_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Normal;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Normal_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Normal;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Normal_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Normal;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Normal_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Normal;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Darken_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Darken;
        Assert.Equal(new Color(106, 113, 38, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(63, 63, 38, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(215, 113, 38, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(172, 50, 38, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Darken_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Darken;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Darken_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Darken;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Darken_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Darken;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Multiply_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
        Assert.Equal(new Color(93, 84, 7, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(55, 28, 17, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(188, 55, 28, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(150, 22, 7, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Multiply_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Multiply_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Multiply_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_ColorBurn_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.ColorBurn;
        Assert.Equal(new Color(85, 108, 0, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(35, 0, 0, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(209, 0, 0, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(160, 0, 0, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_ColorBurn_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.ColorBurn;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_ColorBurn_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.ColorBurn;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_ColorBurn_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.ColorBurn;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Lighten_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Lighten;
        Assert.Equal(new Color(223, 190, 48, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(223, 113, 116, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(223, 123, 186, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(223, 113, 50, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Lighten_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Lighten;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Lighten_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Lighten;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Lighten_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Lighten;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Screen_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Screen;
        Assert.Equal(new Color(236, 219, 79, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(231, 148, 137, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(250, 181, 196, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(245, 141, 81, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Screen_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Screen;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Screen_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Screen;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Screen_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Screen;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_ColorDodge_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.ColorDodge;
        Assert.Equal(new Color(255, 255, 56, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(255, 113, 136, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(255, 221, 219, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(255, 90, 59, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_ColorDodge_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.ColorDodge;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_ColorDodge_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.ColorDodge;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_ColorDodge_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.ColorDodge;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Addition_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Addition;
        Assert.Equal(new Color(255, 255, 86, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(255, 176, 154, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(255, 236, 224, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(255, 163, 88, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Addition_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Addition;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Addition_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Addition;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Addition_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Addition;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Overlay_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Overlay;
        Assert.Equal(new Color(185, 183, 14, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(110, 56, 35, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(245, 109, 138, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(234, 44, 15, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Overlay_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Overlay;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Overlay_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Overlay;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Overlay_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Overlay;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_SoftLight_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.SoftLight;
        Assert.Equal(new Color(150, 184, 21, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(111, 58, 72, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(229, 116, 151, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(200, 45, 22, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_SoftLight_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.SoftLight;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_SoftLight_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.SoftLight;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_SoftLight_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.SoftLight;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_HardLight_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.HardLight;
        Assert.Equal(new Color(218, 168, 14, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(207, 56, 35, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(245, 109, 55, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(234, 44, 15, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_HardLight_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.HardLight;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_HardLight_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.HardLight;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_HardLight_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.HardLight;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Difference_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Difference;
        Assert.Equal(new Color(117, 77, 10, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(160, 50, 78, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(8, 10, 148, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(51, 63, 12, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Difference_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Difference;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Difference_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Difference;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Difference_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Difference;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Exclusion_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Exclusion;
        Assert.Equal(new Color(143, 135, 72, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(176, 120, 120, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(62, 126, 168, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(95, 119, 74, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Exclusion_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Exclusion;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Exclusion_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Exclusion;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Exclusion_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Exclusion;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Subtract_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Subtract;
        Assert.Equal(new Color(0, 77, 10, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(0, 0, 78, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(0, 10, 148, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(0, 0, 12, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Subtract_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Subtract;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Subtract_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Subtract;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Subtract_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Subtract;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Divide_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Divide;
        Assert.Equal(new Color(121, 255, 255, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(72, 142, 255, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(246, 255, 255, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(197, 113, 255, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Divide_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Divide;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Divide_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Divide;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Divide_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Divide;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Hue_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Hue;
        Assert.Equal(new Color(214, 130, 72, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(93, 61, 40, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(199, 145, 107, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(142, 70, 20, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Hue_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Hue;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Hue_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Hue;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Hue_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Hue;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Saturation_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Saturation;
        Assert.Equal(new Color(92, 202, 17, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(92, 29, 214, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(255, 98, 205, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(186, 51, 1, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Saturation_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Saturation;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Saturation_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Saturation;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Saturation_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Saturation;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Color_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Color;
        Assert.Equal(new Color(234, 124, 49, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(127, 51, 0, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(242, 132, 57, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(160, 65, 0, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Color_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Color;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Color_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Color;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Color_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Color;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Luminosity_ColorOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Luminosity;
        Assert.Equal(new Color(94, 178, 36, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(new Color(131, 131, 184, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(new Color(195, 103, 166, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(new Color(223, 101, 101, 255), BlendFunctions.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Luminosity_ColorOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Luminosity;
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Luminosity_TransparentOnColorTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Luminosity;
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));
    }

    [Fact]
    public void BlendFunctions_Blend_Luminosity_TransparentOnTransparentTest()
    {
        AsepriteBlendMode mode = AsepriteBlendMode.Luminosity;
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }
}
