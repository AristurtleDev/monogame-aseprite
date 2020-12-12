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

namespace MonoGame.Aseprite.Documents
{
    /// <summary>
    ///     Values that define the direction in which a the animation
    ///     defined by an <see cref="AsepriteTag"/> should be played.
    /// </summary>
    public enum AsepriteTagDirection
    {
        /// <summary>
        ///     The animation shoudl be played in a forward direction from
        ///     the starting frame to the ending frame.
        /// </summary>
        Forward = 0,

        /// <summary>
        ///     The aniamtion should be played in a reverse direction from
        ///     the ending frame to the starting frame.
        /// </summary>
        Reverse = 1,

        /// <summary>
        ///     The animation should ping pong the direciton played; First it should
        ///     play forward from the starting frame to the ending frame, then it should
        ///     play in reverse from the ending frame to the starting frame.
        /// </summary>
        PingPing = 2,

        /// <summary>
        ///     The animation should only play from the starting frame to the ending frame
        ///     and the stop animation on the ending frame.
        /// </summary>
        OneShot
    }
}
