// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2022 Christopher Whitley

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
// using System.Collections.ObjectModel;

// using Microsoft.Xna.Framework.Graphics;

// namespace MonoGame.Aseprite;

// public sealed class AsepriteSheet : IDisposable
// {
//     private List<Frame> _frames;
//     private List<Tag> _tags;
//     private List<Slice> _slices;

//     public bool IsDisposed { get; private set; }
//     public Texture2D Texture { get; }
//     public int Width => Texture.Width;
//     public int Height => Texture.Height;

//     public ReadOnlyCollection<Frame> Frames => _frames.AsReadOnly();
//     public ReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
//     public ReadOnlyCollection<Slice> Slices => _slices.AsReadOnly();

//     internal AsepriteSheet(Texture2D texture, List<Frame> frames, List<Tag> tags, List<Slice> slices)
//     {
//         Texture = texture;
//         _frames = frames;
//         _tags = tags;
//         _slices = slices;
//     }

//     ~AsepriteSheet() => Dispose(false);

//     public void Dispose()
//     {
//         Dispose(true);
//         GC.SuppressFinalize(this);
//     }

//     private void Dispose(bool isDisposing)
//     {
//         if(IsDisposed)
//         {
//             return;
//         }

//         if(isDisposing)
//         {
//             if(!Texture.IsDisposed)
//             {
//                 Texture.Dispose();
//             }
//         }

//         IsDisposed = true;
//     }


// }
