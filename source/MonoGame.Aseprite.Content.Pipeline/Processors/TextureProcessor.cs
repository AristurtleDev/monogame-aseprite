/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Provides method for processing a single <see cref="Frame"/> of an
///     <see cref="AsepriteFile"/> as an instance of the
///     <see cref="TextureContent"/> class.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Texture Processor - MonoGame.Aseprite")]
public sealed class TextureProcessor : ContentProcessor<AsepriteFile, TextureContent>
{
    /// <summary>
    ///     The index of the <see cref="Frame"/> in the
    ///     <see cref="AsepriteFile"/> to process.
    /// </summary>
    /// <remarks>
    ///     This value is set in the property window of the mgcb-editor
    /// </remarks>
    [DisplayName("Frame Index")]
    public int FrameIndex { get; set; } = 0;

    /// <summary>
    ///     Indicates whether only <see cref="Cel"/> elements that are on
    ///     <see cref="Layer"/> elements that are visible should be used.
    /// </summary>
    /// <remarks>
    ///     This value is set in the property window of the mgcb-editor
    /// </remarks>
    [DisplayName("Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Indicates whether <see cref="Cel"/> elements that are on the
    ///     <see cref="Layer"/> element that is set as the background should be
    ///     included.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A <see cref="Layer"/> has to be set as the background in
    ///         Aseprite.  Being the bottom layer does not automatically mean it
    ///         is a background layer.
    ///     </para>
    ///     <para>
    ///         This value is set in the property window of the mgcb-editor
    ///     </para>
    /// </remarks>
    [DisplayName("Include Background Layer")]
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    ///     Processes a single <see cref="Frame"/> in an
    ///     <see cref="AsepriteFile"/>. The result is a new
    ///     instance of the <see cref="TextureContent"/> class containing the
    ///     content of the texture to be written to the xnb file.
    /// </summary>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The context of the content processor.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="TextureContent"/> class containing
    ///     the content of the texture to be written to the xnb file.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///     Thrown if the <see cref="FrameIndex"/> property of this instance
    ///     is less than zero or is greater than or equal to the total number of
    ///     <see cref="Frame"/> elements in the given
    ///     <see cref="AsepriteFile"/>.
    /// </exception>
    public override TextureContent Process(AsepriteFile file, ContentProcessorContext context)
    {
        if (FrameIndex < 0 || FrameIndex >= file.Frames.Count)
        {
            throw new IndexOutOfRangeException("The 'Frame Index' cannot be less than zero or greater than or equal to the total number of frames in the Aseprite file");
        }

        Color[] pixels = file.Frames[FrameIndex].FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer);

        return new TextureContent(file.FrameWidth, file.FrameHeight, pixels);
    }
}
