using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tank
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tile;
        TileFactory tiles;

        entity Tank;

        public Game1()
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
            // TODO: Add your initialization logic here
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

            tile = Content.Load<Texture2D>("2dztest1");

            tiles = new TileFactory(tile, Content.Load<Texture2D>("entity"),
                this.Window.ClientBounds.Height, this.Window.ClientBounds.Width);

            //Tank = new entity(Content.Load<Texture2D>("entity"));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            Vector2 eup = new Vector2(0, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                eup += new Vector2(0, -1);
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                eup += new Vector2(0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                eup += new Vector2(-10, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                eup += new Vector2(10, 0);

            Vector2 up = new Vector2(0, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                up += new Vector2(0, -1);
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                up += new Vector2(0, 1);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                up += new Vector2(-10, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                up += new Vector2(10, 0);
            /*if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                up.X *= .1f;*/

            int hup = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                hup += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                hup -= 1;
            float sup = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.RightControl))
                sup += .01f;
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                sup -= .01f;

            tiles.Update(up, hup, sup, eup);

            //Tank.update(up);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            tiles.Draw(spriteBatch);
            //Tank.draw(spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
