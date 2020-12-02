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

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.ContentPipeline.Models;

namespace MonoGame.Aseprite.ContentPipeline
{
    public class AsepriteProcessorResult
    {
        /// <summary>
        ///     Gets an array of color data that represents the final packed
        ///     texture. Array is in top-to-bottom order with pixels reading
        ///     left-to-right.
        /// </summary>
        public Color[] ColorData { get; private set; }

        /// <summary>
        ///     Gets an array of coor data the represents the palette of colors
        ///     used in the Aseprite document.
        /// </summary>
        public Color[] Palette { get; private set; }

        /// <summary>
        ///     Gets the width, in pixels, of the final packed texture.
        /// </summary>
        public int TextureWidth { get; private set; }

        /// <summary>
        ///     Gets the height, in pixels, of the final packed texture.
        /// </summary>
        public int TextureHeight { get; private set; }

        /// <summary>
        ///     Gets the collection of defined animations, with the dictionary key
        ///     being the name of the animation.
        /// </summary>
        public Dictionary<string, Animation> Animations { get; internal set; }

        /// <summary>
        ///     Gets the collection of frames.
        /// </summary>
        public List<Frame> Frames { get; internal set; }

        /// <summary>
        ///     Gets the collection of defined slices, with the dictionary key
        ///     being the name of the slice.
        /// </summary>
        public Dictionary<string, Slice> Slices { get; internal set; }

        public AsepriteProcessorResult(AsepriteDocument doc)
        {
            GetPalette(doc);
            GetAnimations(doc);
            GetFrames(doc);
            GetSlices(doc);
        }

        private void GetPalette(AsepriteDocument doc)
        {
            Palette = new Color[doc.Palette.Colors.Length];
            for (int i = 0; i < Palette.Length; i++)
            {
                Palette[i] = doc.Palette.Colors[i].Color;
            }
        }

        private void GetAnimations(AsepriteDocument doc)
        {
            Animations = new Dictionary<string, Animation>();
            for (int i = 0; i < doc.Tags.Count; i++)
            {
                Animation animation = new Animation()
                {
                    From = doc.Tags[i].From,
                    To = doc.Tags[i].To,
                    Name = doc.Tags[i].Name,
                    Color = doc.Tags[i].Color
                };

                Animations.Add(animation.Name, animation);
            }
        }

        private void GetFrames(AsepriteDocument doc)
        {
            //  We need to take each frame from the document, and create a
            //  single Color[] from them that represents a single packed texture.
            //
            //  This is going to be a super basic packing method unless someone wants to
            //  contribute to this and make it more effiecent. The method used below will
            //  just pack all frames into a container using the "Square packing into a Square"
            //  method. More information on this method can be found at
            //
            // https://en.wikipedia.org/wiki/Square_packing_in_a_square

            //  Attempt to square root the frame count to get the number of columns.
            //  This will return back if the attempt resulted in a perfect square
            //  root or not. If it wasn't perfect, we add 1 to the result.
            if (!TrySquareRoot(doc.Frames.Count, out int columns))
            {
                columns += 1;
            }

            //  Attempt to divid the number of frames by the total columns to
            //  get the number of rows. This will return back if the attempt
            //  resulted in a quotent with no remainder. So if false, we
            //  add 1 to the result to get the row count.
            if (!TryDivision(doc.Frames.Count, columns, out int rows))
            {
                rows += 1;
            }

            TextureWidth = columns * doc.Header.Width;
            TextureHeight = rows * doc.Header.Height;

            ColorData = new Color[TextureWidth * TextureHeight];

            Frames = new List<Frame>();

            for (int i = 0; i < doc.Frames.Count; i++)
            {
                //  Calculate the x and y position of the frame's top left
                //  pixel relative to the top-left of the final packed texture
                int frameColumn = i % columns;
                int frameRow = i / columns;

                //  Inject the pixel color data from the frame into the final
                //  packed color data array
                for (int p = 0; p < doc.Frames[i].Pixels.Length; p++)
                {
                    int x = (p % doc.Header.Width) + (frameColumn * doc.Header.Width);
                    int y = (p / doc.Header.Height) + (frameRow * doc.Header.Height);
                    int index = y * TextureWidth + x;

                    ColorData[index] = doc.Frames[i].Pixels[p];
                }

                //  Now create the frame data
                Frame frame = new Frame()
                {
                    X = frameColumn * doc.Header.Width,
                    Y = frameRow * doc.Header.Height,
                    Width = doc.Header.Width,
                    Height = doc.Header.Height,
                    Duration = doc.Frames[i].Duration
                };
                Frames.Add(frame);
            }
        }

