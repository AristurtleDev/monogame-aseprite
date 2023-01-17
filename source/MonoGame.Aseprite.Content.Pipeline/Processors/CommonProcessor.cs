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

using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines common properties and methods for the various MonoGame.Aseprite
///     processors.
/// </summary>
/// <typeparam name="TIn">
///     The type that defines the input for the processor.
/// </typeparam>
/// <typeparam name="TOut">
///     The type that defines the output from the processor.
/// </typeparam>
public abstract class CommonProcessor<TIn, TOut> : ContentProcessor<TIn, TOut>
{
    // ************************************************************************
    //  The following properties are common to all MonoGame.Aseprite processors
    //  that generate a texture, which pretty much all of them do. These
    //  properties are exposed through the mgcb-editor in the property
    //  panel for users to set when using the mgcb-editor to import an Aseprite
    //  file.
    //  The display name for each of these properties begins with (Aseprite) so
    //  that they are listed first in the property panel since the mgcb-editor
    //  does not support the [CategoryAttribute] for properties in the
    //  content processor.
    // ************************************************************************

    // /// <summary>
    // ///     Gets or Sets whether only cel elements that are on visible layers
    // ///     should be processed.
    // /// </summary>
    // [DisplayName("(Aseprite) Only Visible Layers")]
    // public bool OnlyVisibleLayers { get; set; } = true;

    // /// <summary>
    // ///     Gets or Sets whether cel elements that are on a layer that was
    // ///     marked as the background layer in Aseprite should be processed.
    // /// </summary>
    // [DisplayName("(Aseprite) Include Background Layer")]
    // public bool IncludeBackgroundLayer { get; set; } = false;

    // ************************************************************************
    //  The following properties are those that are provided by MonoGame when
    //  importing an image as a Texture2D.  Since all of the processors for
    //  MonoGame.Aseprite generate a texture in some fashion, and use the
    //  actual MonoGame texture processor, these options are supplied as
    //  supplementary for users in the mgcb-editor.
    // ************************************************************************

    /// <summary>
    ///     Gets or Sets the color value to treat as the color that represents
    ///     transparency in the image.
    /// </summary>
    [DisplayName("Color Key Color")]
    [DefaultValue(typeof(Color), "255,0,255,255")]
    public Color ColorKeyColor { get; set; } = new Color(255, 0, 255, 255);

    /// <summary>
    ///     Gets or Sets whether the color key value is enabled.
    /// </summary>
    [DisplayName("Color Key Enabled")]
    [DefaultValue(true)]
    public bool ColorKeyEnabled { get; set; } = true;

    /// <summary>
    ///     Gets or Sets whether mipmaps will be generated.
    /// </summary>
    [DisplayName("Generate MipMaps")]
    [DefaultValue(false)]
    public bool GenerateMipMaps { get; set; } = false;

    /// <summary>
    ///     Gets or Sets whether all pixel color values should be recalculated
    ///     by performing alpha pre-multiplication.
    /// </summary>
    [DisplayName("Premultiply Alpha")]
    [DefaultValue(true)]
    public bool PremultiplyAlpha { get; set; } = true;

    /// <summary>
    ///     Gets or Sets whether the texture should be resized to the nearest
    ///     power of two for the width and/or height.
    /// </summary>
    [DisplayName("Resize to Power of Two")]
    [DefaultValue(false)]
    public bool ResizePowerOfTwo { get; set; } = false;

    /// <summary>
    ///     Gets whether the texture should be resized so that the width and
    ///     height are equal (square texture).
    /// </summary>
    [DisplayName("Make Square")]
    [DefaultValue(false)]
    public bool MakeSquare { get; set; } = false;

    /// <summary>
    ///     Gets or Sets the texture format to use when processing the texture.
    /// </summary>
    [DisplayName("Texture Format")]
    public TextureProcessorOutputFormat TextureFormat { get; set; }

