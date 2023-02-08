<h1 align="center">
<img src="https://raw.githubusercontent.com/AristurtleDev/AsepriteDotNet/stable/.github/images/banner.png" alt="MonoGame.Aseprite Logo">
<br/>
A Cross Platform C# Library That Adds Support For Aseprite Files in MonoGame Projects.

[![build and test](https://github.com/AristurtleDev/AsepriteDotNet/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/AristurtleDev/AsepriteDotNet/actions/workflows/build-and-test.yml)
[![Nuget 0.2.2](https://img.shields.io/nuget/v/AsepriteDotNet?color=blue&style=flat-square)](https://www.nuget.org/packages/AsepriteDotNet/0.2.2)
[![License: MIT](https://img.shields.io/badge/📃%20license-MIT-blue?style=flat)](LICENSE)
[![Twitter](https://img.shields.io/badge/%20-Share%20On%20Twitter-555?style=flat&logo=twitter)](https://twitter.com/intent/tweet?text=MonoGame.Aseprite%20by%20%40aristurtledev%0A%0AA%20cross-platform%20C%23%20library%20that%20adds%20support%20for%20Aseprite%20files%20in%20MonoGame%20projects.%20https%3A%2F%2Fgithub.com%2FAristurtleDev%2Fmonogame-aseprite%0A%0A%23monogame%20%23aseprite%20%23dotnet%20%23csharp%20%23oss%0A)

</h1>

![](https://raw.githubusercontent.com/manbeardgames/monogame-aseprite/gh-pages-develop/static/img/monogame_aseprite_banner_800_200.png)

# MonoGame.Aseprite

`MonoGame.Aseprite` is a free and open source library for the [MonoGame Framework](https://www.monogame.net) that assists in importing [Aseprite](https://www.aseprite.org) (\*.ase/\*.aseprite) files into your game project. No need to expirt a spritesheet from Aseprite and have to deal with a PNG + JSON file. With `MonoGame.Aseprite, you can use the Aseprite file directly.

`MonoGame.Aseprite` supports importing the file **both with and without the MGCB Editor** (also known as the Content Pipeline Tool). Along with importing the file contents, several **processors** have been designed to transform the file contents into a more meaningful state to use within MonoGame.

`MonoGame.Aseprite` also supports outputting the processed file content to disk in a binary format and reader classes to read the processed information back in. This adds support for pre-processing content using any build or content workflow the end user has as long as it can use the `MonoGame.Aseprite` library.

## Getting Started

### Downloading

`MonoGame.Aseprite` is distributed via a NuGet package. You can download it by adding the NuGet package to your project from within your IDE (e.g. the NuGet Package Manager in Visual Studio). Just search for `MonoGame.Aseprite`.

Alternatively, you can add it using the following `dotnet` command form a console/terminal in your project directory:

```
dotnet add package MonoGame.Aseprite --version 4.0.0
```

After adding the `MonoGame.Aseprite` NuGet package to your project, you will have two dll references that are included and will be output as part of each build

| DLL/Assembly                   | Summary                                                                                                                                                       |
| ------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `MonoGame.Aseprite.dll`        | Assembly that contains the runtime classes.                                                                                                                   |
| `MonoGame.Aseprite.Common.dll` | Assembly that contains classes and helpers used to importing, processing, reading, and writing content. This is used by the `MonoGame.Aseprite.dll` assembly. |

### (Optional) MGCB Editor Setup

Starting with `MonoGame.Aseprite` version 4.0, using the MGCB Editor (aka the Content Pipeline Tool) is no longer a requirement. However, `MonoGame.Aseprite` still supports the use of it if it is something the end user would like to do.

You will need to first add the `MonoGame.Aseprite.Content.Pipeline` NuGet package to your project. You can do this in your IDE like before, or using the command line

```
dotnet add package MonoGame.Aseprite.Content.Pipeline --version 4.0.0
```

This will not add any references or output any build items for your project. Instead, the NuGet package that is downloaded contains the `MonoGame.Aseprite.Content.Pipeline.dll` that is added as a reference in the MGCB Editor.

> ⚠ Caution ⚠
>
> The reference you are about to add to the MGCB Editor will use the path to the `MonoGame.Aseprite.Content.Pipeline.dll` file that was downloaded with the NuGet package.
>
> By default, NuGet will download packages to the global packages folder which is `%userprofile%\.nuget\packages\` in Windows and `~/.nuget/packages` on Mac and Linux.
>
> If you use multiple workstations, or you have multiple team members working on the project from a shared git repo, I strongly recommend setting up a **nuget.config** file that tells the NuGet package to be added to a local directory in the project root directory.
>
> More information can be found at https://learn.microsoft.com/en-us/nuget/reference/nuget-config-file.

Next, open your `Content.mgcb` file in the MGCB Editor and perform the following

1. Click the **Content** node in the **Project** panel.
2. In the **Properties** panel below it, scroll down to the bottom to find the **References** field, and click it to open the **Reference Editor** dialog window
3. Click the **Add** button
4. Find and add the `MonoGame.Aseprite.Content.Pipeline.dll` file. 
    * By default, NuGet downloads the package to the following locations:
        * Windows: `%userprofile%\.nuget\packages\monogame-aseprite-content-pipeline\4.0.0\` 
        * Mac/Linux: `~/.nuget/packages/monogame-aseprite-content-pipeline/4.0.0/`

After that you should be set to use the MGCB Editor extensions for importing and processing.

MonoGame.Aseprite is distributed via NuGet as a NuGet package. It can be installed into your existing MonoGame game project using NuGet Package Manger in Visual Studio.

**Package Manager CLI**
`Install-Package MonoGame.Aseprite -Version 3.1.0`

**.NET CLI**
`dotnet add package MonoGame.Aseprite --version 3.1.0`

Documentation has not been updated as of this moment, however, all MonoGame 3.8.1 users can use the **MonoGame 3.8** documentation for getting starting and installation information. This documentation can be found at [https://aristurtle.net/monogame-aseprite/getting-started/monogame38installation](https://aristurtle.net/monogame-aseprite/getting-started/monogame38installation).

## Example Usage

The following are quick examples to get your started with using the `MonoGame.Aseprite` library. For full documentation and examples, please visit the documentation site at https://monogameaseprite.net

### Loading an Aseprite File

The following example demonstrates how to load the Aseprite file as an instance of the `AsepriteFile` class.

```csharp
using MonoGame.Aseprite;

public void MyGame : Game
{
    protected override void LoadContent()
    {
        AsepriteFile aseFile = AsepriteFile.Load("path/to/aseprite/file.aseprite");
    }
}
```

### Create A `Sprite` From a Frame

The following example demonstrates how to create a `Sprite` from a frame in the `AsepriteFile` and how to draw it using the `SpriteBatch`.

```csharp
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Processors;

public void MyGame : Game
{
    private Sprite _sprite;
    private SpriteBatch _spriteBatch;

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);

        //  First load the Aseprite file
        AsepriteFile aseFile = AsepriteFile.Load("path/to/aseprite/file.aseprite");

        //  Use the SpriteProcessor to create a sprite.  You just tell it the index of
        //  the frame
        _sprite = SpriteProcessor.Process(GraphicsDevice, aseFile, frameIndex: 0);

        //  You can supply additional optional parameters to control how the processor generates the source image for the
        //  sprite.
        //
        //  - onlyVisibleLayers: Indicates if only cels on visible layers should be included
        //  - includeBackgroundLayer: Indicates if cels on a layer marked as the background layer are included.
        //  - includeTilemapLayers:  Indicates if cels on a tilemap layer should be included.
        _sprite = SpriteProcessor.Process(GraphicsDevice, aseFile, frameIndex: 0,
                                                                   onlyVisibleLayers: true,
                                                                   includeBackgroundLayer: false,
                                                                   includeTilemapLayers: true);
    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  Use the sprite's Draw method
        _sprite.Draw(position: new Vector2(10, 20));

        //  Or you can use the sprite batch natively
        _spriteBatch.Draw(_sprite, position: new Vector2(30, 40));

        //  Both of those draw methods will use the Color, Transparency, Origin, Scale, Rotation, and LayerDepth
        //  properties of the sprite and plug them in automatically to the sprite batch when it draws.  If you want to
        //  override these and control more of the drawing, instead, draw the TextureRegion of the sprite
        _spriteBatch.Draw(_sprite.TextureRegion, position: new Vector2(50, 60),
                                                 color: Color.White,
                                                 rotation: 0.0f,
                                                 origin: Vector.Zero,
                                                 scale: Vector2.One,
                                                 effects: SpriteEffects.None,
                                                 layerDepth: 0.0f);
        _spriteBatch.End();
    }
}
```

### Create a `TextureAtlas`

The following demonstrates how to create a `TextureAtlas` from an `AsepriteFile` and use it to create `Sprites`.  The advantage to creating `Sprite` instances this way is that all frames are processed as a single source `Texture2D`.  Then each `Sprite` you create using the `TextureAtlas` share a reference to the same source `Texture2D` so it reduces the texture swapping done on the `SpriteBatch`.

```csharp
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Processors;

public void MyGame : Game
{
    private TextureAtlas _atlas;
    private Sprite _sprite0;
    private Sprite _sprite1;

    private SpriteBatch _spriteBatch;

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);

        //  First load the Aseprite file
        AsepriteFile aseFile = AsepriteFile.Load("path/to/aseprite/file.aseprite");

        //  Use the TextureAtlasProcessor to create a texture atlas.
        _atlas = TextureAtlasProcessor.Process(GraphicsDevice, aseFile);

        //  You can supply additional optional parameters to control how the frames are packed into the source image
        //  of the atlas
        //
        //  - onlyVisibleLayers: Indicates if only cels on visible layers should be included
        //  - includeBackgroundLayer: Indicates if cels on a layer marked as the background layer are included.
        //  - includeTilemapLayers:  Indicates if cels on a tilemap layer should be included.
        //  - mergeDuplicates: Indicates if duplicate frames are merged into one.
        //  - borderPadding: The amount of transparent pixels to add to the edge of the generated image.
        //  - spacing: The amount of transparent pixels to add between each region in the generated image.
        //  - innerPadding: The amount of transparent pixels to add around each region in the generated image.
        _atlas = TextureProcessor.Process(GraphicsDevice, aseFile, onlyVisibleLayers: true,
                                                                   includeBackgroundLayer: false,
                                                                   includeTilemapLayers: true,
                                                                   mergeDuplicates: true,
                                                                   borderPadding: 0,
                                                                   spacing: 0,
                                                                   innerPadding: 0);

       //   Now that you have a texture atlas, you can create Sprites from the regions with the atlas.  Each region
       //   index correlates to a frame from the Aseprite file in the same frame order.
       _sprite0 = _atlas.GetRegion(0);
       _sprite1 = _atlas[1];
    }
}
```

### Create a `SpriteSheet`
The following demonstrates how to create a `SpriteSheet` from the `AsepriteFile`.  The `SpriteSheet` is a wrapper around a `TextureAtlas` that also provides details for animations and creating `AnimatedSprite` elements.  The animation details come from the Tags that you created in Aseprite and are needed to create the `AnimatedSprite` elements from.

```csharp
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Processors;

public void MyGame : Game
{
    private SpriteSheet _spriteSheet;
    private AnimatedSprite _idleAnimation;
    private AnimatedSprite _walkingAnimation;
    bool _isWalking = false;

    private SpriteBatch _spriteBatch;

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);

        //  First load the Aseprite file
        AsepriteFile aseFile = AsepriteFile.Load("path/to/aseprite/file.aseprite");

        //  Use the SpriteSheetProcessor to create a spritesheet
        _spriteSheet = SpriteSheetProcessor.Process(GraphicsDevice, aseFile);

        //  You can supply additional optional parameters to control how the frames are packed into the source image
        //  of the atlas
        //
        //  - onlyVisibleLayers: Indicates if only cels on visible layers should be included
        //  - includeBackgroundLayer: Indicates if cels on a layer marked as the background layer are included.
        //  - includeTilemapLayers:  Indicates if cels on a tilemap layer should be included.
        //  - mergeDuplicates: Indicates if duplicate frames are merged into one.
        //  - borderPadding: The amount of transparent pixels to add to the edge of the generated image.
        //  - spacing: The amount of transparent pixels to add between each region in the generated image.
        //  - innerPadding: The amount of transparent pixels to add around each region in the generated image.
        _spriteSheet = SpriteSheetProcessor.Process(GraphicsDevice, aseFile, onlyVisibleLayers: true,
                                                                             includeBackgroundLayer: false,
                                                                             includeTilemapLayers: true,
                                                                             mergeDuplicates: true,
                                                                             borderPadding: 0,
                                                                             spacing: 0,
                                                                             innerPadding: 0);

       //   Now that you have the spritesheet, you can create AnimatedSprites from it.  These are based on the Tags
       //   that you created in Aseprite, so you just need to give it the tag name
       _idleAnimation = _spriteSheet.CreateAnimatedSprite("idle");
       _walkingAnimation = _spriteSheet.CreateAnimatedSprite("walking");
    }

    protected override void Update(GameTime gameTime)
    {
        //  In order for the AnimatedSprite to actually animate, it needs to be updated every frame
        if(_isWalking)
        {
            _walkingAnimation.Update(gameTime);
        }
        else
        {
            _idleAnimation.Update(gameTime);
        }

        //  You can use branch logic with if statements above to control when you update the animation, or you can
        //  use animation controls for the object itself
        //
        //  _walkingAnimation.Pause();
        //  _walkingAnimation.UnPause();
        //  _walkingAnimation.Stop();
        //  _walkingAnimation.Reset();

    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        if(_isWalking)
        {
            //  An AnimatedSprite inherits from the Sprite class, so it has all of the same properties that can be 
            //  adjusted for how it's rendered that get automatically passed into the spritebatch
            _walkingAnimation.Draw(_spriteBatch, position: new Vector2(10, 20));
        }
        else
        {
            //  Additionally, just like with the Sprite, you can override the properties that are passed to the 
            //  sprite batch by using the TextureRegion of the AnimatedSprite and supplying the properties yourself.
            //  For an AnimatedSprite, the TextureRegion property will always be the TextureRegin of the current
            //  frame of animation.
            _spriteBatch.Draw(_idleAnimation.TextureRegion, position: new Vector2(50, 60),
                                                            color: Color.White,
                                                            rotation: 0.0f,
                                                            origin: Vector.Zero,
                                                            scale: Vector2.One,
                                                            effects: SpriteEffects.None,
                                                            layerDepth: 0.0f);
        }

        _spriteBatch.End();
    }
}
```

### Create a `TileSet`
The following demonstrates how to import a `Tileset` from the `AsepriteFile`. 

> ⚠
>
> Tilesets are only supported in Aseprite version 1.3-beta.  However, the class itself is fully constructable and you can create manual instance of it yourself if you'd like. The example below assumes that the Aseprite file was created using Aseprite version >= 1.3-beta21

```csharp
using MonoGame.Aseprite;
using MonoGame.Aseprite.Tilemaps;
using MonoGame.Aseprite.Processors;

public void MyGame : Game
{
    private Tileset _tileset;

    private SpriteBatch _spriteBatch;

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);

        //  First load the Aseprite file
        AsepriteFile aseFile = AsepriteFile.Load("path/to/aseprite/file.aseprite");

        //  Use the TilesetProcessor to create the tileset. Just give it the name that you assigned it in Aseprite.
        _tileset = TilesetProcessor.Process(GraphicsDevice, aseFile, tilesetName: "TinyTown");

        //  You can also use the index of the tileset.  The index corresponds with the tileset ID from Aseprite.
        _tileset = TilesetProcessor.Process(GraphicsDevice, aseFile, tilesetIndex: 0);

    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  A Tileset is just a collection of TextureRegion elements that each represent a "tile" from the tileset.
        //
        //  Note: Aseprite creates the tile at index 0 as an empty tile.  Aseprite also stores the tileset image data
        //        as a vertical strip with only one column and as many rows as tiles.
        //
        //  Since they are just TextureRegion elements, we can draw them
        Vector2 pos = Vector2.Zero;
        for(int i = 0; i < _tileset.Tiles.Length; i++)
        {
            pos.Y += i * _tileset.TileHeight;

            _spriteBatch.Draw(_tileset[i], position: pos, Color.White);
        }

        _spriteBatch.End();
    }
}
```

### Create a `Tilemap` From a Frame
The following demonstrates how to create a `Tilemap` from a frame in the `AsepriteFile`. 

> ⚠
>
> Tilemaps are only supported in Aseprite version 1.3-beta.  However, the class itself is fully constructable and you can create manual instance of it yourself if you'd like. The example below assumes that the Aseprite file was created using Aseprite version >= 1.3-beta21

```csharp
using MonoGame.Aseprite;
using MonoGame.Aseprite.Tilemaps;
using MonoGame.Aseprite.Processors;

public void MyGame : Game
{
    private Tilemap _tilemap;

    private SpriteBatch _spriteBatch;

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);

        //  First load the Aseprite file
        AsepriteFile aseFile = AsepriteFile.Load("path/to/aseprite/file.aseprite");

        //  Use the TilesetProcessor to create the tilemap. Just supply it with the index of the frame that the
        //  tilemap you want to create is on.
        _tilemap = TilemapProcessor.Process(GraphicsDevice, aseFile, frameIndex: 0);

        //  You can supply the additional optional parameter to specify if only visible layers are included.  If you
        //  set this to false, then all tilemap layers will be included, even those you set as Hidden in Aseprite.
        _tilemap = TilemapProcessor.Process(GraphicsDevice, aseFile, frameIndex: 0, onlyVisibleLayers: true);

        //  You can adjust the layers in the tilemap to turn make them visible or not.  
        _tilemap.GetLayer(0).IsVisible = false;
        _tilemap[0].IsVisible = true;
    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  You can draw the tilemap by just calling it's Draw method
        _tilemap.Draw(_spriteBatch, position: Vector.Zero, color: Color.White);

        //  You can also draw it using the spritebatch instead
        //
        //  _spriteBatch.Draw(_tilemap, position: Vector2.Zero, color.White);

        //  Both methods of drawing it have additional parameters that can be passed in for scale and layer depth
        //  Scale can be supplied as a float or a Vector2
        //
        //  _tilemap.Draw(_spriteBatch, position, Vector2.Zero, color: Color.White, scale: Vector2.One, layerDepth: 0);
        //  _spriteBatch.Draw(_tilemap, position: Vector2.Zero, color: Color.White, scale: 1.0f, layerDepth: 0);


        _spriteBatch.End();
    }
}
```

### Create an `AnimatedTilemap` From a Frame
The following demonstrates how to create a `AnimatedTilemap` the  `AsepriteFile`. 

> ⚠
>
> AnimatedTilemaps are only supported in Aseprite version 1.3-beta.  However, the class itself is fully constructable and you can create manual instance of it yourself if you'd like. The example below assumes that the Aseprite file was created using Aseprite version >= 1.3-beta21

```csharp
using MonoGame.Aseprite;
using MonoGame.Aseprite.Tilemaps;
using MonoGame.Aseprite.Processors;

