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
    static class GameOver
    {
        private static Camera camera;

        private static SpriteFont menuFont;
        private static Texture2D logo;
        private static Texture2D scoreboard;
        private static Texture2D bg;

        public static void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            // Load camera
            camera = new ArcBallCamera(new Vector3(0, 0, 0), 0, 0.2f, 0, MathHelper.PiOver2, 4000, 1000, 4000, graphics, 16/9f);

            bg = Content.Load<Texture2D>("Textures/transparentBlack");

            // Load font
            menuFont = Content.Load<SpriteFont>("Fonts/digitaldream2");
            logo = Content.Load<Texture2D>("Textures/logo");
            scoreboard = Content.Load<Texture2D>("Textures/Scoreboard");
        }
        public static void Update()
        {
            // Stop controller vibration
            GamePad.SetVibration(PlayerIndex.One, 0, 0);
            GamePad.SetVibration(PlayerIndex.Two, 0, 0);

            // Rotate camera
            ((ArcBallCamera)camera).RotationX += 0.01f;
            //((ArcBallCamera)camera).RotationY += 0.005f;

            // Update the camera
            camera.Update();

            // Check if A is pressed and if so start playing
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.A))
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

            // Draw logo
            spriteBatch.Draw(logo, new Rectangle(483, 20, logo.Width, logo.Height), Color.White);

            // Show who won
            if (player1.Kills >= Game1.WinLimit && player2.Kills >= Game1.WinLimit)
                spriteBatch.DrawString(menuFont, "It's a tie!", new Vector2(540, 360), new Color(224, 96, 26));
            else if (player1.Kills >= Game1.WinLimit)
                spriteBatch.DrawString(menuFont, "Player 1 Wins", new Vector2(535, 360), new Color(224, 96, 26));
            else if (player2.Kills >= Game1.WinLimit)
                spriteBatch.DrawString(menuFont, "Player 2 Wins", new Vector2(535, 360), new Color(224, 96, 26));


            // Stats/Scoreboard
            spriteBatch.DrawString(menuFont, "Stats:", new Vector2(420, 420), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, "Kills", new Vector2(580, 420), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, "Deaths", new Vector2(740, 420), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, "Player 1", new Vector2(420, 480), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, "Player 2", new Vector2(420, 540), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, player1.Kills.ToString(), new Vector2(610, 480), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, player1.Deaths.ToString(), new Vector2(770, 480), new Color(224, 96, 26));

            spriteBatch.DrawString(menuFont, player2.Kills.ToString(), new Vector2(610, 540), new Color(224, 96, 26));
            spriteBatch.DrawString(menuFont, player2.Deaths.ToString(), new Vector2(770, 540), new Color(224, 96, 26));

            // Draw scoreboard texture
            spriteBatch.Draw(scoreboard, new Vector2(419, 420), Color.White);


            spriteBatch.DrawString(menuFont, "Press A to return to main menu", new Vector2(400, 660), new Color(224, 96, 26));
            spriteBatch.End();
        }
    }
}
