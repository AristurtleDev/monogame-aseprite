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
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Documents
{
    /// <summary>
    ///     A class that provides the information imported from an Aseprite file
    ///     through the content pipeline.
    /// </summary>
    public class AsepriteDocument : IDisposable
    {
        //  Holds a value indicating if this instance has been diposed.
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
        ///     Gest the collection of frames.
        /// </summary>
        public List<AsepriteFrame> Frames { get; internal set; }

        /// <summary>
        ///     Gets the collection of defined slices, with the dictionary key
        ///     being the name of the slice.
        /// </summary>
        public Dictionary<string, AsepriteSlice> Slices { get; internal set; }

        /// <summary>
        ///     Gets the collection of defined animations, with the dictionary key
        ///     being the name of the animation.
        /// </summary>
        public Dictionary<string, AsepriteTag> Tags { get; internal set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteDocument"/> instance.
        /// </summary>
        internal AsepriteDocument()
        {
            Frames = new List<AsepriteFrame>();
            Slices = new Dictionary<string, AsepriteSlice>();
            Tags = new Dictionary<string, AsepriteTag>();
        }

        /// <summary>
        ///     Gracefully disposes of resources that are managed by this instance.
        /// </summary>
        public void Dispose() => Dispose(true);

        /// <summary>
        ///     Gracefully disposes of resources that are managed by this instance.
        /// </summary>
        /// <param name="disposing">
        ///     A value indicating if the resources managed by this instance should
        ///     be disposed.
        /// </param>
        protected void Dispose(bool disposing)
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
    }
}
