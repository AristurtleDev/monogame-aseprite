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

namespace MonoGame.Aseprite.ContentPipeline.Processors
{
    /// <summary>
    ///     Provies the values provided by the user in the content pipeline
    ///     property window.
    /// </summary>
    public struct AsepriteDocumentProcessorOptions
    {
        /// <summary>
        ///     Indicates the way in which the final spritesheet should be
        ///     generated.
        /// </summary>
        public ProcessorSheetType SheetType;

        /// <summary>
        ///     A value indicating if duplicate frames should be merged into
        ///     one frame when processed.
        /// </summary>
        public bool MergeDuplicateFrames;

        /// <summary>
        ///     A value indicating if empty frames should be ignored when
        ///     processing.
        /// </summary>
        public bool IgnoreEmptyFrames;

        /// <summary>
        ///     A value indicating if only layers that are flagged as visible
        ///     in Aseprite should be processed.
        /// </summary>
        public bool OnlyVisibleLayers;

        /// <summary>
        ///     The amount of transparent pixels to add between each frame
        ///     and the edge of the spritesheet;
        /// </summary>
        public int BorderPadding;

        /// <summary>
        ///     The amount of transparent pixels to add between each frame
        ///     in the spritesheet.
        /// </summary>
        public int Spacing;

        /// <summary>
        ///     The amount of transparent pixels to add to the inside of each
        ///     frame's edge.
        /// </summary>
        public int InnerPadding;
    }
}
