//--------------------------------------------------------------------------------
//  AsepriteFrameModel
//  Model used while deseralizing the Aseprite .json file that represents
//  a frame of animation
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
namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteFrameModel
    {
        /// <summary>
        ///     The individual frame image filename
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        ///     The frame rectangle that represents the indivual frame within the master spritesheet
        /// </summary>
        public AsepriteRectangleModel frame { get; set; }

        /// <summary>
        ///     Is it rotated
        /// </summary>
        public bool rotated { get; set; }

        /// <summary>
        ///     Was it trimmed
        /// </summary>
        public bool trimmed { get; set; }

        /// <summary>
        ///     Rectangle size of the source sprite
        /// </summary>
        public AsepriteRectangleModel spriteSourceSize { get; set; }

        /// <summary>
        ///     Size of the frame
        /// </summary>
        public AsepriteSizeModel sourceSize { get; set; }

        /// <summary>
        ///     The amount of time in seconds that the frame should be displayed
        /// </summary>
        /// <remarks>
        ///     For the actual frame in MonoGame.Aseprite.AnimationDefinition.Frame
        ///     the duration is a float. However, Aseprite exports durations in int values
        ///     that are the total milliseconds.  We're going to use int here to import properly
        ///     but we'll divide by 1000 during the content writer so that we can read in 
        ///     the total seconds value in game
        /// </remarks>
        public int duration { get; set; }
    }
}
