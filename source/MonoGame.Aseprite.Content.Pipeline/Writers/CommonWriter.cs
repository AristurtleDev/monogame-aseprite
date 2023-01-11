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
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

/// <summary>
///     An abstract class that provides static method for writing common data
///     to the xnb file for the implementing type.
/// </summary>
/// <typeparam name="T">
/// </typeparam>
public abstract class CommonWriter<T> : ContentTypeWriter<T>
{
    internal static void WritePoint(ContentWriter output, Point value)
    {
        output.Write(value.X);
        output.Write(value.Y);
    }

    internal static void WriteRectangle(ContentWriter output, Rectangle rect)
    {
        WritePoint(output, rect.Location);
        WritePoint(output, rect.Size);
    }

    internal static void WritePixels(ContentWriter output, Color[] pixels)
    {
        output.Write(pixels.Length);

        for (int i = 0; i < pixels.Length; i++)
        {
            output.Write(pixels[i]);
        }
    }

    internal static void WriteTextureContent(ContentWriter output, TextureContent content)
    {
        WritePoint(output, content.Size);
        WritePixels(output, content.Pixels);
    }

    // internal static void WriteTextureAtlasContent(ContentWriter output, TextureAtlasContent content)
    // {
    //     output.Write(content.Name);
    //     WriteTextureContent(output, content.TextureContent);

    //     output.Write(content.Regions.Count);
    //     foreach (TextureAtlasRegionContent region in content.Regions)
    //     {
    //         WriteTextureAtlasRegionContent(output, region);
    //     }
    // }

    // internal static void WriteTextureAtlasRegionContent(ContentWriter output, TextureAtlasRegionContent content)
    // {
    //     output.Write(content.Name);
    //     WriteRectangle(output, content.Bounds);
    // }

    internal static void WriteSpriteSheetContent(ContentWriter output, SpriteSheetContent content)
    {
        output.Write(content.Name);
        WriteTextureContent(output, content.TextureContent);

        output.Write(content.Frames.Count);
        foreach (SpriteSheetFrameContent frameContent in content.Frames)
        {
            WriteSpriteSheetFrameContent(output, frameContent);
        }

        output.Write(content.AnimationDefinitions.Count);
        foreach (SpriteSheetAnimationDefinitionContent animationContent in content.AnimationDefinitions)
        {
            WriteSpriteSheetAnimationDefinitionContent(output, animationContent);
        }
    }

    internal static void WriteSpriteSheetFrameContent(ContentWriter output, SpriteSheetFrameContent content)
    {
        output.Write(content.Name);
        WriteRectangle(output, content.Bounds);
        output.Write(content.Duration);

        output.Write(content.Regions.Count);
        foreach (KeyValuePair<string, SpriteSheetFrameRegionContent> kvp in content.Regions)
        {
            WriteSpriteSheetFrameRegionContent(output, kvp.Value);
        }
    }

    internal static void WriteSpriteSheetFrameRegionContent(ContentWriter output, SpriteSheetFrameRegionContent content)
    {
        output.Write(content.Name);
        output.Write(content.Color);
        WriteRectangle(output, content.Bounds);

        output.Write(content.CenterBounds is not null);
        if (content.CenterBounds is not null)
        {
            WriteRectangle(output, content.CenterBounds.Value);
        }

        output.Write(content.Pivot is not null);
        if (content.Pivot is not null)
        {
            WritePoint(output, content.Pivot.Value);
        }
    }

    internal static void WriteSpriteSheetAnimationDefinitionContent(ContentWriter output, SpriteSheetAnimationDefinitionContent content)
    {
        output.Write(content.Name);
        output.Write(content.LoopReversePingPongMask);

        output.Write(content.FrameIndexes.Length);
        for (int i = 0; i < content.FrameIndexes.Length; i++)
        {
            output.Write(content.FrameIndexes[i]);
        }
    }

    internal static void WriteTilesetCollectionContent(ContentWriter output, TilesetCollectionContent content)
    {
        output.Write(content.Tilesets.Count);

        for (int i = 0; i < content.Tilesets.Count; i++)
        {
            WriteTilesetContent(output, content.Tilesets[i]);
        }
    }

    internal static void WriteTilesetContent(ContentWriter output, TilesetContent content)
    {
        output.Write(content.Name);
        output.Write(content.TileCount);
        WritePoint(output, content.TileSize);
        WriteTextureContent(output, content.TextureContent);
    }
}
