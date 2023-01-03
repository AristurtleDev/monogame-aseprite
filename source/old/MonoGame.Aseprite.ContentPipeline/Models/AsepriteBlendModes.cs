/* ------------------------------------------------------------------------------
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

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provieds the values for the blend mode used by an Aseprite layer.
    /// </summary>
    /// <remarks>
    ///     A blend mode is applied to a Cel when blending it with the cel one layer
    ///     below it.
    ///     <para>
    ///         Aseprite Blend Mode Values documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#layer-chunk-0x2004">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public enum AsepriteBlendMode
    {
        /// <summary>
        ///     Describes that the layer uses Normal blending.
        ///     <para>
        ///         For more informaiton on Normal blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Normal_blend_mode"></a>
        ///     </para>
        /// </summary>
        Normal = 0,

        /// <summary>
        ///     Describes that the layer uses Multiply blending.
        ///     <para>
        ///         For more informaiton on Multiply blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Multiply"></a>
        ///     </para>
        /// </summary>
        Multiply = 1,

        /// <summary>
        ///     Describes that the layer uses Screen blending.
        ///     <para>
        ///         For more informaiton on Screen blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Screen"></a>
        ///     </para>
        /// </summary>
        Screen = 2,

        /// <summary>
        ///     Describes that the layer uses Overlay blending.
        ///     <para>
        ///         For more informaiton on Overlay blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Overlay"></a>
        ///     </para>
        /// </summary>
        Overlay = 3,

        /// <summary>
        ///     Describes that the layer uses Darken blending.
        ///     <para>
        ///         For more informaiton on Darken blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Darken_Only"></a>
        ///     </para>
        /// </summary>
        Darken = 4,

        /// <summary>
        ///     Describes that the layer uses Lighten blending.
        ///     <para>
        ///         For more informaiton on Lighten blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Lighten_Only"></a>
        ///     </para>
        /// </summary>
        Lighten = 5,

        /// <summary>
        ///     Describes that the layer uses Color Dodge blending.
        ///     <para>
        ///         For more informaiton on Color Dodge blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Dodge_and_burn"></a>
        ///     </para>
        /// </summary>
        ColorDodge = 6,

        /// <summary>
        ///     Describes that the layer uses Color Burn blending.
        ///     <para>
        ///         For more informaiton on Color Burn blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Dodge_and_burn"></a>
        ///     </para>
        /// </summary>
        ColorBurn = 7,

        /// <summary>
        ///     Describes that the layer uses Hard Light blending.
        ///     <para>
        ///         For more informaiton on Hard Light blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Hard_Light"></a>
        ///     </para>
        /// </summary>
        HardLight = 8,

        /// <summary>
        ///     Describes that the layer uses Soft Light blending.
        ///     <para>
        ///         For more informaiton on Soft Light blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Soft_Light"></a>
        ///     </para>
        /// </summary>
        SoftLight = 9,

        /// <summary>
        ///     Describes that the layer uses Difference blending.
        ///     <para>
        ///         For more informaiton on Difference blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Difference"></a>
        ///     </para>
        /// </summary>
        Difference = 10,

        /// <summary>
        ///     Describes that the layer uses Exclusion blending.
        ///     <para>
        ///         For more informaiton on Exclusion blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Difference"></a>
        ///     </para>
        /// </summary>
        Exclusion = 11,

        /// <summary>
        ///     Describes that the layer uses Hue blending.
        ///     <para>
        ///         For more informaiton on Hue blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Hue,_saturation_and_luminosity"></a>
        ///     </para>
        /// </summary>
        Hue = 12,

        /// <summary>
        ///     Describes that the layer uses Saturation blending.
        ///     <para>
        ///         For more informaiton on Saturation blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Hue,_saturation_and_luminosity"></a>
        ///     </para>
        /// </summary>
        Saturation = 13,

        /// <summary>
        ///     Describes that the layer uses Color blending.
        ///     <para>
        ///         For more informaiton on Color blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Hue,_saturation_and_luminosity"></a>
        ///     </para>
        /// </summary>
        Color = 14,

        /// <summary>
        ///     Describes that the layer uses Luminosity blending.
        ///     <para>
        ///         For more informaiton on Luminosity blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Hue,_saturation_and_luminosity"></a>
        ///     </para>
        /// </summary>
        Luminosity = 15,

        /// <summary>
        ///     Describes that the layer uses Addition blending.
        ///     <para>
        ///         For more informaiton on Addition blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Addition"></a>
        ///     </para>
        /// </summary>
        Addition = 16,

        /// <summary>
        ///     Describes that the layer uses Subtract blending.
        ///     <para>
        ///         For more informaiton on Subtract blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Subtract"></a>
        ///     </para>
        /// </summary>
        Subtract = 17,

        /// <summary>
        ///     Describes that the layer uses Divide blending.
        ///     <para>
        ///         For more informaiton on Divide blending, see <a href="https://en.wikipedia.org/wiki/Blend_modes#Divide"></a>
        ///     </para>
        /// </summary>
        Divide = 18
    }
}
