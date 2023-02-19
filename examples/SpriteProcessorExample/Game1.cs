/*
This example demonstrates using the SpriteProcessor to create a Sprite
from a frame in the Aseprite file that is loaded.  In this example, the
Sprite is created from frame 0.

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

namespace SpriteProcessorExample;

public class Game1 : Game
{
    private Sprite _sprite;
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
        AsepriteFile aseFile = AsepriteFile.Load(Path.Combine(Content.RootDirectory, "character_robot.aseprite"));

        //  Use the SpriteProcessor to process frame 0 of the Aseprite file as a Sprite
        _sprite = SpriteProcessor.Process(GraphicsDevice, aseFile, aseFrameIndex: 0);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  Draw the sprite
        _sprite.Draw(_spriteBatch, position: new Vector2(10, 10));

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
