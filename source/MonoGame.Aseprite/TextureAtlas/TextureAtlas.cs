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

// using System.Diagnostics.CodeAnalysis;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;

// namespace MonoGame.Aseprite;

// public class TextureAtlas
// {
//     private List<TextureAtlasRegion> _regions = new();
//     private Dictionary<string, TextureAtlasRegion> _regionsLookup = new();

//     public string Name { get; }
//     public Texture2D Texture { get; }

//     public TextureAtlasRegion this[int id] => GetRegion(id);
//     public TextureAtlasRegion this[string name] => GetRegion(name);
//     public int RegionCount => _regions.Count;

//     public TextureAtlas(string name, Texture2D texture)
//     {
//         Name = name;
//         Texture = texture;
//     }

//     private void AddRegion(TextureAtlasRegion region)
//     {
//         _regions.Add(region);
//         _regionsLookup.Add(region.Name, region);
//     }

//     public TextureAtlasRegion CreateRegion(string regionName, int x, int y, int w, int h) =>
//         CreateRegion(regionName, new Rectangle(x, y, w, h));

//     public TextureAtlasRegion CreateRegion(string regionName, Rectangle bounds)
//     {
//         if (_regionsLookup.ContainsKey(regionName))
//         {
//             throw new InvalidOperationException();
//         }

//         int id = _regions.Count;
//         TextureAtlasRegion region = new(regionName, Texture, bounds);
//         AddRegion(region);

//         return region;
//     }

//     public bool ContainsRegion(string regionName) => _regionsLookup.ContainsKey(regionName);

//     public TextureAtlasRegion GetRegion(string regionName)
//     {
//         if (_regionsLookup.TryGetValue(regionName, out TextureAtlasRegion? region))
//         {
//             return region;
//         }

//         throw new KeyNotFoundException();
//     }

//     public TextureAtlasRegion GetRegion(int regionIndex)
//     {
//         if (regionIndex < 0 || regionIndex >= _regions.Count)
//         {
//             throw new ArgumentOutOfRangeException(nameof(regionIndex));
//         }

//         return _regions[regionIndex];
//     }

//     public bool TryGetRegion(string regionName, [NotNullWhen(true)] out TextureAtlasRegion? region) =>
//         _regionsLookup.TryGetValue(regionName, out region);

//     public bool TryGetRegion(int regionIndex, [NotNullWhen(true)] out TextureAtlasRegion? region)
//     {
//         region = default;

//         if (regionIndex < 0 || regionIndex >= _regions.Count)
//         {
//             return false;
//         }

//         region = _regions[regionIndex];
//         return true;
//     }

//     private void RemoveRegion(TextureAtlasRegion region)
//     {
//         _regions.Remove(region);
//         _regionsLookup.Remove(region.Name);
//     }

//     public void RemoveRegion(int regionIndex)
//     {
//         if (regionIndex < 0 || regionIndex >= _regions.Count)
//         {
//             throw new ArgumentOutOfRangeException(nameof(regionIndex));
//         }

//         TextureAtlasRegion region = _regions[regionIndex];
//         RemoveRegion(region);
//     }

//     public void RemoveRegion(string regionName)
//     {
//         if(!_regionsLookup.TryGetValue(regionName, out TextureAtlasRegion? region))
//         {
//             throw new KeyNotFoundException();
//         }

//         RemoveRegion(region);
//     }

//     public bool TryRemoveRegion(int regionIndex)
//     {
//         if(TryGetRegion(regionIndex, out TextureAtlasRegion? region))
//         {
//             RemoveRegion(region);
//             return true;
//         }

//         return false;
//     }

//     public bool TryRemoveRegion(string regionName)
//     {
//         if(TryGetRegion(regionName, out TextureAtlasRegion? region))
//         {
//             RemoveRegion(region);
//             return true;
//         }

//         return false;
//     }

// }
