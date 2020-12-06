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
using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepritePaletteColor
    {
        //  Holds the value of the flag set for this palette color.
        private AsepritePaletteFlags _flag;

        /// <summary>
        ///     Gets the red value for this palette color.
        /// </summary>
        public byte Red { get; private set; }

        /// <summary>
        ///     Gets the green value for this palette color.
        /// </summary>
        public byte Green { get; private set; }

        /// <summary>
        ///     Gets the Blue value for this palette color.
        /// </summary>
        public byte Blue { get; private set; }

        /// <summary>
        ///     Gets the alpha value for this palette color.
        /// </summary>
        public byte Alpha { get; private set; }

        /////////////// <summary>
        ///////////////     Gets a monogame compatible Color that represents
        ///////////////     the color of this palette entry.
        /////////////// </summary>
        ////////////public Color Color { get; private set; }

        /// <summary>
        ///     Gets the name of this palette color if it has a name.
        ///     Check the <see cref="HasName"/> property first to see if
        ///     a name exists.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets a value indicating if this palette color has a name.
        /// </summary>
        public bool HasName => (_flag & AsepritePaletteFlags.HasName) != 0;

        /// <summary>
        ///     Creates a new <see cref="AsepritePaletteColor"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        public AsepritePaletteColor(AsepriteReader reader)
        {
            AsepritePaletteFlags flag = (AsepritePaletteFlags)reader.ReadWORD();
            Red = reader.ReadByte();
            Green = reader.ReadByte();
            Blue = reader.ReadByte();
            Alpha = reader.ReadByte();

            //////////Color = new Color(Utils.BytesToPacked(Red, Green, Blue, Alpha));


            //  If the HasName flag is present, we need to read the string value, however
            //  this seems to always never exist. Not sure what the field is for, but may
            //  just be some compatibility thing with named palette formats like .gpl.
            if ((flag & AsepritePaletteFlags.HasName) != 0)
            {
                Name = reader.ReadString();
            }
        }
    }
}
