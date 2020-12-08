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
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.ContentPipeline.Models;
using MonoGame.Aseprite.ContentPipeline.ThirdParty.Pixman;

namespace MonoGame.Aseprite.ContentPipeline.Processors
{
    /// <summary>
    ///     A class that provides the results of processing an Aseprite file.
    /// </summary>
    public sealed class AsepriteDocumentProcessorResult
    {
        //  The Aseprite doucment that was created form the initial
        //  processing of the Aseprite file.
        private AsepriteDocument _document;

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
        ///     Gets the collection of defined slices, with the dictionary key
        ///     being the name of the slice.
        /// </summary>
        public Dictionary<string, ProcessedSlice> Slices { get; private set; }

        /// <summary>
        ///     Gets the <see cref="AsepriteDocumentProcessorOptions"/> instance values
        ///     defined by the user in the content pipeline properties window.
        /// </summary>
        public AsepriteDocumentProcessorOptions Options { get; private set; }

        /// <summary>
        ///     Gets the collection of defined animations, with the dictionary key
        ///     being the name of the animation.
        /// </summary>
        public Dictionary<string, ProcessedAnimation> Animations { get; private set; }

        /// <summary>
        ///     Gets the collection of frames.
        /// </summary>
        public List<ProcessedFrame> Frames { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteDocumentProcessorResult"/> instance.
        /// </summary>
        /// <param name="document">
        ///     The <see cref="AsepriteDocument"/> instance that was created from processing
        ///     the Aseprite file.
        /// </param>
        /// <param name="options">
        ///     The <see cref="AsepriteDocumentProcessorOptions"/> instance containing the values
        ///     supplied by the user in the content pipeline properties window.
        /// </param>
        public AsepriteDocumentProcessorResult(AsepriteDocument document, AsepriteDocumentProcessorOptions options)
        {
            _document = document;
            Options = options;
            GetPalette();
            GetAnimations();
            CreateTexture();
            GetSlices();
        }

        /// <summary>
        ///     Gets palette values from the aseprite document.
        /// </summary>
        private void GetPalette()
        {
            Palette = new Color[_document.Palette.Colors.Length];
            for (int i = 0; i < Palette.Length; i++)
            {
                AsepritePaletteColor paletteColor = _document.Palette.Colors[i];
                Palette[i] = Color.FromNonPremultiplied(paletteColor.Red, paletteColor.Green, paletteColor.Blue, paletteColor.Alpha);
            }
        }

        /// <summary>
        ///     Gets the animation values from the aseprite document.
        /// </summary>
        private void GetAnimations()
        {
            Animations = new Dictionary<string, ProcessedAnimation>();
            for (int i = 0; i < _document.Tags.Count; i++)
            {
                AsepriteTagChunk tag = _document.Tags[i];
                ProcessedAnimation animation = new ProcessedAnimation()
                {
                    From = tag.From,
                    To = tag.To,
                    Name = tag.Name,
                    Color = Color.FromNonPremultiplied(tag.ColorR, tag.ColorG, tag.ColorB, 255)
                };

                Animations.Add(animation.Name, animation);
            }
        }

        /// <summary>
        ///     Genereates the textre data from the aseprite document.
        /// </summary>
        private void CreateTexture()
        {
            //  A key-value dictionary containing the packed color values for each frame
            //  in the aseprite document.
            Dictionary<int, uint[]> frameColorLookup = new Dictionary<int, uint[]>();

            //  A key-value dictionary that maps the link between a frame and another frame
            //  that is a duplicate of that frame.  The key for the dictionary is the index
            //  of the frame that is a duplicate of another frame, with the value being the index
            //  of the frame that it is a duplicate of.
            Dictionary<int, int> frameDuplicateMap = new Dictionary<int, int>();

            for (int f = 0; f < _document.Frames.Count; f++)
            {
                frameColorLookup.Add(f, FlattenFrame(_document.Frames[f]));
                ////////AsepriteFrame aFrame = _document.Frames[f];
                ////////uint[] framePixels = new uint[_document.Header.Width * _document.Header.Height];

                ////////for (int c = 0; c < aFrame.Cels.Count; c++)
                ////////{
                ////////    AsepriteCelChunk cel = aFrame.Cels[c];

                ////////    if (cel.LinkedCel != null)
                ////////    {
                ////////        cel = cel.LinkedCel;
                ////////    }

                ////////    AsepriteLayerChunk layer = _document.Layers[cel.LayerIndex];

                ////////    if ((layer.Flags & AsepriteLayerFlags.Visible) != 0 || Options.OnlyVisibleLayers == false)
                ////////    {
                ////////        byte opacity = Combine32.MUL_UN8(cel.Opacity, layer.Opacity);

                ////////        for (int p = 0; p < cel.Pixels.Length; p++)
                ////////        {
                ////////            int x = (p % cel.Width) + cel.X;
                ////////            int y = (p / cel.Width) + cel.Y;
                ////////            int index = y * _document.Header.Width + x;

                ////////            //  Sometimes a cell can have a negative x and/or y value. This is caused
                ////////            //  by selecting an area within aseprite and then moving a portion of the
                ////////            //  selected pixels outside the canvas.  We don't care about these pixels
                ////////            //  so if the index is outside the range of the array to store them in
                ////////            //  then we'll just ignore them.
                ////////            if (index < 0 || index >= framePixels.Length) { continue; }

                ////////            uint backdrop = framePixels[index];
                ////////            uint src = cel.Pixels[p];

                ////////            Func<uint, uint, int, uint> blender = Utils.GetBlendFunction(layer.BlendMode);
                ////////            uint blendedColor = blender.Invoke(backdrop, src, opacity);

                ////////            framePixels[index] = blendedColor;
                ////////        }
                ////////    }
                ////////}

                //////////  Add the frame's color to the frame color lookup dictionary
                ////////frameColorLookup.Add(f, framePixels);
            }



            //  Setting up some variables that will be used.
            int columns;
            int rows;
            int totalFrames;


            if (Options.MergeDuplicateFrames)
            {
                for (int i = 0; i < frameColorLookup.Count; i++)
                {
                    for (int d = 0; d < i; d++)
                    {
                        if (frameColorLookup[i].SequenceEqual(frameColorLookup[d]))
                        {
                            frameDuplicateMap.Add(i, d);
                            break;
                        }
                    }
                }

                //  Since we are merging duplicates, we need to subtract the number of
                //  duplicates from the total frame count.
                totalFrames = frameColorLookup.Count - frameDuplicateMap.Count;
            }
            else
            {
                totalFrames = frameColorLookup.Count;
            }

            if (Options.SheetType == ProcessorSheetType.HorizontalStrip)
            {
                //  Horizontal has 1 column per frame and only 1 row total
                columns = totalFrames;
                rows = 1;
            }
            else if (Options.SheetType == ProcessorSheetType.VerticalStrip)
            {
                //  Vertical has 1 row per frame and only 1 column total
                columns = 1;
                rows = totalFrames;
            }
            else
            {
                //  Otherwise we're going to assume the type is packed.
                //  To get the total amount of columns and rows needed, we'll use
                //  a super basic packing method.
                //
                // https://en.wikipedia.org/wiki/Square_packing_in_a_square

                //  Attempt to square root the frame count to get the number of columns.
                //  This will return back if the attempt resulted in a perfect square
                //  root or not.  If it wasn't perfect, we add 1 to the result.
                if (!Utils.TrySquareRoot(totalFrames, out columns))
                {
                    columns += 1;
                }

                //  Attempt to divide the number of frames by the total columns to
                //  get the number of rows.   This will return back if the attempt
                //  resulted in a quotent with no remainder.  So if false, we
                //  add 1 to the result to get the row count.
                if (!Utils.TryDivision(totalFrames, columns, out rows))
                {
                    rows += 1;
                }
            }

            TextureWidth = columns * _document.Header.Width;
            TextureHeight = rows * _document.Header.Height;


            if (Options.BorderPadding > 0)
            {
                TextureWidth += Options.BorderPadding * 2;
                TextureHeight += Options.BorderPadding * 2;
            }

            if (Options.Spacing > 0)
            {
                TextureWidth += Options.Spacing * (columns - 1);
                TextureHeight += Options.Spacing * (rows - 1);
            }

            if (Options.InnerPadding > 0)
            {
                TextureWidth += Options.InnerPadding * 2 * columns;
                TextureHeight += Options.InnerPadding * 2 * rows;
            }

            ColorData = new Color[TextureWidth * TextureHeight];
            Frames = new List<ProcessedFrame>();
            Dictionary<int, ProcessedFrame> originalToDuplicateFrameLookup = new Dictionary<int, ProcessedFrame>();

            int frameOffset = 0;
            for (int f = 0; f < _document.Frames.Count; f++)
            {
                if (!Options.MergeDuplicateFrames || !frameDuplicateMap.ContainsKey(f))
                {
                    //  Calculate the x and y position of the frame's top-left
                    //  pixel relative to the top-left of the final spritesheet.
                    int frameColumn = (f - frameOffset) % columns;
                    int frameRow = (f - frameOffset) / columns;

                    //  Inject the pixel color data from the frame into the final
                    //  spritesheet color data array
                    Color[] pixels = frameColorLookup[f].ToColorArray();
                    for (int p = 0; p < pixels.Length; p++)
                    {
                        int x = (p % _document.Header.Width) + (frameColumn * _document.Header.Width);
                        int y = (p / _document.Header.Width) + (frameRow * _document.Header.Height);

                        if (Options.BorderPadding > 0)
                        {
                            x += Options.BorderPadding;
                            y += Options.BorderPadding;
                        }

                        if (Options.Spacing > 0)
                        {
                            if (frameColumn > 0)
                            {
                                x += Options.Spacing * frameColumn;
                            }

                            if (frameRow > 0)
                            {
                                y += Options.Spacing * frameRow;
                            }
                        }

                        if (Options.InnerPadding > 0)
                        {
                            x += Options.InnerPadding * (frameColumn + 1);
                            y += Options.InnerPadding * (frameRow + 1);

                            if (frameColumn > 0)
                            {
                                x += Options.InnerPadding * frameColumn;
                            }

                            if (frameRow > 0)
                            {
                                y += Options.InnerPadding * frameRow;
                            }
                        }

                        int index = y * TextureWidth + x;

                        ColorData[index] = pixels[p];
                    }


                    //  Now create the frame data
                    ProcessedFrame frame = new ProcessedFrame()
                    {
                        X = (frameColumn * _document.Header.Width),
                        Y = (frameRow * _document.Header.Height),
                        Width = _document.Header.Width,
                        Height = _document.Header.Height,
                        Duration = _document.Frames[f].Duration
                    };


                    if (Options.BorderPadding > 0)
                    {
                        frame.X += Options.BorderPadding;
                        frame.Y += Options.BorderPadding;
                    }

                    if (Options.Spacing > 0)
                    {
                        if (frameColumn > 0)
                        {
                            frame.X += Options.Spacing * frameColumn;
                        }

                        if (frameRow > 0)
                        {
                            frame.Y += Options.Spacing * frameRow;
                        }
                    }

                    if (Options.InnerPadding > 0)
                    {
                        frame.X += Options.InnerPadding * (frameColumn + 1);
                        frame.Y += Options.InnerPadding * (frameRow + 1);

                        if (frameColumn > 0)
                        {
                            frame.X += Options.InnerPadding * frameColumn;
                        }

                        if (frameRow > 0)
                        {
                            frame.Y += Options.InnerPadding * frameRow;
                        }
                    }

                    Frames.Add(frame);
                    originalToDuplicateFrameLookup.Add(f, frame);
                }
                else
                {
                    //  We are merging duplicates and it was detected that the current
                    //  frame to process is a duplicate.  So we still add the Frame data
                    //  but we need to make sure the frame data added for this frame is
                    //  the same as it's duplicate frame.
                    ProcessedFrame frame = originalToDuplicateFrameLookup[frameDuplicateMap[f]];
                    frame.Duration = _document.Frames[f].Duration;
                    Frames.Add(frame);
                    frameOffset++;
                }
            }
        }

        /// <summary>
        ///     Given an <see cref="AsepriteFrame"/> instance, combines all Cels
        ///     of the frame into a single array of packed color values representing
        ///     all pixels within the frame.
        /// </summary>
        /// <param name="frame">
        ///     The <see cref="AsepriteFrame"/> instance to flatten.
        /// </param>
        /// <param name="onlyVisibleLayers">
        ///     A value indicating if only visible layers should be processed.
        /// </param>
        /// <returns></returns>
        private uint[] FlattenFrame(AsepriteFrame frame)
        {
            uint[] framePixels = new uint[_document.Header.Width * _document.Header.Height];

            for (int c = 0; c < frame.Cels.Count; c++)
            {
                AsepriteCelChunk cel = frame.Cels[c];

                if (cel.LinkedCel != null)
                {
                    cel = cel.LinkedCel;
                }

                AsepriteLayerChunk layer = _document.Layers[cel.LayerIndex];

                if ((layer.Flags & AsepriteLayerFlags.Visible) != 0 || !Options.OnlyVisibleLayers)
                {
                    byte opacity = Combine32.MUL_UN8(cel.Opacity, layer.Opacity);

                    for (int p = 0; p < cel.Pixels.Length; p++)
                    {
                        int x = (p % cel.Width) + cel.X;
                        int y = (p / cel.Width) + cel.Y;
                        int index = y * _document.Header.Width + x;

                        //  Sometimes a cell can have a negative x and/or y value. This is caused
                        //  by selecting an area within aseprite and then moving a portion of the
                        //  selected pixels outside the canvas.  We don't care about these pixels
                        //  so if the index is outside the range of the array to store them in
                        //  then we'll just ignore them.
                        if (index < 0 || index >= framePixels.Length) { continue; }

                        uint backdrop = framePixels[index];
                        uint src = cel.Pixels[p];

                        Func<uint, uint, int, uint> blender = Utils.GetBlendFunction(layer.BlendMode);
                        uint blendedColor = blender.Invoke(backdrop, src, opacity);

                        framePixels[index] = blendedColor;
                    }
                }
            }

            return framePixels;
        }

        /// <summary>
        ///     Gets the slice values from the aseprite document.
        /// </summary>
        private void GetSlices()
        {
            //  So, slices are wonky in Aseprite.  Per the developer on the forums,
            //  slices are ment for things like when you want to ninepatch a button
            //  image.
            //
            //  https://community.aseprite.org/t/slice-tool-how-does-it-work/143/4
            //
            //  When doing slices for animations, things get wonky in Aseprite.
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
            Slices = new Dictionary<string, ProcessedSlice>();
            for (int i = 0; i < _document.Slices.Count; i++)
            {
                ProcessedSlice slice = new ProcessedSlice();

                if (_document.Slices[i].HasUserDataColor)
                {
                    slice.Color = Color.FromNonPremultiplied(
                        r: _document.Slices[i].UserDataColor[0],
                        g: _document.Slices[i].UserDataColor[1],
                        b: _document.Slices[i].UserDataColor[2],
                        a: _document.Slices[i].UserDataColor[3]);
                }
                else
                {
                    slice.Color = Color.White;
                }

                slice.Name = _document.Slices[i].Name;
                slice.SliceKeys = new Dictionary<int, ProcessedSliceKey>();

                ProcessedSliceKey lastProcessedKey = new ProcessedSliceKey();

                for (int k = 0; k < _document.Slices[i].Keys.Length; k++)
                {
                    bool hasNinePatch = (_document.Slices[i].Flags & AsepriteSliceFlags.HasNinePatch) != 0;
                    bool hasPivot = (_document.Slices[i].Flags & AsepriteSliceFlags.HasPivot) != 0;

                    ProcessedSliceKey key = new ProcessedSliceKey()
                    {
                        FrameIndex = _document.Slices[i].Keys[k].Frame,
                        X = _document.Slices[i].Keys[k].X,
                        Y = _document.Slices[i].Keys[k].Y,
                        Width = _document.Slices[i].Keys[k].Width,
                        Height = _document.Slices[i].Keys[k].Height,
                        HasNinePatch = hasNinePatch,
                        CenterX = hasNinePatch ? _document.Slices[i].Keys[k].CenterX : 0,
                        CenterY = hasNinePatch ? _document.Slices[i].Keys[k].CenterY : 0,
                        CenterWidth = hasNinePatch ? _document.Slices[i].Keys[k].CenterWidth : 0,
                        CenterHeight = hasNinePatch ? _document.Slices[i].Keys[k].CenterHeight : 0,
                        HasPivot = hasPivot,
                        PivotX = hasPivot ? _document.Slices[i].Keys[k].PivotX : 0,
                        PivotY = hasPivot ? _document.Slices[i].Keys[k].PivotY : 0
                    };

                    if (lastProcessedKey.FrameIndex < key.FrameIndex)
                    {
                        for (int s = 1; s < key.FrameIndex - lastProcessedKey.FrameIndex; s++)
                        {
                            ProcessedSliceKey interpolatedKey = lastProcessedKey;
                            interpolatedKey.FrameIndex += s;
                            slice.SliceKeys.Add(interpolatedKey.FrameIndex, interpolatedKey);
                        }
                    }

                    lastProcessedKey = key;

                    slice.SliceKeys.Add(key.FrameIndex, key);
                }

                if (lastProcessedKey.FrameIndex < Frames.Count - 1)
                {
                    for (int s = 1; s < Frames.Count - lastProcessedKey.FrameIndex; s++)
                    {
                        ProcessedSliceKey interpolatedKey = lastProcessedKey;
                        interpolatedKey.FrameIndex += s;
                        slice.SliceKeys.Add(interpolatedKey.FrameIndex, interpolatedKey);
                    }
                }

                Slices.Add(slice.Name, slice);
            }
        }
    }
}