        private void GetSlices(AsepriteDocument doc)
        {
            //  So, slices are wonky in Aseprite.  Per the developer on the forums,
            //  slices are ment for things like when you want to ninepatch a button
            //  image.  When doing slices for animations, things get wonky in Aseprite.
            //
            //  Aseprite seems to create and store Slices as
            //
            //      "The key for this slice starts on X frame and is valid for all frames
            //      going forward until we hit another key."
            //
            //  So, I'm leaving in porting the slices over for now, but it will be documented
            //  in the repo as an "experimental" features, because as is, there's just no
            //  good way of handling it for animations that I can think of.
            //
            //  If someone has a good idea and want's to contribute to this, please go ahead.
            Slices = new Dictionary<string, Slice>();
            for (int i = 0; i < doc.Slices.Count; i++)
            {
                Slice slice = new Slice();
                slice.Color = doc.Slices[i].HasUserDataColor ? doc.Slices[i].UserDataColor : Color.White;
                slice.Name = doc.Slices[i].Name;
                slice.SliceKeys = new Dictionary<int, SliceKey>();

                SliceKey lastProcessedKey = new SliceKey();

                for (int k = 0; k < doc.Slices[i].Keys.Length; k++)
                {
                    bool hasNinePatch = (doc.Slices[i].Flags & AsepriteSliceFlags.HasNinePatch) != 0;
                    bool hasPivot = (doc.Slices[i].Flags & AsepriteSliceFlags.HasPivot) != 0;

                    SliceKey key = new SliceKey()
                    {
                        FrameIndex = doc.Slices[i].Keys[k].Frame,
                        X = doc.Slices[i].Keys[k].X,
                        Y = doc.Slices[i].Keys[k].Y,
                        Width = doc.Slices[i].Keys[k].Width,
                        Height = doc.Slices[i].Keys[k].Height,
                        HasNinePatch = hasNinePatch,
                        CenterX = hasNinePatch ? doc.Slices[i].Keys[k].CenterX : 0,
                        CenterY = hasNinePatch ? doc.Slices[i].Keys[k].CenterY : 0,
                        CenterWidth = hasNinePatch ? doc.Slices[i].Keys[k].CenterWidth : 0,
                        CenterHeight = hasNinePatch ? doc.Slices[i].Keys[k].CenterHeight : 0,
                        HasPivot = hasPivot,
                        PivotX = hasPivot ? doc.Slices[i].Keys[k].PivotX : 0,
                        PivotY = hasPivot ? doc.Slices[i].Keys[k].PivotY : 0
                    };

                    if(lastProcessedKey.FrameIndex < key.FrameIndex)
                    {
                        for(int s = 1; s < key.FrameIndex - lastProcessedKey.FrameIndex; s++)
                        {
                            SliceKey interpolatedKey = lastProcessedKey;
                            interpolatedKey.FrameIndex += s;
                            slice.SliceKeys.Add(interpolatedKey.FrameIndex, interpolatedKey);
                        }
                    }

                    lastProcessedKey = key;

                    slice.SliceKeys.Add(key.FrameIndex, key);
                }

                if(lastProcessedKey.FrameIndex < Frames.Count-1)
                {
                    for(int s = 1; s < Frames.Count - lastProcessedKey.FrameIndex; s++)
                    {
                        SliceKey interpolatedKey = lastProcessedKey;
                        interpolatedKey.FrameIndex += s;
                        slice.SliceKeys.Add(interpolatedKey.FrameIndex, interpolatedKey);
                    }
                }

                Slices.Add(slice.Name, slice);
            }
        }

        /// <summary>
        ///     Given a value, performs the square root operation on it and returns
        ///     back if the square root is a perfect square root.  The value of
        ///     <paramref name="result"/> will the the integer value of the
        ///     square root (rounded down).
        /// </summary>
        /// <param name="value">
        ///     The value to attempt to square root.
        /// </param>
        /// <param name="result">
        ///     An integer in which to store the result of the square root, as an
        ///     integer.  If the square root is not a perfect root, then the value
        ///     of this will be the integer value of the result rounded down.
        /// </param>
        /// <returns></returns>
        private bool TrySquareRoot(int value, out int result)
        {
            double sqrt = Math.Sqrt(value);
            result = (int)Math.Floor(sqrt);
            return Math.Abs(sqrt % 1) < double.Epsilon;
        }

