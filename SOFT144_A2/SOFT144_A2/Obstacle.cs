#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace SOFT144_A2
{
    class Obstacle : Entity
    {
        private Direction obsDirection;
        public Obstacle()
        {

        }

        public Obstacle(Vector2 currentPosition, Vector2 velocity, World world, Direction direction)
            : base(currentPosition, velocity, world)
        {
            this.baseSpeed = 2;
            obsDirection = direction;
            if (obsDirection == Direction.UP || obsDirection == Direction.DOWN)
            {
                size = new Vector2(128, 64);
            }
            if (obsDirection == Direction.RIGHT || obsDirection == Direction.LEFT)
            {
                size = new Vector2(64, 128);
            }
            hitBox = new Rectangle((int)currentPosition.X, (int)currentPosition.Y, (int)size.X, (int)size.Y);
        }

        public override void LoadContent(ContentManager Content)
        {
            spriteFront = Content.Load<Texture2D>("obstacle");
            hitBoxSprite = Content.Load<Texture2D>("hitbox");
        }

        public override void UnloadContent()
        {
            Obstacle o = this;
            o = null;
            animatedSprite = null;
            hitBox = new Rectangle();
        }

        public override void Update(GameTime gameTime)
        {
            if (obsDirection == Direction.UP || obsDirection == Direction.DOWN)
            {
                int mask = obsDirection == Direction.DOWN ? (int)-baseSpeed : (int)baseSpeed;
                nextPosition.X = CurrentPosition.X + mask;
                velocity.X = mask;
                velocity.Y = 0;
                if (nextPosition.X < 0)
                {
                    world.MarkForDeletion(this);
                    world.ObstacleCount--;
                }
                if (nextPosition.X > Game1.UiWindowWidth)
                {
                    world.MarkForDeletion(this);
                    world.ObstacleCount--;
                }
                hitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, (int)size.X, (int)size.Y);
                if (hitBox.Intersects(world.getPlayer().HitBox))
                {
                    world.getPlayer().setCurrentPosition(world.getPlayer().getCurrentPosition() + velocity);
                }
            }
            else if (obsDirection == Direction.RIGHT || obsDirection == Direction.LEFT)
            {
                int mask = obsDirection == Direction.LEFT ? (int)-baseSpeed : (int)baseSpeed;
                nextPosition.Y = CurrentPosition.Y + mask;
                velocity.Y = mask;
                velocity.X = 0;
                if (nextPosition.Y < 0)
                {
                    world.MarkForDeletion(this);
                    world.ObstacleCount--;
                }
                if (nextPosition.Y > Game1.UiWindowHeight - 1)
                {
                    world.MarkForDeletion(this);
                    world.ObstacleCount--;
                }
                hitBox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, (int)size.X, (int)size.Y);
                if (hitBox.Intersects(world.getPlayer().HitBox))
                {
                    world.getPlayer().setCurrentPosition(world.getPlayer().getCurrentPosition() + velocity);
                }
            }
            CurrentPosition = nextPosition;
            hitBox = new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y, (int)size.X, (int)size.Y);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (spriteFront != null) spriteBatch.Draw(spriteFront, new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y, (int)size.X, (int)size.Y), Color.White);
            if (world.debugMode) spriteBatch.Draw(hitBoxSprite, hitBox, Color.White);
        }

    }
}
