// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AseColor = AsepriteDotNet.Common.Rgba32;
using AsePoint = AsepriteDotNet.Common.Point;
using AseRectangle = AsepriteDotNet.Common.Rectangle;
using AseTexture = AsepriteDotNet.Texture;

namespace MonoGame.Aseprite.Utils;

/// <summary>
/// Defines extension methods for translating between AsepriteDotNet types to MonoGame types.
/// </summary>
public static class AsepriteDotNetExtensions
{
    /// <summary>
    /// Converts an AsepriteDotNet Texture to a MonoGame Texture2D object.
    /// </summary>
    /// <param name="texture">The AsepriteDotNet texture to convert.</param>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <returns>The converted MonoGame Texture2D object.</returns>
    public static Texture2D ToTexture2D(this AseTexture texture, GraphicsDevice device)
    {
        Texture2D texture2D = new Texture2D(device, texture.Size.Width, texture.Size.Height);
        texture2D.Name = texture.Name;
        texture2D.SetData(texture.Pixels.ToArray());
        return texture2D;
    }

    /// <summary>
    /// Converts an AsepriteDotNet color to a MonoGame Color object.
    /// </summary>
    /// <param name="color">The AsepriteDotNet color to convert.</param>
    /// <returns>The converted MonoGame Color object.</returns>
    public unsafe static Color ToXnaColor(this AseColor color) => Unsafe.As<AseColor, Color>(ref color);

    /// <summary>
    /// Converts an AsepriteDotNet point to a MonoGame Point object.
    /// </summary>
    /// <param name="point">The AsepriteDotNet point to convert.</param>
    /// <returns>The converted MonoGame Point object.</returns>
    public static Point ToXnaPoint(this AsePoint point) => new Point(point.X, point.Y);

    /// <summary>
    /// Converts an AsepriteDotNet point to a MonoGame Vector2 object.
    /// </summary>
    /// <param name="point">The AsepriteDotNet point to convert.</param>
    /// <returns>The converted MonoGame Vector2 object.</returns>
    public static Vector2 ToXnaVector2(this AsePoint point) => new Vector2(point.X, point.Y);

    /// <summary>
    /// Converts an AsepriteDotNet rectangle to a MonoGame Rectangle object.
    /// </summary>
    /// <param name="rect">The AsepriteDotNet rectangle to convert.</param>
    /// <returns>The converted MonoGame Rectangle object.</returns>
    public static Rectangle ToXnaRectangle(this AseRectangle rect) => new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
}
