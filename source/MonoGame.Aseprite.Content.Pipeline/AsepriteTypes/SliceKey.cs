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

using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class SliceKey
{
    internal int FrameIndex { get; }
    internal Rectangle Bounds { get; }
    internal Point Size => Bounds.Size;
    internal Rectangle? CenterBounds { get; }
    internal Point? Pivot { get; }

    [MemberNotNullWhen(true, nameof(CenterBounds))]
    internal bool IsNinePatch => CenterBounds is not null;

    [MemberNotNullWhen(true, nameof(Pivot))]
    internal bool HasPivot => Pivot is not null;

    internal SliceKey(int frameIndex, Rectangle bounds, Rectangle? centerBounds, Point? pivot) =>
        (FrameIndex, Bounds, CenterBounds, Pivot) = (frameIndex, bounds, centerBounds, pivot);
}
