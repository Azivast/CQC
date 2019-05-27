using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CQC
{
    class CollisionsManager
    {
        AsteroidField asteroidField;

        PlayerShip player1Ship;
        PlayerShip player2Ship;

        // Arena bounds
        private BoundingSphere arenaBounds = new BoundingSphere(Vector3.Zero, 900);

        public CollisionsManager(PlayerShip player1Ship, PlayerShip player2Ship, AsteroidField asteroidField)
        {
            this.player1Ship = player1Ship;
            this.player2Ship = player2Ship;
            this.asteroidField = asteroidField;
        }

        public void Update()
        {
            // Runs all collision methods
            checkPlayerToPlayer();
            checkPlayerToBullet();
            checkPlayerToAsteroid();
            checkBulletToAsteroid();
            checkPlayersToArenaBounds();
        }

        private void checkPlayersToArenaBounds()
        {
            // Player 1
            if (!player1Ship.Model.BoundingBoxHit.Intersects(arenaBounds))
            {
                player1Ship.OutOfBounds = true;
            }
            else
            {
                player1Ship.OutOfBounds = false;
                player1Ship.ResetBoundsTimer();
            }


            // Player 2
            if (!player2Ship.Model.BoundingBoxHit.Intersects(arenaBounds))
            {
                player2Ship.OutOfBounds = true;
            }
            else
            {
                player2Ship.OutOfBounds = false;
                player2Ship.ResetBoundsTimer();
            }
        }

        private void checkPlayerToPlayer()
        {
            if (player1Ship.Model.BoundingBoxHit.Intersects(player2Ship.Model.BoundingBoxHit))
            {
                // Invert ships' heading
                player1Ship.SendOffCourseAndDamage();
                player2Ship.SendOffCourseAndDamage();
            }
        }

        private void checkPlayerToBullet()
        {
            // Check collision for both players against bullets

            foreach (CustomModel bullet in player1Ship.BulletManager.Bullets)
            {
                if (player2Ship.Model.BoundingBoxHit.Intersects(bullet.BoundingSphereHit))
                {
                    // Damage ship and checks if player 1 got a kill
                    if (player2Ship.DamageShip())
                        player1Ship.Kills++;
                    // Renmove the bullet
                    player1Ship.BulletManager.Bullets.Remove(bullet);

                    break;
                }
            }
            foreach (CustomModel bullet in player2Ship.BulletManager.Bullets)
            {
                if (player1Ship.Model.BoundingBoxHit.Intersects(bullet.BoundingSphereHit))
                {
                    // Damage ship and checks if player 1 got a kill
                    if (player1Ship.DamageShip())
                        player2Ship.Kills++;
                    // Renove the bullet
                    player2Ship.BulletManager.Bullets.Remove(bullet);

                    break;
                }
            }
        }
        private void checkPlayerToAsteroid()
        {
            foreach (CustomModel asteroid in asteroidField.Asteroids)
            {
                if (asteroid.BoundingSphereHit.Intersects(player1Ship.Model.BoundingBoxHit))
                {
                    player1Ship.SendOffCourse();
                    break;
                }
            }
            foreach (CustomModel asteroid in asteroidField.Asteroids)
            {
                if (asteroid.BoundingSphereHit.Intersects(player2Ship.Model.BoundingBoxHit))
                {
                    player2Ship.SendOffCourse();
                    break;
                }
            }
        }
        private void checkBulletToAsteroid()
        {
            // Player1's bullets
            foreach (CustomModel asteroid in asteroidField.Asteroids)
            {
                foreach (CustomModel bullet in player1Ship.BulletManager.Bullets)
                {
                    if (asteroid.BoundingSphereHit.Intersects(bullet.BoundingSphereHit))
                    {
                        // Renmove the bullet
                        player1Ship.BulletManager.Bullets.Remove(bullet);
                        break;
                    }
                }
            }
            // Player2's bullets
            foreach (CustomModel asteroid in asteroidField.Asteroids)
            {
                foreach (CustomModel bullet in player2Ship.BulletManager.Bullets)
                {
                    if (asteroid.BoundingSphereHit.Intersects(bullet.BoundingSphereHit))
                    {
                        // Renmove the bullet
                        player2Ship.BulletManager.Bullets.Remove(bullet);
                        break;
                    }
                }
            }
        }
    }
}
