using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    public abstract class Camera
    {
        // View matrix
        Matrix view;
        // Projection Matrix
        Matrix projection;

        // Get/Set for Projection matrix
        public Matrix Projection
        {
            get { return projection; }
            protected set
            {
                projection = value;
                // Generate a new frustum
                generateFrustum();
            }
        }

        // Get/Set for View matrix
        public Matrix View
        {
            get { return view; }
            protected set
            {
                view = value;
                // Generate a new frustum
                generateFrustum();
            }
        }

        // Frustum (rendered area of the world)
        public BoundingFrustum Frustum { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        // Constructor
        public Camera(GraphicsDevice graphicsDevice, float aspectRatio)
        {
            // Set internal GraphicsDevice to one supplied by constructor
            GraphicsDevice = graphicsDevice;

            // Generate a new projection matrix
            generatePerspectiveProjectionMatrix(MathHelper.PiOver4, aspectRatio);
        }

        // Function for generating the projection matrix
        private void generatePerspectiveProjectionMatrix(float FieldOfView, float aspectRatio)
        {
            // Get presentation parameters
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            //// Calculate aspect ratio (not used)
            //else
            //{
            //    aspectRatio = ((float)pp.BackBufferWidth / 2) / (float)pp.BackBufferHeight;
            //}

            // Build perspective projection matrix
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000000.0f);
        }

        // Update
        public virtual void Update()
        {
        }

        // Function for generating frustum (rendered area of the world)
        private void generateFrustum()
        {
            // Merge view & projection matrices into viewProjection
            Matrix viewProjection = View * Projection;
            // Make frustum from viewProjection
            Frustum = new BoundingFrustum(viewProjection);
        }

        // Function for determining if sphere is in view
        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            // Check that sphere is completely outside frustum
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        // Function for determining if box is in view
        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            // Check that box is completely outside frustum
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
