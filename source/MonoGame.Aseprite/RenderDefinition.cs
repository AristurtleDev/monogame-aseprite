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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite
{
    /// <summary>
    ///     Defines various properties used by the Sprite class during rendering
    /// </summary>
    public class RenderDefinition
    {
        /// <summary>
        ///     The source rectangle to use when rendering
        /// </summary>
        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        ///     The color to use when rendering
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        ///     The amount of rotation to apply when rendering
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        ///     The xy-coordinate to use as the origin point when rendering
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        ///     The xy-values for how much to scale the image when rendering
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        ///     The SpriteEffects to use when rendering
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        ///     The layer depth to render at
        /// </summary>
        public float LayerDepth { get; set; }

        /// <summary>
        ///     Creates a new <see cref="RenderDefinition"/> instance
        /// </summary>
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
