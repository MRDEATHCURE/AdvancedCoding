using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TopDownShooter.Particles
{
    public class ParticleSystem
    {
        private List<Particle> particles;           // Active particles
        private Texture2D particleTexture;          // Texture to draw each particle

        public ParticleSystem(Texture2D particleTexture)
        {
            this.particleTexture = particleTexture;   // Set the texture for particles
            particles = new List<Particle>();         // Initialize the particle list
        }

        public void AddParticle(Particle particle)
        {
            particles.Add(particle); // Add a new particle to the system
        }

        public void Update(float deltaTime)
        {
            // Loop backwards to remove expired particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(deltaTime);         // particle position and lifetime
                if (particles[i].IsExpired())           // Check if particle has expired
                {
                    particles.RemoveAt(i);              // Remove expired particle
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw each particle using the assigned texture
            foreach (var particle in particles)
            {
                particle.Draw(spriteBatch, particleTexture); // Draw particle
            }
        }
    }
}
