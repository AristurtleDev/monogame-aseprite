/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
///     Defines a processor that processes a  <see cref="RawAnimationTag"/> from a tag in an 
///     <see cref="AsepriteFile"/>.
/// </summary>
public static class RawAnimationTagProcessor
{
    /// <summary>
    ///     Processes the <see cref="AsepriteTag"/> at the specified index in the given <see cref="AsepriteFile"/> as a  
    ///     <see cref="RawAnimationTag"/>.
    /// </summary>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> that contains the tag to process.
    /// </param>
    /// <param name="tagIndex">
    ///     The index of the <see cref="AsepriteTag"/> to locate in the  <see cref="AsepriteFile"/>.
    /// </param>
    /// <returns>
    ///     The  <see cref="RawAnimationTag"/> created by this method.
    /// </returns>
    public static RawAnimationTag Process(AsepriteFile file, int tagIndex)
    {
        AsepriteTag aseTag = file.GetTag(tagIndex);
        return Process(aseTag, file.Frames);
    }

    /// <summary>
    ///     Processes the <see cref="AsepriteTag"/> with the specified name in the given <see cref="AsepriteFile"/> as a
    ///     <see cref="RawAnimationTag"/>.
    /// </summary>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> that contains the <see cref="AsepriteTag"/> to process.
    /// </param>
    /// <param name="tagName">
    ///     The name of the <see cref="AsepriteTag"/> to locate in the <see cref="AsepriteFile"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="RawAnimationTag"/> created by this method.
    /// </returns>
    public static RawAnimationTag Process(AsepriteFile file, string tagName)
    {
        AsepriteTag aseTag = file.GetTag(tagName);
        return Process(aseTag, file.Frames);
    }

    internal static RawAnimationTag Process(AsepriteTag aseTag, ReadOnlySpan<AsepriteFrame> aseFrames)
    {
        int frameCount = aseTag.To - aseTag.From + 1;
        RawAnimationFrame[] rawAnimationFrames = new RawAnimationFrame[frameCount];
        int[] frames = new int[frameCount];
        int[] durations = new int[frameCount];

        for (int i = 0; i < frameCount; i++)
        {
            int index = aseTag.From + i;
            rawAnimationFrames[i] = new(index, aseFrames[index].DurationInMilliseconds);
        }

        // In aseprite, all tags are looping
        bool isLooping = true;
        bool isReversed = aseTag.Direction == AsepriteLoopDirection.Reverse;
        bool isPingPong = aseTag.Direction == AsepriteLoopDirection.PingPong;

        return new(aseTag.Name, rawAnimationFrames, isLooping, isReversed, isPingPong);
    }
}