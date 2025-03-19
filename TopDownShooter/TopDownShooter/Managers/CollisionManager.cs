using Microsoft.Xna.Framework;                         
using System;                                          
using System.Collections.Generic;                      
using TopDownShooter.Enemies;                          
using TopDownShooter.Particles;                        

namespace TopDownShooter.Managers
{
    public class CollisionManager
    {
        private Player player;                        // Reference to the player
        private List<BaseEnemy> baseEnemiesList;      // enemies for collision checks
        private List<Bullet> bullets;                 // bullets for collision checks
        private ScoreManager scoreManager;            // Shared score manager
        private ParticleSystem particleSystem;        // spawning particles when enemy dies
        private Random random = new Random();         // Random for generating particle properties

        // Constructor
        public CollisionManager(Player player, List<BaseEnemy> enemies, List<Bullet> bullets, ScoreManager scoreManager, ParticleSystem particleSystem)
        {
            this.player = player;                     
            this.baseEnemiesList = enemies;           
            this.bullets = bullets;                   
            this.scoreManager = scoreManager;         
            this.particleSystem = particleSystem;     
        }

        // collisions between player and enemies, and between bullets and enemies
        public void Update(float deltaTime)
        {
            // Loop over enemies in reverse to safely remove elements if needed
            for (int i = baseEnemiesList.Count - 1; i >= 0; i--)
            {
                // Check collision between player and enemy.
                if (player.Rectangle.Intersects(baseEnemiesList[i].Rectangle))
                {
                    player.TakeDamage(10); // damage on player
                    if (player.IsDead())
                    {
                        // Game over logic
                    }
                }

                // Loop over bullets in reverse.
                for (int j = bullets.Count - 1; j >= 0; j--)
                {
                    if (bullets[j].CheckCollision(baseEnemiesList[i])) // If bullet collides with enemy
                    {
                        baseEnemiesList[i].TakeDamage(20); // damage on enemy
                        bullets.RemoveAt(j);               // Remove the bullet upon collision
                        if (baseEnemiesList[i].IsDead())
                        {
                            // Spawn particles at enemy position
                            Vector2 enemyPos = baseEnemiesList[i].Position; // Get enemy position
                            int numParticles = 15;                          // Number of particles to spawn when enemy dies
                            for (int p = 0; p < numParticles; p++)
                            {
                                float angle = (float)(random.NextDouble() * MathHelper.TwoPi);                         // Random angle from 0 to 2π
                                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 50f;  // Calculate velocity vector with magnitude 50
                                Color particleColor = new Color(random.Next(256), random.Next(256), random.Next(256)); // random color generation for the particle
                                Particle particle = new Particle(enemyPos, velocity, particleColor, 1.5f);             // Create new particle with 1.5s lifetime
                                particleSystem.AddParticle(particle);  // Add particle to particle system
                            }
                            baseEnemiesList.RemoveAt(i);   // Remove enemy from the list
                            scoreManager.AddScore(1);      // Increase score by 1
                            break;                         // Break out of bullet loop for this specific enemy
                        }
                    }
                }
            }
        }
    }
}
