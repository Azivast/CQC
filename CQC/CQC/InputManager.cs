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
        // GamePad states for controller 1 and 2
        public static GamePadState GamePad1;
        public static GamePadState GamePad2;
        public static GamePadState PrevGamePad1;
        public static GamePadState PrevGamePad2;

        // Function to check if a key has just been pressed down, and was previously up
        public static bool IsTapped(Buttons button, PlayerIndex player)
        {
            // Check for player 1
            if (player == PlayerIndex.One)
                return GamePad1.IsButtonDown(button) && PrevGamePad1.IsButtonUp(button);
            // Check for player 2
            else if (player == PlayerIndex.Two)
                return GamePad2.IsButtonDown(button) && PrevGamePad2.IsButtonUp(button);
            // If neither controller 1 or 2 return flase
            else return false;
        }

        // Update
        public static void Update()
        {
            // Update gamepad states
            PrevGamePad1 = GamePad1;
            PrevGamePad2 = GamePad2;
            GamePad1 = GamePad.GetState(PlayerIndex.One);
            GamePad2 = GamePad.GetState(PlayerIndex.Two);
        }
    }
}
