/*
This example demonstrates using the TilemapProcessor to create a 
Tilemap from a frame in the AsepriteFile that is loaded.

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

namespace TilemapProcessorExample;

public class Game1 : Game
{
    private Tilemap _tilemap;

    private Vector2 _scale;


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

        //  Use the TilemapProcessor to process the Tilemap from the Aseprite file.
        _tilemap = TilemapProcessor.Process(GraphicsDevice, aseFile, frameIndex: 0);

        //  We're going to scale the tilemap up to the size of the game window
        //  based on width and height of layer 0.
        _scale.X = _graphics.PreferredBackBufferWidth / (float)_tilemap[0].Width;
        _scale.Y = _graphics.PreferredBackBufferHeight / (float)_tilemap[0].Height;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  Draw the tilemap
        _tilemap.Draw(_spriteBatch, position: Vector2.Zero, color: Color.White, scale: _scale, layerDepth: 0.0f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
