// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2018-2023 Christopher Whitley

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


// namespace MonoGame.Aseprite;

// public sealed class TilemapFrame
// {
//     private List<TilemapLayer> _layers = new();
//     private Dictionary<string, TilemapLayer> _layerLookup = new();

//     public TimeSpan Duration { get; }
//     public string Name { get; }

//     public TilemapFrame(string name, TimeSpan duration)
//     {
//         Name = name;
//         Duration = duration;
//     }

//     public void AddLayer(TilemapLayer layer)
//     {
//         if(_layerLookup.ContainsKey(layer.Name))
//         {
//             throw new InvalidOperationException($"This frame already contains a layer with the anme '{layer.Name}'.  Frames must contain unique named layers");
//         }

//         _layers.Add(layer);
//         _layerLookup.Add(layer.Name, layer);
//     }

//     public void RemoveLayer(TilemapLayer layer)
//     {
//         _layerLookup.Remove(layer.Name);
//         _layers.Remove(layer);
//     }


// }
