// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;

namespace SpriteExample;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Sprite _sprite;

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
        /// Create a sprite from any frame in the aseprite file
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _sprite = aseFile.CreateSprite(GraphicsDevice, 0);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);


        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// SpriteBatch extension methods are provided to draw the sprite
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _spriteBatch.Draw(_sprite, new Vector2(10, 10));

        _spriteBatch.End();
    }
}
