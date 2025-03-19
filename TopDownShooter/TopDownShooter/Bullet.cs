using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TopDownShooter.Enemies;

namespace TopDownShooter
{
    public class Bullet
    {
        public Rectangle Rectangle { get; private set; } // Collision rectangle bullet
        public Vector2 Position { get; private set; }      // Current position of bullet
        private Vector2 direction;                         // Movement direction of bullet
        private float speed = 200f;                        // Speed of the bullet
        private Texture2D bulletTexture;                   // Texture to draw the bullet

        // Lifetime management
        public float Lifetime { get; private set; } = 0f;  // Timer lifetime
        public float MaxLifetime { get; set; } = 3f;         // Maximum lifetime before removal
        public bool IsExpired { get; private set; } = false; // To indicate removal

        // Sine movement if true, bullet will have sine wave offset to thier movement
        public bool SineMovement { get; set; } = false;

        // Constructor
        public Bullet(Vector2 position, Vector2 direction, Texture2D bulletTexture)
        {
            this.Position = position;                      
            this.direction = direction;                    
            this.bulletTexture = bulletTexture;           
            this.Rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height); 
        }

        public void Update(float deltaTime)
        {
            Lifetime += deltaTime;                         // Increment lifetime by deltaTime
            if (Lifetime > MaxLifetime)                    // If bullets lifetime passes max
            {
                IsExpired = true;                          // bullet expired
                return;                                    
            }

            Vector2 movement = direction * speed * deltaTime; // Calculate base movement vector

            if (SineMovement)                              // If sine movement is enabled for bullet
            {
                float amplitude = 20f;                     // Amplitude for sine offset 
                float frequency = 5f;                      // Frequency for sine movement 
                Vector2 perpendicular = new Vector2(-direction.Y, direction.X); // Calculate vector
                float sineOffset = amplitude * (float)Math.Sin(frequency * Lifetime); // Calculate offset
                movement += perpendicular * sineOffset * deltaTime; // Add sine offset to movement
            }

            Position += movement; // Update bullet position
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, bulletTexture.Width, bulletTexture.Height); // Update collision rectangle
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, Position, Color.White); // Draw bullet texture at current position
        }

        public bool CheckCollision(BaseEnemy enemy)
        {
            return Rectangle.Intersects(enemy.Rectangle); // Return true if bullet rectangle intersects enemy rectangle
        }
    }
}
