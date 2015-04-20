#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace SOFT144_A2
{
    class Ghost : Entity
    {
        bool spottedPlayer = false;
        int damage = 100;
        float delay = 2f;
        double curTime = 0f;
        int sight = 200;
        private SoundEffect hit;

        protected enum GhostState : int
        {
            PASSIVE = 0,
            FOLLOWING = 1,
            ATTACKED = 2
        };

        public enum GhostType : int
        {
            AMBUSHER = 0,
            CHASER = 1
        };

        private GhostType type;
        private GhostState state = GhostState.PASSIVE;
        public Ghost(Vector2 currentPosition, Vector2 velocity, World world, GhostType type)
            : base(currentPosition, velocity, world)
        {
            CurrentPosition = currentPosition;
            this.size = new Vector2(35, 60);
            this.hitBox = new Rectangle((int)(CurrentPosition.X), (int)(CurrentPosition.Y), (int)size.X, (int)size.Y);
            this.baseSpeed = 2f;
            this.type = type;
        }

        public override void LoadContent(ContentManager Content)
        {
            spriteFront = Content.Load<Texture2D>("ambusher_neutral");
            spriteBack = Content.Load<Texture2D>("ambusher_alerted");
            hit = Content.Load<SoundEffect>("hurt");
            animatedSprite = new AnimatedSprite(spriteFront, true);
        }

        public override void UnloadContent()
        {
            Ghost g = this;
            g = null;
            animatedSprite = null;
            hitBox = new Rectangle();
            hit = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (state == GhostState.PASSIVE)
            {
                velocity = new Vector2(0, 0);
            }

            if (state == GhostState.ATTACKED)
            {
                spottedPlayer = false;
                Vector2 newVelocity = new Vector2(0, 0);
                Vector2 oldVelocity = velocity;
                velocity = newVelocity;
                curTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                sight = 0;
                Console.WriteLine(curTime);
                if (curTime >= delay)
                {
                    curTime -= delay;
                    sight = 200;
                    velocity = oldVelocity;
                    state = GhostState.PASSIVE;
                }
            }

            Vector2 playerPos = world.getPlayer().getCurrentPosition();
            if (animatedSprite != null) animatedSprite.Update(gameTime);
            spottedPlayer = false;
            float distance = Vector2.Distance(playerPos, CurrentPosition);

            if (distance < sight) spottedPlayer = true;

            else
            {
                spottedPlayer = false;
                if (state != GhostState.ATTACKED) state = GhostState.PASSIVE;
            }

            if (spottedPlayer)
            {
                state = GhostState.FOLLOWING;
                spottedPlayer = true;
                Double angle = Math.Atan2((playerPos.Y - CurrentPosition.Y), (playerPos.X - CurrentPosition.X));
                velocity.X = (float)(baseSpeed * Math.Cos(angle));
                velocity.Y = (float)(baseSpeed * Math.Sin(angle));
                CurrentPosition += velocity;
                if (distance < 20)
                {
                    world.getPlayer().setHealth(world.getPlayer().getHealth() - damage);
                    if (hit != null) hit.Play();
                    state = GhostState.ATTACKED;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(spriteFront, hitBox, Color.White);
            //if (spottedPlayer) {
            if (state == GhostState.FOLLOWING)
            {
                if (animatedSprite != null)
                {
                    animatedSprite.setTexture(spriteBack);
                    animatedSprite.Draw(spriteBatch, CurrentPosition);
                }
            }
            else
            {
                if (animatedSprite != null)
                {
                    animatedSprite.setTexture(spriteFront);
                    animatedSprite.Draw(spriteBatch, CurrentPosition);
                }
            }
            //}
        }
    }
}