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
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

/// <summary>
///     Extension methods for the SpriteBatch that makes uses of the built-in
///     classes in MonoGame.Aseprite.
/// </summary>
public static class SpriteBatchExtensions
{
    /// <summary>
    ///     Draws this <see cref="Sprite"/> using the given
    ///     <see cref="SpriteBatch"/>
    /// </summary>
    /// <remarks>
    ///     The values for position, color, rotation, origin, scale, effects,
    ///     and layer depth that are passed to the SpriteBatch are derived from
    ///     the properties of this <see cref="Sprite"/> instance when using this
    ///     method.  To use different values, use one of the other Draw method
    ///     overloads instead.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when drawing this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The <see cref="Sprite"/> to draw.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Sprite sprite) =>
        sprite.Draw(spriteBatch);

    /// <summary>
    ///     Draws this <see cref="Sprite"/> using the given
    ///     <see cref="SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The <see cref="Sprite"/> to draw.
    /// </param>
    /// <param name="position">
    ///     If a value is provided, overrides the <see cref="Sprite.Position"/>
    ///     value during this render only.
    /// </param>
    /// <param name="color">
    ///     If a value is provided, overrides the <see cref="Sprite.Color"/>
    ///     value during this render only.
    /// </param>
    /// <param name="rotation">
    ///     If a value is provided, overrides the <see cref="Sprite.Rotation"/>
    ///     value during this render only.
    /// </param>
    /// <param name="origin">
    ///     If a value is provided, overrides the <see cref="Sprite.Origin"/>
    ///     value during this render only.
    /// </param>
    /// <param name="scale">
    ///     If a value is provided, overrides the <see cref="Sprite.Scale"/>
    ///     value during this render only.
    /// </param>
    /// <param name="effects">
    ///     If a value is provided, overrides the
    ///     <see cref="Sprite.SpriteEffects"/> value during this render only.
    /// </param>
    /// <param name="layerDepth">
    ///     If a value is provided, overrides the
    ///     <see cref="Sprite.LayerDepth"/> value during this render only.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2? position = default, Color? color = default, float? rotation = default, Vector2? origin = default, Vector2? scale = default, SpriteEffects? effects = default, float? layerDepth = default) =>
        sprite.Draw(spriteBatch, position, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Draws this <see cref="Sprite"/> using the given
    ///     <see cref="SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The <see cref="Sprite"/> to draw.
    /// </param>
    /// <param name="destinationRectangle">
    ///     If a value is provided, overrides the <see cref="Sprite.Position"/>
    ///     and <see cref="Sprite.Scale"/> values to fit the sprite render into
    ///     the bounds given during this render only.
    /// </param>
    /// <param name="color">
    ///     If a value is provided, overrides the <see cref="Sprite.Color"/>
    ///     value during this render only.
    /// </param>
    /// <param name="rotation">
    ///     If a value is provided, overrides the <see cref="Sprite.Rotation"/>
    ///     value during this render only.
    /// </param>
    /// <param name="origin">
    ///     If a value is provided, overrides the <see cref="Sprite.Origin"/>
    ///     value during this render only.
    /// </param>
    /// <param name="effects">
    ///     If a value is provided, overrides the
    ///     <see cref="Sprite.SpriteEffects"/> value during this render only.
    /// </param>
    /// <param name="layerDepth">
    ///     If a value is provided, overrides the
    ///     <see cref="Sprite.LayerDepth"/> value during this render only.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Rectangle? destinationRectangle = default, Color? color = default, float? rotation = default, Vector2? origin = default, SpriteEffects? effects = default, float? layerDepth = default) =>
        sprite.Draw(spriteBatch, destinationRectangle, color, rotation, origin, effects, layerDepth);
}
