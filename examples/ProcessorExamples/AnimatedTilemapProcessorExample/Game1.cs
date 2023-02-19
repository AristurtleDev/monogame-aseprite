/*
This example demonstrates using the AnimatedTilemapProcessor to create an
AnimatedTilemap from the AsepriteFile that is loaded.

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

namespace AnimatedTilemapProcessorExample;

public class Game1 : Game
{
    private AnimatedTilemap _animatedTilemap;

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

        //  Use the AnimatedTilemapProcessor to process the AnimatedTilemap from the Aseprite file.
        _animatedTilemap = AnimatedTilemapProcessor.Process(GraphicsDevice, aseFile);

        //  We're going to scale the tilemap up to the size of the game window
        //  based on width and height of layer 0 in frame 0.
        _scale.X = _graphics.PreferredBackBufferWidth / (float)_animatedTilemap.GetFrame(0).GetLayer(0).Width;
        _scale.Y = _graphics.PreferredBackBufferHeight / (float)_animatedTilemap.GetFrame(0).GetLayer(0).Height;
    }

    protected override void Update(GameTime gameTime)
    {
        //  The AnimatedTilemap needs to be updated each frame to actually animate.
        _animatedTilemap.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  Draw the AnimatedTilemap.
        _animatedTilemap.Draw(_spriteBatch, position: Vector2.Zero, color: Color.White, scale: _scale, layerDepth: 0.0f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
