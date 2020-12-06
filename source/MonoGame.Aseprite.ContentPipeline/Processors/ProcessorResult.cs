using MonoGame.Aseprite.ContentPipeline.Models;

namespace MonoGame.Aseprite.ContentPipeline.Processors
{
    public abstract class ProcessorResult
    {
        protected AsepriteDocument _document;

        public ProcessorResult(AsepriteDocument document)
        {
            _document = document;
        }
    }
}
