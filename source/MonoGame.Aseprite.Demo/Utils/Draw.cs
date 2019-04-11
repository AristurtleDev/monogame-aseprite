using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Aseprite.Demo.Utils
{
    public static class Draw
    {
        private static bool _isInitilized = false;

        private static Texture2D _pixel;

        public static void Initilize(GraphicsDevice graphicsDevice)
        {
            //  Create the pixel
            int pixelWidth = 2;
            int pixelHeight = 2;
            _pixel = new Texture2D(graphicsDevice, pixelWidth, pixelHeight);
            Color[] colors = new Color[pixelWidth * pixelHeight];
            for(int i = 0; i < pixelWidth * pixelHeight; i++)
            {
                colors[i] = Color.White;
            }
            _pixel.SetData<Color>(colors);



            _isInitilized = true;
        }

        public static void DrawHollowRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            if (_isInitilized == false)
            {
                throw new Exception("You must initilize Draw from your GameBase before using");
            }

            //  Calcuate the rects for the top, right, bottom, and left edges of the hollow rectangle
            Rectangle topRect = new Rectangle(rect.X, rect.Y, rect.Width, _pixel.Height);
            Rectangle rightRect = new Rectangle(rect.X + rect.Width, rect.Y, _pixel.Width, rect.Height);
            Rectangle bottomRect = new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, _pixel.Height);
            Rectangle leftRect = new Rectangle(rect.X, rect.Y, _pixel.Width, rect.Height);

            //  Draw the top edge
            spriteBatch.Draw(_pixel, topRect, color);

            //  Draw right edge
            spriteBatch.Draw(_pixel, rightRect, color);

            //  Draw bottom edge
            spriteBatch.Draw(_pixel, bottomRect, color);

            //  Draw left edge
            spriteBatch.Draw(_pixel, leftRect, color);



        }

    }
}
