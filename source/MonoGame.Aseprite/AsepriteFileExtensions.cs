using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Processors;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Utils;
using AseAnimatedTilemap = AsepriteDotNet.AnimatedTilemap;
using AseAnimationFrame = AsepriteDotNet.AnimationFrame;
using AseNinepatchSlice = AsepriteDotNet.NinePatchSlice;
using AseSlice = AsepriteDotNet.Slice;
using AseSprite = AsepriteDotNet.Sprite;
using AseSpriteSheet = AsepriteDotNet.SpriteSheet;
using AseTag = AsepriteDotNet.AnimationTag;
using AseTextureAtlas = AsepriteDotNet.TextureAtlas;
using AseTextureRegion = AsepriteDotNet.TextureRegion;
using AseTilemap = AsepriteDotNet.Tilemap;
using AseTilemapFrame = AsepriteDotNet.TilemapFrame;
using AseTilemapLayer = AsepriteDotNet.TilemapLayer;
using AseTilemapTile = AsepriteDotNet.TilemapTile;
using AseTileset = AsepriteDotNet.Tileset;


namespace MonoGame.Aseprite;

/// <summary>
/// Extension methods for working with an Aseprite File loaded by the AsepriteDotNet library.
/// </summary>
public static class AsepriteFileExtensions
{
    /// <summary>
    /// Creates a new <see cref="Sprite"/> from the specified frame index of the provided aseprite file instance.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="frameIndex">The index of the frame in the aseprite file to create the sprite from.</param>
    /// <param name="options">The options to use when processing the sprite.</param>
    /// <returns>The <see cref="Sprite"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="frameIndex"/> is less than zero or greater than or equal to the total number of
    /// frames in the aseprite file.
    /// </exception>
    public static Sprite CreateSprite(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        AseSprite aseSprite = SpriteProcessor.Process(aseFile, frameIndex, options);
        Texture2D texture = aseSprite.Texture.ToTexture2D(device);
        TextureRegion region = new TextureRegion(texture.Name, texture, texture.Bounds);

        for (int i = 0; i < aseSprite.Slices.Length; i++)
        {
            AddSliceToRegion(region, aseSprite.Slices[i]);
        }

        return new Sprite(aseSprite.Name, region);
    }

