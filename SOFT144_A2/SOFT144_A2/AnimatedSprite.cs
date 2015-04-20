#region using
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace SOFT144_A2
{
    class AnimatedSprite
    {
        #region definitions
        Texture2D sprite;
        private int columns = 8;
        private int curFrame = 0;
        private int timeLast = 0;
        private int frameTime = 70;
        private bool isLooped;
        public bool hasFinished = false;
        #endregion

        public AnimatedSprite(Texture2D texture, bool loop)
        {
            sprite = texture;
            isLooped = loop;
        }

        public void Update(GameTime time)
        {
            timeLast += time.ElapsedGameTime.Milliseconds;
            if (timeLast > frameTime)
            {
                timeLast -= frameTime;
                curFrame++;
                if (curFrame == 8 && isLooped) curFrame = 0;
                else if (curFrame == 8) hasFinished = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = sprite.Width / columns;
            int row = (int)((float)curFrame / (float)columns);
            int column = curFrame % columns;

            spriteBatch.Draw(sprite, new Rectangle((int)location.X, (int)location.Y, width, sprite.Height), new Rectangle(width * column, sprite.Height * row, width, sprite.Height), Color.White);
        }

        public void setTexture(Texture2D texture)
        {
            this.sprite = texture;
        }
    }
}
