// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2023 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */
// using System.Collections.Immutable;

// namespace MonoGame.Aseprite.AsepriteTypes;

// /// <summary>
// ///     Represents a group layer in an Aseprite image.
// /// </summary>
// /// <param name="Children">
// ///     An <see cref="ImmutableArray{T}"/> of <see cref="AsepriteLayer"/>
// ///     elements that are the children of this <see cref="AsepriteGroupLayer"/>.
// /// </param>
// /// <param name="IsVisible">
// ///     Indicates whether the <see cref="AsepriteCel"/> elements that are on
// ///     this <see cref="AsepriteLayer"/> are visible.
// /// </param>
// /// <param name="IsBackground">
// ///     Indicates whether this <see cref="AsepriteGroupLayer"/> was marked as
// ///     the background layer in the Aseprite UI.
// /// </param>
// /// <param name="IsReference">
// ///     Indicates whether this <see cref="AsepriteGroupLayer"/> was marked as a
// ///     reference layer in the Aseprite UI.
// /// </param>
// /// <param name="BlendMode">
// ///     A <see cref="MonoGame.Aseprite.BlendMode"/> value that defines the type
// ///     of pixel blending to use when the <see cref="AsepriteCel"/> elements on
// ///     this <see cref="AsepriteGroupLayer"/> are blended with those below them.
// /// </param>
// /// <param name="Opacity">
// ///     The opacity level of this <see cref="AsepriteGroupLayer"/>.
// /// </param>
// /// <param name="Name">
// ///     The name of this <see cref="AsepriteGroupLayer"/>.
// /// </param>
// public sealed record AsepriteGroupLayer(ImmutableArray<AsepriteLayer> Children, bool IsVisible, bool IsBackground, bool IsReference, BlendMode BlendMode, int Opacity, string Name)
//     : AsepriteLayer(IsVisible, IsBackground, IsReference, BlendMode, Opacity, Name);

// // /// <summary>
// // ///     Represents a group layer in an Aseprite image.
// // /// </summary>
// // public sealed class AsepriteGroupLayer : AsepriteLayer
// // {
// //     private List<AsepriteLayer> _children = new();

// //     /// <summary>
// //     ///     A <see cref="ReadOnlyCollection{T}"/> of <see cref="AsepriteLayer"/>
// //     ///     elements that are the children of this
// //     ///     <see cref="AsepriteGroupLayer"/>.
// //     /// </summary>
// //     public ReadOnlyCollection<AsepriteLayer> Children { get; }

// //     internal AsepriteGroupLayer(bool isVisible, bool isBackground, bool isReference, BlendMode blendMode, int opacity, string name)
// //         : base(isVisible, isBackground, isReference, blendMode, opacity, name)
// //     {
// //         Children = _children.AsReadOnly();
// //     }

// //     internal void AddChild(AsepriteLayer layer) => _children.Add(layer);
// // }
