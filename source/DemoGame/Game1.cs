using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;

namespace DemoGame;

public class Game1 : Game
{
    private SpriteSheet _sheet;
    private TextureRegion _sprite;
    private Animation _animatedSprite;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private int _frameIndex = 0;
    private KeyboardState _curState;
    private KeyboardState _prevState;

    private Point _res = new(1280, 720);
    private Rectangle _topLeft = new(0, 0, 640, 380);
    private Rectangle _topRight = new(640, 0, 640, 360);
    private Rectangle _bottomLeft = new(0, 360, 640, 360);
    private Rectangle _bottomRight = new(640, 360, 640, 360);
    private Texture2D _pixel;
    private int _scale = 1;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = _res.X;
        _graphics.PreferredBackBufferHeight = _res.Y;
        _graphics.ApplyChanges();

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

        // TODO: use this.Content to load your game content here
        _sheet = Content.Load<SpriteSheet>("adventurer");
        _sprite = _sheet.GetRegion("adventurer 0");
        _animatedSprite = _sheet.CreateAnimation("attack3");

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData<Color>(new Color[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _prevState = _curState;
        _curState = Keyboard.GetState();

        if (_curState.IsKeyDown(Keys.Down) && _prevState.IsKeyUp(Keys.Down))
        {
            _frameIndex--;
            if (_frameIndex < 0) { _frameIndex = 0; }
            _sprite = _sheet.GetRegion($"frame_{_frameIndex}");
        }
        else if (_curState.IsKeyDown(Keys.Up) && _prevState.IsKeyUp(Keys.Up))
        {
            _frameIndex++;
            if (_frameIndex >= _sheet.RegionCount) { _frameIndex--; }
            _sprite = _sheet.GetRegion($"frame_{_frameIndex}");
        }

        if (_curState.IsKeyDown(Keys.Left) && _prevState.IsKeyUp(Keys.Left))
        {
            _scale--;
            if (_scale < 1) { _scale = 1; }
        }
        else if (_curState.IsKeyDown(Keys.Right) && _prevState.IsKeyUp(Keys.Right))
        {
            _scale++;
            if (_scale > 10) { _scale = 10; }
        }

        _animatedSprite.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

        DrawSector(_topLeft, Color.White);
        DrawSector(_topRight, Color.Black);
        DrawSector(_bottomLeft, Color.CornflowerBlue);
        DrawSector(_bottomRight, Color.Red);



        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawSector(Rectangle rect, Color color)
    {

        _spriteBatch.Draw(_pixel, rect, color);
        _spriteBatch.Draw(_animatedSprite, position: new Vector2(rect.X, rect.Y), Color.White, 0.0f, Vector2.Zero, new Vector2(_scale, _scale), SpriteEffects.None, 0.0f);

        // _spriteBatch.Draw(_sprite, position: new Vector2(rect.X, rect.Y), scale: new Vector2(_scale, _scale));
    }
}
