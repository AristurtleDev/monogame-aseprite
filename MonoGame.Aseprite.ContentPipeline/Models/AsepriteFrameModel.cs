namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteFrameModel
    {
        public string filename { get; set; }
        public AsepriteRectangleModel frame { get; set; }
        public bool rotated { get; set; }
        public bool trimmed { get; set; }
        public AsepriteRectangleModel spriteSourceSize { get; set; }
        public AsepriteSizeModel sourceSize { get; set; }
        public int duration { get; set; }
    }
}
