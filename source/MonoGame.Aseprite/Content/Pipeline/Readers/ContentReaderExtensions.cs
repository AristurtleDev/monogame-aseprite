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

using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Content.Pipeline.Readers;

internal static class ContentReaderExtensions
{
    internal static Rectangle ReadRectangle(this ContentReader reader)
    {
        Rectangle rect;

        rect.X = reader.ReadInt32();
        rect.Y = reader.ReadInt32();
        rect.Width = reader.ReadInt32();
        rect.Height = reader.ReadInt32();

        return rect;
    }

    internal static Point ReadPoint(this ContentReader reader)
    {
        Point point;

        point.X = reader.ReadInt32();
        point.Y = reader.ReadInt32();

        return point;
    }

    internal static TimeSpan ReadTimeSpan(this ContentReader reader)
    {
        long ticks = reader.ReadInt64();
        return TimeSpan.FromTicks(ticks);
    }

    internal static Texture2D ReadTexture2D(this ContentReader reader, Texture2D? existingInstance = default)
    {
        //  This is ugly, and I apologize for that.
        //  In MonoGame.Aseprite.Content.Pipeline assembly, the texture image
        //  data that is generated is done using the the native MonoGame
        //  TextureProcessor and then written to the xnb file using the same
        //  structure as the native MonoGame Texture2DWriter.  This is done
        //  so that the texture data generated and written are inline with the
        //  native processes.  However, on this side where we are now reading
        //  the data back in, we would want to use the native MonoGame
        //  Texture2DReader.  Unfortunately, they have it marked as internal to
        //  their assembly.  So we'll need to pull it out using reflection.

        //  Using the ContentReader type to get the Assembly since it's in the
        //  same assembly as the internal Texture2DReader
        if (Assembly.GetAssembly(typeof(ContentReader)) is not Assembly assembly)
        {
            throw new InvalidOperationException($"Unable to load Microsoft.Xna.Framework assembly");
        }

        //  Get the Type using the fully qualified namespace
        if (assembly.GetType("Microsoft.Xna.Framework.Content.Texture2DReader") is not Type texture2DReaderType)
        {
            throw new InvalidOperationException($"Unable to load Texture2DReader type from assembly");
        }

        //  Using the type, create an instance. It's parameterless which helps
        //  a lot here
        if (Activator.CreateInstance(texture2DReaderType) is not object texture2DReaderInstance)
        {
            throw new InvalidOperationException($"Unable to create instance of Texture2DReader");
        }

        //  Get the info for the Read method, which is marked as protected
        if (texture2DReaderType.GetMethod("Read", BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(ContentReader), typeof(Texture2D) }) is not MethodInfo readMethod)
        {
            throw new InvalidOperationException($"Unable to get Process method form Texture2DReader type");
        }

        //  Using the method info and the instance that was created above,
        //  execute the method to use the native process to read the texture
        if (readMethod.Invoke(texture2DReaderInstance, new object?[] { reader, existingInstance }) is not Texture2D texture)
        {
            throw new InvalidOperationException("$Unable to create texture from Texture2DReader.Read method");
        }

        return texture;
    }

    internal static SpriteSheet ReadSpriteSheet(this ContentReader reader)
    {
        string name = reader.ReadString();
        Texture2D texture = reader.ReadTexture2D(existingInstance: null);
        SpriteSheet spriteSheet = new(name, texture);

        int regionCount = reader.ReadInt32();
        for (int i = 0; i < regionCount; i++)
        {
            string regionName = reader.ReadString();
            Rectangle bounds = reader.ReadRectangle();
            _ = spriteSheet.CreateRegion(regionName, bounds);
        }

        int animationCount = reader.ReadInt32();
        for (int i = 0; i < animationCount; i++)
        {
            string animationName = reader.ReadString();
            byte flags = reader.ReadByte();

            bool isLooping = (flags & 1) != 0;
            bool isReversed = (flags & 2) != 0;
            bool isPingPong = (flags & 4) != 0;

            AnimationCycleBuilder builder = new(animationName, spriteSheet);
            builder.IsLooping(isLooping)
                   .IsReversed(isReversed)
                   .IsPingPong(isPingPong);

            int frameCount = reader.ReadInt32();
            for (int j = 0; j < frameCount; j++)
            {
                int index = reader.ReadInt32();
                TimeSpan duration = reader.ReadTimeSpan();
                builder.AddFrame(index, duration);
            }

            AnimationCycle cycle = builder.Build();
            spriteSheet.AddAnimationCycle(cycle);
        }

        return spriteSheet;
    }

    internal static Tileset ReadTileset(this ContentReader reader)
    {
        int id = reader.ReadInt32();
        string name = reader.ReadString();
        int tileWidth = reader.ReadInt32();
        int tileHeight = reader.ReadInt32();
        Texture2D texture = reader.ReadTexture2D(existingInstance: null);
        Tileset tileset = new(name, texture, tileWidth, tileHeight);
        return tileset;
    }
}
