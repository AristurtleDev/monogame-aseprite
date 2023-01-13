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

using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

public sealed class Tilemap
{
    private List<TilemapLayer> _layers = new();
    private Dictionary<string, TilemapLayer> _layersByName = new();
    private Vector2 _position;

    /// <summary>
    ///     Gets the name of this <see cref="Tilemap"/>.
    /// </summary>
    public string Name { get; }

    public Tileset Tileset { get; }

    /// <summary>
    ///     Gets or Sets the x- and y-coordinate position to render this
    ///     <see cref="Tilemap"/> at.
    /// </summary>
    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    /// <summary>
    ///     Gets or Sets the x-coordinate position to render this
    ///     <see cref="Tilemap"/> at.
    /// </summary>
    public float X
    {
        get => _position.X;
        set => _position.X = value;
    }

    /// <summary>
    ///     Gets or Sets the y-coordinate position to render this
    ///     <see cref="Tilemap"/> at.
    /// </summary>
    public float Y
    {
        get => _position.Y;
        set => _position.Y = value;
    }

    public Tilemap(string name, Tileset tileSet)
        : this(name, tileSet, Vector2.Zero) { }

    public Tilemap(string name, Tileset tileset, Vector2 position)
    {
        Name = name;
        Tileset = tileset;
        _position = position;
    }

    public void AddLayer(string name, Tileset tileset, Vector2 relativePosition, int[] tiles)
    {

    }
}
