using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CQC
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Variables. Uncommented ones have self documenting names

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Lightning-related effect and material used by ship models.
        public static Effect SimpleEffect;
        public static LightningMaterial ShipMaterial;

        // Number of kills to win
        public static int WinLimit = 5;

        // The different gamestates
        public enum GameState
        {
            MainMenu, PrePlaying, Playing, Paused, GameOver, Credits, Tutorial, Exit
        };

        // Gamestate to start in
        public static GameState gameState = GameState.MainMenu;

        CollisionsManager collisionsManager;

        // Rendertargets for each player's part of the screen
        RenderTarget2D RenderTargetTop;
        RenderTarget2D RenderTargetBottom;
        // Third rendertarget used to scale fullscreen properly
        RenderTarget2D RenderTargetFullscreen;

        // Player ships
        PlayerShip player1Ship;
        PlayerShip player2Ship;
        // Bulletmanagers for the ships
        BulletManager player1bulletManager;
        BulletManager player2bulletManager;
        // Player cameras
        Camera cameraPlayer1;
        Camera cameraPlayer2;


        MouseState lastMouseState;

        SkyBox skyBox;
        AsteroidField asteroidField;

        // Font used by ship HUD. Accessible from all classes
        public static SpriteFont HudFont;

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

            // Set game resolution
            graphics.PreferredBackBufferWidth = 1270;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Render targets
            RenderTargetTop = new RenderTarget2D(
                GraphicsDevice,
                960,
                1080,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
                );
            RenderTargetBottom = new RenderTarget2D(
                GraphicsDevice,
                960,
                1080,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
                );
            RenderTargetFullscreen = new RenderTarget2D(
                GraphicsDevice,
                1920,
                1080,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
                );

            // Load lighting effect and set up material. Give material's color variables some values
            ShipMaterial = new LightningMaterial();
            ShipMaterial.AmbientColor = Color.Red.ToVector3() * .15f;
            ShipMaterial.LightColor = Color.White.ToVector3() * .85f;
            SimpleEffect = Content.Load<Effect>("Effects/LightingEffect");

            // SkyBox
            skyBox = new SkyBox(Content, GraphicsDevice, Content.Load<TextureCube>("Models/clouds"));

            // Static class menus
            MainMenu.LoadContent(Content, GraphicsDevice);
            GameOver.LoadContent(Content, GraphicsDevice);
            Credits.LoadContent(Content, GraphicsDevice);
            Tutorial.LoadContent(Content, GraphicsDevice);

            // Load Ship models for both players
            CustomModel ship1 = new CustomModel(Content.Load<Model>("Models/Ship"), Vector3.Zero, Vector3.Zero, new Vector3(1f), Vector3.Zero, Vector3.Zero, GraphicsDevice);
            CustomModel ship2 = new CustomModel(Content.Load<Model>("Models/Ship"), Vector3.Zero, Vector3.Zero, new Vector3(1f), Vector3.Zero, Vector3.Zero, GraphicsDevice);
            // Load cockpit models for both players
            CustomModel cockpit1 = new CustomModel(Content.Load<Model>("Models/Cockpit"), Vector3.Zero, Vector3.Zero, new Vector3(1f), Vector3.Zero, Vector3.Zero, GraphicsDevice);
            CustomModel cockpit2 = new CustomModel(Content.Load<Model>("Models/Cockpit"), Vector3.Zero, Vector3.Zero, new Vector3(1f), Vector3.Zero, Vector3.Zero, GraphicsDevice);

            // Set models' effect
            ship1.SetModelEffect(SimpleEffect, true);
            ship2.SetModelEffect(SimpleEffect, true);

            // Set models' material
            ship1.Material = ShipMaterial;
            ship2.Material = ShipMaterial;

            // BulletManagers
            player1bulletManager = new BulletManager(Content.Load<Model>("Models/bullet"), 3000);
            player2bulletManager = new BulletManager(Content.Load<Model>("Models/bullet"), 3000);
            // Create player ships and assign controllers
            player1Ship = new PlayerShip(ship1, cockpit1, player1bulletManager, Content.Load<Texture2D>("Textures/crosshair"), Content.Load<Texture2D>("Textures/marker"), Content.Load<Texture2D>("Textures/centerMarker1"), Content.Load<Texture2D>("Textures/centerMarker1"), new Vector3(0, 0, -150), new Vector3(0, 0, 0), PlayerIndex.One);
            player2Ship = new PlayerShip(ship2, cockpit2, player2bulletManager, Content.Load<Texture2D>("Textures/crosshair"), Content.Load<Texture2D>("Textures/marker"), Content.Load<Texture2D>("Textures/centerMarker1"), Content.Load<Texture2D>("Textures/centerMarker1"), new Vector3(0, 30, 150), new Vector3(0, MathHelper.Pi, 0), PlayerIndex.Two);
            // Player's cameras
            cameraPlayer1 = new CockpitCamera(new Vector3(0, 3.1f, -10), new Vector3(0, 3, 0), Vector3.Zero, GraphicsDevice, 8 / 9f);
            cameraPlayer2 = new CockpitCamera(new Vector3(0, 3, -10), new Vector3(0, 3, 0), Vector3.Zero, GraphicsDevice, 8 / 9f);

            // Load asteroid field
            asteroidField = new AsteroidField(Content.Load<Model>("Models/asteroid"), 10);

            // CollisionsManager
            collisionsManager = new CollisionsManager(player1Ship, player2Ship, asteroidField);

            // Load font used for HUD
            HudFont = Content.Load<SpriteFont>("Fonts/digitaldream");

            // Get mouseState to prevent potential error with variable beeing null
            lastMouseState = Mouse.GetState();

            // Load sounds
            SoundManager.LoadContent(Content);

            // Update input manager once before game start to prevent possible gamepad state == null error
            InputManager.Update();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Update camera for player1
        void updateCameraPlayer1(GameTime gameTime)
        {
            //Move the camera
            ((CockpitCamera)cameraPlayer1).Move(player1Ship.Model.Position, player1Ship.Model.Rotation);

            //Update the camera
            cameraPlayer1.Update();
        }
        // Update camera for player2
        void updateCameraPlayer2(GameTime gameTime)
        {
            //Move the camera
            ((CockpitCamera)cameraPlayer2).Move(player2Ship.Model.Position, player2Ship.Model.Rotation);

            //Update the camera
            cameraPlayer2.Update();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exit game when pressing Escape
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            // Toggle fullscreen when pressing F
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }
            // Update input manager for menus
            InputManager.Update();

            // Gamestate specific code
            switch (gameState)
            {
                case GameState.MainMenu:
                    MainMenu.Update();
                    break;

                case GameState.PrePlaying:
                    // Reset everything
                    player1Ship.Reset();
                    player2Ship.Reset();
                    player1bulletManager.Reset();
                    player2bulletManager.Reset();

                    // Move to playing
                    gameState = GameState.Playing;
                    break;

                case GameState.Playing:
                    // Update cameras
                    updateCameraPlayer1(gameTime);
                    updateCameraPlayer2(gameTime);

                    // Update player ships
                    player1Ship.Update(gameTime);
                    player2Ship.Update(gameTime);

                    // Update models
                    asteroidField.Update(gameTime);

                    // Check collisions
                    collisionsManager.Update();


                    // Check if a player has won and if so move to game over screen
                    if (player1Ship.Kills >= WinLimit || player2Ship.Kills >= WinLimit)
                        gameState = GameState.GameOver;
                    // Return to main menu when either controller presses start
                    if (InputManager.IsTapped(Buttons.Start, PlayerIndex.One) || InputManager.IsTapped(Buttons.Start, PlayerIndex.Two))
                        gameState = GameState.MainMenu;

                    break;

                case GameState.GameOver:
                    GameOver.Update();
                    break;
                case GameState.Credits:
                    Credits.Update();
                    break;
                case GameState.Tutorial:
                    Tutorial.Update();
                    break;
                case GameState.Exit:
                    this.Exit();
                    break;
            }

            // Window title
            Window.Title = "CQC";

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear render target
            GraphicsDevice.Clear(Color.Black);

            // Gamestate specific code
            switch (gameState)
            {
                case GameState.MainMenu:
                    // Draw main menu
                    MainMenu.Draw(skyBox, spriteBatch, GraphicsDevice);
                    break;

                case GameState.PrePlaying:
                    // Keep drawing main menu
                    MainMenu.Draw(skyBox, spriteBatch, GraphicsDevice);
                    break;

                case GameState.Playing:
                    // Draw plying
                    DrawPlaying(gameTime);
                    break;

                case GameState.Paused:
                    // Keep drawing playing
                    DrawPlaying(gameTime);
                    break;

                case GameState.GameOver:
                    // Draw game over
                    GameOver.Draw(player1Ship, player2Ship, skyBox, spriteBatch);
                    break;
                case GameState.Credits:
                    // Draw credits
                    Credits.Draw(player1Ship, player2Ship, skyBox, spriteBatch);
                    break;
                case GameState.Tutorial:
                    // Draw tutorial
                    Tutorial.Draw(player1Ship, player2Ship, skyBox, spriteBatch);
                    break;
            }
            base.Draw(gameTime);
        }

        // Draw everything for when playing
        void DrawPlaying(GameTime gameTime)
        {
            // !!!!! Reset render target !!!!!
            GraphicsDevice.SetRenderTarget(null);

            //// !!!!! Set render target to top !!!!!
            GraphicsDevice.SetRenderTarget(RenderTargetTop);

            // Clear render target
            GraphicsDevice.Clear(Color.Black);

            // Draw skybox
            skyBox.Draw(cameraPlayer1.View, cameraPlayer1.Projection, ((CockpitCamera)cameraPlayer1).Position);

            // Draw BulletManagers
            player1Ship.BulletManager.Draw(cameraPlayer1);
            player2Ship.BulletManager.Draw(cameraPlayer1);

            // Draw asteroids
            asteroidField.Draw(cameraPlayer1);

            // Draw each player
            player2Ship.DrawThirdPerson(cameraPlayer1);
            player1Ship.DrawFirstPerson(cameraPlayer1, spriteBatch, GraphicsDevice, player2Ship.Model);


            // !!!!! Set render target to bottom !!!!! 
            GraphicsDevice.SetRenderTarget(RenderTargetBottom);

            // Clear render target
            GraphicsDevice.Clear(Color.Black);

            // Draw skybox
            skyBox.Draw(cameraPlayer2.View, cameraPlayer2.Projection, ((CockpitCamera)cameraPlayer2).Position);

            // Draw BulletManagers
            player1Ship.BulletManager.Draw(cameraPlayer2);
            player2Ship.BulletManager.Draw(cameraPlayer2);

            // Draw asteroids
            asteroidField.Draw(cameraPlayer2);

            // Draw each player
            player1Ship.DrawThirdPerson(cameraPlayer2);
            player2Ship.DrawFirstPerson(cameraPlayer2, spriteBatch, GraphicsDevice, player1Ship.Model);

            // !!!!! Set render target to bottom !!!!! 
            GraphicsDevice.SetRenderTarget(RenderTargetFullscreen);

            // Draw both screens (render targets)
            spriteBatch.Begin();
            spriteBatch.Draw(RenderTargetTop, new Rectangle(0, 0, 960, 1080), Color.White);
            spriteBatch.Draw(RenderTargetBottom, new Rectangle(960, 0, 960, 1080), Color.White);

            spriteBatch.End();

            // !!!!! Reset render target !!!!!
            GraphicsDevice.SetRenderTarget(null);
            // Clear render target
            GraphicsDevice.Clear(Color.Black);

            // Draw the final rendertarget
            spriteBatch.Begin();
            spriteBatch.Draw(RenderTargetFullscreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
        }
    }
}
