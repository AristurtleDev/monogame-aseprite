using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;

namespace DemoGame;

public class Game_Tileset : Game
{
    private Tileset _tileset;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private KeyboardState _curState;
    private KeyboardState _prevState;

    private Point _res = new(1280, 720);
    private Rectangle _topLeft = new(0, 0, 640, 380);
    private Rectangle _topRight = new(640, 0, 640, 360);
    private Rectangle _bottomLeft = new(0, 360, 640, 360);
    private Rectangle _bottomRight = new(640, 360, 640, 360);
    private Texture2D _pixel;
    private int _scale = 1;
    private int _tilesetID = 0;

    public Game_Tileset()
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
        _tileset = Content.Load<Tileset>("townmap");

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
            _tilesetID--;
            if (_tilesetID < 0) { _tilesetID = 0; }
        }
        else if (_curState.IsKeyDown(Keys.Right) && _prevState.IsKeyUp(Keys.Right))
        {
            _tilesetID++;
            if (_tilesetID >= _tileset.TileCount) { _tilesetID--; }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

        _spriteBatch.Draw(_tileset.Texture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, _scale, SpriteEffects.None, 0.0f);
        _spriteBatch.Draw(_tileset[_tilesetID], new Vector2(_res.X, _res.Y) * 0.5f, Color.White, 0.0f, Vector2.One, _scale, SpriteEffects.None, 0.0f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
