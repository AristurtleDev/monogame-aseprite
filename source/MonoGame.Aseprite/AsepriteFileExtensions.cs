// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
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
#pragma warning disable CS0618
    [Obsolete("This method will be removed in a future release.  Users should switch to the other CreateSprite methods instead", false)]
    public static Sprite CreateSprite(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        return CreateSprite(aseFile, device, frameIndex, options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers);
    }
#pragma warning restore CS0618

    /// <summary>
    /// Creates a new <see cref="Sprite"/> from the specified frame index of the provided aseprite file instance.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="frameIndex">The index of the frame in the aseprite file to create the sprite from.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <param name="includeBackgroundLayer">Indicates whether the layer assigned as the background layer should be processed.</param>
    /// <param name="includeTilemapLayers">Indicates whether tilemap layers should be processed.</param>
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
    public static Sprite CreateSprite(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = false)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);


        List<string> layers = new List<string>();
        for (int i = 0; i < aseFile.Layers.Length; i++)
        {
            AsepriteLayer layer = aseFile.Layers[i];
            if (onlyVisibleLayers && !layer.IsVisible) { continue; }
            if (!includeBackgroundLayer && layer.IsBackgroundLayer) { continue; }
            if (!includeTilemapLayers && layer is AsepriteTilemapLayer) { continue; }
            layers.Add(layer.Name);
        }

        return CreateSprite(aseFile, device, frameIndex, layers);
    }

    /// <summary>
    /// Creates a new <see cref="Sprite"/> from the specified frame index of the provided aseprite file instance.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="frameIndex">The index of the frame in the aseprite file to create the sprite from.</param>
    /// <param name="layers">
    /// A collection containing the name of the layers to process.  Only cels on a layer who's name matches a name in
    /// this collection will be processed.
    /// </param>
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
    public static Sprite CreateSprite(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex, ICollection<string> layers)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        AseSprite aseSprite = SpriteProcessor.Process(aseFile, frameIndex, layers);
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
#pragma warning disable CS0618
    [Obsolete("This method will be removed in a future release.  Users should switch to the other CreateTextureAtlas methods instead", false)]
    public static TextureAtlas CreateTextureAtlas(this AsepriteFile aseFile, GraphicsDevice device, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        return CreateTextureAtlas(aseFile, device, options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers, options.MergeDuplicateFrames, options.BorderPadding, options.Spacing, options.InnerPadding);
    }
#pragma warning restore CS0618

    /// <summary>
    /// Creates a new <see cref="TextureAtlas"/> from the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <param name="includeBackgroundLayer">Indicates whether the layer assigned as the background layer should be processed.</param>
    /// <param name="includeTilemapLayers">Indicates whether tilemap layers should be processed.</param>
    /// <param name="mergeDuplicateFrames">Indicates whether duplicates frames should be merged.</param>
    /// <param name="borderPadding">The amount of transparent pixels to add to the edge of the generated texture.</param>
    /// <param name="spacing">The amount of transparent pixels to add between each texture region in the generated texture.</param>
    /// <param name="innerPadding">The amount of transparent pixels to add around the edge of each texture region in the generated texture.</param>
    /// <returns>The <see cref="TextureAtlas"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    public static TextureAtlas CreateTextureAtlas(this AsepriteFile aseFile,
                                                  GraphicsDevice device,
                                                  bool onlyVisibleLayers = true,
                                                  bool includeBackgroundLayer = false,
                                                  bool includeTilemapLayers = false,
                                                  bool mergeDuplicateFrames = true,
                                                  int borderPadding = 0,
                                                  int spacing = 0,
                                                  int innerPadding = 0)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        List<string> layers = new List<string>();
        for (int i = 0; i < aseFile.Layers.Length; i++)
        {
            AsepriteLayer layer = aseFile.Layers[i];
            if (onlyVisibleLayers && !layer.IsVisible) { continue; }
            if (!includeBackgroundLayer && layer.IsBackgroundLayer) { continue; }
            if (!includeTilemapLayers && layer is AsepriteTilemapLayer) { continue; }
            layers.Add(layer.Name);
        }

        return CreateTextureAtlas(aseFile, device, layers, mergeDuplicateFrames, borderPadding, spacing, innerPadding);
    }

    /// <summary>
    /// Creates a new <see cref="TextureAtlas"/> from the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="layers">
    /// A collection containing the name of the layers to process.  Only cels on a layer who's name matches a name in
    /// this collection will be processed.
    /// </param>
    /// <param name="mergeDuplicateFrames">Indicates whether duplicates frames should be merged.</param>
    /// <param name="borderPadding">The amount of transparent pixels to add to the edge of the generated texture.</param>
    /// <param name="spacing">The amount of transparent pixels to add between each texture region in the generated texture.</param>
    /// <param name="innerPadding">The amount of transparent pixels to add around the edge of each texture region in the generated texture.</param>
    /// <returns>The <see cref="TextureAtlas"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    public static TextureAtlas CreateTextureAtlas(this AsepriteFile aseFile, GraphicsDevice device, ICollection<string> layers, bool mergeDuplicateFrames = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        AseTextureAtlas aseAtlas = TextureAtlasProcessor.Process(aseFile, layers, mergeDuplicateFrames, borderPadding, spacing, innerPadding);
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
#pragma warning disable CS0618
    [Obsolete("This method will be removed in a future release.  Users should switch to the other CreateSpriteSheet methods instead", false)]
    public static SpriteSheet CreateSpriteSheet(this AsepriteFile aseFile, GraphicsDevice device, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        return CreateSpriteSheet(aseFile, device, options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers, options.MergeDuplicateFrames, options.BorderPadding, options.Spacing, options.InnerPadding);
    }
