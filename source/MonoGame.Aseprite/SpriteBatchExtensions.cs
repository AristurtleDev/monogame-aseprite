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
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Tilemaps;

namespace MonoGame.Aseprite;

/// <summary>
/// Defines extension methods for the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to render graphical resource types in this library.
/// </summary>
public static class SpriteBatchExtensions
{
    #region Texture Region

    /// <summary>
    ///     Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="region">
    ///     The <see cref="TextureRegion"/> to render.
    /// </param>
    /// <param name="destinationRectangle">
    ///     A rectangular bound that defines the destination to render the <see cref="TextureRegion"/> into.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Rectangle destinationRectangle, Color color) =>
        spriteBatch.Draw(region.Texture, destinationRectangle, region.Bounds, color);

    /// <summary>
    ///     Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="region">
    ///     The <see cref="TextureRegion"/> to render.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="TextureRegion"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color) =>
        spriteBatch.Draw(region.Texture, position, region.Bounds, color);

    /// <summary>
    ///     Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="region">
    ///     The <see cref="TextureRegion"/> to render.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="TextureRegion"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate point of origin to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
    ///     flipping when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(region.Texture, position, region.Bounds, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="region">
    ///     The <see cref="TextureRegion"/> to render.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="TextureRegion"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate point of origin to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
    ///     flipping when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(region.Texture, position, region.Bounds, color, rotation, origin, scale, effects, layerDepth);

    /// <summary>
    ///     Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
    /// </param>
    /// <param name="region">
    ///     The <see cref="TextureRegion"/> to render.
    /// </param>
    /// <param name="destinationRectangle">
    ///     A rectangular bound that defines the destination to render the <see cref="TextureRegion"/> into.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate point of origin to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
    ///     flipping when rendering the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="TextureRegion"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
        spriteBatch.Draw(region.Texture, destinationRectangle, region.Bounds, color, rotation, origin, effects, layerDepth);

    #endregion Texture Region

    /// <summary>
    ///     Draws a <see cref="Sprite"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the 
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="sprite">
    ///     The <see cref="Sprite"/> to render.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="Sprite"/> at.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position) =>
        spriteBatch.Draw(sprite.TextureRegion, position, sprite.Color * sprite.Transparency, sprite.Rotation, sprite.Origin, sprite.Scale, sprite.SpriteEffects, sprite.LayerDepth);


    #region Animated Tilemap

    /// <summary>
    ///     Draws an <see cref="AnimatedTilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the 
    ///     <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="animatedTilemap">
    /// T   he <see cref="AnimatedTilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="AnimatedTilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, AnimatedTilemap animatedTilemap, Vector2 position, Color color) =>
        Draw(spriteBatch, animatedTilemap, position, color, Vector2.One, 0.0f);

    /// <summary>
    ///     Draws an <see cref="AnimatedTilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the 
    ///     <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="animatedTilemap">
    /// T   he <see cref="AnimatedTilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="AnimatedTilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, AnimatedTilemap animatedTilemap, Vector2 position, Color color, float scale, float layerDepth) =>
        Draw(spriteBatch, animatedTilemap, position, color, new Vector2(scale, scale), layerDepth);

    /// <summary>
    ///     Draws an <see cref="AnimatedTilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the 
    ///     <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="animatedTilemap">
    ///     The <see cref="AnimatedTilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="AnimatedTilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="AnimatedTilemap"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, AnimatedTilemap animatedTilemap, Vector2 position, Color color, Vector2 scale, float layerDepth)
    {
        AnimatedTilemapFrame frame = animatedTilemap.CurrentFrame;

        foreach (TilemapLayer layer in frame)
        {
            if (layer.IsVisible)
            {
                Vector2 actualPosition = position + (layer.Offset * scale);
                Draw(spriteBatch, layer, actualPosition, color, scale, layerDepth);
            }
        }
    }

    #endregion Animated Tilemap

    #region Tilemap

    /// <summary>
    ///     Draws a <see cref="Tilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="tilemap">
    ///     The <see cref="Tilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="Tilemap"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Tilemap tilemap, Vector2 position, Color color) =>
        Draw(spriteBatch, tilemap, position, color, Vector2.One, 0.0f);

    /// <summary>
    ///     Draws a <see cref="Tilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="tilemap">
    ///     The <see cref="Tilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="Tilemap"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, Tilemap tilemap, Vector2 position, Color color, float scale, float layerDepth) =>
        Draw(spriteBatch, tilemap, position, color, new Vector2(scale, scale), layerDepth);

    /// <summary>
    ///     Draws a <see cref="Tilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="tilemap">
    ///     The <see cref="Tilemap"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render the <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="Tilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="Tilemap"/>.
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
    ///     Draws a <see cref="TilemapLayer"/> layer using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the 
    ///     <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layer">
    /// The <see cref="TilemapLayer"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw the <see cref="TilemapLayer"/> at.  Drawing the 
    ///     <see cref="TilemapLayer"/> using this method ignores the <see cref="TilemapLayer.Offset"/>.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="TilemapLayer"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TilemapLayer layer, Vector2 position, Color color) =>
        Draw(spriteBatch, layer, position, color, Vector2.One, 0.0f);

    /// <summary>
    ///     Draws a <see cref="TilemapLayer"/> layer using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the 
    ///     <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layer">
    /// The <see cref="TilemapLayer"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw the <see cref="TilemapLayer"/> at.  Drawing the 
    ///     <see cref="TilemapLayer"/> using this method ignores the <see cref="TilemapLayer.Offset"/>.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="TilemapLayer"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TilemapLayer layer, Vector2 position, Color color, float scale, float layerDepth) =>
        Draw(spriteBatch, layer, position, color, new Vector2(scale, scale), layerDepth);

    /// <summary>
    ///     Draws a <see cref="TilemapLayer"/> layer using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the 
    ///     <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layer">
    /// The <see cref="TilemapLayer"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw the <see cref="TilemapLayer"/> at.  Drawing the 
    ///     <see cref="TilemapLayer"/> using this method ignores the <see cref="TilemapLayer.Offset"/>.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="TilemapLayer"/>.
    /// </param>
    public static void Draw(this SpriteBatch spriteBatch, TilemapLayer layer, Vector2 position, Color color, Vector2 scale, float layerDepth)
    {
        Vector2 tPosition = position;

        for (int column = 0; column < layer.Columns; column++)
        {
            for (int row = 0; row < layer.Rows; row++)
            {
                Tile tile = layer.GetTile(column, row);
                if (tile.IsEmpty) { continue; }

                tPosition.X = position.X + (column * layer.Tileset.TileWidth * scale.X);
                tPosition.Y = position.Y + (row * layer.Tileset.TileHeight * scale.Y);
                Color renderColor = color * layer.Transparency;

                SpriteEffects flipEffects = SpriteEffects.None |
                                            (tile.FlipVertically ? SpriteEffects.FlipVertically : 0) |
                                            (tile.FlipHorizontally ? SpriteEffects.FlipHorizontally : 0);

                TextureRegion textureRegion = layer.Tileset[tile.TilesetTileID];

                Draw(spriteBatch, textureRegion, tPosition, renderColor, tile.Rotation, Vector2.Zero, scale, flipEffects, layerDepth);
            }
        }
    }

    #endregion Tilemap Layer
}
