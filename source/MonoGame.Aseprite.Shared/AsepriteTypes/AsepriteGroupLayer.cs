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
using System.Collections.ObjectModel;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a group layer in an Aseprite image.
/// </summary>
public sealed class AsepriteGroupLayer : AsepriteLayer
{
    private List<AsepriteLayer> _layers = new();

    /// <summary>
    ///     A read-only collection of all child layers of this group layer.
    /// </summary>
    public ReadOnlyCollection<AsepriteLayer> Children => _layers.AsReadOnly();

    /// <summary>
    ///     The total number of children layers in this group layer.
    /// </summary>
    public int ChildCount => _layers.Count;

    internal AsepriteGroupLayer(bool isVisible, bool isBackground, bool isReference, BlendMode blendMode, int opacity, string name)
        : base(isVisible, isBackground, isReference, blendMode, opacity, name) { }

    internal void AddChild(AsepriteLayer layer) => _layers.Add(layer);
}
