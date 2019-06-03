using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    public class ArcBallCamera : Camera
    {
        // Rotation around X and Y
        public float RotationX { get; set; }
        public float RotationY { get; set; }

        // Restrictions on Y rotation
        public float MinRotationY { get; set; }
        public float MaxRotationY { get; set; }

        // Distance between target and camera
        public float Distance { get; set; }

        // Restrictions on said distance
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }

        // Position of camera
        public Vector3 Position { get; private set; }
        // Position of target
        public Vector3 Target { get; set; }

        // Constructor (Inherits from Camera.cs)
        public ArcBallCamera(Vector3 Target,
            float RotationX, float RotationY, float MinRotationY, float MaxRotationY,
            float Distance, float MinDistance, float MaxDistance,
            GraphicsDevice graphicsDevice, float AspectRatio)
            : base(graphicsDevice, AspectRatio)
        {
            // Upate internal variables to ones supplied by constructor
            this.Target = Target;
            this.MinRotationY = MinRotationY;
            this.MaxRotationY = MaxRotationY;
            // Keep y rotation value within range of min/max values
            this.RotationY = MathHelper.Clamp(RotationY, MinRotationY, MaxRotationY);
            this.RotationX = RotationX;
            this.MinDistance = MinDistance;
            this.MaxDistance = MaxDistance;
            // Keep distance within range of min/max values
            this.Distance = MathHelper.Clamp(Distance, MinDistance, MaxDistance);
        }

        // Function for zooming / changing distance to target
        public void Move(float DistanceChange)
        {
            // Add change to distance
            this.Distance += DistanceChange;

            // Keep distance within range of min/max values
            this.Distance = MathHelper.Clamp(Distance, MinDistance, MaxDistance);
        }

        // Function for rotating around target
        public void Rotate(float RotationXChange, float RotationYChange)
        {
            // Do rotation
            this.RotationX += RotationXChange;
            this.RotationY += -RotationYChange;

            // Keep y rotation value within range of min/max values
            this.RotationY = MathHelper.Clamp(RotationY, MinRotationY, MaxRotationY);
        }

        // Function for changing cameras position
        public void Translate(Vector3 PositionChange)
        {
            this.Position += PositionChange;
        }

        // Update
        public override void Update()
        {
            // Calculate rotation matrix from the rotation value
            Matrix rotation = Matrix.CreateFromYawPitchRoll(RotationX, -RotationY, 0);

            // Set movement translation to do to distance
            Vector3 translation = new Vector3(0, 0, Distance);
            // Rotate movement to allign with the camera
            translation = Vector3.Transform(translation, rotation);
            // Move the camera
            Position = Target + translation;

            // Calculate "up" vector from rotation matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate View matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
