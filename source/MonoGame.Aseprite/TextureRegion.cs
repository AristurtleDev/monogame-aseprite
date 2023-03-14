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
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a named rectangular region that represents the location and extents of a region within a source texture.
/// </summary>
public class TextureRegion
{
    private Dictionary<string, Slice> _objects = new();

    /// <summary>
    ///     Gets the name assigned to this <see cref="TextureRegion"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the source texture used by this <see cref="TextureRegion"/>.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    ///     Gets the rectangular bounds that define the location and width and height extents, in pixels, of the region
    ///     within the source texture that is represented by this <see cref="TextureRegion"/>.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextureRegion"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="texture">
    ///     The source texture image this region is from.
    /// </param>
    /// <param name="bounds">
    ///     The rectangular bounds of this region within the source texture.
    /// </param>
    public TextureRegion(string name, Texture2D texture, Rectangle bounds) =>
        (Name, Texture, Bounds) = (name, texture, bounds);


    /// <summary>
    ///     Creates and adds a new <see cref="Slice"/> element to this <see cref="TextureRegion"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the <see cref="Slice"/> that is created by this method.  The name must be
    ///     unique across all <see cref="Slice"/> elements in this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="bounds">
    ///     The bounds to assign the <see cref="Slice"/> created by this method.  This should be relative to the bounds
    ///     of this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate origin point to assign the <see cref="Slice"/> created by this method.
    ///     This should be relative to the upper-left corner of the bounds of this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Microsoft.Xna.Framework.Color"/> value to assign the <see cref="Slice"/> created
    ///     by this method.
    /// </param>
    /// <returns>
    ///     The <see cref="Slice"/> created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="TextureRegion"/> already contains a <see cref="Slice"/> with the
    ///     specified name.
    /// </exception>
    public Slice CreateSlice(string name, Rectangle bounds, Vector2 origin, Color color)
    {
        Slice slice = new(name, bounds, origin, color);
        AddSlice(slice);
        return slice;
    }

    /// <summary>
    ///     Creates and adds a new <see cref="NinePatchSlice"/> element to this <see cref="TextureRegion"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the <see cref="NinePatchSlice"/> that is created by this method.  The name must be
    ///     unique across all <see cref="Slice"/> elements in this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="bounds">
    ///     The bounds to assign the <see cref="NinePatchSlice"/> created by this method.  This should be relative to 
    ///     the bounds of this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="centerBounds">
    ///     The center bounds to assign the <see cref="NinePatchSlice"/> created by this method.  This should be
    ///     relative to the <paramref name="bounds"/>.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate origin point to assign the <see cref="NinePatchSlice"/> created by this method.
    ///     This should be relative to the upper-left corner of the bounds of this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="color">
    ///     A <see cref="Microsoft.Xna.Framework.Color"/> value to assign the <see cref="NinePatchSlice"/> created
    ///     by this method.
    /// </param>
    /// <returns>
    ///     The <see cref="NinePatchSlice"/> created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="TextureRegion"/> already contains a <see cref="Slice"/> with the
    ///     specified name.
    /// </exception>
    public NinePatchSlice CreateNinePatchSlice(string name, Rectangle bounds, Rectangle centerBounds, Vector2 origin, Color color)
    {
        NinePatchSlice slice = new(name, bounds, centerBounds, origin, color);
        AddSlice(slice);
        return slice;
    }

    private void AddSlice(Slice slice)
    {
        if (_objects.ContainsKey(slice.Name))
        {
            throw DuplicateSliceNameException(slice.Name);
        }

        _objects.Add(slice.Name, slice);
    }

    /// <summary>
    ///     Returns the <see cref="Slice"/> element with the specified name from this <see cref="TextureRegion"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="Slice"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="Slice"/> element located.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="TextureRegion"/> does not contain a <see cref="Slice"/> element with the specified
    ///     name.
    /// </exception>
    public Slice GetSlice(string name)
    {
        if (_objects.TryGetValue(name, out Slice? slice))
        {
            return slice;
        }

        throw SliceNameNotFoundException(name);
    }

    public bool TryGetSlice(string name, out Slice? slice) => _objects.TryGetValue(name, out slice);

    /// <summary>
    ///     Returns the <see cref="Slice"/> element with the specified name from this <see cref="TextureRegion"/> as the
    ///     type specified.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to return the located <see cref="Slice"/> element as.  Must derived from the base type 
    ///     <see cref="Slice"/>.
    /// </typeparam>
    /// <param name="name">
    ///     The name of the <see cref="Slice"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="Slice"/> element located as the type specified.
    /// </returns>
    public T GetSlice<T>(string name) where T : Slice
    {
        if (_objects.TryGetValue(name, out Slice? slice))
        {
            return (T)slice;
        }

        throw SliceNameNotFoundException(name);
    }

    public bool TryGetSlice<T>(string name, out T? slice) where T : Slice
    {
        slice = null;

        var result = _objects.TryGetValue(name, out var objSlice);

        if (result && objSlice != null)
        {
            slice = (T)objSlice;
        }

        return result;
    }

    /// <summary>
    ///     Removes the <see cref="Slice"/> element with the specified name from this <see cref="TextureRegion"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="Slice"/> element to remove.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Slice"/> element was successfully removed; otherwise,
    ///     <see langword="false"/>.  This method returns <see langword="false"/> when this <see cref="TextureRegion"/>
    ///     does not have a <see cref="Slice"/> element with the specified name.
    /// </returns>
    public bool RemoveSlice(string name) => _objects.Remove(name);

    /// <summary>
    ///     Removes all <see cref="Slice"/> elements from this <see cref="TextureRegion"/>.
    /// </summary>
    public void RemoveAllSlices() => _objects.Clear();

    /// <summary>
    ///     Draws this <see cref="TextureRegion"/> instance using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="destinationRectangle">
    ///     A rectangular bound that defines the destination to render this <see cref="TextureRegion"/> into.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color) =>
        spriteBatch.Draw(this, destinationRectangle, color);

    /// <summary>
    ///     Draws this <see cref="TextureRegion"/> instance using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="TextureRegion"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color) =>
        spriteBatch.Draw(this, position, color);

    /// <summary>
    ///     Draws this <see cref="TextureRegion"/> using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="TextureRegion"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate point of origin to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
    ///     flipping when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(this, position, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Draws this <see cref="TextureRegion"/> using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="TextureRegion"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate point of origin to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
    ///     flipping when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(this, position, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Draws this <see cref="TextureRegion"/> using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="destinationRectangle">
    ///     A rectangular bound that defines the destination to render this <see cref="TextureRegion"/> into.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate point of origin to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
    ///     flipping when rendering this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="TextureRegion"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(this, destinationRectangle, color, rotation, origin, effects, layerDepth);

    private InvalidOperationException DuplicateSliceNameException(string name)
    {
        string message = $"This {nameof(TextureRegion)} already contains a {nameof(Slice)} with the name '{name}'.";
        return new(message);
    }
    private KeyNotFoundException SliceNameNotFoundException(string name)
    {
        string message = $"This {nameof(TextureRegion)} does not contain a {nameof(Slice)} with the name '{name}'.";
        return new KeyNotFoundException(message);
    }
}
