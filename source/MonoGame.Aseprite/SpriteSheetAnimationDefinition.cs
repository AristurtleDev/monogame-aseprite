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
    ///     Gets the collection of <see cref="SpriteSheetAnimationFrame"/>
    ///     elements in this <see cref="SpriteSheetAnimationDefinition"/>.
    /// </summary>
    public List<SpriteSheetAnimationFrame> Frames { get; } = new();

    /// <summary>
    ///     Gets the name of this <see cref="SpriteSheetAnimationDefinition"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation defined by
    ///     this <see cref="SpriteSheetAnimationDefinition"/> should loop.
    /// </summary>
    public bool IsLooping { get; set; } = true;

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation defined by
    ///     this <see cref="SpriteSheetAnimationDefinition"/> should play in
    ///     reverse.
    /// </summary>
    public bool IsReversed { get; set; } = false;

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation defined by
    ///     this <see cref="SpriteSheetAnimationDefinition"/> should ping-pong.
    /// </summary>
    public bool IsPingPong { get; set; } = false;

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="SpriteSheetAnimationDefinition"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name of this <see cref="SpriteSheetAnimationDefinition"/>.
    /// </param>
    public SpriteSheetAnimationDefinition(string name) => Name = name;

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="SpriteSheetAnimationDefinition"/> class with the
    ///     collection of <see cref="SpriteSheetAnimationFrame"/> elements
    ///     provided.
    /// </summary>
    /// <param name="name">
    ///     The name of this <see cref="SpriteSheetAnimationDefinition"/>.
    /// </param>
    /// <param name="frames">
    ///     The collection who's <see cref="SpriteSheetAnimationFrame"/>
    ///     elements should be added to this
    ///     <see cref="SpriteSheetAnimationDefinition"/>
    /// </param>
    public SpriteSheetAnimationDefinition(string name, IEnumerable<SpriteSheetAnimationFrame> frames)
        : this(name) => Frames.AddRange(frames);
}