    /// <summary>
    /// Creates a new <see cref="TextureAtlas"/> from the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="options">The options to use when processing the texture atlas..</param>
    /// <returns>The <see cref="TextureAtlas"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    public static TextureAtlas CreateTextureAtlas(this AsepriteFile aseFile, GraphicsDevice device, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        AseTextureAtlas aseAtlas = TextureAtlasProcessor.Process(aseFile, options);
        Texture2D texture = aseAtlas.Texture.ToTexture2D(device);
        TextureAtlas atlas = new TextureAtlas(texture.Name, texture);
        GenerateRegions(atlas, aseAtlas);
        return atlas;
    }

    /// <summary>
    /// Creates a new <see cref="SpriteSheet"/> from the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="options">The options to use when processing the sprite sheet..</param>
    /// <returns>The <see cref="SpriteSheet"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    public static SpriteSheet CreateSpriteSheet(this AsepriteFile aseFile, GraphicsDevice device, ProcessorOptions? options =  null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        AseSpriteSheet aseSheet = SpriteSheetProcessor.Process(aseFile, options);
        Texture2D texture = aseSheet.TextureAtlas.Texture.ToTexture2D(device);
        TextureAtlas atlas = new TextureAtlas(texture.Name, texture);
        GenerateRegions(atlas, aseSheet.TextureAtlas);
        SpriteSheet sheet = new SpriteSheet(atlas.Name, atlas);

        for (int i = 0; i < aseSheet.Tags.Length; i++)
        {
            AseTag aseTag = aseSheet.Tags[i];
            sheet.CreateAnimationTag(aseTag.Name, builder =>
            {
                builder.LoopCount(aseTag.LoopCount)
                       .IsReversed(aseTag.IsReversed)
                       .IsPingPong(aseTag.IsPingPong);

                for (int j = 0; j < aseTag.Frames.Length; j++)
                {
                    AseAnimationFrame aseAnimationFrame = aseTag.Frames[j];
                    builder.AddFrame(aseAnimationFrame.FrameIndex, aseAnimationFrame.Duration);
                }
            });
        }


        return sheet;
    }

    /// <summary>
    /// Creates a new <see cref="Tileset"/> from the frame at the specified index in provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="frameIndex">The index of the frame that contains the tileset.</param>
    /// <returns>The <see cref="Tileset"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Throw if the <paramref name="frameIndex"/> is less than zero or greater than or equal to the total number of
    /// frames in the aseprite file
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    public static Tileset CreateTileset(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        AseTileset aseTileset = TilesetProcessor.Process(aseFile, frameIndex);
        return aseTileset.CreateTileset(device);
    }

    /// <summary>
    /// Creates a new <see cref="Tileset"/> with the specified name in provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="tilesetName">The name of the tileset.</param>
    /// <returns>The <see cref="Tileset"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Throw if no tileset with the specified <paramref name="tilesetName"/> exists in the aseprite file.
    /// </exception>
    public static Tileset CreateTileset(this AsepriteFile aseFile, GraphicsDevice device, string tilesetName)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        AseTileset aseTileset = TilesetProcessor.Process(aseFile, tilesetName);
        return aseTileset.CreateTileset(device);
    }

    /// <summary>
    /// Creates a new <see cref="Tileset"/> from the specified aseprite tileset.
    /// </summary>
    /// <param name="aseTileset">The Aseprite tileset to create the tileset from..</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <returns>The <see cref="Tileset"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseTileset"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    public static Tileset CreateTileset(this AseTileset aseTileset, GraphicsDevice device)
    {
        ArgumentNullException.ThrowIfNull(aseTileset);
        ArgumentNullException.ThrowIfNull(device);

        Texture2D texture = aseTileset.Texture.ToTexture2D(device);
        return new Tileset(aseTileset.Name,
                          texture,
                          aseTileset.TileSize.Width,
                          aseTileset.TileSize.Height);
    }

    /// <summary>
    /// Creates a new <see cref="Tilemap"/> from a specified frame in the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="frameIndex">The index of the frame with the tilemap.</param>
    /// <param name="options">The options to use when processing the tilemap.</param>
    /// <returns>The <see cref="Tilemap"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="aseFile"/> is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// throw if <paramref name="device"/> is <see langword="null"/>.
    /// </exception>
    public static Tilemap CreateTilemap(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        AseTilemap aseTilemap = TilemapProcessor.Process(aseFile, frameIndex, options);
        Tilemap tilemap = new Tilemap(aseTilemap.Name);
        Dictionary<int, Tileset> tilesets = GenereateTilesets(device, aseTilemap);

        for (int i = 0; i < aseTilemap.Layers.Length; i++)
        {
            AseTilemapLayer aseTilemapLayer = aseTilemap.Layers[i];
            Tileset tileset = tilesets[aseTilemapLayer.TilesetID];
            TilemapLayer tilemapLayer = tilemap.CreateLayer(aseTilemapLayer.Name,
                                                            tileset,
                                                            aseTilemapLayer.Columns,
                                                            aseTilemapLayer.Rows,
                                                            aseTilemapLayer.Offset.ToXnaVector2());

            for (int t = 0; t < aseTilemapLayer.Tiles.Length; t++)
            {
                AseTilemapTile aseTilemapTile = aseTilemapLayer.Tiles[t];
                tilemapLayer.SetTile(t,
                                     aseTilemapTile.TilesetTileID,
                                     aseTilemapTile.FlipHorizontally,
                                     aseTilemapTile.FlipVertically,
                                     aseTilemapTile.FlipDiagonally);
            }
        }

        return tilemap;
    }

    /// <summary>
    /// Creates a new <see cref="AnimatedTilemap"/> from the all frames in the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="options">The options to use when processing the animated tilemap.</param>
    /// <returns>the <see cref="AnimatedTilemap"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="aseFile"/> is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// throw if <paramref name="device"/> is <see langword="null"/>.
    /// </exception>
    public static AnimatedTilemap CreateAnimatedTilemap(this AsepriteFile aseFile, GraphicsDevice device, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        AseAnimatedTilemap aseAnimatedTilemap = AnimatedTilemapProcessor.Process(aseFile, options);
        AnimatedTilemap animatedTilemap = new AnimatedTilemap(aseAnimatedTilemap.Name);
        Dictionary<int, Tileset> tilesets = new Dictionary<int, Tileset>();

        ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
        for (int i = 0; i < aseAnimatedTilemap.Tilesets.Length; i++)
        {
            AseTileset aseTileset = aseAnimatedTilemap.Tilesets[i];
            Texture2D texture = aseTileset.Texture.ToTexture2D(device);
            Tileset tileset = new Tileset(aseTileset.Name,
                                          texture,
                                          aseTileset.TileSize.Width,
                                          aseTileset.TileSize.Height);
            tilesets.Add(aseTileset.ID, tileset);
        }

        for (int i = 0; i < aseAnimatedTilemap.Frames.Length; i++)
        {
            AseTilemapFrame aseTilemapFrame = aseAnimatedTilemap.Frames[i];
            AnimatedTilemapFrame animatedTilemapFrame = animatedTilemap.CreateFrame(aseTilemapFrame.Duration);

            for (int l = 0; l < aseTilemapFrame.Layers.Length; l++)
            {
                AseTilemapLayer aseTilemapLayer = aseTilemapFrame.Layers[l];
                TilemapLayer tilemapLayer = animatedTilemapFrame.CreateLayer(aseTilemapLayer.Name,
                                                                             tilesets[aseTilemapLayer.TilesetID],
                                                                             aseTilemapLayer.Columns,
                                                                             aseTilemapLayer.Rows,
                                                                             aseTilemapLayer.Offset.ToXnaVector2());

                for (int t = 0; t < aseTilemapLayer.Tiles.Length; t++)
                {
                    AseTilemapTile aseTilemapTile = aseTilemapLayer.Tiles[t];
                    tilemapLayer.SetTile(t,
                                         aseTilemapTile.TilesetTileID,
                                         aseTilemapTile.FlipHorizontally,
                                         aseTilemapTile.FlipVertically,
                                         aseTilemapTile.FlipDiagonally);
                }
            }
        }


        return animatedTilemap;
    }


    private static void GenerateRegions(TextureAtlas atlas, AseTextureAtlas aseAtlas)
    {
        for (int i = 0; i < aseAtlas.Regions.Length; i++)
        {
            AseTextureRegion aseRegion = aseAtlas.Regions[i];
            TextureRegion region = atlas.CreateRegion(aseRegion.Name, aseRegion.Bounds.ToXnaRectangle());

            for (int j = 0; j < aseRegion.Slices.Length; j++)
            {
                AddSliceToRegion(region, aseRegion.Slices[j]);
            }
        }

    }

    private static void AddSliceToRegion(TextureRegion region, AseSlice aseSlice)
    {
        if (aseSlice is AseNinepatchSlice aseNinePatchSlice)
        {
            region.CreateNinePatchSlice(aseNinePatchSlice.Name,
                                        aseNinePatchSlice.Bounds.ToXnaRectangle(),
                                        aseNinePatchSlice.CenterBounds.ToXnaRectangle(),
                                        aseNinePatchSlice.Origin.ToXnaVector2(),
                                        aseNinePatchSlice.Color.ToXnaColor());
        }
        else
        {
            region.CreateSlice(aseSlice.Name,
                               aseSlice.Bounds.ToXnaRectangle(),
                               aseSlice.Origin.ToXnaVector2(),
                               aseSlice.Color.ToXnaColor());
        }
    }

    private static Dictionary<int, Tileset> GenereateTilesets(GraphicsDevice device, AseTilemap aseTilemap)
    {
        Dictionary<int, Tileset> result = new Dictionary<int, Tileset>();
        for (int i = 0; i < aseTilemap.Tilesets.Length; i++)
        {
            AseTileset aseTileset = aseTilemap.Tilesets[i];
            Texture2D texture = aseTileset.Texture.ToTexture2D(device);
            Tileset tileset = new Tileset(aseTileset.Name,
                                          texture,
                                          aseTileset.TileSize.Width,
                                          aseTileset.TileSize.Height);
            result.Add(aseTileset.ID, tileset);
        }

        return result;
    }
}
