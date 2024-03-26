// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;

namespace SpritesheetExample;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SpriteSheet _spriteSheet;
    private AnimatedSprite _attackCycle;
    private AnimatedSprite _runCycle;
    private AnimatedSprite _walkCycle;

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
        /// Load the file. In this example, we're not using the MGCB/Content Pipeline and have the Aseprite file set as
        /// a file in our project that is copied the output directory.  Because of this, we can use the
        /// TitleContainer.OpenStream to get a stream to the file and use that to load it.
        /// 
        /// You can optionally enable/disable premultiply alpha for the color values when the file is loaded.  If not
        /// specified, it will default to true.
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        AsepriteFile aseFile;
        using (Stream stream = TitleContainer.OpenStream("character_robot.aseprite"))
        {
            aseFile = AsepriteFileLoader.FromStream("character_robot", stream, preMultiplyAlpha: true);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Create a sprite sheet from any frame in the aseprite file
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _spriteSheet = aseFile.CreateSpriteSheet(GraphicsDevice);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Create the animated sprites from the sprite sheet.
        /// Each animated sprite correlates to a tag from Aseprite.
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _walkCycle = _spriteSheet.CreateAnimatedSprite("walk");
        _runCycle = _spriteSheet.CreateAnimatedSprite("run");
        _attackCycle = _spriteSheet.CreateAnimatedSprite("attack");

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Tell the animated sprite to play.
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _walkCycle.Play();
        _runCycle.Play();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// You can even set a specific loop count when telling it to play.  Setting this will override the "Repeat"
        /// value that was set in Aseprite.
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _attackCycle.Play(loopCount: 3);
    }

    protected override void Update(GameTime gameTime)
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Animations need to be updated every frame
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _walkCycle.Update(gameTime);
        _runCycle.Update(gameTime);
        _attackCycle.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// SpriteBatch extension methods are provided to draw the animated sprites
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _spriteBatch.Draw(_attackCycle, new Vector2(10, 10));
        _spriteBatch.Draw(_walkCycle, new Vector2(_attackCycle.Width, 10));
        _spriteBatch.Draw(_runCycle, new Vector2(_attackCycle.Width * 2, 10));


        _spriteBatch.End();
    }
}
