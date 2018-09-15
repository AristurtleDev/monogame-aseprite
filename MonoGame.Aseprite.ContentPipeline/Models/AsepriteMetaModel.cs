using System.Collections.Generic;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteMetaModel
    {
        public string app { get; set; }
        public string version { get; set; }
        public string image { get; set; }
        public string format { get; set; }
        public AsepriteSizeModel size { get; set; }
        public int scale { get; set; }
        public List<AsepriteFrameTagModel> frameTags { get; set; }
        public List<AsepriteLayerModel> layers;
    }
}
