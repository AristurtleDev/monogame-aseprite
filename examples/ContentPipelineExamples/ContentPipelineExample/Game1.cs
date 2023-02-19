using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Content.Processors;

namespace ContentPipelineExample;

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

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //  Load the Aseprite file from the content pipeline
        AsepriteFile aseFile = Content.Load<AsepriteFile>("character_robot");

        //  Do something with it (see the ProcessorsExamples folder for more examples)
        SpriteSheet sheet = SpriteSheetProcessor.Process(GraphicsDevice, aseFile);
        _walkCycle = sheet.CreateAnimatedSprite("walk");

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _walkCycle.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _walkCycle.Draw(_spriteBatch, position: new Vector2(10, 10));
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
