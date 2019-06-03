using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    class BulletManager
    {
        // List of the bullets
        public List<CustomModel> Bullets = new List<CustomModel>();

        // The bullets model
        private Model bulletModel;

        // The bullets speed
        private float bulletSpeed = 10;

        // Scale of the bullet
        private float bulletScale = 0.3f;

        // Maximum distance bullet can travel before being removed
        private float maxDistance;

        // Constructor
        public BulletManager(Model bulletModel, float maxDistance)
        {
            // Set internal variables to ones supplied by constructor
            this.bulletModel = bulletModel;
            this.maxDistance = maxDistance;
        }

        // Fires a bullet. originModel is the model from which the bullet is fired
        public void Shoot(CustomModel originModel, Vector3 Velocity)
        {
            // Create a new bullet at the position of the model it is fired from
            CustomModel thisBullet = new CustomModel(
                bulletModel,
                originModel.Position,
                originModel.Rotation,
                Vector3.One * bulletScale,
                Vector3.Zero,
                Vector3.Zero,
                null);

            // Translate Vector3 rotation to rotation matrix
            Matrix Rotation = Matrix.CreateFromYawPitchRoll(
                thisBullet.Rotation.Y,
                thisBullet.Rotation.X,
                thisBullet.Rotation.Z);

            // Calculate new position of bullet
            thisBullet.Velocity += Vector3.Transform(Vector3.Backward, Rotation) * bulletSpeed;

            // Add it to the list
            Bullets.Add(thisBullet);

            // Play shoot sound
            SoundManager.Shoot.Play();
        }

        // Reset. Removes all bullets
        public void Reset()
        {
            Bullets.Clear();
        }

        // Update
        public void Update(GameTime gameTime, Vector3 sourcePosition)
        {
            // Run for each bullet
            foreach (CustomModel bullet in Bullets)
            {
                // Get distance
                float distance = Vector3.Distance(sourcePosition, bullet.Position);

                // Remove bullet and break if too far away
                if (distance > maxDistance)
                {
                    Bullets.Remove(bullet);
                    break;
                }

                // Update the bullet
                bullet.Update(gameTime);
            }
        }

        // Draw
        public void Draw(Camera camera)
        {
            // Draw each bullet
            foreach (CustomModel bullet in Bullets)
            {
                bullet.Draw(camera.View, camera.Projection, Vector3.Zero);
            }
        }
    }
}
