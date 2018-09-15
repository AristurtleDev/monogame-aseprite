using Microsoft.Xna.Framework.Content.Pipeline;
using TInput = System.String;
using TOutput = MonoGame.Aseprite.ContentPipeline.Models.AsepriteModel;

namespace MonoGame.Aseprite.ContentPipeline
{
    [ContentProcessor(DisplayName = "Aseprite Animation Processor")]
    public class Processor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            //  Convert the input json string to an AsepriteModel
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Aseprite.ContentPipeline.Models.AsepriteModel>(input);
        }
    }
}