// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

/// <summary>
/// Defines a named slice for a <see cref="TextureRegion"/> with a bounds, origin, and color.
/// </summary>
public class Slice
{
    /// <summary>
    /// Gets the name assigned to this <see cref="Slice"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the rectangular bounds of this <see cref="Slice"/> relative to the bounds of the
    /// <see cref="TextureRegion"/> it is in.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    /// Gets the x- and y-coordinate origin point for this <see cref="Slice"/> relative to the
    /// upper-left corner of the bonds of the <see cref="TextureRegion"/> it is in.
    /// </summary>
    public Vector2 Origin { get; }

    /// <summary>
    /// Gets the <see cref="Microsoft.Xna.Framework.Color"/> value assigned to this 
    /// <see cref="Slice"/>.
    /// </summary>
    public Color Color { get; }

    internal Slice(string name, Rectangle bounds, Vector2 origin, Color color) =>
        (Name, Bounds, Origin, Color) = (name, bounds, origin, color);
}