using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooter.Particles
{
    public class Particle
    {
        public Vector2 Position;           // Particle position
        public Vector2 Velocity;           // Particle velocity
        public Color Color;                // Particle color
        public float Lifetime;             // Current lifetime of the particle
        public float MaxLifetime;          // Maximum lifetime before particle expires

        public Particle(Vector2 position, Vector2 velocity, Color color, float maxLifetime)
        {
            Position = position;           
            Velocity = velocity;           
            Color = color;                 
            MaxLifetime = maxLifetime;     
            Lifetime = 0f; // Initialize lifetime to 0
        }

        public void Update(float deltaTime)
        {
            Lifetime += deltaTime; // Increment lifetime by deltaTime
            Position += Velocity * deltaTime; // Update position based on velocity
        }

        public bool IsExpired()
        {
            return Lifetime >= MaxLifetime; // Return true if lifetime passes maximum lifetime
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            float scale = 2f; // Scale to size up particels
            spriteBatch.Draw(texture, Position, null, Color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f); // Draw the particle with the  scale
        }
    }
}
