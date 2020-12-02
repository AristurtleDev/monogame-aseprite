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

namespace MonoGame.Aseprite
{
    /// <summary>
    ///     Represents a sliced (predefined) area of a frame of animation
    /// </summary>
    public struct Slice
    {
        /// <summary>
        ///     The name of the slice
        /// </summary>
        public string name;

        /// <summary>
        ///     The color of the slice
        /// </summary>
        public Color color;

        /// <summary>
        ///     The colleciton of <see cref="SliceKey"/> structures
        /// </summary>
        public Dictionary<int, SliceKey> keys;

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> structures</param>
        public Slice(string name, Dictionary<int, SliceKey> keys)
        {
            this.name = name;
            this.color = Color.White;
            this.keys = keys;
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The naem of the slice</param>
        /// <param name="keys">A collection of <see cref="SliceKey"/> structures to add to the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of the <see cref="SliceKey"/> structures in the collection given contain,
        ///     the same name
        /// </exception>
        public Slice(string name, IEnumerable<SliceKey> keys)
        {
            this.name = name;
            this.color = Color.White;
            this.keys = new Dictionary<int, SliceKey>();

            foreach (SliceKey key in keys)
            {
                if (!this.keys.ContainsKey(key.frame))
                {
                    this.keys.Add(key.frame, key);
                }
                else
                {
                    throw new ArgumentException($"The slice {name} already contains a SliceKey for frame {key.frame}.");
                }
            }
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The naem of the slice</param>
        /// <param name="keys">A collection of <see cref="SliceKey"/> structures to add to the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of the <see cref="SliceKey"/> structures in the collection given contain,
        ///     the same name
        /// </exception>
        public Slice(string name, params SliceKey[] keys)
        {
            this.name = name;
            this.color = Color.White;
            this.keys = new Dictionary<int, SliceKey>();
            foreach (SliceKey key in keys)
            {
                if (!this.keys.ContainsKey(key.frame))
                {
                    this.keys.Add(key.frame, key);
                }
                else
                {
                    throw new Exception($"The slice {name} already contains a SliceKey for frame {key.frame}.");
                }
            }
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The name of the slice</param>
        /// <param name="keys">The collection of <see cref="SliceKey"/> structures</param>
        /// <param name="color">The color of the slice</param>
        public Slice(string name, Color color, Dictionary<int, SliceKey> keys)
        {
            this.name = name;
            this.color = color;
            this.keys = keys;
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The naem of the slice</param>
        /// <param name="keys">A collection of <see cref="SliceKey"/> structures to add to the slice</param>
        /// <param name="color">The color of the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of the <see cref="SliceKey"/> structures in the collection given contain,
        ///     the same name
        /// </exception>
        public Slice(string name, Color color, IEnumerable<SliceKey> keys)
        {
            this.name = name;
            this.color = color;
            this.keys = new Dictionary<int, SliceKey>();

            foreach (SliceKey key in keys)
            {
                if (!this.keys.ContainsKey(key.frame))
                {
                    this.keys.Add(key.frame, key);
                }
                else
                {
                    throw new Exception($"The slice {name} already contains a SliceKey for frame {key.frame}.");
                }
            }
        }

        /// <summary>
        ///     Creates a new <see cref="Slice"/> structure
        /// </summary>
        /// <param name="name">The naem of the slice</param>
        /// <param name="keys">A collection of <see cref="SliceKey"/> structures to add to the slice</param>
        /// <param name="color">The color of the slice</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of the <see cref="SliceKey"/> structures in the collection given contain,
        ///     the same name
        /// </exception>
        public Slice(string name, Color color, params SliceKey[] keys)
        {
            this.name = name;
            this.color = color;
            this.keys = new Dictionary<int, SliceKey>();
            foreach (SliceKey key in keys)
            {
                if (!this.keys.ContainsKey(key.frame))
                {
                    this.keys.Add(key.frame, key);
                }
                else
                {
                    throw new Exception($"The slice {name} already contains a SliceKey for frame {key.frame}.");
                }
            }
        }

    }
}
