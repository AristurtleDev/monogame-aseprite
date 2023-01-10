using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;

namespace DemoGame;

public class GameTilesetCollection : Game
{
    private TilesetCollection _tilesets;
    private int _tilesetIndex = 0;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private KeyboardState _curState;
    private KeyboardState _prevState;

    private Point _res = new(1280, 720);
    private int _scale = 1;

    public GameTilesetCollection()
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
        _tilesets = Content.Load<TilesetCollection>("tileset-collection");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _prevState = _curState;
        _curState = Keyboard.GetState();

        if (_curState.IsKeyDown(Keys.Down) && _prevState.IsKeyUp(Keys.Down))
        {
            _tilesetIndex--;
            if (_tilesetIndex < 0) { _tilesetIndex = 0; }
        }
        else if (_curState.IsKeyDown(Keys.Up) && _prevState.IsKeyUp(Keys.Up))
        {
            _tilesetIndex++;
            if (_tilesetIndex >= _tilesets.Count) { _tilesetIndex--; }
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

        _spriteBatch.Draw(_tilesets[_tilesetIndex].Texture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, new Vector2(_scale, _scale), SpriteEffects.None, 0.0f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
