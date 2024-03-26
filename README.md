<h1 align="center">
<img src="https://raw.githubusercontent.com/AristurtleDev/monogame-aseprite/main/.images/banner.png" alt="MonoGame.Aseprite Logo">
<br/>
A Cross Platform C# Library That Adds Support For Aseprite Files in MonoGame Projects.

[![build-and-test](https://github.com/AristurtleDev/monogame-aseprite/actions/workflows/main.yml/badge.svg)](https://github.com/AristurtleDev/monogame-aseprite/actions/workflows/main.yml)
[![NuGet 6.0.0](https://img.shields.io/nuget/v/MonoGame.Aseprite?color=blue&style=flat-square)](https://www.nuget.org/packages/MonoGame.Aseprite/6.0.0)
[![License: MIT](https://img.shields.io/badge/📃%20license-MIT-blue?style=flat)](LICENSE)
[![Twitter](https://img.shields.io/badge/%20-Share%20On%20Twitter-555?style=flat&logo=twitter)](https://twitter.com/intent/tweet?text=MonoGame.Aseprite%20by%20%40aristurtledev%0A%0AA%20cross-platform%20C%23%20library%20that%20adds%20support%20for%20Aseprite%20files%20in%20MonoGame%20projects.%20https%3A%2F%2Fgithub.com%2FAristurtleDev%2Fmonogame-aseprite%0A%0A%23monogame%20%23aseprite%20%23dotnet%20%23csharp%20%23oss%0A)

</h1>

**Monogame.Aseprite** is a free and open source library for the [MonoGame Framework](https://monogame.net) that assists in importing [Aseprite](https://www.aseprite.org) files into your game project.  No need to export a spritesheet from Aseprite and have to deal with a PNG + JSON import.  With **MonoGame.Aseprite**, you can use the Aseprite file directly.

# Features
- **MonoGame.Aseprite** internally uses [AsepriteDotNet](https://github.com/AristurtleDev/AsepriteDotNet) to read and parse the aseprite files.  This means you get all the features of AsepriteDotNet including
    - Supports Aseprite files using **RGBA**, **Grayscale**, and **Indexed** color modes.
    - Supports all Aseprite layer blend modes.
    - Supports Aseprite tilesets, tilemap layers, and tilemap cels.
- Provides processors to convert the Aseprite data into common formats including:
    - Sprite
    - SpriteSheet
    - TextureAtlas
    - Tileset
    - Tilemap
    - AnimatedTilemap


# Installation
Install via NuGet
```
dotnet add package MonoGame.Aseprite --version 6.0.0
```

# Usage
Please refer to the [Examples](./examples) directory for samples of how to use this library.  Each example is thoroughly commented with information to show you how to load the Aseprite file and use it within Monogame.

# What Next?

- Read the [documentation](https://monogameaseprite.net/).
- Join the [Discord](https://discord.gg/8jFvHhuMJU) to ask questions or keep up to date.
- Submit an [issue on GitHub](https://github.com/AristurtleDev/monogame-aseprite/issues).
- [Twitter](https://www.twitter.com/aristurtledev).

## Games Made With MonoGame.Aseprite
The following are games that have been made using MonoGame.Aseprite as part of the content workflow in MonoGame

| Game | Developer Links |
|--- |---|
| <h3 align="center">Superstar Strategy</h3> <img src="https://cdn.akamai.steamstatic.com/steam/apps/1756730/header.jpg?t=1696817764"> | <ul><li>Steam: <a href="https://store.steampowered.com/app/1756730/Superstar_Strategy/">https://store.steampowered.com/app/1756730/Superstar_Strategy/</a></li><li>Twitter: <a href="https://x.com/TalberonGames">https://x.com/TalberonGames</a></li></ul>
| <h3 align="center">Unnamed</h3> <img src="https://img.itch.zone/aW1nLzEzNjQ1MTM5LnBuZw==/original/yOAqCD.png"/> | <ul><li>Itch.io: <a href="https://fypur.itch.io/unnamed">https://fypur.itch.io/unnamed</a></li><li>Youtube: <a href="https://youtube.com/c/fypur">https://youtube.com/c/fypur</a></li></ul> |

# Support

If you would like to support this project in any way, there are quite a few ways to do this no matter who you are.

- Tell others about this project.
- Contribute to this project (Please ensure you read the [Contributing Guide](./CONTRIBUTING.md)).
- [Submit an Issue](https://github.com/AristurtleDev/monogame-aseprite/issues) if you find a problem
- [Star](https://docs.github.com/en/get-started/exploring-projects-on-github/saving-repositories-with-stars) this project on Github.

If you would prefer to send me a tip/donation, you can do so at my [Ko-fi page](https://ko-fi.com/aristurtledev). This is always appreciated and all donations go towards funding projects of mine such as this one.

[![](.images/kofi-bg-white.webp)](https://ko-fi.com/aristurtledev)

You can also support me through [GitHub Sponsors](https://github.com/sponsors/AristurtleDev). This and the Ko-Fi links can both be found in the sidebar to the right on this repository page.

The following is a list of amazing people that have donated to sponsor this project. If you have donated but weren't added to the list, please get in contact with me.


* [fdrobidoux](https://github.com/fdrobidoux)
* [Anticdope](https://twitter.com/anticdope) 

---

# License
**MonoGame.Aseprite** is licensed under the **MIT License**.  Please refer to the [LICENSE](LICENSE) file for full license text.

# Contributors
<a href="https://github.com/aristurtledev/monoame-aseprite/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=aristurtledev/monogame-aseprite" />
</a>

Made with [contrib.rocks](https://contrib.rocks).