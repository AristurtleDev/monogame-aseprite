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
///     Defines extension methods for the sprite batch for rendering graphic resource types in this library.
/// </summary>
public static class SpriteBatchExtensions
{
    #region Texture Region

    /// <summary>
    /// Draws a texture region using the sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
    /// <param name="region">The texture region to render.</param>
    /// <param name="destinationRectangle">
    /// A rectangular bound that defines the destination to render the texture region into.
    /// </param>
    /// <param name="color">The color mask to apply when rendering the texture region.</param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Rectangle destinationRectangle, Color color) =>
        spriteBatch.Draw(region.Texture, destinationRectangle, region.Bounds, color);

    /// <summary>
    /// Draws a texture region using the sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
    /// <param name="region">The texture region to render.</param>
    /// <param name="position">The x- and y-coordinate location to render the texture region at.</param>
    /// <param name="color">The color mask to apply when rendering the texture region.</param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color) =>
        spriteBatch.Draw(region.Texture, position, region.Bounds, color);

    /// <summary>
    /// Draws a texture region using the sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
    /// <param name="region">The texture region to render.</param>
    /// <param name="position">The x- and y-coordinate location to render the texture region at.</param>
    /// <param name="color">The color mask to apply when rendering the texture region.</param>
    /// <param name="rotation">The amount of rotation, in radians, to apply when rendering the texture region.</param>
    /// <param name="origin">The x- and y-coordinate point of origin to apply when rendering the texture region.</param>
    /// <param name="scale">The amount of scaling to apply when rendering the texture region.</param>
    /// <param name="effects">
    /// The sprite effects for horizontal and vertical axis flipping to apply when rendering the texture region.
    /// </param>
    /// <param name="layerDepth">The layer depth to apply when rendering the texture region.</param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(region.Texture, position, region.Bounds, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    /// Draws a texture region using the sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
    /// <param name="region">The texture region to render.</param>
    /// <param name="position">The x- and y-coordinate location to render the texture region at.</param>
    /// <param name="color">The color mask to apply when rendering the texture region.</param>
    /// <param name="rotation">The amount of rotation, in radians, to apply when rendering the texture region.</param>
    /// <param name="origin">The x- and y-coordinate point of origin to apply when rendering the texture region.</param>
    /// <param name="scale">The amount of scaling to apply when rendering the texture region.</param>
    /// <param name="effects">
    /// The sprite effects for horizontal and vertical axis flipping to apply when rendering the texture region.
    /// </param>
    /// <param name="layerDepth">The layer depth to apply when rendering the texture region.</param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(region.Texture, position, region.Bounds, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    /// Renders a texture region using the sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for rendering the texture region.</param>
    /// <param name="region">The texture region to render.</param>
    /// <param name="destinationRectangle">
    /// A rectangular bound that defines the destination to render the texture region into
    /// .</param>
    /// <param name="color">The color mask to apply when rendering the texture region.</param>
    /// <param name="rotation">The amount of rotation, in radians, to apply when rendering the texture region.</param>
    /// <param name="origin">The x- and y-coordinate point of origin to apply when rendering the texture region.</param>
    /// <param name="effects">
    /// The sprite effects for horizontal and vertical axis flipping to apply when rendering the texture region.
    /// </param>
    /// <param name="layerDepth">The layer depth to apply when rendering the texture region.</param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(region.Texture, destinationRectangle, region.Bounds, color, rotation, origin, effects, layerDepth);

    #endregion Texture Region

