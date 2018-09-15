# MonoGame.Aseprite

MonoGame.Aseprite is an extension for [MonoGame Framework](http://www.monogame.net) to import the .json file produced by [Aseprite](http://www.aseprite.org) using the content pipeline, with a collection of classes to support rending the animation.


![](https://i.imgur.com/lksiazd.gif)

## Current Release Download
You can find the current release download here https://gitlab.com/manbeardgames/monogame-aseprite/tags/version-1.0

## Usage
The following is a brief explination of how to use this in your MonoGame project. 
[**For a more detailed explination, including images, please check the wiki**](https://gitlab.com/manbeardgames/monogame-aseprite/wikis/home)  

* In your MonoGame project, add a reference to the MonoGame.Aseprite.dll
* In the MonoGame Content Pipeline tool for your project, add a reference to MonoGame.Aseprite.ContentPipeline.dll
* Export your spritesheet from Aseprite
    * Select Array for meta information not Hash
    * Frame Tags must be checked
    * Trim and Padding is currently not supported
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



## Notes
This project is a example of how I did this. You should definitly fork this and modify it to suit your project.

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




