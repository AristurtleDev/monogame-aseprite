// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;

namespace TilemapExample;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Tilemap _tilemap;
    private Vector2 _scale;

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
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        AsepriteFile aseFile;
        using (Stream stream = TitleContainer.OpenStream("townmap.aseprite"))
        {
            aseFile = AsepriteFileLoader.FromStream("townmap", stream);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Create a tilemap from the file based on the frame the tilemap is on.
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _tilemap = aseFile.CreateTilemap(GraphicsDevice, frameIndex: 0);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Size the tilemap is created 1:1 with the size it is in Aseprite, we're going to create a scale factor here
        /// in this example to be the size of the game window.  
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _scale.X = _graphics.PreferredBackBufferWidth / (float)_tilemap[0].Width;
        _scale.Y = _graphics.PreferredBackBufferHeight / (float)_tilemap[0].Height;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);


        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Spritebatch extension are provided to draw the tilemap
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _spriteBatch.Draw(_tilemap, Vector2.Zero, Color.White, _scale, 0.0f);

        _spriteBatch.End();
    }
}
