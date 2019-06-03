using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQC
{
    class PlayerShip
    {
        // 3rd person model
        private CustomModel model;
        // 1st person model
        private CustomModel cockpitModel;
        // Index of player (player number)
        private PlayerIndex playerIndex;

        // Crosshair texture
        private Texture2D crosshair;
        // Texture of enemy marker
        private Texture2D enemyMarker;
        // Textures of marker indicating center or world
        private Texture2D centerMarkerForward;
        private Texture2D centerMarkerBackward;

        // Bullet Manager
        private BulletManager bulletManager;

        // Ship velocity
        public Vector3 Velocity;

        // Position and rotatation at which ship spawns
        private Vector3 spawnPosition;
        private Vector3 spawnRotation;

        // Score variables
        public int Kills;
        public int Deaths;

        // Health and Shield variables
        public float Health;
        public float Shields;
        public float MaxHealth = 5;
        public float MaxShields = 5;
        public float timeSinceLastHit = 0;
        private bool shieldsOnline;


        // Maximum velocity in a given direction
        private float maxVelocity = 1.5f;

        // Timer for when to fire
        private float shootTimer = 0;

        // If player has left fighting area (is out of bounds)
        public bool OutOfBounds = false;
        // Time player can spend out of bounds before dying
        private float boundsTimerMax = 20;
        // Timer left out of bounds
        private float boundsTimer;


        // Public Get/Sets

        // Position
        public Vector3 Position
        {
            get { return model.Position; }
            set { model.Position = value; }
        }
        // Vector3 of model's current Rotation
        public Vector3 Rotation
        {
            get { return model.Rotation; }
        }

        public CustomModel Model
        {
            get { return model; }
            set { model = value; }
        }

        // Returns cockpit model at the same position and rotation as ship model
        public CustomModel CockpitModel
        {
            get { return new CustomModel(cockpitModel.Model, model.Position, model.Rotation, model.Scale, model.Velocity, model.RotationVelocity, null); }
            set { cockpitModel = value; }
        }

        public BulletManager BulletManager
        {
            get { return bulletManager; }
            set { bulletManager = value; }
        }

        // Constructor
        public PlayerShip(CustomModel model, CustomModel cockpitModel, BulletManager bulletManager, Texture2D crosshair, Texture2D enemyMarker, Texture2D centerMarkerForward, Texture2D centerMarkerBackward, Vector3 spawnPosition, Vector3 spawnRotation, PlayerIndex playerIndex)
        {
            // Update internal variables to ones supplied by constructor
            this.playerIndex = playerIndex;
            this.model = model;
            this.cockpitModel = cockpitModel;
            this.bulletManager = bulletManager;
            this.spawnPosition = spawnPosition;
            this.spawnRotation = spawnRotation;
            this.crosshair = crosshair;
            this.enemyMarker = enemyMarker;
            this.centerMarkerForward = centerMarkerForward;
            this.centerMarkerBackward = centerMarkerBackward;

            // Set health to max
            Health = MaxHealth;
            Shields = MaxHealth;

            // Set bounds timer to max
            boundsTimer = boundsTimerMax;

            // Set position and rotation
            Position = spawnPosition;
            model.Rotation = spawnRotation;
        }

        // Resets bounds timer
        public void ResetBoundsTimer()
        {
            boundsTimer = boundsTimerMax;
        }

        // Reset ship
        public void Reset()
        {
            // Stop ship
            Velocity = Vector3.Zero;
            // Reset rotation
            model.Rotation = spawnRotation;

            // Move to spawn position
            Position = spawnPosition;

            // Bring shields online
            shieldsOnline = true;

            // Reset health
            Health = MaxHealth;
            Shields = MaxHealth;

            // Reset bounds timer
            boundsTimer = boundsTimerMax;

            // Reset counters
            Deaths = 0;
            Kills = 0;
        }

        // Destroy and respawn ship
        public void DestroyShip()
        {
            // Play explosion sound
            SoundManager.Explosion.Play();

            // Stop ship
            Velocity = Vector3.Zero;
            // Reset rotation
            model.Rotation = spawnRotation;

            // Move to spawn position
            Position = spawnPosition;

            // Bring shields online
            shieldsOnline = true;

            // Reset health
            Health = MaxHealth;
            Shields = MaxHealth;

            // Add one to death counter
            Deaths++;
        }

        // Invert velocity and damage ship if moving too fast
        public void SendOffCourse()
        {
            // If velocity is faster than 0.1, damage ship
            if (Velocity.Length() > 0.1)
            {
                DamageShip();
            }

            // Move ship out of collision area
            Position += Velocity * -1;

            // Sends ship other way
            Velocity *= -0.3f;
        }

        // Invert ship's velocity and damage ship
        public void SendOffCourseAndDamage()
        {
            DamageShip();

            // Move ship out of collision area
            Position += Velocity * -1;

            // Sends ship other way
            Velocity *= -0.3f;
        }

        // Damages ship and returns if the player has died
        public bool DamageShip()
        {
            // Reset time since last hit
            timeSinceLastHit = 0;

            // If shields are online
            if (Shields > 0)
            {
                // Damage shields
                Shields--;
                // Play shield it sound
                SoundManager.ShieldHit.Play();

                // If shields go below 0%
                if (Shields <= 0)
                {
                    // Damage hull by amount under 0%
                    Health += Shields;

                    // Set shields to 0%
                    Shields = 0;

                    // Play "shields offline" sound depending on player index
                    if (playerIndex == PlayerIndex.One)
                        SoundManager.ShieldsOffline1.Play();
                    else
                        SoundManager.ShieldsOffline2.Play();

                    // Set shields to offline
                    shieldsOnline = false;
                }
            }

            // If shields are offline
            else if (Shields <= 0)
            {
                // Decreas health
                Health--;
                // Play hit sound
                SoundManager.Hit.Play();
            }


            // If health and shields are gone
            if (Health <= 0 && Shields <= 0)
            {
                // Destroy ship and return that ship has been destroyed
                DestroyShip();
                return true;
            }
            // Otherwise return that ship was not destroyed
            return false;
        }

        // Handle Rotation
        private void doRotation(GameTime gameTime, GamePadState gamePadState)
        {
            // Vector3 of Yaw/pitch/roll from controller
            Vector3 rotationInput = Vector3.Zero;

            // Store controller rotation input in the Vector3
            //if (gamePadState.IsButtonDown(Buttons.LeftTrigger))
            //    rotationInput.Z = 0.5f;
            //if (gamePadState.IsButtonDown(Buttons.RightTrigger))
            //    rotationInput.Z = -0.5f;
            rotationInput.X = gamePadState.ThumbSticks.Right.Y;
            rotationInput.Y = -gamePadState.ThumbSticks.Right.X;
            // Lower vector3 to something smaller
            rotationInput *= .025f;

            //model.Rotation = MatrixHelper.ExtractYawPitchRoll(finalMatrix);

            // Rotate model
            model.Rotation += rotationInput;

            // Keep player from rotating ship far enough to invert controls
            model.Rotation = new Vector3(MathHelper.Clamp(model.Rotation.X, -1.7f, 1.7f), model.Rotation.Y, model.Rotation.Z);
        }

        // Handle movement
        private void doMovement(GameTime gameTime, GamePadState gamePadState)
        {
            // Reset movement
            Vector3 velocityInput = Vector3.Zero;
            // Amount to lower speed by
            var speed = 0.01f;

            // Get movement to make from controller input
            if (gamePadState.IsButtonDown(Buttons.LeftShoulder))
                velocityInput.Y = 0.5f;
            if (gamePadState.IsButtonDown(Buttons.RightShoulder))
                velocityInput.Y = -0.5f;
            velocityInput.X = -gamePadState.ThumbSticks.Left.X;
            velocityInput.Z = gamePadState.ThumbSticks.Left.Y;

            // Lower to something smaller
            velocityInput *= speed;

            // Add new movement to velocity in direction given by rotation matrix
            Velocity += Vector3.Transform(velocityInput, model.RotationAsMatrix);  // * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;

            // Limit velocity if velocity is higher than allowed
            if (Velocity.Length() > maxVelocity)
            {
                // Set velocity to max velocty
                Velocity = Vector3.Normalize(Velocity) * maxVelocity;
            }

            // Brake when pressing X
            if (gamePadState.IsButtonDown(Buttons.X))
            {
                // Decrease velocity by 3%
                Velocity = Velocity * 0.97f;
            }


            // Move model
            model.Position += Velocity;
        }


        // Update
        public void Update(GameTime gameTime)
        {
            // Get gamepad state
            GamePadState gamePadState = GamePad.GetState(playerIndex);

            // Fire when pressing Right tumbstick
            if (gamePadState.Triggers.Right > 0)
            {
                // Count up time since last shot
                shootTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                // Shoot when enough time has passed
                if (shootTimer >= 200)
                {
                    // Shoot
                    bulletManager.Shoot(model, Velocity);
                    // Vibrate controller
                    GamePad.SetVibration(playerIndex, 10, 0);
                }
            }
            // Reset vibration when not fireing
            else
                GamePad.SetVibration(playerIndex, 0, 0);

            // Update movement and rotation
            doMovement(gameTime, gamePadState);
            doRotation(gameTime, gamePadState);

            // Update bulletManager
            bulletManager.Update(gameTime, model.Position);

            // Regen shields if enough time has passed since last hit
            if (Shields < MaxShields && timeSinceLastHit > 15)
            {
                // Regen by small amont
                Shields += 0.01f;

                // If shields are higher than 0% but set to be offline
                if (Shields > 0 && !shieldsOnline)
                {
                    // Bring shields online
                    shieldsOnline = true;
                    // Play sound based on player index
                    if (playerIndex == PlayerIndex.One)
                        SoundManager.ShieldsOnline1.Play();
                    else
                        SoundManager.ShieldsOnline2.Play();
                }

                // Keep shields from going over 100%
                if (Shields > MaxShields)
                    Shields = MaxShields;
            }

            // Reset shot timer if over 200
            if (shootTimer > 200)
                shootTimer = 0;

            // Count time since last hit
            timeSinceLastHit += (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Count time spent outside arena when outside arena
            if (OutOfBounds)
            {
                // Count in seconds
                boundsTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Check if player has been out of bound long enough to die
                if (boundsTimer <= 0)
                {
                    // Kill player
                    DestroyShip();

                    // Reset timer
                    boundsTimer = boundsTimerMax;
                }

            }

        }

        // Draw stuff seen in first person
        public void DrawFirstPerson(Camera camera, SpriteBatch spriteBatch, GraphicsDevice graphics, CustomModel enemyModel)
        {
            // Disable the depth buffer
            graphics.DepthStencilState = DepthStencilState.None;

            // Draw cockpit model
            CockpitModel.Draw(camera.View, camera.Projection, Vector3.Zero);

            // Draw everything in HUD
            spriteBatch.Begin();
            spriteBatch.DrawString(Game1.HudFont, "shields: " + ((int)((Shields / MaxShields) * 100)).ToString() + "%", new Vector2(120, 840), new Color(224, 96, 26));
            spriteBatch.DrawString(Game1.HudFont, "speed: " + ((int)((Velocity.Length()) * 100)).ToString(), new Vector2(430, 840), new Color(224, 96, 26));
            spriteBatch.DrawString(Game1.HudFont, "hull: " + ((int)((Health / MaxHealth) * 100)).ToString() + "%", new Vector2(700, 840), new Color(224, 96, 26));

            spriteBatch.DrawString(Game1.HudFont, "kills: " + Kills.ToString(), new Vector2(140, 170), new Color(224, 96, 26));
            spriteBatch.DrawString(Game1.HudFont, "deaths: " + Deaths.ToString(), new Vector2(700, 170), new Color(224, 96, 26));

            // Calculate crosshair position
            Vector2 crosshairPos = new Vector2(480 - (crosshair.Width / 2), 540 - (crosshair.Height / 2));

            // If player is pitching too far up/down notify using HUD
            if (model.Rotation.X > 1)
                spriteBatch.DrawString(Game1.HudFont, "WARNING: PITCH UP", new Vector2(350, 640), new Color(224, 30, 00));
            if (model.Rotation.X < -1)
                spriteBatch.DrawString(Game1.HudFont, "WARNING: PITCH DOWN", new Vector2(350, 640), new Color(224, 30, 00));
            // If player is out of bounds notify using HUD
            if (OutOfBounds)
            {
                spriteBatch.DrawString(Game1.HudFont, "WARNING: RETURN TO COMBAT AREA", new Vector2(260, 440), new Color(224, 30, 00));
                spriteBatch.DrawString(Game1.HudFont, boundsTimer.ToString("00.00"), new Vector2(445, 490), new Color(224, 30, 00));
            }

            // Calculate position to draw enemy marker at
            Vector2 enemyMarkerPos = new Vector2(
                graphics.Viewport.Project(Vector3.Zero, camera.Projection, camera.View, enemyModel.WorldMatrix).X - (enemyMarker.Width / 2),
                graphics.Viewport.Project(Vector3.Zero, camera.Projection, camera.View, enemyModel.WorldMatrix).Y - (enemyMarker.Height / 2));
            // Clamp marker pos to keep it from going outside the screen
            enemyMarkerPos = Vector2.Clamp(enemyMarkerPos, Vector2.Zero, new Vector2(920 - enemyMarker.Width, 1080 - enemyMarker.Height));
            // Only draw marker when enemy is not behind player
            if (graphics.Viewport.Project(Vector3.Zero, camera.Projection, camera.View, enemyModel.WorldMatrix).Z < 1)
                spriteBatch.Draw(enemyMarker, enemyMarkerPos, new Color(224, 30, 00));

            // Calculate position to draw center marker at
            Vector2 centerPos = new Vector2(
                graphics.Viewport.Project(Vector3.Zero, camera.Projection, camera.View, Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up)).X - (centerMarkerForward.Width / 2),
                graphics.Viewport.Project(Vector3.Zero, camera.Projection, camera.View, Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up)).Y - (centerMarkerForward.Height / 2));
            // Clamp marker pos to keep it from going outside the screen
            centerPos = Vector2.Clamp(centerPos, Vector2.Zero, new Vector2(920 - centerMarkerForward.Width, 1080 - centerMarkerForward.Height));
            // Only draw marker with texture based on if marker is behind player or not
            if (graphics.Viewport.Project(Vector3.Zero, camera.Projection, camera.View, enemyModel.WorldMatrix).Z < 1)
                spriteBatch.Draw(centerMarkerForward, centerPos, new Color(216, 180, 19));
            //else spriteBatch.Draw(centerMarkerBackward, centerPos, new Color(216, 180, 19));

            // Draw crosshair
            spriteBatch.Draw(crosshair, crosshairPos, new Color(224, 96, 26));

            spriteBatch.End();

            // Enable the depth buffer
            graphics.DepthStencilState = DepthStencilState.Default;
        }

        // Draw stuff seen in thrid person
        public void DrawThirdPerson(Camera camera)
        {
            // Draw ship
            model.Draw(camera.View, camera.Projection, Vector3.Zero);
        }
    }
}
