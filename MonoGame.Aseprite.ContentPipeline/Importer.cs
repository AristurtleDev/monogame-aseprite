using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using TInput = System.String;

namespace MonoGame.Aseprite.ContentPipeline
{
    [ContentImporter(".json", DisplayName = "Aseprite Animation Importer", DefaultProcessor = "Processor")]
    public class Importer : ContentImporter<TInput>
    {

        public override TInput Import(string filename, ContentImporterContext context)
        {
            //  Read the json from the file and return it
            return File.ReadAllText(filename);

            //string json = File.ReadAllText(filename);

            ////  Convert the json into an AsepriteModel
            //TInput model = Newtonsoft.Json.JsonConvert.DeserializeObject<TInput>(json);

            ////  The way this is setup, the actual image file that this animates needs to be
            ////  located in the same folder that this json file is. So we validate it is there
            ////  and if it is not, then we throw an error
            //var imageFileName = Path.GetFileName(model.meta.image);

            //var pathToCheck = Path.Combine(Path.GetDirectoryName(filename), imageFileName);
            
            

            //if (!File.Exists(pathToCheck))
            //{
            //    //  Build excpetion message
            //    string exceptionMessage = $"The file <{imageFileName}> must be located in the same directory as <{filename}>";

            //    //  Log message
            //    context.Logger.LogImportantMessage(exceptionMessage);

            //    //  Throw excpetion
            //    throw new Exception(exceptionMessage);
            //}
            //else
            //{
            //    //  The image file was there (it will be imported by the content pipeline importer defaults)
            //    //  so we're good, return the model
            //    return model;
            //}

        }

    }

}
