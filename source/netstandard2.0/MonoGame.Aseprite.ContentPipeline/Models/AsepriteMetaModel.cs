//--------------------------------------------------------------------------------
//  AsepriteMetaModel
//  Model used while deseralizing the Aseprite .json file that represents
//  the "meta" node of the json
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
using System.Collections.Generic;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    public class AsepriteMetaModel
    {
        /// <summary>
        ///     The url to the aseprite application
        /// </summary>
        public string app { get; set; }

        /// <summary>
        ///     The version number of aseprite used 
        /// </summary>
        public string version { get; set; }

        /// <summary>
        ///     Full path to the image file
        /// </summary>
        public string image { get; set; }

        /// <summary>
        ///     Color format used
        /// </summary>
        public string format { get; set; }

        /// <summary>
        ///     Total pixel size of the image file
        /// </summary>
        public AsepriteSizeModel size { get; set; }

        /// <summary>
        ///     Scale of the image
        /// </summary>
        public int scale { get; set; }

        /// <summary>
        ///     Tags used to tag groups of frames
        /// </summary>
        public List<AsepriteFrameTagModel> frameTags { get; set; }

        /// <summary>
        ///     Layers used in the image
        /// </summary>
        public List<AsepriteLayerModel> layers;

        /// <summary>
        ///     Sices defined for the animations
        /// </summary>
        public List<AsepriteSliceModel> slices;
    }
}
