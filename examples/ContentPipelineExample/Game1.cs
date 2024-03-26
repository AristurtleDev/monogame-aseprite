// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;

namespace ContentPipelineExample;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private AnimatedSprite _animatedSprite;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Load the Aseprite file using the Content Manager
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        AsepriteFile aseFile = Content.Load<AsepriteFile>("character_robot");

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Do something with i (see the other examples for more information
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        SpriteSheet sheet = aseFile.CreateSpriteSheet(GraphicsDevice);
        _animatedSprite = sheet.CreateAnimatedSprite("walk");
        _animatedSprite.Play();
    }

    protected override void Update(GameTime gameTime)
    {
        _animatedSprite.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_animatedSprite, new Vector2(10, 10));
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
