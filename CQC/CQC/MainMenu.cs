using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CQC
{
    // Class with everything in main menu
    static class MainMenu
    {
	// Self documenting variables
        private static CustomModel ship;
        private static Camera camera;
        private static SpriteFont menuFont;
        private static Texture2D logo;
	// Background
        private static Texture2D bg;
        private static ButtonManager buttonManager;


	// Load all content used by main menu
        public static void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            // Load ship
            ship = new CustomModel(Content.Load<Model>("Models/Ship"), Vector3.Zero, Vector3.Zero, new Vector3(100f), Vector3.Zero, Vector3.Zero, graphics);
            // Apply the ship effect & material from game1 to ship.
	    ship.SetModelEffect(Game1.SimpleEffect, true);
            ship.Material = Game1.ShipMaterial;

            // Load camera
            camera = new ArcBallCamera(ship.Position + new Vector3(0, 0, 100), 0, 0.2f, 0, MathHelper.PiOver2, 4000, 1000, 4000, graphics, 16 / 9f);

            // Load font
            menuFont = Content.Load<SpriteFont>("Fonts/digitaldream2");

            // Load textures
            logo = Content.Load<Texture2D>("Textures/logo");
            bg = Content.Load<Texture2D>("Textures/transparentBlack");

            // Set up buttonmanager
            buttonManager = new ButtonManager(new Vector2(640, 450), menuFont);
            // Add buttons
            buttonManager.AddButton("Play");
            buttonManager.AddButton("Tutorial");
            buttonManager.AddButton("Credits");
            buttonManager.AddButton("Exit");

        }
	// Update
        public static void Update()
        {
            // Rotate camera
            ((ArcBallCamera)camera).RotationX += 0.01f;
            //((ArcBallCamera)camera).RotationY += 0.005f;

            // Update the camera
            camera.Update();

            // Check if controller 1 presses A
            if (InputManager.IsTapped(Buttons.A, PlayerIndex.One))
            {
                // Change gamestate depending on  which button is selected
                switch (buttonManager.SelectedButton)
                {
                    case 0:
                        Game1.gameState = Game1.GameState.PrePlaying;
                        break;
                    case 1:
                        Game1.gameState = Game1.GameState.Tutorial;
                        break;
                    case 2:
                        Game1.gameState = Game1.GameState.Credits;
                        break;
                    case 3:
                        Game1.gameState = Game1.GameState.Exit;
                        break;
                }
            }

	    // Get dpad input from controller 1 and change selected input depending on input.
            if (InputManager.IsTapped(Buttons.DPadUp, PlayerIndex.One))
            {
                buttonManager.PrevButton();
            }
            else if (InputManager.IsTapped(Buttons.DPadDown, PlayerIndex.One))
            {
                buttonManager.NextButton();
            }

	    // Get left thumbstick input form controller 1 and change selected button depending on input
            if (InputManager.GamePad1.ThumbSticks.Left.Y > 0.5f && InputManager.PrevGamePad1.ThumbSticks.Left.Y < 0.5f)
            {
                buttonManager.PrevButton();
            }
            else if (InputManager.GamePad1.ThumbSticks.Left.Y <= -0.5f && InputManager.PrevGamePad1.ThumbSticks.Left.Y > -0.5f)
            {
                buttonManager.NextButton();
            }

            
            // Check if controller 2 presses A
            if (InputManager.IsTapped(Buttons.A, PlayerIndex.Two))
            {
                // Change gamestate depending on which button is selected
                switch (buttonManager.SelectedButton)
                {
                    case 0:
                        Game1.gameState = Game1.GameState.PrePlaying;
                        break;
                    case 1:
                        Game1.gameState = Game1.GameState.Tutorial;
                        break;
                    case 2:
                        Game1.gameState = Game1.GameState.Credits;
                        break;
                    case 3:
                        Game1.gameState = Game1.GameState.Exit;
                        break;
                }
            }

	    // Get dpad input from controller 2 and change button depending on input 
            if (InputManager.IsTapped(Buttons.DPadUp, PlayerIndex.Two))
            {
                buttonManager.PrevButton();
            }
            else if (InputManager.IsTapped(Buttons.DPadDown, PlayerIndex.Two))
            {
                buttonManager.NextButton();
            }
	    // Get left thumbstick input from controller 2 and change button depending on input
            if (InputManager.GamePad2.ThumbSticks.Left.Y > 0.5f && InputManager.PrevGamePad2.ThumbSticks.Left.Y < 0.5f)
            {
                buttonManager.PrevButton();
            }
            else if (InputManager.GamePad2.ThumbSticks.Left.Y <= -0.5f && InputManager.PrevGamePad2.ThumbSticks.Left.Y > -0.5f)
            {
                buttonManager.NextButton();
            }
        }

	// Draw
        public static void Draw(SkyBox skyBox, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            // Draw skybox
            skyBox.Draw(camera.View, camera.Projection, ((ArcBallCamera)camera).Position);

            // Start SpriteBatch to draw 2D components
            spriteBatch.Begin();

            // Draw transparent background
            spriteBatch.Draw(bg, new Rectangle(0, 0, 1920, 1080), Color.White);

            // Draw logo
            spriteBatch.Draw(logo, new Rectangle(483, 20, logo.Width, logo.Height), Color.White);

            // Check if player1's controller is connected and write text accordingly 
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                spriteBatch.DrawString(menuFont, " Controller 1 Conmnected", new Vector2(450, 640), new Color(224, 96, 26));
            else
                spriteBatch.DrawString(menuFont, "Controller 1 Disconnected", new Vector2(440, 640), new Color(224, 30, 00));

            // Check if player2's controller is connected and write text accordingly 
            if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                spriteBatch.DrawString(menuFont, " Controller 2 Conmnected ", new Vector2(450, 680), new Color(224, 96, 26));
            else
                spriteBatch.DrawString(menuFont, "Controller 2 Disconnected", new Vector2(440, 680), new Color(224, 30, 00));


            // Draw buttons
            buttonManager.Draw(spriteBatch);

            spriteBatch.End();

            // Reset states changed by spritebatch to draw 3D correctly again
            graphics.BlendState = BlendState.Opaque;
            graphics.DepthStencilState = DepthStencilState.Default;

            // Draw ship
            ship.Draw(camera.View, camera.Projection, ((ArcBallCamera)camera).Position);
        }
    }
}
