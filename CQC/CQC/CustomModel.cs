using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CQC
{
    public class CustomModel
    {
        // Pos/Rot/Scale of model
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Matrix RotationAsMatrix {
            get { return Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z); }
        }

        public Vector3 Velocity { get; set; }
        public Vector3 RotationVelocity { get; set; }

        // World matrix
        private Matrix baseWorld;

        // The model
        public Model Model { get; set; }

        // Matrix with every vertex in model
        private Matrix[] modelTransforms;

        // Model's material
        public Material Material { get; set; }

        // Model's hitbox
        private BoundingSphere boundingSphere;
        private BoundingBox boundingBox;

        // Returns the  bounding sphere
        public BoundingSphere BoundingSphereHit
        {
            get
            {
                // No rotation need since it's a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

                // Calculate the bounding sphere
                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);

                // Return the transformed sphere
                return transformed;
            }
        }
        // Returns the  bounding sphere
        public BoundingBox BoundingBoxHit
        {
            get
            {
                buildBoundingBox();
                return boundingBox;
            }
        }

        // Returns world matrix
        public Matrix WorldMatrix
        {
            get
            {
                return baseWorld;
            }
        }

        // GraphicsDevice for primitive-based rendering
        private GraphicsDevice graphicsDevice;

        // Constructor
        public CustomModel(Model Model, Vector3 Position, Vector3 Rotation, Vector3 Scale, Vector3 Velocity, Vector3 RotationVelocity, GraphicsDevice graphicsDevice)
        {
            // Update model to one supplied by constructor
            this.Model = Model;

            // Update modelTransforms to a matrix array with length of number of parts in model
            modelTransforms = new Matrix[Model.Bones.Count];
            // Populate said array with the model's parts
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            // Update Pos/Rot/Scale variables to ones supplied by constructor
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;

            this.Velocity = Velocity;
            this.RotationVelocity = RotationVelocity;

            // Create a bounding sphere
            buildBoundingSphere();
            // Create a bounding box
            buildBoundingBox();

            // Update graphicsDevice to one supplied by constructor
            this.graphicsDevice = graphicsDevice;

            // Generate tags for the model
            generateTags();

            this.Material = new Material();
        }

        // Create bounding sphere for the model
        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);

                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }

            this.boundingSphere = sphere;
        }


        // Create bounding box for the model
        private void buildBoundingBox()
        {
            // Create variables to create min and max xyz values for the model
            Vector3 modelMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 modelMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix worldTransform = Matrix.CreateScale(Scale) *
                    Matrix.CreateTranslation(Position) * Matrix.CreateTranslation(Rotation);

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Loops through all vertices
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        // I'm not sure what "i + 1" and "i + 2" does by they offset the hitbox so they have been tweaked to better match the ships
                        //Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1] -2, vertexData[i]), worldTransform);


                        modelMin = Vector3.Min(modelMin, transformedPosition);
                        modelMax = Vector3.Max(modelMax, transformedPosition);
                    }
                }
            }

            // Return the finished bounding box
            this.boundingBox = new BoundingBox(modelMin, modelMax);
        }

        void setEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null)
                return;

            if (val is Vector3)
                effect.Parameters[paramName].SetValue((Vector3)val);
            else if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            else if(val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            else if(val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);
        }

        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;

                    // Copy the effect if necessary
                    if (CopyEffect)
                        toSet = effect.Clone();

                    MeshTag tag = ((MeshTag)part.Tag);

                    // If this ModelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                        setEffectParameter(toSet, "TextureEnabled", false);

                    // Set remaining parameters to the effect
                    setEffectParameter(toSet, "DiffuseColor", tag.Color);
                    setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);

                    part.Effect = toSet;
                }
        }

        private void generateTags()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor, effect.Texture, effect.SpecularPower);
                        part.Tag = tag;
                    }
                }
            }
        }

        // Store references to all of the model's current effects
        public void CacheEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    ((MeshTag)part.Tag).CachedEffect = part.Effect;
        }

        // Restore the effects referenced by the model's cache
        public void RestoreEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (((MeshTag)part.Tag).CachedEffect != null)
                        part.Effect = ((MeshTag)part.Tag).CachedEffect;
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Move model
            Position += Velocity;
            // Rotate model
            Rotation += RotationVelocity;
        }

        // Draw
        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            // Calculate World Matrix
            baseWorld = Matrix.CreateScale(Scale) * RotationAsMatrix * Matrix.CreateTranslation(Position);

            // Draw all meshes
            foreach (ModelMesh mesh in Model.Meshes)
            {
                // Calculate each mesh's world matrix
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Set material effect from mesh
                    Effect effect = meshPart.Effect;

                    // If effect is BasicEffect use BasicEffect
                    if (effect is BasicEffect)
                    {
                        // Set the world view and projection matrices to the effect
                        ((BasicEffect)effect).World = localWorld;
                        ((BasicEffect)effect).View = View;
                        ((BasicEffect)effect).Projection = Projection;

                        // Use default lighting
                        ((BasicEffect)effect).EnableDefaultLighting();
                    }

                    // Else use effect as material
                    else
                    {
                        setEffectParameter(effect, "World", localWorld);
                        setEffectParameter(effect, "View", View);
                        setEffectParameter(effect, "Projection", Projection);
                        setEffectParameter(effect, "CameraPosition", CameraPosition);

                        Material.SetEffectParameters(effect);
                    }
                }

                // Draw the mesh
                mesh.Draw();
            }

        }
    }

    // Stores information of each polygon in the model
    public class MeshTag
    {
        public Vector3 Color;
        public Texture2D Texture;
        public float SpecularPower;
        public Effect CachedEffect = null;

        public MeshTag(Vector3 Color, Texture2D Texture, float SpecularPower)
        {
            this.Color = Color;
            this.Texture = Texture;
            this.SpecularPower = SpecularPower;
        }
    }
}
