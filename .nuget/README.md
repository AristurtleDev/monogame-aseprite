![MonoGame.Aseprite Banner](https://raw.githubusercontent.com/AristurtleDev/monogame-aseprite/main/.github/images/banner.png)

A Cross Platform C# Library That Adds Support For Aseprite Files in MonoGame Projects.

[![build-and-test](https://github.com/AristurtleDev/monogame-aseprite/actions/workflows/buildandtest.yml/badge.svg)](https://github.com/AristurtleDev/monogame-aseprite/actions/workflows/buildandtest.yml)
[![Nuget 4.0.0](https://img.shields.io/nuget/v/MonoGame.Aseprite?color=blue&style=flat-square)](https://www.nuget.org/packages/MonoGame.Aseprite/4.0.0)
[![License: MIT](https://img.shields.io/badge/📃%20license-MIT-blue?style=flat)](LICENSE)
[![Twitter](https://img.shields.io/badge/%20-Share%20On%20Twitter-555?style=flat&logo=twitter)](https://twitter.com/intent/tweet?text=MonoGame.Aseprite%20by%20%40aristurtledev%0A%0AA%20cross-platform%20C%23%20library%20that%20adds%20support%20for%20Aseprite%20files%20in%20MonoGame%20projects.%20https%3A%2F%2Fgithub.com%2FAristurtleDev%2Fmonogame-aseprite%0A%0A%23monogame%20%23aseprite%20%23dotnet%20%23csharp%20%23oss%0A)

[MonoGame.Aseprite](https://monogameaseprite.net) is a free and open source library for the [MonoGame Framework](https://www.monogame.net) that assists in importing [Aseprite](https://www.aseprite.org) (.ase | .aseprite) files into your game project. No need to export a spritesheet from Aseprite and have to deal with a PNG + JSON file. With [MonoGame.Aseprite](https://monogameaseprite.net), you can use the Aseprite file directly.

[MonoGame.Aseprite](https://monogameaseprite.net) supports importing the file **both with and without the MGCB Editor** (also known as the Content Pipeline Tool). Along with importing the file contents, several [**processors**](https://monogameaseprite.net/docs/processors/processors-overview) have been designed to transform the file contents into a more meaningful state to use within MonoGame.

[MonoGame.Aseprite](https://monogameaseprite.net) also supports outputting the processed file content to disk in a binary format and reader classes to read the processed information back in. This adds support for pre-processing content using any build or content workflow the end user has as long as it can use the [MonoGame.Aseprite](https://monogameaseprite.net) library.

## Getting Started
To get started using [MonoGame.Aseprite](https://monogameaseprite.net) start with the [Installation document](https://monogameaseprite.net/docs/getting-started/installation).

## Features
* Import your Aseprite file at runtime with and without the MGCB Editor (Content Pipeline Tool)
* Multiple built-in [processors](https://monogameaseprite.net/docs/processors/processors-overview) that can be used to transform the data from your Aseprite file into any of the following: 
    * [Sprite](https://monogameaseprite.net/docs/api/MonoGame.Aseprite/Sprites/Sprite/)
    * [TextureAtlas](https://monogameaseprite.net/docs/api/MonoGame.Aseprite/Sprites/TextureAtlas/)
    * [SpriteSheet](https://monogameaseprite.net/docs/api/MonoGame.Aseprite/Sprites/SpriteSheet/)
    * [Tileset](https://monogameaseprite.net/docs/api/MonoGame.Aseprite/Tilemaps/Tileset/)
    * [Tilemap](https://monogameaseprite.net/docs/api/MonoGame.Aseprite/Tilemaps/Tilemap/)
    * [AnimatedTilemap](https://monogameaseprite.net/docs/api/MonoGame.Aseprite/Tilemaps/AnimatedTilemap/).
* All blend modes in Aseprite supported 1:1.
* Runtime writers and readers that can be used in custom content processing workflows to preprocess content outside of the game.

## What Next?

- Read the [documentation](https://monogameaseprite.net/).
- Join the [Discord](https://discord.gg/8jFvHhuMJU) to ask questions or keep up to date.
- Submit an [issue on GitHub](https://github.com/AristurtleDev/monogame-aseprite/issues).
- Follow me on [Mastodon](https://mastodon.gamedev.place/@aristurtle) or [Twitter](https://www.twitter.com/aristurtledev).  

##  Support
If you would like to support this project in any way, there are quite a few ways to do this no matter who you are.
  * Tell others about this project.  
  * Contribute to this project (Please ensure you read the [Contributing Guide](./CONTRIBUTING.md)).
  * [Submit an Issue](https://github.com/AristurtleDev/monogame-aseprite/issues) if you find a problem
  * [Star](https://docs.github.com/en/get-started/exploring-projects-on-github/saving-repositories-with-stars) this project on Github.

If you would prefer to send me a tip/donation, you can do so at my [Ko-fi page](https://ko-fi.com/aristurtledev).  This is always appreciated and all donations go towards funding projects of mine such as this one.

[![](.github/images/kofi-bg-white.webp#gh-dark-mode-only)](https://ko-fi.com/aristurtledev)

You can also support me through [GitHub Sponsors](https://github.com/sponsors/AristurtleDev).  This and the Ko-Fi links can both be found in the sidebar to the right on this repository page.


## License

Copyright(c) 2018-2023 Chris Whitley

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
