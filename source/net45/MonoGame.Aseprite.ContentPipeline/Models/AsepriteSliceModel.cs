using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteSliceModel
    {
        /// <summary>
        ///     The name of the slice
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///     The color defined for the slice
        /// </summary>
        public string color { get; set; }

        /// <summary>
        ///     The keys for the slice
        /// </summary>
        public List<AsepriteSliceKeyModel> keys { get; set; }
    }
}
