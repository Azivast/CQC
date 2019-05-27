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
    static class Tutorial
    {
        private static Camera camera;

        private static SpriteFont menuFont;
        private static Texture2D scoreboard;
        private static Texture2D controller;
        private static Texture2D bg;

        private static ButtonManager buttonManager;

        public static void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            // Load camera
            camera = new ArcBallCamera(new Vector3(0, 0, 0), 0, 0.2f, 0, MathHelper.PiOver2, 4000, 1000, 4000, graphics, 16 / 9f);

            // Load font
            menuFont = Content.Load<SpriteFont>("Fonts/digitaldream2");

            // load textures
            scoreboard = Content.Load<Texture2D>("Textures/Scoreboard");
            controller = Content.Load<Texture2D>("Textures/controls");
            bg = Content.Load<Texture2D>("Textures/transparentBlack");

            // Set up buttonManager
            buttonManager = new ButtonManager(new Vector2(640, 660), menuFont);
            // Add buttons
            buttonManager.AddButton("press A to return to main menu");
        }
        public static void Update()
        {
            // Rotate camera
            ((ArcBallCamera)camera).RotationX += 0.01f;
            //((ArcBallCamera)camera).RotationY += 0.005f;

            // Update the camera
            camera.Update();


            // Check controller input 1
            if (InputManager.IsTapped(Buttons.A, PlayerIndex.One))
            {
                Game1.gameState = Game1.GameState.MainMenu;
            }


            // Check controller input 2
            if (InputManager.IsTapped(Buttons.A, PlayerIndex.Two))
            {
                Game1.gameState = Game1.GameState.MainMenu;
            }
        }
        public static void Draw(PlayerShip player1, PlayerShip player2, SkyBox skyBox, SpriteBatch spriteBatch)
        {
            // Draw skybox
            skyBox.Draw(camera.View, camera.Projection, ((ArcBallCamera)camera).Position);


            // Draw menu items
            spriteBatch.Begin();

            // Draw transparent background
            spriteBatch.Draw(bg, new Rectangle(0, 0, 1920, 1080), Color.White);

            // Draw controller
            spriteBatch.Draw(controller, new Rectangle(344, 50, controller.Width, controller.Height), Color.White);

            spriteBatch.DrawString(menuFont, "Watch your shields and don't let your hull reach 0%", new Vector2(260, 450), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Avoid collisions", new Vector2(510, 490), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "First player to gain " + Game1.WinLimit.ToString() + " kills wins", new Vector2(397, 530), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Press F To toggle fullscreen", new Vector2(420, 570), new Color(224, 96, 26));

            // controller scheme
            spriteBatch.DrawString(menuFont, "Thrust/Strafe", new Vector2(120, 167), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Ascend", new Vector2(235, 95), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, "Descend", new Vector2(950, 95), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Fire", new Vector2(950, 67), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Brake", new Vector2(950, 167), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Menu select", new Vector2(950, 197), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Pitch/Yaw", new Vector2(950, 232), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Return to main menu", new Vector2(950, 40), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Menu up", new Vector2(217, 220), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, "Menu down", new Vector2(190, 257), new Color(224, 96, 26));


            // Draw buttons
            buttonManager.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
