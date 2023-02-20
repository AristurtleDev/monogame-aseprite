using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Content.Readers;
using System.IO;
using MonoGame.Aseprite.RawTypes;

namespace CustomProcessingExample;

public class Game1 : Game
{
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

        //  Since the content was preprocessed as a SpriteSheet by the 
        //  ContentProcessor project, we can read the raw value in first
        //  Then create the instance from the raw value
        RawSpriteSheet rawSpriteSheet = RawSpriteSheetReader.Read(Path.Combine(Content.RootDirectory, "character_robot.rawSpriteSheet"));
        SpriteSheet spriteSheet = SpriteSheet.FromRaw(GraphicsDevice, rawSpriteSheet);

        //  Now use it like normal, like creating an AnimatedSprite
        _walkCycle = spriteSheet.CreateAnimatedSprite("walk");

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _walkCycle.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _walkCycle.Draw(_spriteBatch, position: new Vector2(10, 10));

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
