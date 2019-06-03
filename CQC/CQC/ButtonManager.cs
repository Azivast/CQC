using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CQC
{
    class ButtonManager
    {
        // List of the text of each button
        public List<string> Buttons = new List<string>();
        // Coordinates for the screens' center
        private Vector2 centerPosition;
        // Font
        private SpriteFont font;
        // Margin between buttons
        public int buttonMargin = 40;
        // Color of the button text
        public Color textColor = new Color(224, 96, 26);
        // Currently selected button
        public int SelectedButton = 0;


        // Constructor
        public ButtonManager(Vector2 centerPosition, SpriteFont font)
        {
            // Update variables
            this.centerPosition = centerPosition;
            this.font = font;
        }

        // Function for selecting the next button
        public void NextButton()
        {
            // If not att the last button select the following/next button
            if (SelectedButton < Buttons.Count - 1)
            {
                SelectedButton++;
                //SoundManager.Select.Play();
            }
            // If at the last button select the first button
            else if (SelectedButton == Buttons.Count - 1)
            {
                SelectedButton = 0;
                //SoundManager.Select.Play();
            }
        }

        // Function for selecting the previous button
        public void PrevButton()
        {
            // If not at the first button select the previous button
            if (SelectedButton > 0)
            {
                SelectedButton--;
                //SoundManager.Select.Play();
            }
            // If at the first button select the last button
            else if (SelectedButton == 0)
            {
                SelectedButton = Buttons.Count - 1;
                //SoundManager.Select.Play();
            }
        }

        // Function for adding buttons
        public void AddButton(string text)
        {
            // Add the button to the list
            Buttons.Add(text);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // Run for each button
            for (int i = Buttons.Count - 1; i >= 0; i--)
            {
                // Get the size of the string
                Vector2 textSize = font.MeasureString(Buttons[i]);
                // Calculate the position of text centered on centerPosition
                Vector2 textPos = new Vector2(centerPosition.X - (textSize.X / 2), centerPosition.Y - (textSize.Y / 2) + (buttonMargin * i));

                // If a the button is selected draw it in white
                if (i == SelectedButton)
                {
                    spriteBatch.DrawString(font, Buttons[i], textPos, Color.White);
                }
                // Otherwise draw it with 'textColor'
                else
                {
                    spriteBatch.DrawString(font, Buttons[i], textPos, textColor);
                }
            }
        }
    }
}
