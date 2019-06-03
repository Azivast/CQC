using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    public class Material
    {
	// No use unless changed through inheritance
        public virtual void SetEffectParameters(Effect effect)
        {

        }
    }

    // LightningMaterial inherits everything from Material 
    public class LightningMaterial : Material
    {
	// Lightning variables
        public Vector3 AmbientColor { get; set; }
        public Vector3 LightDirection { get; set; }
        public Vector3 LightColor { get; set; }
        public Vector3 SpecularColor { get; set; }

	// Constructor
        public LightningMaterial()
        {
	    // Give lightning materials some values. Change as you see fit
            AmbientColor = new Vector3(.1f, .1f, .1f);
            LightDirection = new Vector3(1, 1, 1);
            LightColor = new Vector3(.9f, .9f, .9f);
            SpecularColor = new Vector3(1, 1, 1);
        }

        // Set the effect parameters to the ones supplied as long as they are not null
        public override void SetEffectParameters(Effect effect)
        {
            if (effect.Parameters["AmbientColor"] != null)
                effect.Parameters["AmbientColor"].SetValue(AmbientColor);

            if (effect.Parameters["LightDirection"] != null)
                effect.Parameters["LightDirection"].SetValue(LightDirection);

            if (effect.Parameters["LightColor"] != null)
                effect.Parameters["LightColor"].SetValue(LightDirection);

            if (effect.Parameters["SpecularColor"] != null)
                effect.Parameters["SpecularColor"].SetValue(SpecularColor);
        }
    }
}
