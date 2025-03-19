using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TopDownShooter.Enums;
using TopDownShooter.Interfaces.TopDownShooter;

namespace TopDownShooter.Spawners
{
    public class BreakableStatic : IStatic, IBreakable
    {
        public Vector2 Position { get; private set; }             // Position for drawing/collision
        private Texture2D texture;                                // Texture for drawing
        private int maxHealth;                                    // Maximum health
        private int currentHealth;                                // Current health
        private Random random = new Random();                     // Random for buff generation
        public bool IsDestroyed { get; private set; } = false;    // indicating whether the object is destroyed

        public BreakableStatic(Vector2 position, Texture2D texture, int health)
        {
            Position = position;
            this.texture = texture;
            maxHealth = health;
            currentHealth = health;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDestroyed)                                     // Draw if not destroyed
                spriteBatch.Draw(texture, Position, Color.White); // Draw texture at Position
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height); // Return collision rectangle
        }

        public void TakeDamage(int damage)
        {
            if (IsDestroyed)                                      // If already destroyed, do nothing
                return;
            currentHealth -= damage;                              // Current Health reduced by damage
            if (currentHealth <= 0)                               // If health 0 or below
                IsDestroyed = true;                               // destroyed
        }

        public BuffType GenerateRandomBuff()
        {
            int buffCount = Enum.GetNames(typeof(BuffType)).Length; // Get total number of buff types
            int index = random.Next(0, buffCount);                // Pick a random index between 0 and buffCount - 1
            return (BuffType)index;                               // Return the buff type that fits to the random index
        }
    }
}
