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
    // Class handeling everything used in credits
    static class Credits
    {
        // Variables. Uncommented ones have self-documenting names
        private static Camera camera;
        private static SpriteFont menuFont;
        private static Texture2D logo;
        // Background
        private static Texture2D bg;
        private static ButtonManager buttonManager;

        // Load content used by credits menu
        public static void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            // Load camera
            camera = new ArcBallCamera(new Vector3(0, 0, 0), 0, 0.2f, 0, MathHelper.PiOver2, 4000, 1000, 4000, graphics, 16/9f);

            // Load font
            menuFont = Content.Load<SpriteFont>("Fonts/digitaldream2");
            // Load logo
            logo = Content.Load<Texture2D>("Textures/logo");
            // Load background texture
            bg = Content.Load<Texture2D>("Textures/transparentBlack");
            
            // Set up buttonManager
            buttonManager = new ButtonManager(new Vector2(640, 660), menuFont);
            // Add button
            buttonManager.AddButton("press A to return to main menu");
        }

        // Update
        public static void Update()
        {
            // Rotate camera
            ((ArcBallCamera)camera).RotationX += 0.01f;
            //((ArcBallCamera)camera).RotationY += 0.005f;

            // Update the camera
            camera.Update();


            // Return to main menu if controller 1 presses A
            if (InputManager.IsTapped(Buttons.A, PlayerIndex.One))
            {
                Game1.gameState = Game1.GameState.MainMenu;
            }


            // Return to main menu if controller 2 presses A
            if (InputManager.IsTapped(Buttons.A, PlayerIndex.Two))
            {
                Game1.gameState = Game1.GameState.MainMenu;
            }
        }
         
        // Draw
        public static void Draw(PlayerShip player1, PlayerShip player2, SkyBox skyBox, SpriteBatch spriteBatch)
        {
            // Draw skybox
            skyBox.Draw(camera.View, camera.Projection, ((ArcBallCamera)camera).Position);



            // Draw menu items

            spriteBatch.Begin();

            // Draw transparent background
            spriteBatch.Draw(bg, new Rectangle(0, 0, 1920, 1080), Color.White);

            // Draw logo
            spriteBatch.Draw(logo, new Rectangle(483, 20, logo.Width, logo.Height), Color.White);

            // Draw the actual credits
            spriteBatch.DrawString(menuFont, "Made by:", new Vector2(580, 320), new Color(224, 96, 26));
            // "}" Replaced with "É" in font texture
            spriteBatch.DrawString(menuFont, "Olle Astr}", new Vector2(560, 350), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Sound effects by:", new Vector2(515, 420), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, "https://www.zapsplat.com", new Vector2(460, 450), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Skybox Generate with:", new Vector2(485, 520), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, "Space Engine", new Vector2(545, 550), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, "http://spaceengine.org", new Vector2(470, 580), new Color(224, 96, 26));

            // Draw buttons
            buttonManager.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
