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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Content.Pipeline.Readers;

/// <summary>
///     An abstract class that provides static methods for reading common data
///     from an xnb file for the implementing type.
/// </summary>
/// <typeparam name="T">
/// </typeparam>
public abstract class CommonReader<T> : ContentTypeReader<T>
{
    internal static Point ReadPoint(ContentReader input)
    {
        int x = input.ReadInt32();
        int y = input.ReadInt32();
        return new(x, y);
    }

    internal static Rectangle ReadRectangle(ContentReader input)
    {
        Point location = ReadPoint(input);
        Point size = ReadPoint(input);
        return new(location, size);
    }

    internal static void ReadPixels(ContentReader input, Color[] pixels)
    {
        int len = input.ReadInt32();

        if (pixels.Length != len)
        {
            throw new InvalidOperationException($"Pixel count does not match expected number of pixels to read");
        }

        for (int i = 0; i < len; i++)
        {
            pixels[i] = input.ReadColor();
        }
    }

    internal static Texture2D ReadTexture(ContentReader input)
    {
        Point size = ReadPoint(input);

        Color[] pixels = new Color[size.X * size.Y];
        ReadPixels(input, pixels);

        Texture2D texture = CreateTexture(input.GetGraphicsDevice(), size, pixels);
        return texture;
    }

    private static Texture2D CreateTexture(GraphicsDevice device, Point size, Color[] pixels)
    {
        Texture2D texture = new(device, size.X, size.Y, false, SurfaceFormat.Color);
        texture.SetData<Color>(pixels);
        return texture;
    }

    // internal static TextureAtlas ReadTextureAtlas(ContentReader input)
    // {
    //     string name = input.ReadString();
    //     Texture2D texture = ReadTexture(input);

    //     TextureAtlas atlas = new(name, texture);
    //     ReadTextureAtlasRegions(input, atlas);
    // }

    // internal static void ReadTextureAtlasRegions(ContentReader input, TextureAtlas atlas)
    // {
    //     int count = input.ReadInt32();

    //     for (int i = 0; i < count; i++)
    //     {
    //         string name = input.ReadString();
    //         Rectangle bounds = ReadRectangle(input);

    //         atlas.CreateRegion(name, bounds);
    //     }
    // }


    internal static SpriteSheet ReadSpriteSheet(ContentReader input)
    {
        string name = input.ReadString();
        Texture2D texture = ReadTexture(input);

        SpriteSheet spriteSheet = new(name, texture);
        ReadSpritesheetFrames(input, spriteSheet);
        ReadSpriteSheetAnimationDefinitions(input, spriteSheet);

        return spriteSheet;
    }

    internal static void ReadSpritesheetFrames(ContentReader input, SpriteSheet spriteSheet)
    {
        int count = input.ReadInt32();

        for (int i = 0; i < count; i++)
        {
            ReadSpritesheetFrame(input, spriteSheet);
        }
    }

    internal static void ReadSpritesheetFrame(ContentReader input, SpriteSheet spriteSheet)
    {
        string name = input.ReadString();
        Rectangle bounds = ReadRectangle(input);
        int durationInMilliseconds = input.ReadInt32();

        TimeSpan duration = TimeSpan.FromMilliseconds(durationInMilliseconds);

        SpriteSheetFrame frame = spriteSheet.CreateFrame(name, bounds, duration);

        ReadSpriteSheetFrameRegions(input, frame);
    }

    internal static void ReadSpriteSheetFrameRegions(ContentReader input, SpriteSheetFrame frame)
    {
        int count = input.ReadInt32();

        for (int i = 0; i < count; i++)
        {
            ReadSpriteSheetFrameRegion(input, frame);
        }
    }

    internal static void ReadSpriteSheetFrameRegion(ContentReader input, SpriteSheetFrame frame)
    {
        string name = input.ReadString();
        Color color = input.ReadColor();
        Rectangle bounds = ReadRectangle(input);

        Rectangle? centerBounds = default;
        Point? pivot = default;

        if (input.ReadBoolean())
        {
            centerBounds = ReadRectangle(input);
        }

        if (input.ReadBoolean())
        {
            pivot = ReadPoint(input);
        }

        frame.AddRegion(name, bounds, color, centerBounds, pivot);
    }

    internal static void ReadSpriteSheetAnimationDefinitions(ContentReader input, SpriteSheet spriteSheet)
    {
        int count = input.ReadInt32();

        for (int i = 0; i < count; i++)
        {
            ReadSpriteSheetAnimationDefinition(input, spriteSheet);
        }
    }

    internal static void ReadSpriteSheetAnimationDefinition(ContentReader input, SpriteSheet spriteSheet)
    {
        string name = input.ReadString();
        byte flags = input.ReadByte();
        int indexCount = input.ReadInt32();

        int[] indexes = new int[indexCount];
        for (int i = 0; i < indexCount; i++)
        {
            indexes[i] = input.ReadInt32();
        }

        bool isLooping = (flags & 1) != 0;
        bool isReversed = (flags & 2) != 0;
        bool isPingPong = (flags & 4) != 0;

        _ = spriteSheet.CreateAnimation(name, indexes, isLooping, isReversed, isPingPong);
    }

    internal static TilesetCollection ReadTilesetCollection(ContentReader input)
    {
        TilesetCollection collection = new();
        ReadTilesets(input, collection);
        return collection;
    }

    internal static void ReadTilesets(ContentReader input, TilesetCollection collection)
    {
        int count = input.ReadInt32();

        for (int i = 0; i < count; i++)
        {
            Tileset tileset = ReadTileset(input);
            collection.AddTileset(tileset);
        }
    }

    internal static Tileset ReadTileset(ContentReader input)
    {
        string name = input.ReadString();
        int count = input.ReadInt32();
        Point tileSize = ReadPoint(input);
        Texture2D texture = ReadTexture(input);
        Tileset tileset = new(name, texture, tileSize);
        return tileset;
    }




}
