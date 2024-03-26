// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a <see cref="Slice"/> with center bounds.
/// </summary>
public sealed class NinePatchSlice : Slice
{
    /// <summary>
    ///     Gets the rectangular bounds of the center rectangle for this <see cref="NinePatchSlice"/>,
    ///     relative to it's bounds.
    /// </summary>
    public Rectangle CenterBounds { get; }

    internal NinePatchSlice(string name, Rectangle bounds, Rectangle centerBounds, Vector2 origin, Color color)
        : base(name, bounds, origin, color) => CenterBounds = centerBounds;
}