namespace MonoGame.Aseprite.ContentPipeline.Processors.Animation
{
    public struct AnimationProcessorOptions
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

        /// <summary>
        ///     The fully qualified path to output the generated spritesheet
        ///     texture to.
        /// </summary>
        public string OutputSpriteSheet;
    }
}
