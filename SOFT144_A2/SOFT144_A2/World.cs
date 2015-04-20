#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace SOFT144_A2
{
    class World
    {
        #region definitions
        private List<Entity> entityList = new List<Entity>();
        private List<Obstacle> obstacleList = new List<Obstacle>();
        private List<Ghost> ghostList = new List<Ghost>();
        private List<Collectable> collectableList = new List<Collectable>();
        private Texture2D darkCorners;

        private List<Entity> entityDeletionList = new List<Entity>();
        private Player player;
        private Companion companion;
        private Controller controller;
        private Random collectableRandom = new Random();
        private Random obstacleRandom = new Random();
        private Random ghostRandom = new Random();
        private int collectableSpawnDelay;
        private double collectableTimeLastSpawn = 0;
        private double obstacleSpawnDelay;
        private double obstacleTimeLastSpawn = 0;
        private int ghostSpawnDelay;
        private double ghostTimeLastSpawn = 0;
        public bool debugMode = false;
        private int debugChanged = 60;

        public int ObstacleCount { get; set; }
        private int maxObstacles = 7;

        public enum GameState : int
        {
            ALIVE = 0,
            DEAD = 1,
            WIN = 2
        };

        public GameState currentState;
        public static ContentManager content;
        #endregion

        public World(ContentManager Content)
        {
            content = Content;
            this.player = new Player(new Vector2(10, 0), new Vector2(0, 0), this);
            this.companion = new Companion(new Vector2(10, 0), new Vector2(0, 0), this);
            this.controller = new Controller(player, companion);

            entityList.Add(player);
            entityList.Add(companion);
        }

        public void LoadContent()
        {
            #region foreach
            foreach (var i in entityList)
            {
                i.LoadContent(content);
            }

            foreach (var i in ghostList)
            {
                i.LoadContent(content);
            }
            #endregion
            darkCorners = content.Load<Texture2D>("darkCorners");
        }

        //Update Method - Called from Game
        public void Update(GameTime gameTime)
        {
            if(this.getPlayer().getHealth() <= 0)
            {
                currentState = GameState.DEAD;
            }

            if(this.getPlayer().getHappiness() >= 5000)
            {
                currentState = GameState.WIN;
            }

            debugChanged++;
            if (Keyboard.GetState().IsKeyDown(Keys.F2) && debugChanged > 60)
            {
                debugMode = !debugMode;
                debugChanged = 0;
            }

            collectableSpawnDelay = collectableRandom.Next(6) + 2;
            obstacleSpawnDelay = 0.1 + (obstacleRandom.NextDouble() - 0.13);
            ghostSpawnDelay = ghostRandom.Next(4, 10);

            spawnCollectables(gameTime);
            spawnObstacles(gameTime);
            spawnGhosts(gameTime);

            controller.Update(gameTime);

            #region foreach
            foreach (var i in entityList)
            {
                i.Update(gameTime);
            }

            foreach (var i in collectableList)
            {
                i.Update(gameTime);
            }

            foreach (var i in ghostList)
            {
                i.Update(gameTime);
            }

            foreach (var i in obstacleList)
            {
                i.Update(gameTime);
            }
            #endregion
            RemoveEntities();
        }

        //Draw Method - Called from Game
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            #region foreach
            foreach (var i in entityList)
            {
                i.Draw(gameTime, spriteBatch);
            }

            foreach (var i in collectableList)
            {
                i.Draw(gameTime, spriteBatch);
            }

            foreach (var i in ghostList)
            {
                i.Draw(gameTime, spriteBatch);
            }

            foreach (var i in obstacleList)
            {
                i.Draw(gameTime, spriteBatch);
            }
            #endregion
            spriteBatch.Draw(darkCorners, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.DrawString(Game1.font, "Health: " + player.getHealth(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(Game1.font, "Love: " + player.getHappiness().ToString("00000000.##"), new Vector2(Game1.UiWindowWidth - 160, 0), Color.White);
            if (debugMode)
            {
                spriteBatch.DrawString(Game1.font, "Player Position: " + player.getCurrentPosition(), new Vector2(0, Game1.UiWindowHeight - 15), Color.White);
                spriteBatch.DrawString(Game1.font, "Dog position: " + companion.getCurrentPosition(), new Vector2(Game1.UiWindowWidth - 200, Game1.UiWindowHeight - 20), Color.White);
            }
        }

        public void MarkForDeletion(Entity remove)
        {
            entityDeletionList.Add(remove);
        }

        #region getters
        public List<Entity> getEntities()
        {
            return entityList;
        }

        public List<Collectable> getCollectables()
        {
            return collectableList;
        }

        public List<Ghost> getGhosts()
        {
            return ghostList;
        }

        public List<Obstacle> getObstacles()
        {
            return obstacleList;
        }

        public Player getPlayer()
        {
            return entityList[0] as Player;
        }

        public Companion getCompanion()
        {
            return entityList[1] as Companion;
        }

        public GameState getState()
        {
            return this.currentState;
        }

        #endregion

        public void UnloadContent()
        {
            content.Unload();
        }

        private void RemoveEntities()
        {
            for (var i = entityDeletionList.Count - 1; i >= 0; i--)
            {
                entityList.Remove(entityDeletionList[i]);
                entityDeletionList[i].UnloadContent();
                entityDeletionList.RemoveAt(i);
            }
            entityDeletionList.Clear();
        }

        #region spawn
        private void spawnCollectables(GameTime gameTime)
        {
            Collectable c = new Collectable(new Vector2(collectableRandom.Next(Game1.UiWindowHeight), collectableRandom.Next(Game1.UiWindowHeight)), new Vector2(0, 0), this, Util.random_member_of<Collectable.CollectableType>());
            collectableTimeLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((int)collectableTimeLastSpawn >= collectableSpawnDelay)
            {
                collectableTimeLastSpawn -= collectableSpawnDelay;
                Console.WriteLine("Spawning pickup with delay of {0}", collectableSpawnDelay);
                c.LoadContent(content);
                Game1.spriteBatch.Begin();
                c.Draw(gameTime, Game1.spriteBatch);
                Game1.spriteBatch.End();
                collectableList.Add(c);
            }
        }

        private void spawnGhosts(GameTime gameTime)
        {
            Ghost g = new Ghost(new Vector2(collectableRandom.Next(Game1.UiWindowHeight), collectableRandom.Next(Game1.UiWindowHeight)), new Vector2(0, 0), this, Util.random_member_of<Ghost.GhostType>());
            ghostTimeLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((int)ghostTimeLastSpawn >= ghostSpawnDelay)
            {
                ghostTimeLastSpawn -= ghostSpawnDelay;
                Console.WriteLine("Spawning ghost with delay of {0}", ghostSpawnDelay);
                g.LoadContent(content);
                Game1.spriteBatch.Begin();
                g.Draw(gameTime, Game1.spriteBatch);
                Game1.spriteBatch.End();
                ghostList.Add(g);
            }
        }

        private void spawnObstacles(GameTime gameTime)
        {
            if (ObstacleCount < maxObstacles)
            {
                Obstacle o = new Obstacle(new Vector2(obstacleRandom.Next(Game1.UiWindowHeight), obstacleRandom.Next(Game1.UiWindowHeight)), new Vector2(0, 0), this, Util.random_member_of<Entity.Direction>());
                if (!o.HitBox.Intersects(this.getPlayer().HitBox))
                {
                    obstacleTimeLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if ((int)obstacleTimeLastSpawn >= obstacleSpawnDelay)
                    {
                        obstacleTimeLastSpawn = 0;
                        Console.WriteLine("Spawning obstacle with delay of {0}", obstacleSpawnDelay);
                        o.LoadContent(content);
                        Game1.spriteBatch.Begin();
                        o.Draw(gameTime, Game1.spriteBatch);
                        Game1.spriteBatch.End();
                        obstacleList.Add(o);
                    }
                }
            }
        }
        #endregion
    }
}