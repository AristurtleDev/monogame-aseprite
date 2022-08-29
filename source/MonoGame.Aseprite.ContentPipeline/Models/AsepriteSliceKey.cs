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

using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values for a slice key in an Aseprite file.
    /// </summary>
    /// <remarks>
    ///     A slice key defines the rectangular bounds of a slice during a
    ///     specific frame of animation.
    ///     <para>
    ///         Aseprite Slice Key documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#slice-chunk-0x2022">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepriteSliceKey
    {
        /// <summary>
        ///     Gets the frame that this slice key is valid starting
        ///     on.
        /// </summary>
        public int Frame { get; private set; }

        /// <summary>
        ///     Gets the top-left x-coordinate position of this slice key.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        ///     Gets the top-left y-coordinate position of this slice key.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        ///     Gets the width, in pixels, of this slice key.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     Gets the height, in pixxels, of this slice key.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Gets the top-left x-coordinate position of the center
        ///     rectangle of this slice key, if the slice contains
        ///     nine-patch data.
        /// </summary>
        public int CenterX { get; private set; }

        /// <summary>
        ///     Gets the top-left y-coordinate position of the center
        ///     rectangle of this slice key, if the slice contains
        ///     nine-patch data.
        /// </summary>
        public int CenterY { get; private set; }

        /// <summary>
        ///     Gets the width, in pixels, of the center rectangle
        ///     of this slice key, if hte slice contains nine-patch data.
        /// </summary>
        public int CenterWidth { get; private set; }

        /// <summary>
        ///     Gets the height, in pixels, of the center rectangle
        ///     of this slice key, if the slice contains nine-patch data.
        /// </summary>
        public int CenterHeight { get; private set; }

        /// <summary>
        ///     Gets the x-coordinate of the pivot origin point for this
        ///     slice key if the slice contains pivot data.
        /// </summary>
        public int PivotX { get; private set; }

        /// <summary>
        ///     Gets the y-coordinate of the pivot origin point for this
        ///     slice key if the slice contains pivot data.
        /// </summary>
        public int PivotY { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteSliceKey"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        /// <param name="flags">
        ///     The <see cref="AsepriteSliceFlags"/> value of the slice this
        ///     slice key belongs to.
        /// </param>
        internal AsepriteSliceKey(AsepriteReader reader, AsepriteSliceFlags flags)
        {
            Frame = (int)reader.ReadDWORD();
            X = reader.ReadLONG();
            Y = reader.ReadLONG();
            Width = (int)reader.ReadDWORD();
            Height = (int)reader.ReadDWORD();

            if ((flags & AsepriteSliceFlags.HasNinePatch) != 0)
            {
                CenterX = reader.ReadLONG();
                CenterY = reader.ReadLONG();
                Width = (int)reader.ReadDWORD();
                Height = (int)reader.ReadDWORD();
            }

            if ((flags & AsepriteSliceFlags.HasPivot) != 0)
            {
                PivotX = reader.ReadLONG();
                PivotY = reader.ReadLONG();
            }
        }
    }
}