#pragma warning restore CS0618

    /// <summary>
    /// Creates a new <see cref="SpriteSheet"/> from the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <param name="includeBackgroundLayer">Indicates whether the layer assigned as the background layer should be processed.</param>
    /// <param name="includeTilemapLayers">Indicates whether tilemap layers should be processed.</param>
    /// <param name="mergeDuplicateFrames">Indicates whether duplicates frames should be merged.</param>
    /// <param name="borderPadding">The amount of transparent pixels to add to the edge of the generated texture.</param>
    /// <param name="spacing">The amount of transparent pixels to add between each texture region in the generated texture.</param>
    /// <param name="innerPadding">The amount of transparent pixels to add around the edge of each texture region in the generated texture.</param>
    /// <returns>The <see cref="SpriteSheet"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    public static SpriteSheet CreateSpriteSheet(this AsepriteFile aseFile,
                                                 GraphicsDevice device,
                                                 bool onlyVisibleLayers = true,
                                                 bool includeBackgroundLayer = false,
                                                 bool includeTilemapLayers = false,
                                                 bool mergeDuplicateFrames = true,
                                                 int borderPadding = 0,
                                                 int spacing = 0,
                                                 int innerPadding = 0)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        List<string> layers = new List<string>();
        for (int i = 0; i < aseFile.Layers.Length; i++)
        {
            AsepriteLayer layer = aseFile.Layers[i];
            if (onlyVisibleLayers && !layer.IsVisible) { continue; }
            if (!includeBackgroundLayer && layer.IsBackgroundLayer) { continue; }
            if (!includeTilemapLayers && layer is AsepriteTilemapLayer) { continue; }
            layers.Add(layer.Name);
        }

        return CreateSpriteSheet(aseFile, device, layers, mergeDuplicateFrames, borderPadding, spacing, innerPadding);
    }

    /// <summary>
    /// Creates a new <see cref="SpriteSheet"/> from the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="layers">
    /// A collection containing the name of the layers to process.  Only cels on a layer who's name matches a name in
    /// this collection will be processed.
    /// </param>
    /// <param name="mergeDuplicateFrames">Indicates whether duplicates frames should be merged.</param>
    /// <param name="borderPadding">The amount of transparent pixels to add to the edge of the generated texture.</param>
    /// <param name="spacing">The amount of transparent pixels to add between each texture region in the generated texture.</param>
    /// <param name="innerPadding">The amount of transparent pixels to add around the edge of each texture region in the generated texture.</param>
    /// <returns>The <see cref="SpriteSheet"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="aseFile"/> parameter is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// Thrown if the <paramref name="device"/> parameter is <see langword="null"/>.
    /// </exception>
    public static SpriteSheet CreateSpriteSheet(this AsepriteFile aseFile, GraphicsDevice device, ICollection<string> layers, bool mergeDuplicateFrames = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        AseSpriteSheet aseSheet = SpriteSheetProcessor.Process(aseFile, layers, mergeDuplicateFrames, borderPadding, spacing, innerPadding);
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
#pragma warning disable CS0618
    [Obsolete("This method will be removed in a future release.  Users should switch to the other CreateTilemap methods instead", false)]
    public static Tilemap CreateTilemap(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        return CreateTilemap(aseFile, device, frameIndex, options.OnlyVisibleLayers);
    }
#pragma warning restore CS0618

    /// <summary>
    /// Creates a new <see cref="Tilemap"/> from a specified frame in the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="frameIndex">The index of the frame with the tilemap.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <returns>The <see cref="Tilemap"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="aseFile"/> is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// throw if <paramref name="device"/> is <see langword="null"/>.
    /// </exception>
    public static Tilemap CreateTilemap(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex, bool onlyVisibleLayers = true)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        List<string> layers = new List<string>();
        for (int i = 0; i < aseFile.Layers.Length; i++)
        {
            AsepriteLayer layer = aseFile.Layers[i];
            if (layer is not AsepriteTilemapLayer) { continue; }
            if (onlyVisibleLayers && !layer.IsVisible) { continue; }
            layers.Add(layer.Name);
        }

        return CreateTilemap(aseFile, device, frameIndex, layers);
    }

    /// <summary>
    /// Creates a new <see cref="Tilemap"/> from a specified frame in the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="frameIndex">The index of the frame with the tilemap.</param>
    /// <param name="layers">
    /// A collection containing the name of the layers to process.  Only cels on a layer who's name matches a name in
    /// this collection will be processed.
    /// </param>
    /// <returns>The <see cref="Tilemap"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="aseFile"/> is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// throw if <paramref name="device"/> is <see langword="null"/>.
    /// </exception>
    public static Tilemap CreateTilemap(this AsepriteFile aseFile, GraphicsDevice device, int frameIndex, ICollection<string> layers)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        AseTilemap aseTilemap = TilemapProcessor.Process(aseFile, frameIndex, layers);
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
#pragma warning disable CS0618
    [Obsolete("This method will be removed in a future release.  Users should switch to the other AnimatedTilemap methods instead", false)]
    public static AnimatedTilemap CreateAnimatedTilemap(this AsepriteFile aseFile, GraphicsDevice device, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        options ??= ProcessorOptions.Default;
        return CreateAnimatedTilemap(aseFile, device, options.OnlyVisibleLayers);
    }
