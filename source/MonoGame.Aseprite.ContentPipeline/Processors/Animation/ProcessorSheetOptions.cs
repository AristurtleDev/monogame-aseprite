using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Aseprite.ContentPipeline.Processors.Animation
{
    [Flags]
    public enum ProcessorSheetOptions
    {
        MergeDuplicates = 1,
        IgnoreEmpty = 2
    }
}
