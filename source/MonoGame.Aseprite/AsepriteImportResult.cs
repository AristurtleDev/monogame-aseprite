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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite
{
    public sealed class AsepriteImportResult : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        ///     Gets the <see cref="Texture2D"/> containing a packed spritesheet
        ///     of all frames from the Aseprite file.
        /// </summary>
        public Texture2D Texture { get; internal set; }

        /// <summary>
        ///     Gets the width, in pixels, of the <see cref="Texture"/>
        /// </summary>
        public int TextureWidth { get; internal set; }

        /// <summary>
        ///     Gets the height, in pixels, of hte <see cref="Texture"/>
        /// </summary>
        public int TextureHeight { get; internal set; }

        /// <summary>
        ///     Gets the collection of defined animations, with the dictionary key
        ///     being the name of the animation.
        /// </summary>
        public Dictionary<string, Animation> Animations { get; internal set; }

        /// <summary>
        ///     Gest the collection of frames.
        /// </summary>
        public List<Frame> Frames { get; internal set; }

        /// <summary>
        ///     Gets the collection of defined slices, with the dictionary key
        ///     being the name of the slice.
        /// </summary>
        public Dictionary<string, Slice> Slices { get; internal set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteImportResult"/> instance.
        /// </summary>
        internal AsepriteImportResult()
        {
            Frames = new List<Frame>();
            Animations = new Dictionary<string, Animation>();
            Slices = new Dictionary<string, Slice>();
        }

        public void Dispose() => Dispose(true);
        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (Texture != null && !Texture.IsDisposed)
                {
                    Texture.Dispose();
                    Texture = null;
                }
            }

            _isDisposed = true;
        }


        public struct Frame
        {
            /// <summary>
            ///     The top-left x-coordinate position of the frame relative
            ///     to the top-left of the texture.
            /// </summary>
            public int X;

            /// <summary>
            ///     The top-left y-coordinate position of the frame relative
            ///     to the top-left of the texture.
            /// </summary>
            public int Y;

            /// <summary>
            ///     The width, in pixels, of the frame.
            /// </summary>
            public int Width;

            /// <summary>
            ///     The height, in pixels, of the frame.
            /// </summary>
            public int Height;

            /// <summary>
            ///     The duration, in seconds, of the frame.
            /// </summary>
            public float Duration;
        }

        public struct Animation
        {
            /// <summary>
            ///     The name of the animation.
            /// </summary>
            public string Name;

            /// <summary>
            ///     The starting frame of the animation.
            /// </summary>
            public int From;

            /// <summary>
            ///     The ending frame of the animation.
            /// </summary>
            public int To;

            /// <summary>
            ///     The color of the animation as defined as the
            ///     tag color in Asperite.
            /// </summary>
            public Color Color;

            /// <summary>
            ///     A value that indicates the direction the animation is played in.
            ///     0 = Forward, 1 = Reverse, 2 = Ping Pong.
            /// </summary>
            public int Direction;
        }

        public struct Slice
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
            public Dictionary<int, SliceKey> SliceKeys;
        }

        public struct SliceKey
        {
            /// <summary>
            ///     The index of the frame that the slice is valid
            ///     starting on.
            /// </summary>
            public int FrameIndex;

            /// <summary>
            ///     The x-coordinate position of the slice relative to
            ///     the bounds of the frame.
            /// </summary>
            public int X;

            /// <summary>
            ///     The y-coordinate position of the slice relative to
            ///     the bounds of the frame.
            /// </summary>
            public int Y;

            /// <summary>
            ///     The width, in pixels, of the slice.
            /// </summary>
            public int Width;

            /// <summary>
            ///     The height, in pixels, of the slice.
            /// </summary>
            public int Height;

            /// <summary>
            ///     A value indicating if this slicekey has nine patch
            ///     data.
            /// </summary>
            public bool HasNinePatch;

            /// <summary>
            ///     The top-left x-coordinate position of the nine patch
            ///     center rect if this slice contains ninepatch data.
            /// </summary>
            public int CenterX;

            /// <summary>
            ///     The top-left y-coordinate position of the nine patch
            ///     center rect if this slice contains ninepatch data.
            /// </summary>
            public int CenterY;

            /// <summary>
            ///     The width, in pixels, of the nine patch center rect
            ///     if this slice contains ninepatch data.
            /// </summary>
            public int CenterWidth;

            /// <summary>
            ///     The height, in pixels, of the nine patch center rect
            ///     if this slice contains ninepatch data.
            /// </summary>
            public int CenterHeight;

            /// <summary>
            ///     Gets a value indicating if this slicekey has pivot data.
            /// </summary>
            public bool HasPivot;

            /// <summary>
            ///     The x-coordinate origin point of the pivot if this slice
            ///     contains pivot data.
            /// </summary>
            public int PivotX;

            /// <summary>
            ///     The y-coordinate origin point of the pivot if this slice
            ///     contains pivot data.
            /// </summary>
            public int PivotY;
        }



    }
}
