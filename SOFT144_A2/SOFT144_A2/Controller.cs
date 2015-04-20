#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace SOFT144_A2
{
    class Controller
    {
        private Player player;
        private Companion companion;

        public Controller(Player player, Companion companion)
        {
            this.player = player;
            this.companion = companion;
        }

        public void Update(GameTime gameTime)
        {
            #region flags
            bool up = false;
            bool down = false;
            bool right = false;
            bool left = false;
            #endregion

            #region movement
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W)) up = true;
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.S)) down = true;
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A)) left = true;
            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D)) right = true;
            if (up && !(right | left)) { player.moveForwards(); companion.moveForwards(); }
            if (down && !(right | left)) { player.moveBackwards(); companion.moveBackwards(); }
            if (left && !(up | down)) { player.moveLeft(); companion.moveLeft(); }
            if (right && !(up | down)) { player.moveRight(); companion.moveRight(); }
            #endregion
        }
    }
}
