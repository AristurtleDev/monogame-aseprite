/*
This example demonstrates using the SpriteSheetProcessor to create a 
SpriteSheet from the AsepriteFile that is loaded.  An AnimatedSprite is
then created from the SpriteSheet.

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

namespace SpriteSheetProcessorExample;

public class Game1 : Game
{
    private SpriteSheet _spriteSheet;

    private AnimatedSprite _attackCycle;
    private AnimatedSprite _runCycle;
    private AnimatedSprite _walkCycle;

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

        //  Use the SpriteSheetProcessor to process the SpriteSheet from the Aseprite file.
        _spriteSheet = SpriteSheetProcessor.Process(GraphicsDevice, aseFile);

        //  Create the AnimatedSprite using the SpriteSheet.
        //  The name of the Tags you added in Aseprite are the names of the 
        //  AnimationTags in the SpriteSheet.
        _walkCycle = _spriteSheet.CreateAnimatedSprite("walk");
        _runCycle = _spriteSheet.CreateAnimatedSprite("run");
        _attackCycle = _spriteSheet.CreateAnimatedSprite("attack");
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        //  The animated sprite needs to be updated in order for it to actually animated
        _attackCycle.Update(gameTime);
        _walkCycle.Update(gameTime);
        _runCycle.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //  Draw the AnimatedSprite
        _attackCycle.Draw(_spriteBatch, position: new Vector2(10, 10));
        _walkCycle.Draw(_spriteBatch, position: new Vector2(_attackCycle.Width, 10));
        _runCycle.Draw(_spriteBatch, position: new Vector2(_attackCycle.Width * 2, 10));

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
