#region using
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace SOFT144_A2
{
    class Entity
    {
        #region definitions
        protected World world;
        protected Vector2 CurrentPosition;
        protected Vector2 nextPosition;
        protected Vector2 velocity;
        protected float baseSpeed = 4;
        protected Texture2D spriteFront;
        protected Texture2D spriteBack;
        protected Texture2D spriteLeft;
        protected Texture2D spriteRight;
        protected Texture2D hitBoxSprite;
        protected AnimatedSprite animatedSprite;
        protected Vector2 size = new Vector2(32, 32);
        protected Rectangle hitBox;

        public enum Direction : int
        {
            UP = 0,
            DOWN = 1,
            RIGHT = 2,
            LEFT = 3
        };

        protected Direction direction;

        protected ContentManager content;
        #endregion

        public Entity()
        {
        }

        public virtual void LoadContent(ContentManager Content)
        {
            this.content = World.content;
        }

        public Entity(Vector2 currentPosition, Vector2 velocity, World world)
        {
            this.CurrentPosition = currentPosition;
            this.nextPosition = currentPosition;
            this.velocity = velocity;
            this.world = world;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public virtual void UnloadContent()
        {
            Entity e = this;
            e = null;
            animatedSprite = null;
            hitBox = new Rectangle();
        }

        public Rectangle HitBox
        {
            get { return hitBox; }
            set { hitBox = value; }
        }
    }
}
