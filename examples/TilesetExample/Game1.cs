// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;

namespace TilesetExample;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Tileset _tileset;
    private TextureRegion _greenBushTile;
    private TextureRegion _yellowBushTile;
    private TextureRegion _mushroomsTile;



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
        /// Create a tileset from the file based on the index of the tileset
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _tileset = aseFile.CreateTileset(GraphicsDevice, 0);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Create texture regions from the tiles in the tileset.   You just give it the index of the tile in the
        /// tileset.  You can use the GetTile() method of the this[] indexor method
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _yellowBushTile = _tileset.GetTile(28);
        _greenBushTile = _tileset[29];
        _mushroomsTile = _tileset[30];
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Spritebatch extension are provided to draw the tiles
        /// 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _spriteBatch.Draw(_greenBushTile, new Vector2(10, 10), Color.White);
        _spriteBatch.Draw(_yellowBushTile, new Vector2(10 + _tileset.TileWidth, 10), Color.White);
        _spriteBatch.Draw(_mushroomsTile, new Vector2(10 + _tileset.TileWidth * 2, 10), Color.White);

        _spriteBatch.End();
    }
}
