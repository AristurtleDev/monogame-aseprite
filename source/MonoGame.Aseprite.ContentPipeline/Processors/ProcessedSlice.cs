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

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.ContentPipeline.Processors
{
    /// <summary>
    ///     Defines the values of an Aseprite slice that has been
    ///     processed and is ready to be written out.
    /// </summary>
    public struct ProcessedSlice
    {
        /// <summary>
        ///     The name of the slice.
        /// </summary>
        public string Name;

        /// <summary>
        ///     The color of the slice.
        /// </summary>
        public Color Color;

        /// <summary>
        ///     The dictionary of slicekeys where the dictionary key
        ///     is the frme the slice is valid on.
        /// </summary>
        public Dictionary<int, ProcessedSliceKey> SliceKeys;
    }
}
