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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Utils;

namespace MonoGame.Aseprite.Sprites;

/// <summary>
///     <para>
///         Defines a named sprite
///     </para>
///     <para>
///         The <see cref="Sprite"/> class is a general purpose wrapper around a 
///         <see cref="MonoGame.Aseprite.TextureRegion"/> with properties to control how it is rendered.  When creating
///         a <see cref="Sprite"/> from an <see cref="AsepriteFile"/>, it represents the image of the frame used to
///         create it.
///     </para>
///     <para>
///         The most common methods for creating a <see cref="Sprite"/> will be either by using the 
///         <see cref="MonoGame.Aseprite.Content.Processors.SpriteProcessor"/> to create an instance from a frame in
///         your Aseprite File, or by using a <see cref="TextureAtlas"/> to create a <see cref="Sprite"/> from one of
///         the regions in the atlas.  An instance can also be created manually using the constructor for a more general
///         purpose use.
///     </para>
/// </summary>
/// <remarks>
///     <para>
///         The <see cref="Sprite.Color"/>, <see cref="Sprite.Origin"/>, <see cref="Sprite.Rotation"/>,
///         <see cref="Sprite.Scale"/>, and <see cref="Sprite.LayerDepth"/> are passed automatically to the
///         <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> when rendering this sprite. For one-off rendering
///         where you can override the parameter values passed to the
///         <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>, you can render the
///         <see cref="Sprite.TextureRegion"/> instead.  
///     </para>  
///     
///     ### Performance Considerations  
///     
///     <para>
///         If you plan to create multiple <see cref="Sprite"/> instances from various frames in your Aseprite file,
///         consider first creating a <see cref="TextureAtlas"/>, then creating each <see cref="Sprite"/> instance
///         using the <see cref="TextureAtlas"/>.  By doing this, you will be generating a single source
///         <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> for the <see cref="TextureAtlas"/>.  Each
///         <see cref="Sprite"/> that is then created from the <see cref="TextureAtlas"/> will be references the single
///         source <see cref="Texture2D"/> instead of separate <see cref="Texture2D"/> instances per
///         <see cref="Sprite"/>.
///     </para>
///     <para>
///         This is beneficial because it reduces the amount of texture swapping done on the
///         <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> when rendering the <see cref="Sprite"/>
///         instances.
///     </para>
/// </remarks>
/// <example>
///     <para>
///         The following examples demonstrate various ways to create a <see cref="Sprite"/>:
///     </para>
///     <code language="cs {5} title='Create Sprite using SpriteProcessor' showLineNumbers">
///         //  Load an Aseprite file
///         AsepriteFile aseFile = AsepriteFile.Load("path-to-file");
///           
///         //  Use the SpriteProcessor to create a Sprite
///         Sprite sprite = SpriteProcessor.Process(GraphicsDevice, aseFile, frameIndex: 0);
///     </code>
///     
///     <code language="cs {8} title='Create Sprite using TextureAtlas' showLineNumbers">
///         //  Load an Aseprite File
///         AsepriteFile aseFile = AsepriteFile.Load("path-to-file")
///           
///         //  Create a TextureAtlas from the AsepriteFile using the TextureAtlasProcessor
///         TextureAtlas atlas = TextureAtlasProcessor.Process(GraphicsDevice, aseFile);
///           
///         //  Create a Sprite from region 0 in the TextureAtlas
///         Sprite sprite = atlas.CreateSprite(regionIndex: 0);
///     </code>
///     
///     <code language="cs {8} title='Create Sprite using SpriteSheet' showLineNumbers">
///         //  Load an Aseprite File
///         AsepriteFile aseFile = AsepriteFile.Load("path-to-file")
///           
///         //  Create a SpriteSheet from the AsepriteFile using the SpriteSheetProcessor
///         SpriteSheet spriteSheet = SpriteSheetProcessor.Process(GraphicsDevice, aseFile);
///           
///         //  Create a Sprite from region 0 in the SpriteSheet
///         Sprite sprite = spriteSheet.CreateSprite(regionIndex: 0);
///     </code>
/// </example>
/// <seealso cref="MonoGame.Aseprite.Content.Processors.SpriteProcessor"/>
/// <seealso cref="SpriteSheet"/>
/// <seealso cref="TextureAtlas"/>
/// <seealso cref="TextureRegion"/>
/// <seealso href="https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html"/>
/// <seealso href="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>
public class Sprite
{
    private TextureRegion _textureRegion;

    private Vector2 _origin;
    private Vector2 _scale;
    private float _transparency;

    /// <summary>
    ///     Gets the name assigned to this <see cref="Sprite"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the source <see cref="TextureRegion"/> of this <see cref="Sprite"/>.
    /// </summary>
    public TextureRegion TextureRegion
    {
        get => _textureRegion;
        protected set => _textureRegion = value;
    }

    /// <summary>
    ///     Gets the width, in pixels, of this <see cref="Sprite"/>
    /// </summary>
    /// <value>
    ///     The width, in pixels, of this <see cref="Sprite"/>.
    /// </value>
    public int Width => _textureRegion.Bounds.Width;

    /// <summary>
    ///     Gets the height, in pixels, of this <see cref="Sprite"/>
    /// </summary>
    /// <value>
    ///     The height, in pixels, of this <see cref="Sprite"/>.
    /// </value>
    public int Height => _textureRegion.Bounds.Height;

