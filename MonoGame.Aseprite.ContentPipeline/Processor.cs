//--------------------------------------------------------------------------------
//  Processor
//  Processes the information given by the importer to prep it for writing
//--------------------------------------------------------------------------------
//
//                              License
//  
//    Copyright(c) 2018 Chris Whitley
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in
//    all copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//    THE SOFTWARE.
//--------------------------------------------------------------------------------
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