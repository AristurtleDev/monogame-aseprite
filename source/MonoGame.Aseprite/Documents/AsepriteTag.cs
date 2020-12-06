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

using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.Documents
{
    /// <summary>
    ///     A class that provides the information about an animation tag
    ///     for an <see cref="AsepriteDocument"/>.
    /// </summary>
    public sealed class AsepriteTag
    {
        /// <summary>
        ///     Gets the name of the animation.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        ///     Gets the starting frame of the animation.
        /// </summary>
        public int From { get; internal set; }

        /// <summary>
        ///     Gets the ending frame of the animation.
        /// </summary>
        public int To { get; internal set; }

        /// <summary>
        ///     Gets the color of the animation as defined as the
        ///     tag color in Asperite.
        /// </summary>
        public Color Color { get; internal set; }

        /// <summary>
        ///     A value that indicates the direction the animation is played in.
        ///     0 = Forward, 1 = Reverse, 2 = Ping Pong.
        /// </summary>
        public AsepriteTagDirection Direction { get; internal set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteTag"/> instance.
        /// </summary>
        internal AsepriteTag() { }
    }
}