public void MyGame : Game
{
    private AnimatedTilemap _tilemap;

    private SpriteBatch _spriteBatch;

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);

        //  First load the Aseprite file
        AsepriteFile aseFile = AsepriteFile.Load("path/to/aseprite/file.aseprite");

        //  Use the AnimatedTilemapProcessor to create the animated tilemap.
        _tilemap = AnimatedTilemapProcessor.Process(GraphicsDevice, aseFile);

        //  You can supply an additional optional parameter to specify if only visible layers are included.  If you
        //  set this to false, then all tilemap layers will be included, even those you set as Hidden in Aseprite.
        _tilemap = AnimatedTilemapProcessor.Process(GraphicsDevice, aseFile, onlyVisibleLayers: true);
    }

    protected override void Update(GameTime gameTime)
    {
        //  In order for the AnimatedTilemap to actually to animate, it needs to be updated every frame
        _tilemap.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  You can draw the tilemap by just calling it's Draw method
        _tilemap.Draw(_spriteBatch, position: Vector.Zero, color: Color.White);

        //  You can also draw it using the spritebatch instead
        //
        //  _spriteBatch.Draw(_tilemap, position: Vector2.Zero, color.White);

        //  Both methods of drawing it have additional parameters that can be passed in for scale and layer depth
        //  Scale can be supplied as a float or a Vector2
        //
        //  _tilemap.Draw(_spriteBatch, position, Vector2.Zero, color: Color.White, scale: Vector2.One, layerDepth: 0);
        //  _spriteBatch.Draw(_tilemap, position: Vector2.Zero, color: Color.White, scale: 1.0f, layerDepth: 0);


        _spriteBatch.End();
    }
}
```




The following is a quick example of using MonoGame.Aseprite in your game.

**Add Using Statements**

```csharp
//  Add using statements
using MonoGame.Aseprite.Documents;
using MonoGame.Aseprite.Graphics;
```

**Load the Content**

```csharp
//  Load the AsepriteDocument
AsepriteDocument aseDoc = Content.Load<AsepriteDocument>("myAseFile");

