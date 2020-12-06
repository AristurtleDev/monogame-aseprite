using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Demo.Utils;

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
                if (_sprite != null)
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
            AsepriteImportResult aseprite = content.Load<AsepriteImportResult>("sokoban_player");
            _sprite = new AnimatedSprite(aseprite, Position);
            _sprite.RenderDefinition.Scale = new Vector2(3.0f, 3.0f);
            //AnimationDefinition animationDefinition = content.Load<AnimationDefinition>("playerAnimation");
            //Texture2D texture = content.Load<Texture2D>("player");
            //_sprite = new AnimatedSprite(texture, animationDefinition, this.Position);

        }

        /// <summary>
        ///     Updates the player
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //  Update the sprite by passing in your GameTime object
            _sprite.Update(gameTime);

            // ------------------------------------------------------------------
            //  Can also pass in delta time if you're using that instead
            //  
            //  float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //  _sprite.Update(deltaTime);
            //
            // ------------------------------------------------------------------


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
                this._sprite.Play("walk_up");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //  Move down
                this._currentDirection = Vector2.UnitY;
                this.Position += this._currentDirection * _speed * delatTime;
                this._sprite.Play("walk_down");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //  Move left
                this._currentDirection = Vector2.UnitX * -1;
                this.Position += this._currentDirection * _speed * delatTime;
                this._sprite.Play("walk_left");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                //   Move right
                this._currentDirection = Vector2.UnitX;
                this.Position += this._currentDirection * _speed * delatTime;
                this._sprite.Play("walk_right");
            }
            else
            {
                //  No movement, so use the current direction value to set the idle animation
                if (this._currentDirection == Vector2.UnitY * -1) { this._sprite.Play("idle_up"); }
                else if (this._currentDirection == Vector2.UnitY) { this._sprite.Play("idle_down"); }
                else if (this._currentDirection == Vector2.UnitX * -1) { this._sprite.Play("idle_left"); }
                else if (this._currentDirection == Vector2.UnitX) { this._sprite.Play("idle_right"); }

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

            //  Get the slice rectangle for the current frame.  The name passed
            //  must be the same as the name of the slice in Aseprite
            var bodyHitBox = this._sprite.GetCurrentFrameSlice("body_hitbox");
            var hatHitBox = this._sprite.GetCurrentFrameSlice("hat_hitbox");
            //var footHitBox = this._sprite.GetCurrentFrameSlice("foot-on-ground-hitbox");

            //  Check if there is a value. We have to check since the return above is a nullable Rectangle.
            //  If it doesn't have a value, that means there was no SliceKey defined for the current frame of
            //  animation for the slice
            if (bodyHitBox.HasValue)
            {
                //  If you want to use the same colors you used in Aseprite for the color of the slice
                //  you can retrive that like this
                Color sliceColor = this._sprite.GetSliceColor("body_hitbox");
                spriteBatch.DrawHollowRectangle(bodyHitBox.Value, sliceColor);
            }

            if (hatHitBox.HasValue)
            {
                Color sliceColor = this._sprite.GetSliceColor("hat_hitbox");
                spriteBatch.DrawHollowRectangle(hatHitBox.Value, sliceColor);
            }

            //if (footHitBox.HasValue)
            //{
            //    Color sliceColor = this._sprite.GetSliceColor("foot-on-ground-hitbox");
            //    spriteBatch.DrawHollowRectangle(footHitBox.Value, sliceColor);
            //}
        }
    }
}
