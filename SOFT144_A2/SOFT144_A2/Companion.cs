#region using
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace SOFT144_A2
{
    class Companion : Entity
    {
        public Vector2 forwardPos = new Vector2(28, 74);
        public Vector2 backPos = new Vector2(22, -15);
        public Vector2 leftPos = new Vector2(64, 43);
        public Vector2 rightPos = new Vector2(-16, 43);
        private Texture2D spriteHearts;
        private AnimatedSprite dogParticle;
        private bool fed;

        public Companion(Vector2 currentPosition, Vector2 velocity, World world)
            : base(currentPosition, velocity, world)
        {
            CurrentPosition = currentPosition;
            size = new Vector2(16, 16);
            this.hitBox = new Rectangle((int)(CurrentPosition.X), (int)(CurrentPosition.Y), (int)size.X, (int)size.Y);
        }

        public override void Update(GameTime gameTime)
        {
            CurrentPosition = CurrentPosition + world.getPlayer().getVelocity();

            #region wrapping
            if (world.getPlayer().getNextPosition().Y > Game1.UiWindowHeight - 1)
            {
                nextPosition = backPos;
            }
            else if (world.getPlayer().getNextPosition().Y < 0)
            {
                nextPosition.Y = Game1.UiWindowHeight - 1;
            }
            else if (world.getPlayer().getNextPosition().X < 0)
            {
                nextPosition.X = Game1.UiWindowWidth;
            }
            else if (world.getPlayer().getNextPosition().X > Game1.UiWindowWidth)
            {
                nextPosition.X = 0;
            }
            #endregion

            if (animatedSprite != null) animatedSprite.Update(gameTime);
            if (fed) dogParticle.Update(gameTime);
            if (dogParticle.hasFinished)
            {
                fed = false;
                dogParticle = new AnimatedSprite(spriteHearts, false);
            }
            //this.hitBox = new Rectangle((int)(nextPosition.X + 5), (int)(nextPosition.Y + 10), (int)size.X, (int)size.Y);
            base.Update(gameTime);
            if (world.getPlayer().getDirection() == Direction.DOWN) moveBackwards();
            if (world.getPlayer().getDirection() == Direction.UP) moveForwards();
            if (world.getPlayer().getDirection() == Direction.LEFT) moveLeft();
            if (world.getPlayer().getDirection() == Direction.RIGHT) moveRight();
        }

        public override void LoadContent(ContentManager Content)
        {
            spriteFront = Content.Load<Texture2D>("dog_front");
            spriteBack = Content.Load<Texture2D>("dog_back");
            spriteLeft = Content.Load<Texture2D>("dog_left");
            spriteRight = Content.Load<Texture2D>("dog_right");
            spriteHearts = Content.Load<Texture2D>("dog_particle");
            //hitBoxSprite = Content.Load<Texture2D>("hitbox");
            animatedSprite = new AnimatedSprite(spriteFront, true);
            dogParticle = new AnimatedSprite(spriteHearts, false);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(hitBoxSprite, hitBox, Color.White);
            if (animatedSprite != null) animatedSprite.Draw(spriteBatch, CurrentPosition);
            if (fed) dogParticle.Draw(spriteBatch, CurrentPosition);
        }

        public void getFed()
        {
            fed = true;
        }

        public override void UnloadContent()
        {
            Companion c = this;
            c = null;
            animatedSprite = null;
            hitBox = new Rectangle();
        }

        #region getters
        public Vector2 getCurrentPosition()
        {
            return this.CurrentPosition;
        }

        public Vector2 getNextPosition()
        {
            return this.nextPosition;
        }
        #endregion

        #region setters
        public void setNextPosition(Vector2 nextPosition)
        {
            this.nextPosition = nextPosition;
        }
        #endregion

        #region Movement
        public void moveForwards()
        {
            //nextPosition.Y -= baseSpeed;
            if (!(velocity.X == 0 && velocity.Y == -baseSpeed) || world.getPlayer().hasExitedScreen())
            {
                this.CurrentPosition = world.getPlayer().getCurrentPosition();
                CurrentPosition += forwardPos;
                nextPosition = CurrentPosition;
                velocity.Y = -baseSpeed;
                velocity.X = 0;
                animatedSprite = new AnimatedSprite(spriteBack, true);
            }
        }
        public void moveBackwards()
        {
            if (!(velocity.X == 0 && velocity.Y == baseSpeed) || world.getPlayer().hasExitedScreen())
            {
                this.CurrentPosition = world.getPlayer().getCurrentPosition();
                CurrentPosition += backPos;
                nextPosition = CurrentPosition;
                velocity.Y = baseSpeed;
                velocity.X = 0;
                animatedSprite = new AnimatedSprite(spriteFront, true);
            }
        }
        public void moveRight()
        {
            if (!(velocity.X == baseSpeed && velocity.Y == 0) || world.getPlayer().hasExitedScreen())
            {
                this.CurrentPosition = world.getPlayer().getCurrentPosition();
                CurrentPosition += rightPos;
                nextPosition = CurrentPosition;
                velocity.X = +baseSpeed;
                velocity.Y = 0;
                //nextPosition.X += baseSpeed;
                animatedSprite = new AnimatedSprite(spriteRight, true);
            }
        }
        public void moveLeft()
        {
            //nextPosition.X -= baseSpeed;
            if (!(velocity.X == -baseSpeed && velocity.Y == 0) || world.getPlayer().hasExitedScreen())
            {
                this.CurrentPosition = world.getPlayer().getCurrentPosition();
                CurrentPosition += leftPos;
                nextPosition = CurrentPosition;
                velocity.X = -baseSpeed;
                velocity.Y = 0;
                animatedSprite = new AnimatedSprite(spriteLeft, true);
            }
        }
        #endregion
    }
}
