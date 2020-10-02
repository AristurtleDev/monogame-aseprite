using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteSliceKeyModel
    {
        /// <summary>
        ///     The frame for the slice key
        /// </summary>
        public int frame { get; set; }

        /// <summary>
        ///     The deifned bounds for the slice key
        /// </summary>
        public AsepriteRectangleModel bounds { get; set; }
    }
}