        /// <summary>
        ///     Performs integer division using the values given in the form of
        ///     <paramref name="a"/> / <paramref name="b"/>.  If the result
        ///     of the division is an integer with no remainder, the a value
        ///     of true is returned.  The result of the division is stored
        ///     in the <paramref name="result"/>.
        /// </summary>
        /// <param name="a">
        ///     The value on the left side of the division symbol.
        /// </param>
        /// <param name="b">
        ///     The value on the right side of the division symbol.
        /// </param>
        /// <param name="result">
        ///     The result of the division as an integer.
        /// </param>
        /// <returns></returns>
        private bool TryDivision(int a, int b, out int result)
        {
            result = a / b;
            return a % b == 0;
        }

        public struct Frame
        {
            /// <summary>
            ///     The x-coordinate position of the frame relative
            ///     to the final color data.
            /// </summary>
            public int X;

            /// <summary>
            ///     The y-coordinate position of the frame relative
            ///     to the final color data.
            /// </summary>
            public int Y;

            /// <summary>
            ///     The width, in pixels, of the frame.
            /// </summary>
            public int Width;

            /// <summary>
            ///     The height, in pixels, of the frame.
            /// </summary>
            public int Height;

            /// <summary>
            ///     The duration, in seconds, of the frame.
            /// </summary>
            public int Duration;
        }

        public struct Animation
        {
            /// <summary>
            ///     The name of the animation.
            /// </summary>
            public string Name;

            /// <summary>
            ///     The starting frame of the animation.
            /// </summary>
            public int From;

            /// <summary>
            ///     The ending frame of the animation.
            /// </summary>
            public int To;

            /// <summary>
            ///     The color of the animation as defined as the
            ///     tag color in Asperite.
            /// </summary>
            public Color Color;

            /// <summary>
            ///     A value that indicates the direction the animation is played in.
            ///     0 = Forward, 1 = Reverse, 2 = Ping Pong.
            /// </summary>
            public int Direction;
        }

        public struct Slice
        {
            /// <summary>
            ///     The name of the slice.
            /// </summary>
            public string Name;

            /// <summary>
            ///     The color of the slice.
            /// </summary>
            public Color Color;

            /// <summary>
            ///     The dictionary of slicekeys where the dictionary key
            ///     is the frme the slice is valid on.
            /// </summary>
            public Dictionary<int, SliceKey> SliceKeys;
        }

        public struct SliceKey
        {
            /// <summary>
            ///     The index of the frame that the slice is valid
            ///     starting on.
            /// </summary>
            public int FrameIndex;

            /// <summary>
            ///     The x-coordinate position of the slice relative to
            ///     the bounds of the frame.
            /// </summary>
            public int X;

            /// <summary>
            ///     The y-coordinate position of the slice relative to
            ///     the bounds of the frame.
            /// </summary>
            public int Y;

            /// <summary>
            ///     The width, in pixels, of the slice.
            /// </summary>
            public int Width;

            /// <summary>
            ///     The height, in pixels, of the slice.
            /// </summary>
            public int Height;

            /// <summary>
            ///     A value indicating if this slicekey has nine patch
            ///     data.
            /// </summary>
            public bool HasNinePatch;

            /// <summary>
            ///     The top-left x-coordinate position of the nine patch
            ///     center rect if this slice contains ninepatch data.
            /// </summary>
            public int CenterX;

            /// <summary>
            ///     The top-left y-coordinate position of the nine patch
            ///     center rect if this slice contains ninepatch data.
            /// </summary>
            public int CenterY;

            /// <summary>
            ///     The width, in pixels, of the nine patch center rect
            ///     if this slice contains ninepatch data.
            /// </summary>
            public int CenterWidth;

            /// <summary>
            ///     The height, in pixels, of the nine patch center rect
            ///     if this slice contains ninepatch data.
            /// </summary>
            public int CenterHeight;

            /// <summary>
            ///     Gets a value indicating if this slicekey has pivot data.
            /// </summary>
            public bool HasPivot;

            /// <summary>
            ///     The x-coordinate origin point of the pivot if this slice
            ///     contains pivot data.
            /// </summary>
            public int PivotX;

            /// <summary>
            ///     The y-coordinate origin point of the pivot if this slice
            ///     contains pivot data.
            /// </summary>
            public int PivotY;
        }
    }
}
