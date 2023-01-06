/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
namespace MonoGame.Aseprite;

public sealed class SpriteSheetAnimationDefinition
{
    /// <summary>
    ///     Gets an array that contains the indexes of the
    ///     <see cref="SpriteSheetFrame"/> elements that are played in the
    ///     animation defined by this
    ///     <see cref="SpriteSheetAnimationDefinition"/>.  Order of indexes is
    ///     the order of the frames in the animation if it was playing from
    ///     start to end in a forward direction.
    /// </summary>
    public int[] FrameIndexes { get; }

    /// <summary>
    ///     Gets the name of this <see cref="SpriteSheetAnimationDefinition"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets whether the animation defined by this
    ///     <see cref="SpriteSheetAnimationDefinition"/> should loop.
    /// </summary>
    public bool IsLooping { get; }

    /// <summary>
    ///     Gets whether the animation defined by this
    ///     <see cref="SpriteSheetAnimationDefinition"/> should play in reverse.
    /// </summary>
    public bool IsReversed { get; }

    /// <summary>
    ///     Gets whether the animation defined by this
    ///     <see cref="SpriteSheetAnimationDefinition"/> should ping-pong.
    /// </summary>
    public bool IsPingPong { get; }


    internal SpriteSheetAnimationDefinition(string name, bool isLooping, bool isReversed, bool isPingPong, int[] indexes) =>
        (Name, IsLooping, IsReversed, IsPingPong, FrameIndexes) = (name, isLooping, isReversed, isPingPong, indexes);
}
