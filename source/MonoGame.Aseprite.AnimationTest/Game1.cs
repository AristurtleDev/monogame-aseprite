using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Aseprite.AnimationTest
{
    public class Game1 : Game
    {
        enum RenderType
        {
            Texture,
            Animations
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _pixel;
        private SpriteFont _font;
        private Rectangle _reusableRect;

        private int _gridColumnWidth;
        private int _gridRowHeight;
        private int _gridColumnCount;
        private int _gridRowCount;
        private int _resolutionWidth;
        private int _resolutionHeight;

        private RenderType _renderType;
        private bool _renderAnimationSlices;
        AsepriteAnimationDocument _aseprite;


        private double _frameTimer;
        private int _currentFrameIndex;
        private int _currentAnimationIndex;
        private AsepriteAnimationDocument.Animation[] _animations;
        private AsepriteAnimationDocument.Animation _currentAnimation;
        private AsepriteAnimationDocument.Frame _currentFrame;

        private float _renderScale;

        private KeyboardState _currentKeyboard;
        private KeyboardState _previousKeyboard;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _resolutionWidth = 1280;
            _resolutionHeight = 720;

            _gridColumnWidth = 64;
            _gridRowHeight = 64;
            _gridColumnCount = (int)Math.Ceiling(_resolutionWidth / (float)_gridColumnWidth);
            _gridRowCount = (int)Math.Ceiling(_resolutionHeight / (float)_gridRowHeight);

            _renderScale = 2.0f;
            _renderAnimationSlices = false;


        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            _graphics.PreferredBackBufferWidth = _resolutionWidth;
            _graphics.PreferredBackBufferHeight = _resolutionHeight;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //_aseprite = Content.Load<AsepriteImportResult>("adventurer_duplicate_test");


            #region Other code not relavent to the video

            // TODO: use this.Content to load your game content here
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });

            _font = Content.Load<SpriteFont>("font");

            string filepath = Environment.CurrentDirectory + @"\adventurer_duplicate_test.aseprite";

            ContentPipeline.AsepriteImporter importer = new ContentPipeline.AsepriteImporter();
            ContentPipeline.Processors.Animation.AnimationProcessor processor = new ContentPipeline.Processors.Animation.AnimationProcessor();

            processor.SheetType = ContentPipeline.Processors.Animation.ProcessorSheetType.Packed;
            processor.MergeDuplicateFrames = true;
            processor.OnlyVisibleLayers = true;
            processor.OutputSpriteSheet = @"C:\Users\Dart\Desktop\gamedev\output_sheet_10bp.png";
            processor.BorderPadding = 10;
            processor.Spacing = 0;
            processor.InnerPadding = 0;

            ContentPipeline.AsepriteWriter writer = new ContentPipeline.AsepriteWriter();

            ContentPipeline.AsepriteImporterResult importResult = importer.Import(filepath);

            ContentPipeline.Processors.Animation.AnimationProcessorResult processorResult = processor.Process(importResult);
            byte[] buffer = writer.Write(processorResult);

            _aseprite = new AsepriteContentTypeReader().Read(buffer, GraphicsDevice);

      

            if (_aseprite.Animations.Count > 0)
            {
                _animations = _aseprite.Animations.Values.ToArray();
                _currentAnimationIndex = 0;
                LoadAnimation(_currentAnimationIndex);
                //_currentAnimation = _aseprite.Animations.First().Value;
                //_frameTimer = _aseprite.Frames[_currentAnimation.From].Duration;
                //_currentFrame = _aseprite.Frames[_currentAnimation.From];
            }

            #endregion Other code not relavent to the video
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            _frameTimer -= deltaTime;

            if (_frameTimer <= 0)
            {
                _currentFrameIndex += 1;
                if (_currentFrameIndex > _currentAnimation.To)
                {
                    _currentFrameIndex = _currentAnimation.From;
                }

                _currentFrame = _aseprite.Frames[_currentFrameIndex];
                _frameTimer = _currentFrame.Duration;
            }

            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();

            if (WasScaleUpPressed())
            {
                _renderScale = Math.Min(_renderScale + 1.0f, 10.0f);
            }
            else if (WasScaleDownPressed())
            {
                _renderScale = Math.Max(_renderScale - 1.0f, 1.0f);
            }
            else if (WasRenderTypeChangePressed())
            {
                if (_renderType == RenderType.Animations)
                {
                    _renderType = RenderType.Texture;
                }
                else
                {
                    _renderType = RenderType.Animations;
                }
            }
            else if (WasNextAnimationPressed())
            {
                _currentAnimationIndex += 1;
                if (_currentAnimationIndex >= _animations.Length)
                {
                    _currentAnimationIndex = 0;
                }
                LoadAnimation(_currentAnimationIndex);

            }
            else if (WasPreviousAnimationPressed())
            {
                _currentAnimationIndex -= 1;
                if (_currentAnimationIndex < 0)
                {
                    _currentAnimationIndex = _animations.Length - 1;
                }
                LoadAnimation(_currentAnimationIndex);
            }
            else if (WasRenderSlicePressed())
            {
                _renderAnimationSlices = !_renderAnimationSlices;
            }



            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);


            for (int column = 0; column < _gridColumnCount; column++)
            {
                for (int row = 0; row < _gridRowCount; row++)
                {
                    _reusableRect.X = column * _gridColumnWidth;
                    _reusableRect.Y = row * _gridRowHeight;
                    _reusableRect.Width = _gridColumnWidth;
                    _reusableRect.Height = _gridRowHeight;

                    //Rectangle rect = new Rectangle()
                    //{
                    //    X = column * _gridColumnWidth,
                    //    Y = row * _gridRowHeight,
                    //    Width = _gridColumnWidth,
                    //    Height = _gridRowHeight
                    //};

                    Color color = new Color(192, 192, 192, 255);
                    if ((column % 2 == 0 && row % 2 == 0) || (column % 2 != 0 && row % 2 != 0))
                    {
                        color = new Color(128, 128, 128, 255);
                    }

                    _spriteBatch.Draw(_pixel, _reusableRect, color);
                }
            }

            if (_renderType == RenderType.Texture)
            {
                DrawTexture();
            }
            else if (_renderType == RenderType.Animations)
            {
                DrawAnimation();
            }

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DrawAnimation()
        {
            Vector2 pos = new Vector2()
            {
                X = _resolutionWidth / 2 - ((_currentFrame.Width * _renderScale) / 2),
                Y = _resolutionHeight / 2 - ((_currentFrame.Height * _renderScale) / 2)
            };

            _spriteBatch.Draw(
                texture: _aseprite.Texture,
                position: pos,
                sourceRectangle: new Rectangle(_currentFrame.X, _currentFrame.Y, _currentFrame.Width, _currentFrame.Height),
                color: Color.White,
                rotation: 0.0f,
                origin: Vector2.Zero,
                scale: new Vector2(_renderScale, _renderScale),
                effects: SpriteEffects.None,
                layerDepth: 0.0f);



            if (_renderAnimationSlices)
            {
                if (_aseprite.Slices.TryGetValue("hit-box", out AsepriteAnimationDocument.Slice slice))
                {
                    if (slice.SliceKeys.TryGetValue(_currentFrameIndex, out AsepriteAnimationDocument.SliceKey key))
                    {
                        int x = (int)pos.X + (key.X * (int)_renderScale);
                        int y = (int)pos.Y + (key.Y * (int)_renderScale);
                        int width = key.Width * (int)_renderScale;
                        int height = key.Height * (int)_renderScale;

                        _reusableRect.X = x;
                        _reusableRect.Y = y;
                        _reusableRect.Width = width;
                        _reusableRect.Height = _pixel.Height * (int)_renderScale;
                        _spriteBatch.Draw(_pixel, _reusableRect, slice.Color);

                        _reusableRect.X = x + width;
                        _reusableRect.Y = y;
                        _reusableRect.Width = _pixel.Width * (int)_renderScale;
                        _reusableRect.Height = height;
                        _spriteBatch.Draw(_pixel, _reusableRect, slice.Color);

                        _reusableRect.X = x;
                        _reusableRect.Y = y + height;
                        _reusableRect.Width = width;
                        _reusableRect.Height = _pixel.Height * (int)_renderScale;
                        _spriteBatch.Draw(_pixel, _reusableRect, slice.Color);

                        _reusableRect.X = x;
                        _reusableRect.Y = y;
                        _reusableRect.Width = _pixel.Width * (int)_renderScale;
                        _reusableRect.Height = height;
                        _spriteBatch.Draw(_pixel, _reusableRect, slice.Color);
                    }
                }
            }

            Vector2 nameSize = _font.MeasureString(_currentAnimation.Name);

            _reusableRect.Width = _resolutionWidth;
            _reusableRect.Height = (int)nameSize.Y + 20;
            _reusableRect.X = 0;
            _reusableRect.Y = _resolutionHeight - _reusableRect.Height;

            Vector2 namePos = new Vector2()
            {
                X = _reusableRect.Center.X,
                Y = _reusableRect.Center.Y
            };

            _spriteBatch.Draw(_pixel, _reusableRect, _currentAnimation.Color);

            _spriteBatch.DrawString(
                spriteFont: _font,
                text: _currentAnimation.Name,
                position: namePos,
                color: GetContrastColor(_currentAnimation.Color),
                rotation: 0.0f,
                origin: new Vector2(nameSize.X / 2.0f, nameSize.Y / 2.0f),
                scale: Vector2.One,
                effects: SpriteEffects.None,
                layerDepth: 0.0f);
        }

        private void DrawTexture()
        {
            Vector2 pos = new Vector2()
            {
                X = _resolutionWidth / 2 - ((_aseprite.Texture.Width * _renderScale) / 2),
                Y = _resolutionHeight / 2 - ((_aseprite.Texture.Height * _renderScale) / 2)
            };

            _spriteBatch.Draw(
                texture: _aseprite.Texture,
                position: pos,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0.0f,
                origin: Vector2.Zero,
                scale: new Vector2(_renderScale, _renderScale),
                effects: SpriteEffects.None,
                layerDepth: 0.0f);
        }


        private void LoadAnimation(int index)
        {
            _currentAnimation = _animations[index];
            _currentFrameIndex = _currentAnimation.From;
            _currentFrame = _aseprite.Frames[_currentFrameIndex];
            _frameTimer = _currentFrame.Duration;
        }


        private bool WasRenderSlicePressed()
        {
            return _currentKeyboard.IsKeyDown(Keys.Enter) && _previousKeyboard.IsKeyUp(Keys.Enter);
        }

        private bool WasScaleUpPressed()
        {
            return _currentKeyboard.IsKeyDown(Keys.Up) && _previousKeyboard.IsKeyUp(Keys.Up);
        }

        private bool WasScaleDownPressed()
        {
            return _currentKeyboard.IsKeyDown(Keys.Down) && _previousKeyboard.IsKeyUp(Keys.Down);
        }

        private bool WasRenderTypeChangePressed()
        {
            return _currentKeyboard.IsKeyDown(Keys.Space) && _previousKeyboard.IsKeyUp(Keys.Space);
        }

        private bool WasNextAnimationPressed()
        {
            return _currentKeyboard.IsKeyDown(Keys.Right) && _previousKeyboard.IsKeyUp(Keys.Right);
        }

        private bool WasPreviousAnimationPressed()
        {
            return _currentKeyboard.IsKeyDown(Keys.Left) && _previousKeyboard.IsKeyUp(Keys.Left);
        }

        private Color GetContrastColor(Color bgColor)
        {
            float lumLimit = 0.179f;

            if (CalculateLumiance(bgColor) > lumLimit)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        private float CalculateLumiance(Color color)
        {
            return 0.2126f * CalculateLight(color.R) + 0.7152f * CalculateLight(color.G) + 0.0722f * CalculateLight(color.B);
        }

        private float CalculateLight(int value)
        {
            float c = value / 255.0f;
            if (c <= 0.03928)
            {
                c = c / 12.92f;
            }
            else
            {
                c = (float)Math.Pow((c + 0.055) / 1.055, 2.4);
            }
            return c;
        }
    }
}
