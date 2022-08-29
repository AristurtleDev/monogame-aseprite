![](https://raw.githubusercontent.com/manbeardgames/monogame-aseprite/gh-pages-develop/static/img/monogame_aseprite_banner_800_200.png)

# MonoGame.Aseprite
MonoGame.Aseprite is an extension for the [MonoGame Framework](https://www.monogame.net) that allows you to import [Aseprite](https://www.aseprite.org) \*.ase/\*.aseprite files into your game project using the MGCB Editor (also known as the Content Pipeline Tool).

No need to export a sprite sheet from Aseprite and have to deal with a PNG image file and a JSON file. With MonoGame.Aseprite the import process takes the single Aseprite file and generates a `AsepriteDocument` object, containing a `Texture2D` generated sprite sheet and all the data you need to animate those sweet pixels in game.

MonoGame.Aseprite also provides an out-of-the-box AnimatedSprite class that can be used with the imported `AsepriteDocument` to get you started quickly if you prefer this as well.

## Getting Started
MonoGame.Aseprite is distributed via NuGet as a NuGet package. It can be installed into your existing MonoGame game project using NuGet Package Manger in Visual Studio.

**Package Manager CLI**
```Install-Package MonoGame.Aseprite -Version 3.1.0```

**.NET CLI**
```dotnet add package MonoGame.Aseprite --version 3.1.0```

Documentation has not been updated as of this moment, however, all MonoGame 3.8.1 users can use the **MonoGame 3.8** documentation for getting starting and installation information.  This documentation can be found at [https://aristurtle.net/monogame-aseprite/getting-started/monogame38installation](https://aristurtle.net/monogame-aseprite/getting-started/monogame38installation).

## Example Usage
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
* Read the [documentation](https://aristurtle.net/monogame-aseprite).
* Join the [Discord](https://discord.gg/8jFvHhuMJU) to ask questions or keep up to date.
* Submit an [issue on GitHub](https://github.com/AristurtleDev/monogame-aseprite/issues).
* Follow me on [Twitter @aristurtledev](https://www.twitter.com/aristurtledev).

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




