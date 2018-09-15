using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite
{
    public class RenderDefinition
    {
        public Rectangle? SourceRectangle { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public float LayerDepth { get; set; }

        public RenderDefinition()
        {
            //  Define default values
            this.SourceRectangle = null;
            this.Color = Color.White;
            this.Rotation = 0.0f;
            this.Origin = Vector2.Zero;
            this.Scale = Vector2.One;
            this.SpriteEffect = SpriteEffects.None;
            this.LayerDepth = 0.0f;
        }


    }
}
