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
namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a layer in an Aseprite image.
/// </summary>
public abstract class AsepriteLayer
{
    /// <summary>
    ///     Indicates whether the cels on this layer are visible.
    /// </summary>
    public bool IsVisible { get; }

    /// <summary>
    ///     Indicates whether this layer was marked as the background layer
    ///     in Aseprite.
    /// </summary>
    public bool IsBackground { get; }

    /// <summary>
    ///     Indicates whether this layer is a reference layer.
    /// </summary>
    public bool IsReference { get; }

    /// <summary>
    ///     The blend mode to use when blending the cels of this layer with the
    ///     layer below it.
    /// </summary>
    public BlendMode BlendMode { get; }

    /// <summary>
    ///     The opacity level of this layer.
    /// </summary>
    public int Opacity { get; }

    /// <summary>
    ///     The name of this layer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The custom user data that was set for this layer in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new();

    internal AsepriteLayer(bool isVisible, bool isBackground, bool isReference, BlendMode blend, int opacity, string name)
    {
        IsVisible = isVisible;
        IsBackground = isBackground;
        IsReference = isReference;
        BlendMode = blend;
        Opacity = opacity;
        Name = name;
    }
}
