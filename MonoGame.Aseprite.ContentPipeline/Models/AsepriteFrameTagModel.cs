//--------------------------------------------------------------------------------
//  AsepriteFrameTagModel
//  Model used while deseralizing the Aseprite .json file that represents
//  a frame tag used to tag a group of frames in the animation
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
    public class AsepriteFrameTagModel
    {
        /// <summary>
        ///     The name of the tag
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///     The starting frame
        /// </summary>
        public int from { get; set; }

        /// <summary>
        ///     The ending frame
        /// </summary>
        public int to { get; set; }

        /// <summary>
        ///     The direction of the animation
        /// </summary>
        public string direction { get; set; }
    }
}
