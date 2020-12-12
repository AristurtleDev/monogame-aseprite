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

namespace MonoGame.Aseprite.Graphics
{
    /// <summary>
    ///     Represents the definition of an animation
    /// </summary>
    public struct Animation
    {
        /// <summary>
        ///     The name of this animation.
        /// </summary>
        public string Name;

        /// <summary>
        ///     The index of the frame that this animation starts on.
        ///     The starting frame
        /// </summary>
        public int From;

        /// <summary>
        ///     The index of the frame that this animation ends on.
        /// </summary>
        public int To;

        /// <summary>
        ///     The direciton in which the animation loops through its frames.
        /// </summary>
        public AnimationLoopDirection Direction;

        /// <summary>
        ///     A value indicating if the animation is a one-shot animation, meaning
        ///     that when it finished playing, it should stop animation on the last frame
        ///     and not loop.
        /// </summary>
        public bool IsOneShot;

        /// <summary>
        ///     Creates a new <see cref="Animation"/> structure with a forward
        ///     animation direction.
        /// </summary>
        /// <param name="name">
        ///     The name of this animation.
        /// </param>
        /// <param name="from">
        ///     The index of the frame that this animation starts on.
        /// </param>
        /// <param name="to">
        ///     The index of the frame that this animation ends on.
        /// </param>
        public Animation(string name, int from, int to)
        {
            Name = name;
            From = from;
            To = to;
            Direction = AnimationLoopDirection.Forward;
            IsOneShot = false;
        }

        /// <summary>
        ///     Creates a new <see cref="Animation"/> structure
        /// </summary>
        /// <param name="name">
        ///     The name of this animation.
        /// </param>
        /// <param name="from">
        ///     The index of the frame that this animation starts on.
        /// </param>
        /// <param name="to">
        ///     The index of the frame that this animation ends on.
        /// </param>
        /// <param name="direction">
        ///     The direction in which the animation loops through its frames.
        /// </param>
        public Animation(string name, int from, int to, AnimationLoopDirection direction)
        {
            Name = name;
            From = from;
            To = to;
            Direction = direction;
            IsOneShot = false;
        }

        /// <summary>
        ///     Creates a new <see cref="Animation"/> structure
        /// </summary>
        /// <param name="name">
        ///     The name of this animation.
        /// </param>
        /// <param name="from">
        ///     The index of the frame that this animation starts on.
        /// </param>
        /// <param name="to">
        ///     The index of the frame that this animation ends on.
        /// </param>
        /// <param name="direction">
        ///     The direction in which the animation loops through its frames.
        /// </param>
        /// <param name="isOneShot">
        ///     A value indicating if the animation should only play once and not loop.
        /// </param>
        public Animation(string name, int from, int to, AnimationLoopDirection direction, bool isOneShot)
        {
            Name = name;
            From = from;
            To = to;
            Direction = direction;
            IsOneShot = isOneShot;
        }
    }
}
