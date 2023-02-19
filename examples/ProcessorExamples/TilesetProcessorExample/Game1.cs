/*
This example demonstrates using the TilesetProcessor to create a 
Tileset from a tileset in the AsepriteFile that is loaded.

The Aseprite file that is loaded can be found in the Content directory.  It
was placed here so that use can be made of the Content.RootDirectory path.
The Aseprite file itself is copied to the build directory by a configuration
in the .csproj file of this project. 

*/
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Tilemaps;

namespace TilesetProcessorExample;

public class Game1 : Game
{
    private Tileset _tileset;

    private TextureRegion _greenBushTile;
    private TextureRegion _yellowBushTile;
    private TextureRegion _mushroomsTile;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //  Load the Aseprite file
        AsepriteFile aseFile = AsepriteFile.Load(Path.Combine(Content.RootDirectory, "townmap.aseprite"));

        //  Use the TilesetProcessor to process the Tileset from the Aseprite file.
        _tileset = TilesetProcessor.Process(GraphicsDevice, aseFile, tilesetIndex: 0);

        //  Create TextureRegions from the tiles in the tileset.  You just give it the index
        //  of the tile in the tileset.
        //  You can use the GetTile() method or the this[] indexor method
        _yellowBushTile = _tileset.GetTile(28);
        _greenBushTile = _tileset[29];
        _mushroomsTile = _tileset[30];
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  Draw the tiles
        _greenBushTile.Draw(_spriteBatch, position: new Vector2(10, 10), Color.White);
        _yellowBushTile.Draw(_spriteBatch, position: new Vector2(10 + _tileset.TileWidth, 10), Color.White);
        _mushroomsTile.Draw(_spriteBatch, position: new Vector2(10 + _tileset.TileWidth * 2, 10), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
