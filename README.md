# MonoGame.Aseprite

MonoGame.Aseprite is an extension for [MonoGame Framework](http://www.monogame.net) to import the .json file produced by [Aseprite](http://www.aseprite.org) using the content pipeline, with a collection of classes to support rending the animation.


![](https://github.com/manbeardgames/monogame-aseprite/blob/master/docs/images/preview.gif)

## Getting Started
MonoGame.Aseprite is distributed via NuGet as a NuGet package. It can be installed inot your existing MonOGame project using NuGet Package Manger in Visual Studio.  You can also use the following commands to install it.

**Package Manager**  
```PM> Install-Package MonoGame.Aseprite -Version 1.1.0 ```

**.Net CLI**  
```> dotnet add package MonoGame.Aseprite --version 1.1.0 ```  


Once you've added the NuGet package, you'll also need to add a reference to **MonoGame.Aseprite.ContentPipeline.dll** in the Content Pipeline Tool.  This file can be found in the "/packages/MonoGame.Aseprite.1.0.0/content" folder in the root directory of your project folder after installing the NuGet package.

For more information on doing this, check out this [wiki page](https://github.com/manbeardgames/monogame-aseprite/wiki/downloading-and-adding-references)

## Usage
The following is a brief explination of how to use this in your MonoGame project. 
[**For a more detailed explination, including images, please check the wiki**](https://github.com/manbeardgames/monogame-aseprite/wiki)  

* Export your spritesheet from Aseprite
    * Select Array for meta information not Hash
    * Frame Tags must be checked
    * Slices must be checked
    * Trim and Padding is currently not supported. These must be unchecked.
* Add the exported spritesheet and associated .json file from Aseprite to your project using the MonoGame Pipeline Tool
    * The spritesheet can be imported normally using the Texture importer and processor
    * For the .json file, use the Aseprite Animation Importer and the Aseprite Animation Processor
* Create a new AnimatedSprite object using the spritesheet and the animation definition.
    * The animation defintion is loaded in your game using the content manger. It loads the .json file you imported.

    ```csharp
    //  Be sure to add the using statement at the top
    using MonoGame.Aseprite;
    
    ...
    
    //  Load the .json file
    AnimationDefinition animationDefinition = content.Load<AnimationDefinition>("animationDefinition");
    
    //  Load the sprite sheet
    Texture2D spritesheet = content.load<Texture2D>("spritesheet");
    
    //  Create the AnimatedSprite based on the animation definition and the sprite sheet
    AnimatedSprite animatedSprite = new AnimatedSprite(spriteSheet, animationDefinition);
    ```

## What Next?
* Check out the [wiki](https://github.com/manbeardgames/monogame-aseprite/wiki)
* Read about [using slices](https://github.com/manbeardgames/monogame-aseprite/wiki/using-slices-from-aseprite) if you plan to do that
* Submit an [issue on GitHub](https://github.com/manbeardgames/monogame-aseprite/issues)
* Hit me up on [Twitter @manbeardgames](https://www.twitter.com/manbeardgames) if you have questions



## License
Copyright(c) 2018 Chris Whitley

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




