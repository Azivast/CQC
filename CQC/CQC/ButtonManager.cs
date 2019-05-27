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
        public List<string> Buttons = new List<string>();
        private Vector2 centerPosition;
        private SpriteFont font;
        public int buttonMargin = 40;
        public Color textColor = new Color(224, 96, 26);

        public int SelectedButton = 0;


        // Constructor
        public ButtonManager(
            Vector2 centerPosition,
            SpriteFont font)
        {
            this.centerPosition = centerPosition;
            this.font = font;


        }

        public void NextButton()
        {
            if (SelectedButton < Buttons.Count - 1)
            {
                SelectedButton++;
                //SoundManager.Select.Play();
            }
            else if (SelectedButton == Buttons.Count - 1)
            {
                SelectedButton = 0;
                //SoundManager.Select.Play();
            }
        }

        public void PrevButton()
        {
            if (SelectedButton > 0)
            {
                SelectedButton--;
                //SoundManager.Select.Play();
            }
            else if (SelectedButton == 0)
            {
                SelectedButton = Buttons.Count - 1;
                //SoundManager.Select.Play();
            }
        }

        // Function for adding buttons
        public void AddButton(string text)
        {
            Buttons.Add(text);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = Buttons.Count - 1; i >= 0; i--)
            {
                // String size
                Vector2 textSize = font.MeasureString(Buttons[i]);
                // Position of text centered on centerPosition
                Vector2 textPos = new Vector2(centerPosition.X - (textSize.X / 2), centerPosition.Y - (textSize.Y / 2) + (buttonMargin * i));

                if (i == SelectedButton)
                {
                    spriteBatch.DrawString(font, Buttons[i], textPos, Color.White);
                }
                else
                {
                    spriteBatch.DrawString(font, Buttons[i], textPos, textColor);
                }
            }
        }
    }
}
