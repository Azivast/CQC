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
	
	// Constructor
        public CollisionsManager(PlayerShip player1Ship, PlayerShip player2Ship, AsteroidField asteroidField)
        {
	    // Update internal variables
            this.player1Ship = player1Ship;
            this.player2Ship = player2Ship;
            this.asteroidField = asteroidField;
        }

	// Update
        public void Update()
        {
            // Run all collision methods (Check all collisions)
            checkPlayerToPlayer();
            checkPlayerToBullet();
            checkPlayerToAsteroid();
            checkBulletToAsteroid();
            checkPlayersToArenaBounds();
        }
	
	// Check if players are outside the arena and if so kill said player
        private void checkPlayersToArenaBounds()
        {
            // Check if player 1 is outside arena bounds
            if (!player1Ship.Model.BoundingBoxHit.Intersects(arenaBounds))
            {	
		// Let player class know that player is out of bounds
                player1Ship.OutOfBounds = true;
            }
            else
            {	
		// Let player class know that player is within bounds
                player1Ship.OutOfBounds = false;
		// Reset the time spent out of bounds
                player1Ship.ResetBoundsTimer();
            }


            // Check if player 2 is outside the arena and if so kill the player
            if (!player2Ship.Model.BoundingBoxHit.Intersects(arenaBounds))
            {
		// Let player class know that player is out of bounds
                player2Ship.OutOfBounds = true;
            }
            else
            {
		// Let player class know that player is within bounds
                player2Ship.OutOfBounds = false;
		// Reset time spent out of bounds
                player2Ship.ResetBoundsTimer();
        }
	
	// Check if players are colliding with each other
        private void checkPlayerToPlayer()
        {
	    // Check if player's hitboxes are intersecting 
            if (player1Ship.Model.BoundingBoxHit.Intersects(player2Ship.Model.BoundingBoxHit))
            {
                // Invert ships' heading and damage bot ships
                player1Ship.SendOffCourseAndDamage();
                player2Ship.SendOffCourseAndDamage();
            }
        }

	// Check if a player is hit by a bullet
        private void checkPlayerToBullet()
        {
	    // Check collision between player 2 and each bullet fired by player 1
            foreach (CustomModel bullet in player1Ship.BulletManager.Bullets)
            {
                if (player2Ship.Model.BoundingBoxHit.Intersects(bullet.BoundingSphereHit))
                {
                    // Damage ship and give player 1 a kill if player 2 died
                    if (player2Ship.DamageShip())
                        player1Ship.Kills++;
                    // Renmove the bullet
                    player1Ship.BulletManager.Bullets.Remove(bullet);
		    // Break loop
                    break;
                }
            }
	    // Check for collisions between player 1 and each bullet fired by player 2
            foreach (CustomModel bullet in player2Ship.BulletManager.Bullets)
            {
                if (player1Ship.Model.BoundingBoxHit.Intersects(bullet.BoundingSphereHit))
                {
                    // Damage ship and give player 1 a kill if player 2 died
                    if (player1Ship.DamageShip())
                        player2Ship.Kills++;
                    // Renove the bullet
                    player2Ship.BulletManager.Bullets.Remove(bullet);
		    // Break loop
                    break;
                }
            }
        }

	// Check if player is colliding with an asteroid
        private void checkPlayerToAsteroid()
        {
	    // Check collision agains every asteroid
            foreach (CustomModel asteroid in asteroidField.Asteroids)
            {
		// Check if player is colliding
                if (asteroid.BoundingSphereHit.Intersects(player1Ship.Model.BoundingBoxHit))
                {	
		    // Invert player's heading and damage ship
                    player1Ship.SendOffCourse();
		    // Break loop
                    break;
                }
            }
	    // Check collision agains every asteroid
            foreach (CustomModel asteroid in asteroidField.Asteroids)
            {
		// Check if player is colliding
                if (asteroid.BoundingSphereHit.Intersects(player2Ship.Model.BoundingBoxHit))
                {	
		    // Invert player's heading and damage ship
                    player2Ship.SendOffCourse();
		    // Break loop
                    break;
                }
            }
        }
	
	// Check if a bullet hits an asteroid
        private void checkBulletToAsteroid()
        {
            // Check against each asteroid
            foreach (CustomModel asteroid in asteroidField.Asteroids)
            {	
		// Check against each bullet fired by player 1
                foreach (CustomModel bullet in player1Ship.BulletManager.Bullets)
                {
		    // If bullet hits asteroid
                    if (asteroid.BoundingSphereHit.Intersects(bullet.BoundingSphereHit))
                    {
                        // Renmove the bullet
                        player1Ship.BulletManager.Bullets.Remove(bullet);
			// Break loop
                        break;
                    }
                }
            }
            // Check against each asteroid
            foreach (CustomModel asteroid in asteroidField.Asteroids)
            {
		// Check against each bullet fired by player 2
                foreach (CustomModel bullet in player2Ship.BulletManager.Bullets)
                {
		    // If bullet and asteroid collides
                    if (asteroid.BoundingSphereHit.Intersects(bullet.BoundingSphereHit))
                    {
                        // Renmove the bullet
                        player2Ship.BulletManager.Bullets.Remove(bullet);
			// Break loop
                        break;
                    }
                }
            }
        }
    }
}
