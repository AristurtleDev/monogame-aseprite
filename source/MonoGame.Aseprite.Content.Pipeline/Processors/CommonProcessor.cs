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
    //  The following properties are those that are provided by MonoGame when
    //  importing an image as a Texture2D.  Since all of the processors for
    //  MonoGame.Aseprite generate a texture in some fashion, and use the
    //  actual MonoGame texture processor, these options are supplied as
    //  supplementary for users in the mgcb-editor.
    // ************************************************************************

    /// <summary>
    ///     Gets or Sets a <see cref="Microsoft.Xna.Framework.Color"/> value that should be treated as the color of
    ///     transparency in the image.
    /// </summary>
    [DisplayName("Color Key Color")]
    [DefaultValue(typeof(Color), "255,0,255,255")]
    public Color ColorKeyColor { get; set; } = new Color(255, 0, 255, 255);

    /// <summary>
    ///     Gets or Sets a value that indicates whether the <see cref="ColorKeyColor"/> value is enabled.
    /// </summary>
    [DisplayName("Color Key Enabled")]
    [DefaultValue(true)]
    public bool ColorKeyEnabled { get; set; } = true;

    /// <summary>
    ///     Gets or Sets a value that indicates whether mipmaps will be generated for the texture.
    /// </summary>
    [DisplayName("Generate MipMaps")]
    [DefaultValue(false)]
    public bool GenerateMipMaps { get; set; } = false;

    /// <summary>
    ///     Gets or Sets a value that indicates whether the value of each <see cref="Microsoft.Xna.Framework.Color"/>
    ///     element should have the alpha pre-multiplied when generating color data.
    /// </summary>
    [DisplayName("Premultiply Alpha")]
    [DefaultValue(true)]
    public bool PremultiplyAlpha { get; set; } = true;

    /// <summary>
    ///     Gets or Sets a value that indicates whether the texture content generated should be resized to a power of
    ///     two for the width and/or height.
    /// </summary>
    [DisplayName("Resize to Power of Two")]
    [DefaultValue(false)]
    public bool ResizePowerOfTwo { get; set; } = false;

    /// <summary>
    ///     Gets or Sets a value that indicates whether the texture content generated should be resized so that the
    ///     width and height are equal (square texture).
    /// </summary>
    [DisplayName("Make Square")]
    [DefaultValue(false)]
    public bool MakeSquare { get; set; } = false;

    /// <summary>
    ///     Gets or Sets the <see cref="TextureProcessorOutputFormat"/> to use when generating the texture.
    /// </summary>
    [DisplayName("Texture Format")]
    public TextureProcessorOutputFormat TextureFormat { get; set; }

    protected TextureContent CreateTextureContent(string sourceName, ReadOnlySpan<Color> pixels, int imageWidth, int imageHeight, ContentProcessorContext processorContext)
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

    private static void PixelsToBytes(ReadOnlySpan<Color> src, Span<byte> dest)
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
        int columns = cel.Columns;
        int rows = cel.Rows;
        Point offset = cel.Position;
        string tilesetName = cel.Tileset.Name;
        TileContent[] tiles = CreateCelTileContent(cel);

        return new TilemapLayerContent(name, tilesetID, columns, rows, offset, tiles);
    }

    internal TileContent[] CreateCelTileContent(AsepriteTilemapCel cel)
    {
        TileContent[] tiles = new TileContent[cel.TileCount];

        for (int i = 0; i < cel.TileCount; i++)
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
