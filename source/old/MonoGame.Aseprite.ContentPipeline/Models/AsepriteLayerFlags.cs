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

using System;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values for the flags set in the Aseprtie layer.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Aseprite Layer Flags documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#layer-chunk-0x2004">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    [Flags]
    public enum AsepriteLayerFlags
    {
        /// <summary>
        ///     When this flag is set, indicates that the layer is visible.
        /// </summary>
        Visible = 1,

        /// <summary>
        ///     When this flag is set, indicates that the layer is not locked and
        ///     can be edited.
        /// </summary>
        Editable = 2,

        /// <summary>
        ///     When this flag is set, indicates that the layer can not be moved up
        ///     or down.
        /// </summary>
        LockMovement = 4,

        /// <summary>
        ///     When this flag is set, indicates taht the layer is a background layer
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         For more information on layers flagged as a background, see the documentation
        ///         provided <a href="https://www.aseprite.org/docs/layers/#background-layer">here</a>.
        ///     </para>
        /// </remarks>
        Background = 8,

        /// <summary>
        ///     When this falg is set, indicates that the layer prefers by default that the cels
        ///     across each frame be a linked cel.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         For more information on linked cells, see the documentation provided
        ///         <a href="https://www.aseprite.org/docs/linked-cels/">here</a>
        ///     </para>
        /// </remarks>
        PreferLinkedCels = 16,

        /// <summary>
        ///     When this flag is set, indicates that the layer is collapsed.
        /// </summary>
        Collapsed = 32,

        /// <summary>
        ///     When this flag is set, indicats that this layer is a reference layer.
        /// </summary>
        /// <remarks>
        ///     Reference layers contain images that users use as a reference when creating the
        ///     actual artwork. As such, reference layers will be ignored and not processed on
        ///     import.
        /// </remarks>
        Reference = 64
    }
}