    protected TextureContent CreateTextureContent(string sourceName, Color[] pixels, int imageWidth, int imageHeight, ContentProcessorContext processorContext)
    {
        //  During the Aseprite file import, no matter the color mode that was
        //  set in Aseprite, all color data was translated to 32-bits per pixel.
        const int bpp = 32;

        //  Translate the pixel color data in the image to byte data
        //  TODO: Look into not translating the color data in Aseprite to Color
        //        value types, and instead just keep the byte[] of the raw image
        //        data and see if that can be used.  For now, since doing the
        //        the color translation works, we'll just keep doing this, but
        //        there's no need to translate it once on import just to
        //        translate it back to byte data here
        byte[] data = new byte[((imageWidth * imageHeight * bpp - 1) / 8) + 1];
        PixelsToBytes(pixels, data);

        //  Create the BitmapContent
        BitmapContent face = new PixelBitmapContent<Color>(imageWidth, imageHeight);
        face.SetPixelData(data);

        //  Create the Texture2D content
        TextureContent content = new Texture2DContent() { Identity = new ContentIdentity(sourceName) };
        content.Faces[0].Add(face);

        //  Make use of the native MonoGame Texture Processor instead of
        //  implementing something custom.  Why reinvent the wheel?
        TextureProcessor nativeProcessor = new()
        {
            ColorKeyColor = ColorKeyColor,
            ColorKeyEnabled = ColorKeyEnabled,
            GenerateMipmaps = GenerateMipMaps,
            PremultiplyAlpha = PremultiplyAlpha,
            ResizeToPowerOfTwo = ResizePowerOfTwo,
            MakeSquare = MakeSquare,
            TextureFormat = TextureFormat
        };

        content = nativeProcessor.Process(content, processorContext);
        return content;
    }

    private static void PixelsToBytes(Color[] src, byte[] dest)
    {
        for (int i = 0, b = 0; i < src.Length; i++, b += 4)
        {
            dest[b] = src[i].R;
            dest[b + 1] = src[i].G;
            dest[b + 2] = src[i].B;
            dest[b + 3] = src[i].A;
        }
    }

    //  TODO: This is marked internal since Tileset accessability is internal
    //        but would rather logically mark this as protected. If still
    //        considering moving the file import stuff to a separate library,
    //        that might work then.
    internal TilesetContent CreateTilesetContent(AsepriteTileset tileset, string fileName, ContentProcessorContext processorContext)
    {
        string sourceName = $"{fileName} {tileset.Name}.aseprite";
        TextureContent textureContent = CreateTextureContent(sourceName, tileset.Pixels, tileset.Width, tileset.Height, processorContext);
        return new(tileset.Name, tileset.TileCount, tileset.TileWidth, tileset.TileHeight, textureContent);
    }

    internal TilemapLayerContent CreateTilemapLayerContent(AsepriteTilemapCel cel)
    {
        string name = cel.Layer.Name;
        //  TODO: Can the TilesetID be added to the Tileset itself instead of
        //        having to get the layer as then pulling it from the layer
        //        since the cel already has access to the Tileset???
        int tilesetID = cel.LayerAs<AsepriteTilemapLayer>().TilesetID;
        int columns = cel.Width;
        int rows = cel.Height;
        Point offset = cel.Position;
        string tilesetName = cel.Tileset.Name;
        TileContent[] tiles = CreateCelTileContent(cel);

        return new TilemapLayerContent(name, tilesetID, columns, rows, offset, tiles);
    }

    internal TileContent[] CreateCelTileContent(AsepriteTilemapCel cel)
    {
        TileContent[] tiles = new TileContent[cel.Tiles.Count];

        for (int i = 0; i < cel.Tiles.Count; i++)
        {
            AsepriteTile tile = cel.Tiles[i];

            byte flag = 0;
            flag |= (byte)(tile.XFlip != 0 ? 1 : 0);
            flag |= (byte)(tile.YFlip != 0 ? 2 : 0);

            TileContent tileContent = new(flag, tile.Rotation, tile.TilesetTileID);
            tiles[i] = tileContent;
        }

        return tiles;
    }
}
