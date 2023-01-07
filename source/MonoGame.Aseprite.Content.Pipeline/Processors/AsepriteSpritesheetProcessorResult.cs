/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

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
using System.Collections.Immutable;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Represents the result of the <see cref="AsepriteSpritesheetProcessor"/>.
/// </summary>
/// <param name="Name">
///     The name of the Aseprite file without extension.
/// </param>
/// <param name="Size">
///     The width and height extents, in pixels, of the generated spritesheet
///     image.
/// </param>
/// <param name="Pixels">
///     An <see cref="ImmutableArray{T}"/> of
///     <see cref="Microsoft.Xna.Framework.Color"/> values that represent the
///     pixels of the generated spritesheet image.
/// </param>
/// <param name="Frames">
///     An <see cref="ImmutableArray{T}"/> of <see cref="Frame"/> elements for
///     each frame in the spritesheet.
/// </param>
/// <param name="Tags">
///     An <see cref="ImmutableArray{T}"/> of <see cref="Tag"/> elements for
///     each tag in the spritesheet.
/// </param>
/// <param name="Slices">
///     An <see cref="ImmutableArray{T}"/> of <see cref="Slice"/> elements for
///     each slice in the spritesheet.
/// </param>
public sealed record AsepriteSpritesheetProcessorResult(string Name, Size Size, ImmutableArray<Color> Pixels, ImmutableArray<Frame> Frames, ImmutableArray<Tag> Tags, ImmutableArray<Slice> Slices);

// public sealed class AsepriteSpritesheetProcessorResult
// {
//     internal string Name { get; }
//     internal int Width { get; }
//     internal int Height { get; }
//     internal Color[] Pixels { get; }
//     internal List<Frame> Frames { get; }
//     internal List<Tag> Tags { get; }
//     internal List<Slice> Slices { get; }

//     internal AsepriteSpritesheetProcessorResult(string name, int width, int height, Color[] pixels, List<Frame> frames, List<Tag> tags, List<Slice> slices)
//     {
//         Name = name;
//         Width = width;
//         Height = height;
//         Pixels = pixels;
//         Frames = frames;
//         Tags = tags;
//         Slices = slices;
//     }
// }
