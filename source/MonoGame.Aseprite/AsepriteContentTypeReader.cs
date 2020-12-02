/* ------------------------------------------------------------------------------
    Copyright (c) 2020 Christopher Whitley

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:
    
    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
------------------------------------------------------------------------------ */

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite
{
    public class AsepriteContentTypeReader : ContentTypeReader<AsepriteImportResult>
    {
        protected override AsepriteImportResult Read(ContentReader input, AsepriteImportResult existingInstance)
        {
            if (existingInstance != null)
            {
                return existingInstance;
            }

            AsepriteImportResult result = new AsepriteImportResult();

            result.TextureWidth = input.ReadInt32();
            result.TextureHeight = input.ReadInt32();

            int totalPixels = input.ReadInt32();
            Color[] colorData = new Color[totalPixels];

            for (int i = 0; i < totalPixels; i++)
            {
                colorData[i] = input.ReadColor();
            }

            result.Texture = new Texture2D(input.GetGraphicsDevice(), result.TextureWidth, result.TextureHeight);
            result.Texture.SetData<Color>(colorData);

            int totalFrames = input.ReadInt32();
            for (int i = 0; i < totalFrames; i++)
            {
                AsepriteImportResult.Frame frame = new AsepriteImportResult.Frame();

                frame.X = input.ReadInt32();
                frame.Y = input.ReadInt32();
                frame.Width = input.ReadInt32();
                frame.Height = input.ReadInt32();
                frame.Duration = input.ReadInt32() / 1000.0f;

                result.Frames.Add(frame);
            }

            int totalAnimations = input.ReadInt32();
            for (int i = 0; i < totalAnimations; i++)
            {
                AsepriteImportResult.Animation animation = new AsepriteImportResult.Animation();

                animation.Name = input.ReadString();
                animation.From = input.ReadInt32();
                animation.To = input.ReadInt32();
                animation.Direction = input.ReadInt32();
                animation.Color = input.ReadColor();

                result.Animations.Add(animation.Name, animation);
            }

            int totalSlices = input.ReadInt32();
            for (int i = 0; i < totalSlices; i++)
            {
                AsepriteImportResult.Slice slice = new AsepriteImportResult.Slice();

                slice.Name = input.ReadString();
                slice.Color = input.ReadColor();

                int totalSliceKeys = input.ReadInt32();
                slice.SliceKeys = new Dictionary<int, AsepriteImportResult.SliceKey>();
                for (int k = 0; k < totalSliceKeys; k++)
                {
                    AsepriteImportResult.SliceKey sliceKey = new AsepriteImportResult.SliceKey();

                    sliceKey.FrameIndex = input.ReadInt32();
                    sliceKey.X = input.ReadInt32();
                    sliceKey.Y = input.ReadInt32();
                    sliceKey.Width = input.ReadInt32();
                    sliceKey.Height = input.ReadInt32();

                    sliceKey.HasNinePatch = input.ReadBoolean();
                    sliceKey.CenterX = sliceKey.HasNinePatch ? input.ReadInt32() : 0;
                    sliceKey.CenterY = sliceKey.HasNinePatch ? input.ReadInt32() : 0;
                    sliceKey.CenterWidth = sliceKey.HasNinePatch ? input.ReadInt32() : 0;
                    sliceKey.CenterHeight = sliceKey.HasNinePatch ? input.ReadInt32() : 0;

                    sliceKey.HasPivot = input.ReadBoolean();
                    sliceKey.PivotX = sliceKey.HasPivot ? input.ReadInt32() : 0;
                    sliceKey.PivotY = sliceKey.HasPivot ? input.ReadInt32() : 0;

                    slice.SliceKeys.Add(sliceKey.FrameIndex, sliceKey);
                }

                result.Slices.Add(slice.Name, slice);
            }

            return result;
        }
    }
}
