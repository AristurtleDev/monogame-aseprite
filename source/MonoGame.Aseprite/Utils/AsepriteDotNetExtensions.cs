using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
