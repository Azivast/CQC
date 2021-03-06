﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    class CockpitCamera : Camera
    {
        // Position and target of camera
        public Vector3 Position { get; private set; }
        public Vector3 Target { get; private set; }

        // Position and rotation of object to follow
        public Vector3 FollowTargetPosition { get; private set; }
        public Vector3 FollowTargetRotation { get; private set; }

        // Offsets for position and target
        public Vector3 PositionOffset { get; set; }
        public Vector3 TargetOffset { get; set; }

        // Camera rotation relative to the target objects rotation
        public Vector3 RelativeCameraRotation { get; set; }

        // How stiffly the camera position should follow the target position
        float springiness = 0.99f;

        // Change springiness (see above)
        public float Springiness
        {
            get { return springiness; }
            // Keep value between 0 and 1
            set { springiness = MathHelper.Clamp(value, 0, 1); }
        }

        //Constructor
        public CockpitCamera(Vector3 PositionOffset, Vector3 TargetOffset, Vector3 RelativeCameraRotation, GraphicsDevice graphicsDevice, float AspectRatio) : base(graphicsDevice, AspectRatio)
        {
            // Update internal variables to ones supplied by constructor
            this.PositionOffset = PositionOffset;
            this.TargetOffset = TargetOffset;
            this.RelativeCameraRotation = RelativeCameraRotation;
        }

        // Move camera using object to follow's position and rotation
        public void Move(Vector3 NewFollowTargetPosition, Vector3 NewFollowTargetRotation)
        {
            this.FollowTargetPosition = NewFollowTargetPosition;
            this.FollowTargetRotation = NewFollowTargetRotation;
        }

        // Rotate camera
        public void Rotate(Vector3 RotationChange)
        {
            this.RelativeCameraRotation += RotationChange;
        }

        // Update
        public override void Update()
        {
            // Merge rotation of model and camera to check that the camera is positioned correctly relative to the target model
            Vector3 combinedRotation = FollowTargetRotation + RelativeCameraRotation;

            // Calculate rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(combinedRotation.Y, combinedRotation.X, combinedRotation.Z);

            // Calculate where camera should/wants to be
            Vector3 desiredPosition = FollowTargetPosition + Vector3.Transform(PositionOffset, rotation);

            // Set position to an interpolation between the current position and the desired position
            Position = Vector3.Lerp(Position, desiredPosition, Springiness);

            // Calculate the new target from rotation matrix
            Target = FollowTargetPosition + Vector3.Transform(TargetOffset, rotation);

            // Get up from rotation matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
