#region using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace SOFT144_A2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        static public SpriteFont font;
        Texture2D bg;
        Texture2D deathScreen;
        Texture2D winScreen;
        private World world;
        static private readonly int windowWidth = 800;
        static private readonly int windowHeight = 600;

        static public int UiWindowWidth = windowWidth;
        static public int UiWindowHeight = windowHeight - 10;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            world = new World(Content);
            graphics.PreferredBackBufferWidth = UiWindowWidth;
            graphics.PreferredBackBufferHeight = UiWindowHeight;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bg = Content.Load<Texture2D>("background");
            font = Content.Load<SpriteFont>("Main");
            deathScreen = Content.Load<Texture2D>("DeathScreen");
            winScreen = Content.Load<Texture2D>("WinScreen");
            world.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (world.getState() == World.GameState.DEAD)
            {
                this.Draw(gameTime);
                base.UnloadContent();
            }
            else if (world.getState() == World.GameState.WIN)
            {
                this.Draw(gameTime);
                base.UnloadContent();
            }
            if (world.getState() == World.GameState.ALIVE)
            {
                base.Update(gameTime);
                world.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BlanchedAlmond);
            spriteBatch.Begin();
            spriteBatch.Draw(bg, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            if (world.getState() == World.GameState.DEAD) spriteBatch.Draw(deathScreen, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            if (world.getState() == World.GameState.WIN) spriteBatch.Draw(winScreen, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            if (world.getState() == World.GameState.ALIVE) world.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}