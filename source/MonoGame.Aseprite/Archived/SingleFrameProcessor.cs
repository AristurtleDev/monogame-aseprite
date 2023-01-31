// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2018-2023 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */

// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using MonoGame.Aseprite.AsepriteTypes;

// namespace MonoGame.Aseprite.Processors;

// /// <summary>
// /// Defines a processor that process a sprite from a frame in an aseprite file.
// /// </summary>
// public static class SingleFrameProcessor
// {
//     /// <summary>
//     /// Processes a sprite from a frame in an aseprite file.
//     /// </summary>
//     /// <param name="device">The graphics device used to create graphical resources.</param>
//     /// <param name="file">The Aseprite file containing the frame to process the sprite from.</param>
//     /// <param name="configuration">The configuration for this process.</param>
//     /// <returns>The sprite that is created by this method.</returns>
//     public static Sprite Process(GraphicsDevice device, AsepriteFile file, SingleFrameProcessorConfiguration configuration)
//     {
//         RawTexture rawTexture = CreateRawTexture(file, configuration);
//         Texture2D texture = CreateTexture(device, rawTexture);
//         TextureRegion region = new(texture.Name, texture, texture.Bounds);
//         return new(region.Name, region);
//     }

//     /// <summary>
//     ///     Processes a single <see cref="AsepriteFrame"/> from an <see cref="AsepriteFile"/> as a
//     ///     <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/>.
//     /// </summary>
//     /// <param name="device">
//     ///     The instance of the <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> class used to create the
//     ///     graphical resources.
//     /// </param>
//     /// <param name="file">
//     ///     The instance of the <see cref="AsepriteFile"/> to process the <see cref="AsepriteFrame"/> from.
//     /// </param>
//     /// <param name="configure">
//     ///     An action method used to build the instance of the <see cref="SingleFrameProcessorConfiguration"/> class
//     ///     that defines the configurations for this process.
//     /// </param>
//     /// <returns>
//     ///     The instance of the <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> class that is created by this
//     ///     method.
//     /// </returns>
//     public static Texture2D Process(GraphicsDevice device, AsepriteFile file, Action<SingleFrameProcessorConfiguration> configure)
//     {
//         SingleFrameProcessorConfiguration configuration = new();
//         configure(configuration);
//         return Process(device, file, configuration);
//     }

//     internal static RawTexture CreateRawTexture(AsepriteFile file, SingleFrameProcessorConfiguration configuration)
//     {
//         if (configuration.FrameIndex < 0 || configuration.FrameIndex >= file.Frames.Length)
//         {
//             throw new ArgumentOutOfRangeException(nameof(configuration.FrameIndex),
//                                                   $"{configuration.FrameIndex} cannot be less than zero or greater than or equal to the total number of frames in the Aseprite file.");
//         }

//         Color[] pixels = file.Frames[configuration.FrameIndex].FlattenFrame(configuration.OnlyVisibleLayers, configuration.IncludeBackgroundLayer, configuration.IncludeTilemapLayers);

//         return new(file.Name, pixels, file.CanvasWidth, file.CanvasHeight);
//     }

//     internal static Texture2D CreateTexture(GraphicsDevice device, RawTexture rawTexture)
//     {
//         Texture2D texture = new(device, rawTexture.Width, rawTexture.Height, mipmap: false, SurfaceFormat.Color);
//         texture.SetData<Color>(rawTexture.Pixels);
//         return texture;
//     }
// }