using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CQC
{
    static class InputManager
    {
        public static GamePadState GamePad1;
        public static GamePadState GamePad2;
        public static GamePadState PrevGamePad1;
        public static GamePadState PrevGamePad2;

        // Function to check if a key has just been pressed down, and was previously up
        public static bool IsTapped(Buttons button, PlayerIndex player)
        {
            if (player == PlayerIndex.One)
                return GamePad1.IsButtonDown(button) && PrevGamePad1.IsButtonUp(button);
            else if (player == PlayerIndex.One)
                return GamePad2.IsButtonDown(button) && PrevGamePad2.IsButtonUp(button);
            else return false;
        }

        // Update
        public static void Update()
        {
            PrevGamePad1 = GamePad1;
            PrevGamePad2 = GamePad2;
            GamePad1 = GamePad.GetState(PlayerIndex.One);
            GamePad2 = GamePad.GetState(PlayerIndex.Two);
        }
    }
}
