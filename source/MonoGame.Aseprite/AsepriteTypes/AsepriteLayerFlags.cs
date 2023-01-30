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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
/// Defines the flag values for a layer in aseprite.
/// </summary>
public enum AsepriteLayerFlags : ushort
{
    /// <summary>
    /// Defines that no flags are set
    /// </summary>
    None = 0,

    /// <summary>
    /// Defines that the layer is visible.
    /// </summary>
    Visible = 1,

    /// <summary>
    /// Defines that the layer is editable in the Aseprite application.
    /// </summary>
    Editable = 2,

    /// <summary>
    /// Defines that the layer movement is locked in the Aseprite application.
    /// </summary>
    LockMovement = 4,

    /// <summary>
    /// Defines that the layer was set as the background layer.
    /// </summary>
    Background = 8,

    /// <summary>
    /// Defines that the layer prefers linked cels.
    /// </summary>
    PreferLinkedCels = 16,

    /// <summary>
    /// Defines that the layer should be displayed collapsed in the Aseprite application.
    /// </summary>
    DisplayedCollapsed = 32,

    /// <summary>
    /// Defines that the layer is a reference layer.
    /// </summary>
    ReferenceLayer = 64
}