#pragma warning restore CS0618

    /// <summary>
    /// Creates a new <see cref="AnimatedTilemap"/> from the all frames in the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <returns>the <see cref="AnimatedTilemap"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="aseFile"/> is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// throw if <paramref name="device"/> is <see langword="null"/>.
    /// </exception>
    public static AnimatedTilemap CreateAnimatedTilemap(this AsepriteFile aseFile, GraphicsDevice device, bool onlyVisibleLayers = true)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        List<string> layers = new List<string>();
        for (int i = 0; i < aseFile.Layers.Length; i++)
        {
            AsepriteLayer layer = aseFile.Layers[i];
            if (layer is not AsepriteTilemapLayer) { continue; }
            if (onlyVisibleLayers && !layer.IsVisible) { continue; }
            layers.Add(layer.Name);
        }

        return CreateAnimatedTilemap(aseFile, device, layers);
    }

    /// <summary>
    /// Creates a new <see cref="AnimatedTilemap"/> from the all frames in the provided aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file instance.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="layers">
    /// A collection containing the name of the layers to process.  Only cels on a layer who's name matches a name in
    /// this collection will be processed.
    /// </param>
    /// <returns>the <see cref="AnimatedTilemap"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="aseFile"/> is <see langword="null"/>.
    /// 
    /// -or-
    /// 
    /// throw if <paramref name="device"/> is <see langword="null"/>.
    /// </exception>
    public static AnimatedTilemap CreateAnimatedTilemap(this AsepriteFile aseFile, GraphicsDevice device, ICollection<string> layers)
    {
        ArgumentNullException.ThrowIfNull(aseFile);
        ArgumentNullException.ThrowIfNull(device);

        AseAnimatedTilemap aseAnimatedTilemap = AnimatedTilemapProcessor.Process(aseFile, layers);
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
