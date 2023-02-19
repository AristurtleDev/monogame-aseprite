/*
This example demonstrates using the TextureAtlasProcessor to create a 
TextureAtlas from the AsepriteFile that is loaded.  Then three Sprites are
created using the TextureAtlas.

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

namespace TextureAtlasProcessorExample;

public class Game1 : Game
{
    private TextureAtlas _atlas;

    private Sprite _sprite1;
    private Sprite _sprite2;
    private Sprite _sprite3;

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

        //  Use the TextureAtlasProcessor to process the TextureAtlas from the Aseprite file.
        _atlas = TextureAtlasProcessor.Process(GraphicsDevice, aseFile);

        //  Create the Sprites using the TextureAtlas.
        _sprite1 = _atlas.CreateSprite(regionIndex: 0);
        _sprite2 = _atlas.CreateSprite(regionIndex: 1);

        //  You can also create sprites from the atlas using the region name.
        //  Region names are generated automatically by the processor and follow
        //  the format of 
        //  
        //  "[fileName] [frameIndex]"
        //  
        //  So for this file "character_robot", if we wanted to use frame 2
        //  we would do the following:
        _sprite3 = _atlas.CreateSprite("character_robot 2");

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  Draw the sprite
        _sprite1.Draw(_spriteBatch, position: new Vector2(10, 10));
        _sprite2.Draw(_spriteBatch, position: new Vector2(_sprite1.Width, 10));
        _sprite3.Draw(_spriteBatch, position: new Vector2(_sprite1.Width * 2, 10));
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
