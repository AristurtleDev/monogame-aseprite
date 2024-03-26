using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AseColor = AsepriteDotNet.Common.Rgba32;
using AsePoint = AsepriteDotNet.Common.Point;
using AseRectangle = AsepriteDotNet.Common.Rectangle;
using AseTexture = AsepriteDotNet.Texture;

namespace MonoGame.Aseprite.Utils;

internal static class AsepriteDotNetExtensions
{
    internal static Texture2D ToTexture2D(this AseTexture texture, GraphicsDevice device)
    {
        Texture2D texture2D = new Texture2D(device, texture.Size.Width, texture.Size.Height);
        texture2D.Name = texture.Name;
        texture2D.SetData(texture.Pixels.ToArray());
        return texture2D;
    }

    internal unsafe static Color ToXnaColor(this AseColor color) => Unsafe.As<AseColor, Color>(ref color);

    internal static Point ToXnaPoint(this AsePoint point) => new Point(point.X, point.Y);

    internal static Vector2 ToXnaVector2(this AsePoint point) => new Vector2(point.X, point.Y);
    internal static Rectangle ToXnaRectangle(this AseRectangle rect) => new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
}