    /// <summary>
    /// Renders a sprite using the sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for rendering the texture region.</param>
    /// <param name="sprite">The sprite to render.</param>
    /// <param name="position">The x- and y-coordinate location to render the sprite at.</param>
    public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position) =>
        spriteBatch.Draw(sprite.TextureRegion, position, sprite.Color * sprite.Transparency, sprite.Rotation, sprite.Origin, sprite.Scale, sprite.SpriteEffects, sprite.LayerDepth);


    #region Tilemap

    /// <summary>
    ///     Draws a <see cref="Tilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The instance of the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> class to use for rendering.
    /// </param>
    /// <param name="layer">
    ///     The instance of the <see cref="Tilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw the <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when drawing the <see cref="Tilemap"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Tilemap tilemap, Vector2 position, Color color) =>
        Draw(spriteBatch, tilemap, position, color, Vector2.One, 0.0f);

    /// <summary>
    ///     Draws a <see cref="Tilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The instance of the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> class to use for rendering.
    /// </param>
    /// <param name="layer">
    ///     The instance of the <see cref="Tilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw the <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when drawing the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply to both the x-axis (horizontal) and y-axis (vertical) when drawing the
    ///     <see cref="Tilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when drawing the <see cref="Tilemap"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Tilemap tilemap, Vector2 position, Color color, float scale, float layerDepth) =>
        Draw(spriteBatch, tilemap, position, color, new Vector2(scale, scale), layerDepth);

    /// <summary>
    ///     Draws a <see cref="Tilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The instance of the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> class to use for rendering.
    /// </param>
    /// <param name="layer">
    ///     The instance of the <see cref="Tilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw the <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when drawing the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of x-axis (horizontal) and y-axis (vertical) scaling to apply when drawing the
    ///     <see cref="Tilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when drawing the <see cref="Tilemap"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Tilemap tilemap, Vector2 position, Color color, Vector2 scale, float layerDepth)
    {
        foreach (TilemapLayer layer in tilemap)
        {
            if (layer.IsVisible)
            {
                Vector2 actualPosition = position + (layer.Offset * scale);
                Draw(spriteBatch, layer, actualPosition, color, scale, layerDepth);
            }
        }
    }

    #endregion Tilemap

    #region Tilemap Layer

    /// <summary>
    ///     Draws a <see cref="TilemapLayer"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The instance of the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> class to use for rendering.
    /// </param>
    /// <param name="layer">
    ///     The instance of the <see cref="TilemapLayer"/> to draw.
    /// </param>
    /// <param name="position">
    ///     <para>
    ///         The x- and y-coordinate location to draw the <see cref="TilemapLayer"/> at.
    ///     </para>
    ///     <para>
    ///         Drawing a <see cref="TilemapLayer"/> directly in this method will ignore the
    ///         <see cref="TilemapLayer.Offset"/> value since.
    ///     </para>
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when drawing the <see cref="TilemapLayer"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TilemapLayer layer, Vector2 position, Color color) =>
        Draw(spriteBatch, layer, position, color, Vector2.One, 0.0f);

    /// <summary>
    ///     Draws a <see cref="TilemapLayer"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The instance of the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> class to use for rendering.
    /// </param>
    /// <param name="layer">
    ///     The instance of the <see cref="TilemapLayer"/> to draw.
    /// </param>
    /// <param name="position">
    ///     <para>
    ///         The x- and y-coordinate location to draw the <see cref="TilemapLayer"/> at.
    ///     </para>
    ///     <para>
    ///         Drawing a <see cref="TilemapLayer"/> directly in this method will ignore the
    ///         <see cref="TilemapLayer.Offset"/> value since.
    ///     </para>
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when drawing the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply to both the x-axis (horizontal) and y-axis (vertical) when drawing the
    ///     <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when drawing the <see cref="TilemapLayer"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TilemapLayer layer, Vector2 position, Color color, float scale, float layerDepth) =>
        Draw(spriteBatch, layer, position, color, new Vector2(scale, scale), layerDepth);

    /// <summary>
    ///     Draws a <see cref="TilemapLayer"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>
    /// </summary>
    /// <param name="spriteBatch">
    ///     The instance of the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> class to use for rendering.
    /// </param>
    /// <param name="layer">
    ///     The instance of the <see cref="TilemapLayer"/> to draw.
    /// </param>
    /// <param name="position">
    ///     <para>
    ///         The x- and y-coordinate location to draw the <see cref="TilemapLayer"/> at.
    ///     </para>
    ///     <para>
    ///         Drawing a <see cref="TilemapLayer"/> directly in this method will ignore the
    ///         <see cref="TilemapLayer.Offset"/> value since.
    ///     </para>
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when drawing the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of x-axis (horizontal) and y-axis (vertical) scaling to apply when drawing the
    ///     <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when drawing the <see cref="TilemapLayer"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TilemapLayer layer, Vector2 position, Color color, Vector2 scale, float layerDepth)
    {
        Vector2 tPosition = position;

        for (int column = 0; column < layer.ColumnCount; column++)
        {
            for (int row = 0; row < layer.RowCount; row++)
            {
                tPosition.X = position.X + (column * layer.Tileset.TileWidth * scale.X);
                tPosition.Y = position.Y + (row * layer.Tileset.TileHeight * scale.Y);
                Color renderColor = color * layer.Transparency;
                Tile tile = layer.GetTile(column, row);
                SpriteEffects flipEffects = SpriteEffects.None |
                                            (tile.FlipVertically ? SpriteEffects.FlipVertically : 0) |
                                            (tile.FlipHorizontally ? SpriteEffects.FlipHorizontally : 0);

                Draw(spriteBatch, tile.TextureRegion, tPosition, renderColor, tile.Rotation, Vector2.Zero, scale, flipEffects, layerDepth);
            }
        }
    }

    #endregion Tilemap Layer
}
