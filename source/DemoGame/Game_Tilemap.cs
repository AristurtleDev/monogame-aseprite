using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;

namespace DemoGame;

public class Game_Tilemap : Game
{
    private Tilemap _tilemap;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private KeyboardState _curState;
    private KeyboardState _prevState;

    // private Point _res = new(384, 224);
    private Point _res = new(384 * 4, 224 * 4);
    private float _scale = 1.0f;


    public Game_Tilemap()
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
        _tilemap = Content.Load<Tilemap>("townmap");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _prevState = _curState;
        _curState = Keyboard.GetState();

        if (_curState.IsKeyDown(Keys.D1) && _prevState.IsKeyUp(Keys.D1))
        {
            TilemapLayer layer = _tilemap.GetLayer(0);
            layer.IsVisible = !layer.IsVisible;
        }

        if (_curState.IsKeyDown(Keys.D2) && _prevState.IsKeyUp(Keys.D2))
        {
            TilemapLayer layer = _tilemap.GetLayer(1);
            layer.IsVisible = !layer.IsVisible;
        }

        if (_curState.IsKeyDown(Keys.D3) && _prevState.IsKeyUp(Keys.D3))
        {
            TilemapLayer layer = _tilemap.GetLayer(2);
            layer.IsVisible = !layer.IsVisible;
        }


        if (_curState.IsKeyDown(Keys.Down) && _prevState.IsKeyUp(Keys.Down))
        {
            _scale--;
            if (_scale < 1) { _scale = 1; }
        }
        else if (_curState.IsKeyDown(Keys.Up) && _prevState.IsKeyUp(Keys.Up))
        {
            _scale++;
            if (_scale > 10) { _scale = 10; }
        }

        if (_curState.IsKeyDown(Keys.Left) && _prevState.IsKeyUp(Keys.Left))
        {

        }
        else if (_curState.IsKeyDown(Keys.Right) && _prevState.IsKeyUp(Keys.Right))
        {

        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

        _spriteBatch.Draw(_tilemap, Vector2.Zero, Color.White, new Vector2(_scale, _scale), 0.0f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
