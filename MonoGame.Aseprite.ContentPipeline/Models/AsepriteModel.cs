using System.Collections.Generic;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteModel
    {
        public AsepriteMetaModel meta { get; set; }
        public List<AsepriteFrameModel> frames { get; set; }
    }
}
