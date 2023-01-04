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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

public static class SpriteBatchExtensions
{
    /// <summary>
    ///     Renders the sprite using the given SpriteBatch with the values
    ///     from the properties of the sprite.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The Sprite instance to render.
    /// </param>
    public static void Render(this SpriteBatch spriteBatch, Sprite sprite) =>
        sprite.Render(spriteBatch);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The Sprite instance that contains the texture to render.
    /// </param>
    /// <param name="destinationRectangle">
    ///     The rectangular area that defines the destination to render the
    ///     texture of this sprite.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    public static void Render(this SpriteBatch spriteBatch, Sprite sprite, Rectangle destinationRectangle, Color color) =>
        sprite.Render(spriteBatch, destinationRectangle, color);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The Sprite instance that contains the texture to render.
    /// </param>
    /// <param name="destinationRectangle">
    ///     The rectangular area that defines the destination to render the
    ///     texture of this sprite.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    public static void Render(this SpriteBatch spriteBatch, Sprite sprite, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color) =>
        sprite.Render(spriteBatch, destinationRectangle, sourceRectangle, color);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The Sprite instance that contains the texture to render.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render the texture at.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    public static void Render(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Rectangle? sourceRectangle, Color color) =>
        sprite.Render(spriteBatch, position, sourceRectangle, color);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The Sprite instance that contains the texture to render.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render the texture at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    public static void Render(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color color) =>
        sprite.Render(spriteBatch, position, color);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The Sprite instance that contains the texture to render.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render the texture at.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation to apply to the texture when rendering.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate origin point.
    /// </param>
    /// <param name="scale">
    ///     The scale to render the texture at.
    /// </param>
    /// <param name="effects">
    ///     The SpriteEffects to apply to the texture when rendering.
    /// </param>
    /// <param name="layerDepth">
    ///     THe layer depth to apply when rendering the texture.
    /// </param>
    public static void Render(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
        sprite.Render(spriteBatch, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The Sprite instance that contains the texture to render.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to render the texture at.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation to apply to the texture when rendering.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate origin point.
    /// </param>
    /// <param name="scale">
    ///     The x- and y-scale value to use when rendering the texture.
    /// </param>
    /// <param name="effects">
    ///     The SpriteEffects to apply to the texture when rendering.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the texture.
    /// </param>
    public static void Render(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
        sprite.Render(spriteBatch, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Renders the Texture of this sprite using the given SpriteBatch with
    ///     the given parameters.
    /// </summary>
    /// <remarks>
    ///     This render method ignores the properties set for this Sprite
    ///     instance and instead uses only the parameters given as if you were
    ///     just rendering the texture normally using the SpriteBatch.
    /// </remarks>
    /// <param name="spriteBatch">
    ///     The SpriteBatch instance to use when rendering this sprite.
    /// </param>
    /// <param name="sprite">
    ///     The Sprite instance that contains the texture to render.
    /// </param>
    /// <param name="destinationRectangle">
    ///     The rectangular area that defines the destination to render the
    ///     texture of this sprite.
    /// </param>
    /// <param name="sourceRectangle">
    ///     The rectangular area within the texture to be rendered.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation to apply to the texture when rendering.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate origin point.
    /// </param>
    /// <param name="effects">
    ///     The SpriteEffects to apply to the texture when rendering.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the texture.
    /// </param>
    public static void Render(this SpriteBatch spriteBatch, Sprite sprite, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
        sprite.Render(spriteBatch, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
}
