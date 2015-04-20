#region using
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace SOFT144_A2
{
    class Collectable : Entity
    {
        #region definitions
        public enum CollectableType : int
        {
            HEALTH = 0, LOVE = 1
        };

        public int healthValue = 25;
        public int happinessValue = 50;

        public bool isPickedUp;

        protected CollectableType type;
        private SoundEffect pickUp;
        #endregion

        public Collectable(Vector2 currentPosition, Vector2 velocity, World world, CollectableType t)
            : base(currentPosition, velocity, world)
        {
            isPickedUp = false;
            size = new Vector2(16, 16);
            hitBox = new Rectangle((int)currentPosition.X, (int)currentPosition.Y, (int)size.X, (int)size.Y);
            this.type = t;
        }

        public Collectable(){}

        public override void LoadContent(ContentManager Content)
        {
            if (type == CollectableType.LOVE)
            {
                spriteFront = Content.Load<Texture2D>("dog_biscuit");
            }
            else if (type == CollectableType.HEALTH)
            {
                spriteFront = Content.Load<Texture2D>("heart");
            }
            animatedSprite = new AnimatedSprite(spriteFront, true);
            hitBoxSprite = Content.Load<Texture2D>("hitbox");
            pickUp = Content.Load<SoundEffect>("pickup");
        }

        public override void UnloadContent()
        {
            Collectable c = this;
            c = null;
            animatedSprite = null;
            hitBox = new Rectangle();
        }

        public override void Update(GameTime gameTime)
        {
            if(animatedSprite != null)
            {
                animatedSprite.Update(gameTime);
                if (isPickedUp)
                {
                    this.UnloadContent();
                    this.isPickedUp = false;
                    pickUp.Play();
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(animatedSprite != null)
            {
                animatedSprite.Draw(spriteBatch, CurrentPosition);
            }
            if (world.debugMode)
            {
                //spriteBatch.Draw(hitBoxSprite, hitBox, Color.White);
            }
        }

        public CollectableType getType()
        {
            return this.type;
        }
    }
}
