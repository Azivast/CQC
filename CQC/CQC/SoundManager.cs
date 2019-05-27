using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    static class SoundManager
    {
        // Generic sounds
        public static SoundEffect Shoot;
        public static SoundEffect Hit;
        public static SoundEffect ShieldHit;
        public static SoundEffect Explosion;

        // Sounds for player 1
        public static SoundEffect ShieldsOffline1;
        public static SoundEffect ShieldsOnline1;
        public static SoundEffect TakingDamage1;

        // Sounds for player 2
        public static SoundEffect ShieldsOffline2;
        public static SoundEffect ShieldsOnline2;
        public static SoundEffect TakingDamage2;


        // Load all tiles for level
        public static void LoadContent(ContentManager Content)
        {
            // Load sound effects
            Shoot = Content.Load<SoundEffect>(@"Sounds/Shoot");
            Hit = Content.Load<SoundEffect>(@"Sounds/Hit");
            ShieldHit = Content.Load<SoundEffect>(@"Sounds/ShieldHit");
            Explosion = Content.Load<SoundEffect>(@"Sounds/Explosion");

            ShieldsOffline1 = Content.Load<SoundEffect>(@"Sounds/shieldsoffline1");
            ShieldsOnline1 = Content.Load<SoundEffect>(@"Sounds/shieldsonline1");
            ShieldsOffline2 = Content.Load<SoundEffect>(@"Sounds/shieldsoffline2");
            ShieldsOnline2 = Content.Load<SoundEffect>(@"Sounds/shieldsonline2");


            // Lower sound to something that doesn't blow out your ear drums
            SoundEffect.MasterVolume = 0.5f;
        }
        public static void ToggleSound()
        {
            if (SoundEffect.MasterVolume == 1f)
                SoundEffect.MasterVolume = 0;
            else
                SoundEffect.MasterVolume = 1;
        }
    }
}
