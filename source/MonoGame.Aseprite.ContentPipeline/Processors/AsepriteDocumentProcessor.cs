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

using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.ContentPipeline.Importers;
using MonoGame.Aseprite.ContentPipeline.Models;
using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Processors
{
    [ContentProcessor(DisplayName = "Aseprite Animation Processor")]
    public sealed class AsepriteDocumentProcessor : ContentProcessor<AsepriteImporterResult, AsepriteDocumentProcessorResult>
    {
        /// <summary>
        ///     Gets or Sets a value indicating the way in which the final sprite sheet
        ///     should be generated. 
        /// </summary>
        [DisplayName("Sheet Type")]
        public ProcessorSheetType SheetType { get; set; } = ProcessorSheetType.Packed;

        /// <summary>
        ///     Gets or Sets a value indicating if duplicate frames should be merged
        ///     into one frame when processed.
        /// </summary>
        [DisplayName("Merge Duplicate Frames")]
        public bool MergeDuplicateFrames { get; set; } = true;

        /// <summary>
        ///     Gets or Sets a value indicating if empty frames should be ignored
        ///     when processed.
        /// </summary>
        /// <remarks>
        ///     TODO: Implement this at some point. It proved tricky for now.
        ///     This is not implemented yet, so it is hidden from the property
        ///     window in the content pipeline tool.
        /// </remarks>
        [DisplayName("Ignore Empty Frames")]
        [System.ComponentModel.Browsable(false)]
        public bool IgnoreEmptyFrames { get; set; } = true;

        /// <summary>
        ///     Gets or Sets a value indicating if only layers that are flaged as
        ///     visible in Aseprite should be imported.
        /// </summary>
        [DisplayName("Only Visible Layers")]
        public bool OnlyVisibleLayers { get; set; } = true;

        /// <summary>
        ///     Gets or Sets the amount of transparent pixels to add between
        ///     each frame and the edge of the spritesheet.
        /// </summary>
        [DisplayName("Border Padding")]
        public int BorderPadding { get; set; } = 0;

        /// <summary>
        ///     Gets or Sets the amount of transparent pixels to add between
        ///     each frame in the spritesheet.
        /// </summary>
        [DisplayName("Spacing")]
        public int Spacing { get; set; } = 0;

        /// <summary>
        ///     Gets or Sets the amount of transparent pixels to add inside
        ///     each frames edge.
        /// </summary>
        [DisplayName("Inner Padding")]
        public int InnerPadding { get; set; } = 0;

        /// <summary>
        ///     Gets or Sets the fully qualified path to output the generated
        ///     spritesheet texture to.
        /// </summary>
        [DisplayName("Output Spritesheet")]
        public string OutputSpriteSheet { get; set; } = string.Empty;

        /// <summary>
        ///     Process method that can be used without the content pipeline.
        /// </summary>
        /// <remarks>
        ///     This overload should only be used when processing the import results of
        ///     an Aseprite file outside of the Content Pipeline Tool.
        /// </remarks>
        /// <param name="input">
        ///     The <see cref="AsepriteImporterResult"/> instance created as part
        ///     of the import process.
        /// </param>
        /// <returns>
        ///     A new <see cref="AsepriteDocumentProcessorResult"/> instance containing the result
        ///     of the processing.
        /// </returns>
        public AsepriteDocumentProcessorResult Process(AsepriteImporterResult input) => Process(input, null);

        /// <summary>
        ///     Proces method that is called by the content pipeline.
        /// </summary>
        /// <remarks>
        ///     If you are using this processor without the assistance of the Content Pipeline Tool,
        ///     then the other Process(AsepriteImporterResult) overload should be used instead.
        /// </remarks>
        /// <param name="input">
        ///     The <see cref="AsepriteImporterResult"/> instance created as part
        ///     of the import process.
        /// </param>
        /// <param name="context"></param>
        /// <returns>
        ///     A new <see cref="AsepriteDocumentProcessorResult"/> instance containing the result
        ///     of the processing.
        /// </returns>
        public override AsepriteDocumentProcessorResult Process(AsepriteImporterResult input, ContentProcessorContext context)
        {
            AsepriteDocumentProcessorOptions options = new AsepriteDocumentProcessorOptions();
            options.BorderPadding = BorderPadding;
            options.IgnoreEmptyFrames = IgnoreEmptyFrames;
            options.InnerPadding = InnerPadding;
            options.MergeDuplicateFrames = MergeDuplicateFrames;
            options.OnlyVisibleLayers = OnlyVisibleLayers;
            options.OutputSpriteSheet = OutputSpriteSheet;
            options.SheetType = SheetType;
            options.Spacing = Spacing;

            //  Read the aseprite document from the stream.
            AsepriteDocument doc;
            using (MemoryStream stream = new MemoryStream(input.Data))
            {
                using (AsepriteReader reader = new AsepriteReader(stream))
                {
                    doc = new AsepriteDocument(reader); ;
                }
            }

            //  Convert the document into an AnimationProcessorResult.
            AsepriteDocumentProcessorResult result = new AsepriteDocumentProcessorResult(doc, options);
            return result;
        }
    }
}
