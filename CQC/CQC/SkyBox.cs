using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    public class SkyBox
    {	
	// Model of skybox
        private CustomModel model;
	// Effect used by skybox
        private Effect effect;
	// GraphicsDevice
        private GraphicsDevice graphics;

	// Constructor
        public SkyBox(ContentManager Content, GraphicsDevice GraphicsDevice, TextureCube Texture)
        {
            {
		// Load skydbox model
                model = new CustomModel(Content.Load<Model>("Models/skysphere_mesh"), Vector3.Zero, Vector3.Zero, Vector3.One, Vector3.Zero, Vector3.Zero, GraphicsDevice);

		// Load effect and assign texture to effect
                effect = Content.Load<Effect>("Effects/skysphere_effect");
                effect.Parameters["CubeMap"].SetValue(Texture);
		// Assign the model the effect
                model.SetModelEffect(effect, false);

		// Update GraphicsDevice to one supplied by constructor
                this.graphics = GraphicsDevice;
            }
        }

	// Draw 
        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            // Disable the depth buffer
            graphics.DepthStencilState = DepthStencilState.None;

            // Move skybox to cameras position
            model.Position = CameraPosition;
	    // Draw the skybox
            model.Draw(View, Projection, CameraPosition);

	    // Enable the depth buffer
            graphics.DepthStencilState = DepthStencilState.Default;
        }
    }
}
