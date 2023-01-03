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
using System.Collections.ObjectModel;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a named region slice in an Aseprite image.
/// </summary>
public sealed class AsepriteSlice
{
    private List<AsepriteSliceKey> _keys = new();

    /// <summary>
    ///     Indicates whether the keys of this slice have nine-patch values.
    /// </summary>
    public bool IsNinePatch { get; }

    /// <summary>
    ///     Indicates whether the keys of this slice have pivot values.
    /// </summary>
    public bool HasPivot { get; }

    /// <summary>
    ///     The name of this slice.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     A read-only collection of all slice keys for this slice.
    /// </summary>
    public ReadOnlyCollection<AsepriteSliceKey> Keys => _keys.AsReadOnly();

    /// <summary>
    ///     The total number of slice keys in this slice.
    /// </summary>
    public int KeyCount => _keys.Count;

    /// <summary>
    ///     Returns the slice key at the specified index in this slice.
    /// </summary>
    /// <param name="index">
    ///     The index of the slice key.
    /// </param>
    /// <returns>
    ///     The slice key at the specified index.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than
    ///     or equal to the total number of keys in this slice.
    /// </exception>
    public AsepriteSliceKey this[int index]
    {
        get
        {
            if (index < 0 || index >= KeyCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _keys[index];
        }
    }

    /// <summary>
    ///     The custom user data that was set for this slice in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new();


    internal AsepriteSlice(string name, bool isNine, bool hasPivot)
    {
        Name = name;
        IsNinePatch = isNine;
        HasPivot = hasPivot;
    }

    internal void AddKey(AsepriteSliceKey key) => _keys.Add(key);
}
