// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;

namespace TextureAtlasExample;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private TextureAtlas _atlas;
    private Sprite _sprite1;
    private Sprite _sprite2;
    private Sprite _sprite3;

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
        /// Create a texture atlas from the aseprite file
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _atlas = aseFile.CreateTextureAtlas(GraphicsDevice);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Create the Sprites using the texture atlas and specifying the region index. The region index will the 
        /// the same as the frame index in Aseprite.
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _sprite1 = _atlas.CreateSprite(regionIndex: 0);
        _sprite2 = _atlas.CreateSprite(regionIndex: 1);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// You can also create sprites from the atlas using the region name.  Region names are generated automatically
        /// by the processor an follow the format of
        /// 
        /// "[filename] [frameIndex]"
        /// 
        /// So for this file "character_robot", if we wanted to use frame 2, we would do the following
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _sprite3 = _atlas.CreateSprite("character_robot 2");
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// SpriteBatch extension methods are provided to draw the sprites
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _spriteBatch.Draw(_sprite1, new Vector2(10, 10));
        _spriteBatch.Draw(_sprite2, new Vector2(_sprite1.Width, 10));
        _spriteBatch.Draw(_sprite3, new Vector2(_sprite1.Width * 2, 10));

        _spriteBatch.End();
    }
}
