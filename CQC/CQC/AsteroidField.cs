using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    class AsteroidField
    {
        public List<CustomModel> Asteroids = new List<CustomModel>();

        // Size of asteroid field
        private int Width = 10000;
        private int Height = 10000;
        private int Length = 10000;
        // Number of asteroids to spawn
        private int numberOfAsteroids;
        // Model of the asteroid
        private Model asteroidModel;

        // Constructor
        public AsteroidField(Model asteroidModel, int numberOfAsteroids)
        {
            // Update internal variables to ones supplied by constructor
            this.numberOfAsteroids = numberOfAsteroids;
            this.asteroidModel = asteroidModel;

            // Spawn a predefined field
            // Can be replaced with "GenerateNewAsteroidField()" to randomize a new field
            SpawnPreDefinedAsteroidField();
        }

        // Generates a random asteroid field
        public void GenerateNewAsteroidField()
        {
            // Create a new random
            Random rnd = new Random();

            // Add as many asteroids as numberOfAsteroids is set to
            for (int i = 1; i <= numberOfAsteroids; i++)
            {
                // Generate a random position for asteroid
                Vector3 position = new Vector3(
                    rnd.Next(-Width, Width),
                    rnd.Next(-Height, Height),
                    rnd.Next(-Length, Length)
                    );

                // Create a custom model with the position
                CustomModel asteroid = new CustomModel(asteroidModel, position, Vector3.One, new Vector3(300, 300, 300), new Vector3(0.1f), Vector3.Zero, null);

                // Add the model to the Asteroids list
                Asteroids.Add(asteroid);
            }
        }

        // Spawn a predefined asteroid field
        private void SpawnPreDefinedAsteroidField()
        {
            // Add each asteroid to the Asteroids list with manual coordinates
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-150, 30, -30), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.001f, 0, 0), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(50, 40, 37), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0, -0.001f, 0), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(126, 1, 35), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0, 0.001f, 0), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-93, 10, 1), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0, 0, -0.0001f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(53, -43, 22), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0, -0.001f, 0), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(74, 15, 680), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(-0.002f, 0, 0), null));

            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-345, 115, -12), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.001f, 0.00f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(456, 0, 124), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.00f, -0.002f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(234, -306, 256), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(-0.01f, 0.00f, 0.00f), null));

            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(140, 115, -500), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, -0.001f, 0.00f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-140, -115, 5), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.00f, -0.001f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(140, 115, 5), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(-0.007f, 0.00f, 0.00f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(1424, 500, 1244), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.0012f, 0.00f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(140, -488, -125), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.00f, 0.0015f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(234, -789, 545), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.0008f, 0.00f, 0.00f), null));

            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(234, -78, 245), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.002f, 0.00f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(634, 567, 505), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.00f, 0.001f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(3234, -389, -345), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.0004f, 0.00f, 0.00f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-567, -189, -365), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, -0.006f, 0.00f), null));

            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(230, -1, 350), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.00f, -0.001f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-500, 100, -500), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.002f, 0.00f, 0.00f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-412, -123, 300), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, -0.0016f, 0.00f), null));
            
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-500, -50, 150), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, -0.0008f, 0.00f), null));
            Asteroids.Add(new CustomModel(asteroidModel, new Vector3(-20, -130, 400), Vector3.One, new Vector3(3, 3, 3), Vector3.Zero, new Vector3(0.00f, 0.00f, 0.0012f), null));
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Update each asteroid in the Asteroids list
            foreach (CustomModel asteroid in Asteroids)
            {
                asteroid.Update(gameTime);
            }
        }

        // Draw
        public void Draw(Camera camera)
        {
            // Draw each asteroid in the Asteroids list
            foreach (CustomModel asteroid in Asteroids)
            {
                asteroid.Draw(camera.View, camera.Projection, Vector3.Zero);
            }
        }
    }
}
