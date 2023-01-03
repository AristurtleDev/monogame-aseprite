/* ------------------------------------------------------------------------------
    Copyright (c) 2022 Christopher Whitley

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

using MonoGame.Aseprite.ContentPipeline.Serialization;

namespace MonoGame.Aseprite.ContentPipeline.Models
{
    /// <summary>
    ///     Provides the values found inside a Layer chunk in an Aseprite file.
    /// </summary>
    /// <remarks>
    ///     A layer chunk contains data such as name, opacity, and blend mode.
    ///     <para>
    ///         Aseprite Layer Chunk documentation:
    ///         <a href="https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md#layer-chunk-0x2004">
    ///             Click to view.
    ///         </a>
    ///     </para>
    /// </remarks>
    public sealed class AsepriteLayerChunk : AsepriteChunk
    {
        /// <summary>
        ///     Gets the <see cref="AsepriteLayerFlags"/> that have been
        ///     set for this layer.
        /// </summary>
        public AsepriteLayerFlags Flags { get; private set; }

        /// <summary>
        ///     Gets the <see cref="AsepriteLayerTypes"/> value that
        ///     describes what type of layer this is.
        /// </summary>
        public AsepriteLayerType Type { get; private set; }

        /// <summary>
        ///     Gets the sub level of this layer if it is a child
        ///     of another layer.
        /// </summary>
        public ushort ChildLevel { get; private set; }

        /// <summary>
        ///     Gets the <see cref="AsepriteBlendModes"/> value that indicates
        ///     which blend mode to use for the color data of all cels within
        ///     this layer.
        /// </summary>
        public AsepriteBlendMode BlendMode { get; private set; }

        /// <summary>
        ///     Gets the level of opacity for this layer.
        /// </summary>
        public byte Opacity { get; private set; }

        /// <summary>
        ///     Gets the name of this layer.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Creates a new <see cref="AsepriteLayerChunk"/> instance.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="AsepriteReader"/> instance being used to read the
        ///     Aseprite file.
        /// </param>
        internal AsepriteLayerChunk(AsepriteReader reader)
        {
            //  Read the flags that are set for this layer.
            Flags = (AsepriteLayerFlags)reader.ReadWORD();

            //  Read the type of layer we're dealing with.
            Type = (AsepriteLayerType)reader.ReadWORD();

            //  Read the sub level of this layer. If this layer is not a
            //  child layer, this value will be 0.
            ChildLevel = reader.ReadWORD();

            //  Per ase file spec, default layer width in pixels is ignored
            _ = reader.ReadWORD();

            //  Per ase file spec, default layer height in pixels is ignored
            _ = reader.ReadWORD();

            //  Read the blend mode used by this layer.
            BlendMode = (AsepriteBlendMode)reader.ReadWORD();

            //  Read the opacity level for this layer.
            Opacity = reader.ReadByte();

            //  Per ase file spec, ignroe next 3 bytes, they are reserved for future use
            reader.Ignore(3);

            //  Read the name of this layer.
            Name = reader.ReadString();
        }
    }
}