//  Create a new AnimatedSprite from the document
AnimatedSprite sprite = new AnimatedSprite(aseDoc);

```

**Update the AnimatedSprite Instance**

```csharp
sprite.Update(gameTime);
```

**Drawing the AnimatedSprite**

```csharp
sprite.Render(spriteBatch);
```

## What Next?

- Read the [documentation](https://aristurtle.net/monogame-aseprite).
- Join the [Discord](https://discord.gg/8jFvHhuMJU) to ask questions or keep up to date.
- Submit an [issue on GitHub](https://github.com/AristurtleDev/monogame-aseprite/issues).
- Follow me on [Twitter @aristurtledev](https://www.twitter.com/aristurtledev).

## Sponsor On GitHub

[![](https://raw.githubusercontent.com/aristurtledev/monogame-aseprite/gh-pages-develop/static/img/github_sponsor.png)](https://github.com/sponsors/manbeardgames)
Hi, my name is Christopher Whitley. I am an indie game developer and game development tool developer. I create tools primary for the MonoGame framework. All of the tools I develop are released as free and open-sourced software (FOSS), just like this **Monogame.Aseprite** library.

If you'd like to buy me a cup of coffee or just sponsor me and my projects in general, you can do so on [GitHub Sponsors](https://github.com/sponsors/manbeardgames).

## License

Copyright(c) 2022 Chris Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
