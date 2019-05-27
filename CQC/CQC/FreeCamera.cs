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

        private Vector3 translation;

        public FreeCamera(Vector3 Position, float Yaw, float Pitch, GraphicsDevice graphicsDevice, float AspectRatio) : base (graphicsDevice, AspectRatio)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;

            translation = Vector3.Zero;
        }

        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
        }

        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }

        // Update
        public override void Update()
        {
            // Calculate rotation matrix from the yaw and pitch
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);


            // Move camera and reset translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;

            // Calculate a new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            // Calculate "up" vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
