using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    class FreeCamera : Camera
    {
        // Yaw and Pitch rotation
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        // Position of camera
        public Vector3 Position { get; set; }
        // Position of target
        public Vector3 Target { get; set; }

        // Velocity / movement translation of camera
        private Vector3 translation;

        // Constructor
        public FreeCamera(Vector3 Position, float Yaw, float Pitch, GraphicsDevice graphicsDevice, float AspectRatio) : base (graphicsDevice, AspectRatio)
        {
            // Update internal variables to ones supplied by constructor
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;

            // Reset movement translation / velocty
            translation = Vector3.Zero;
        }

        // Rotate
        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
        }

        // Move
        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }

        // Update
        public override void Update()
        {
            // Calculate rotation matrix from the yaw and pitch
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);


            // Rotate translation so that it is applied in the correct direction relative to the camera
            translation = Vector3.Transform(translation, rotation);
            // Move camera
            Position += translation;
            // Reset translation
            translation = Vector3.Zero;

            // Get the forward direction and calculate a new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            // Calculate "up" vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
