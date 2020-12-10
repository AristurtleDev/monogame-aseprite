![](https://raw.githubusercontent.com/manbeardgames/monogame-aseprite/gh-pages-develop/static/img/monogame_aseprite_banner_800_200.png)

# MonoGame.Aseprite
MonoGame.Aseprite is an extension for the [MonoGame Framework](https://www.monogame.net) that allows you to import [Aseprite](https://www.aseprite.org) \*.ase/\*.aseprite files into your game project using the MGCB Editor (also known as the Content Pipeline Tool).

No need to export a sprite sheet from Aseprite and have to deal with a PNG image file and a JSON file. With MonoGame.Aseprite the import process takes the single Aseprite file and generates a `AsepriteDocument` object, containing a `Texture2D` generated sprite sheet and all the data you need to animate those sweet pixels in game. 

MonoGame.Aseprite also provides an out-of-the-box AnimatedSprite class that can be used with the imported `AsepriteDocument` to get you started quickly if you prefer this as well.

## Getting Started
MonoGame.Aseprite is distributed via NuGet as a NuGet package. It can be installed into your existing MonoGame game project using NuGet Package Manger in Visual Studio. 

For **MonoGame 3.7.1** (.NET Framework >= 4.5) users, please refer to the [installation documentation here](https://manbeardgames.com/monogame-aseprite/getting-started/monogame37installation).

For **MonoGame 3.8** (.NET Core) users, please refer to the [installation documentation here](https://manbeardgames.com/monogame-aseprite/getting-started/monogame38installation).

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
* Read the [documentation](https://manbeardgames.com/monogame-aseprite).
* Join the [Discord](https://discord.gg/8jFvHhuMJU) to ask questions or keep up to date.
* Submit an [issue on GitHub](https://github.com/manbeardgames/monogame-aseprite/issues).
* Follow me on [Twitter @manbeardgames](https://www.twitter.com/manbeardgames).

## Patreon Support
[![image](https://raw.githubusercontent.com/manbeardgames/monogame-aseprite/gh-pages-develop/static/img/patreon.png)](https://www.patreon.com/manbeardgames)
If you would like, you can support me and this project on Patreon by clicking the image above. Thank you to all who support.





## License
Copyright(c) 2020 Chris Whitley

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




