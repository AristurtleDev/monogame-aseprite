//--------------------------------------------------------------------------------
//  Player
//
//  This is a simple player class that makes use of MonoGame.Aseprite.AnimatedSprite
//  in order to render the player with animations
//
//--------------------------------------------------------------------------------
//
//                              License
//  
//    Copyright(c) 2018 Chris Whitley
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in
//    all copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//    THE SOFTWARE.
//--------------------------------------------------------------------------------


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Aseprite.Demo
{
    public class Player
    {
        /// <summary>
        ///     The <see cref="AnimatedSprite"/> used for graphics for the player
        /// </summary>
        AnimatedSprite _sprite;

        /// <summary>
        ///     The current direction the player is moving
        /// </summary>
        Vector2 _currentDirection = Vector2.UnitY;
        


        /// <summary>
        ///     The xy-coordinate position of the player
        /// </summary>
        public Vector2 Position
        {
            get { return this._position; }
            set
            {
                if (this._position == value) { return; }
                this._position = value;
                if(_sprite != null)
                {
                    _sprite.Position = value;
                }
            }
        }
        private Vector2 _position;


        private float _speed = 150;

        /// <summary>
        ///     Creates a new player at the given position
        /// </summary>
        /// <param name="position">The position to start the player</param>
        public Player(Vector2 position)
        {
            this.Position = position;
        }


        /// <summary>
        ///     Loads the content for the player
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            AnimationDefinition animationDefinition = content.Load<AnimationDefinition>("playerAnimation");
            Texture2D texture = content.Load<Texture2D>("player");
            _sprite = new AnimatedSprite(texture, animationDefinition, this.Position);

        }

        /// <summary>
        ///     Updates the player
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);
            UpdateInput(gameTime);
        }

        /// <summary>
        ///     Handles updating input for the player and acting on input detected
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateInput(GameTime gameTime)
        {
            float delatTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //  Move up
                this._currentDirection = Vector2.UnitY * -1;
                this.Position += this._currentDirection * _speed * delatTime;
                this._sprite.Play("walk up");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //  Move down
                this._currentDirection = Vector2.UnitY;
                this.Position += this._currentDirection * _speed * delatTime;
                this._sprite.Play("walk down");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //  Move left
                this._currentDirection = Vector2.UnitX * -1;
                this.Position += this._currentDirection * _speed * delatTime;
                this._sprite.Play("walk left");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                //   Move right
                this._currentDirection = Vector2.UnitX;
                this.Position += this._currentDirection * _speed * delatTime;
                this._sprite.Play("walk right");
            }
            else
            {
                //  No movement, so use the current direction value to set the idle animation
                if (this._currentDirection == Vector2.UnitY * -1) { this._sprite.Play("idle up"); }
                else if (this._currentDirection == Vector2.UnitY) { this._sprite.Play("idle down"); }
                else if (this._currentDirection == Vector2.UnitX * -1) { this._sprite.Play("idle left"); }
                else if (this._currentDirection == Vector2.UnitX) { this._sprite.Play("idle right"); }

            }
        }

        /// <summary>
        ///     Renders the player to the screen
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Render(SpriteBatch spriteBatch)
        {
            this._sprite.Render(spriteBatch);
        }
    }
}
