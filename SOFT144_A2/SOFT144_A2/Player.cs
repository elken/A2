#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace SOFT144_A2
{
    class Player : Entity
    {
        #region definitions
        private int health = 100;
        private long happiness = 0;
        private bool exitedScreen = false;
        private SoundEffect clearStage;
        private bool cleared = false;
        private Texture2D bark;
        private AnimatedSprite dogBark;

        protected Vector2 hitBoxOffset = new Vector2(25, 15);
        bool colCollision = false;
        bool entCollision = false;
        bool isAdded;
        #endregion

        public Player(Vector2 currentPosition, Vector2 velocity, World world)
            : base(currentPosition, velocity, world)
        {
            CurrentPosition = currentPosition;
            this.size = new Vector2(35, 60);
            this.hitBox = new Rectangle((int)(CurrentPosition.X + hitBoxOffset.X), (int)(CurrentPosition.Y + hitBoxOffset.Y), (int)size.X, (int)size.Y);
        }

        public override void Update(GameTime gameTime)
        {
            isAdded = false;
            exitedScreen = false;
            nextPosition = CurrentPosition + velocity;
            if (animatedSprite != null) animatedSprite.Update(gameTime);
            #region wrapping
            if (nextPosition.Y + 32 > Game1.UiWindowHeight - 1)
            {
                nextPosition.Y = -32;
                exitedScreen = true;
            }
            else if (nextPosition.Y + 32 < 0)
            {
                nextPosition.Y = Game1.UiWindowHeight - 32;
                exitedScreen = true;
            }
            else if (nextPosition.X + 32 < 0)
            {
                nextPosition.X = Game1.UiWindowWidth - 32;
                exitedScreen = true;
            }
            else if (nextPosition.X + 32 > Game1.UiWindowWidth)
            {
                nextPosition.X = -32;
                exitedScreen = true;
            }
            #endregion
            this.hitBox = new Rectangle((int)(nextPosition.X + hitBoxOffset.X), (int)(nextPosition.Y + hitBoxOffset.Y), (int)size.X, (int)size.Y);
            entityCollisionCheck();
            collectableCollisionCheck();
            base.Update(gameTime);
            world.getCompanion().moveRight();
            if (happiness % 500 != 0 && dogBark.hasFinished) cleared = false;
            if (cleared) dogBark.Update(gameTime);
            if (happiness % 500 == 0 && happiness != 0 && !cleared)
            {
                clearStage.Play();
                cleared = true;
                dogBark = new AnimatedSprite(bark, false);

                foreach (var i in world.getGhosts())
                {
                    world.MarkForDeletion(i);
                }
            }
        }

        public void collectableCollisionCheck()
        {
            Collectable c = new Collectable();
            {
                foreach (Collectable i in world.getCollectables())
                {
                    colCollision = HitBox.Intersects(i.HitBox);
                    if (colCollision)
                    {
                        c = i;
                        break;
                    }
                }
            }

            if (colCollision)
            {
                c.isPickedUp = true;
                if (c.getType() == Collectable.CollectableType.HEALTH)
                {
                    if ((this.health + c.healthValue) > 100)
                    {
                        this.health = 100;
                    }
                    else
                    {
                        this.health += c.healthValue;
                    }
                }
                else if (c.getType() == Collectable.CollectableType.LOVE)
                {
                    if (!isAdded)
                    {
                        this.happiness += c.happinessValue;
                        world.getCompanion().getFed();
                        isAdded = true;
                    }
                }
                else
                {
                    Console.WriteLine("Something went awry. Type is {0}", c.getType());
                }
            }
        }

        public void entityCollisionCheck()
        {
            this.hitBox = new Rectangle((int)(nextPosition.X + hitBoxOffset.X), (int)(nextPosition.Y + hitBoxOffset.Y), (int)size.X, (int)size.Y);
            foreach (var i in world.getObstacles())
            {
                entCollision = HitBox.Intersects(i.HitBox);
                if (entCollision) break;
            }

            if (entCollision)
            {
                nextPosition = CurrentPosition;
                if (direction == Direction.UP) moveBackwards();
                else if (direction == Direction.DOWN) moveForwards();
                else if (direction == Direction.RIGHT) moveLeft();
                else if (direction == Direction.LEFT) moveRight();
                this.hitBox = new Rectangle((int)(CurrentPosition.X + hitBoxOffset.X) - 1, (int)(CurrentPosition.Y + hitBoxOffset.Y), (int)size.X, (int)size.Y);
            }
            else
            {
                CurrentPosition = nextPosition;
            }
        }

        public override void LoadContent(ContentManager Content)
        {
            spriteFront = Content.Load<Texture2D>("player_front");
            spriteBack = Content.Load<Texture2D>("player_back");
            spriteLeft = Content.Load<Texture2D>("player_left");
            spriteRight = Content.Load<Texture2D>("player_right");
            hitBoxSprite = Content.Load<Texture2D>("hitbox");
            clearStage = Content.Load<SoundEffect>("clearStage");
            bark = Content.Load<Texture2D>("woof");
            animatedSprite = new AnimatedSprite(spriteFront, true);
            dogBark = new AnimatedSprite(bark, false);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (world.debugMode)
            {
                spriteBatch.Draw(hitBoxSprite, hitBox, Color.White);
            }

            if (cleared)
            {
                dogBark.Draw(spriteBatch, new Vector2(0, 0));
            }
            if (animatedSprite != null) animatedSprite.Draw(spriteBatch, CurrentPosition);
        }

        public override void UnloadContent()
        {
            Player p = this;
            p = null;
            animatedSprite = null;
            hitBox = new Rectangle();
        }

        #region getters
        public int getHealth()
        {
            return this.health;
        }

        public long getHappiness()
        {
            return this.happiness;
        }

        public Vector2 getCurrentPosition()
        {
            return this.CurrentPosition;
        }

        public void setCurrentPosition(Vector2 CurrentPosition)
        {
            this.CurrentPosition = CurrentPosition;
        }

        public Vector2 getNextPosition()
        {
            return this.nextPosition;
        }

        public Vector2 getVelocity()
        {
            return this.velocity;
        }

        public Direction getDirection()
        {
            return this.direction;
        }

        public void setHealth(int health)
        {
            this.health = health;
        }

        public bool hasExitedScreen()
        {
            return exitedScreen;
        }
        #endregion

        #region Movement
        public void moveForwards()
        {
            //nextPosition.Y -= baseSpeed;
            if (!(velocity.X == 0 && velocity.Y == -baseSpeed))
            {
                velocity.Y = -baseSpeed;
                velocity.X = 0;
                direction = Direction.UP;
                animatedSprite = new AnimatedSprite(spriteBack, true);
            }
        }
        public void moveBackwards()
        {
            //nextPosition.Y += baseSpeed;
            if (!(velocity.X == 0 && velocity.Y == baseSpeed))
            {
                velocity.Y = baseSpeed;
                velocity.X = 0;
                direction = Direction.DOWN;
                animatedSprite = new AnimatedSprite(spriteFront, true);
            }
        }
        public void moveRight()
        {
            if (!(velocity.X == baseSpeed && velocity.Y == 0))
            {
                velocity.X = +baseSpeed;
                velocity.Y = 0;
                direction = Direction.RIGHT;
                animatedSprite = new AnimatedSprite(spriteRight, true);
            }
        }
        public void moveLeft()
        {
            //nextPosition.X -= baseSpeed;
            if (!(velocity.X == -baseSpeed && velocity.Y == 0))
            {
                velocity.X = -baseSpeed;
                velocity.Y = 0;
                direction = Direction.LEFT;
                animatedSprite = new AnimatedSprite(spriteLeft, true);
            }
        }
        #endregion
    }
}
