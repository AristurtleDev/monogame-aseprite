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
namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class AsepriteSlice
{
    internal bool IsNinePatch { get; }
    internal bool HasPivot { get; }
    internal string Name { get; }
    internal List<AsepriteSliceKey> Keys { get; } = new();
    internal int KeyCount => Keys.Count;
    internal AsepriteUserData UserData { get; } = new();

    internal AsepriteSliceKey this[int index]
    {
        get
        {
            if(index < 0 || index >= KeyCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return Keys[index];
        }
    }

    internal AsepriteSlice(string name, bool isNine, bool hasPivot)
    {
        Name = name;
        IsNinePatch = isNine;
        HasPivot = hasPivot;
    }
}