    /// <summary>
    ///     Gets or Sets the color mask to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    /// <value>
    ///     The color mask to apply when rendering this <see cref="Sprite"/>.
    /// </value>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or Sets the level of transparency, between 0.0f, and 1.0f, to apply when rendering this
    ///     <see cref="Sprite"/>.
    /// </summary>
    public float Transparency
    {
        get => _transparency;
        set => _transparency = MathHelper.Clamp(value, 0.0f, 1.0f);
    }

    /// <summary>
    ///     Gets or Sets the rotation, in radians, to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    ///     Gets or Sets the x- and y-coordinate point of origin to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public Vector2 Origin
    {
        get => _origin;
        set => _origin = value;
    }

    /// <summary>
    ///     Gets or Sets the x-coordinate point of origin to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float OriginX
    {
        get => _origin.X;
        set => _origin.X = value;
    }

    /// <summary>
    ///     Gets or Sets the y-coordinate point of origin to apply when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float OriginY
    {
        get => _origin.Y;
        set => _origin.Y = value;
    }

    /// <summary>
    ///     Gets or Sets the x- and y-axis scale factor to use when rendering this <see cref="Sprite"/>.
    /// </summary>
    public Vector2 Scale
    {
        get => _scale;
        set => _scale = value;
    }

    /// <summary>
    ///     Gets or Sets the x-axis scale factor to use when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float ScaleX
    {
        get => _scale.X;
        set => _scale.X = value;
    }

    /// <summary>
    ///     Gets or Sets the y-axis scale factor to use when rendering this <see cref="Sprite"/>.
    /// </summary>
    public float ScaleY
    {
        get => _scale.Y;
        set => _scale.Y = value;
    }

    /// <summary>
    ///     Gets or Sets the <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for vertical and
    ///     horizontal flipping when rendering this <see cref="Sprite"/>.
    /// </summary>
    public SpriteEffects SpriteEffects { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether to flip this <see cref="Sprite"/> horizontally when rendering.
    /// </summary>
    public bool FlipHorizontally
    {
        get => SpriteEffects.HasFlag(SpriteEffects.FlipHorizontally);
        set => SpriteEffects = value ? (SpriteEffects | SpriteEffects.FlipHorizontally) : (SpriteEffects & ~SpriteEffects.FlipHorizontally);
    }

    /// <summary>
    ///     Gets or Sets a value that indicates whether to flip this <see cref="Sprite"/> vertically when rendering.
    /// </summary>
    public bool FlipVertically
    {
        get => SpriteEffects.HasFlag(SpriteEffects.FlipVertically);
        set => SpriteEffects = value ? (SpriteEffects | SpriteEffects.FlipVertically) : (SpriteEffects & ~SpriteEffects.FlipVertically);
    }

    /// <summary>
    ///     Gets or Sets the layer depth to render this <see cref="Sprite"/> at.
    /// </summary>
    public float LayerDepth { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates if this <see cref="Sprite"/> is visible and can be rendered.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <param name="name" cref="string">
    ///     The name to assign the <see cref="Sprite"/>.
    /// </param>
    /// <param name="textureRegion" cref="TextureRegion">
    ///     The source <see cref="TextureRegion"/> to assign the <see cref="Sprite"/>.
    /// </param>
    public Sprite(string name, TextureRegion textureRegion)
    {
        _textureRegion = textureRegion;
        Color = Color.White;
        _transparency = 1.0f;
        Rotation = 0.0f;
        _origin = Vector2.Zero;
        _scale = Vector2.One;
        SpriteEffects = SpriteEffects.None;
        LayerDepth = 0.0f;
        IsVisible = true;
        Name = name;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the <see cref="Sprite"/>.
    /// </param>
    /// <param name="texture">
    ///     The source image for the <see cref="Sprite"/>.
    /// </param>
    public Sprite(string name, Texture2D texture)
        : this(name, new TextureRegion(name, texture, texture.Bounds)) { }

    /// <summary>
    ///     Renders this <see cref="Sprite"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="Sprite"/> at.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position) => spriteBatch.Draw(this, position);

    public static Sprite FromFile(GraphicsDevice device, AseFile file, int frameNumber, AseProcessorOptions options)
    {
        options ??= AseProcessorOptions.Default;
        AseSprite aseSprite = AseSpriteProcessor.Process(file, frameNumber, options);
        Texture2D texture = aseSprite.Texture.ToTexture2D(device);
        texture.Name = aseSprite.Name;
        texture.SetData(aseSprite.Texture.Pixels.ToArray());

        TextureRegion textureRegion = new TextureRegion(texture.Name, texture, texture.Bounds);
        for(int i = 0; i < aseSprite.Slices.Length; i++)
        {
            AseSlice aseSlice = aseSprite.Slices[i];
            if (aseSlice is AseNinepatchSlice aseNinePatchSlice)
            {
                textureRegion.CreateNinePatchSlice(aseNinePatchSlice.Name,
                                                   aseNinePatchSlice.Bounds.ToXnaRectangle(),
                                                   aseNinePatchSlice.CenterBounds.ToXnaRectangle(),
                                                   aseNinePatchSlice.Origin.ToXnaVector2(),
                                                   aseNinePatchSlice.Color.ToXnaColor());
            }
            else
            {
                textureRegion.CreateSlice(aseSlice.Name,
                                          aseSlice.Bounds.ToXnaRectangle(),
                                          aseSlice.Origin.ToXnaVector2(),
                                          aseSlice.Color.ToXnaColor());
            }
        }

        return new Sprite(aseSprite.Name, textureRegion);
    }
}
