using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TopDownShooter.Spawners;

namespace TopDownShooter.Enemies
{
    public class BaseEnemy : Character
    {
        private List<StaticSpawn> staticSpawnList; // static objects for collision checking
        private int screenWidth;                   // Screen width used for random spawning
        private int screenHeight;                  // Screen height used for random spawning
        private Random random = new Random();      // Random instance for spawning

        public BaseEnemy(Vector2 position, float speed, int health, Texture2D texture, List<StaticSpawn> staticObjects, int monitorWidth, int monitorHeight)
            : base(position, speed, health, texture) // base class constructor
        {
            screenWidth = monitorWidth;            
            screenHeight = monitorHeight;          
            staticSpawnList = staticObjects;       

            rectangle.X = (int)(position.X - texture.Width); //  collision rectangle X based on texture size
            rectangle.Y = (int)(position.Y - texture.Height); //  collision rectangle Y based on texture size
        }

        public void Update(Vector2 playerPosition)
        {
            Vector2 direction = playerPosition - position; // vector direction to the player
            if (direction.LengthSquared() > 0)
            {
                direction.Normalize();                     

            }
            Vector2 fullNewPosition = position + direction * speed; //  potential new position

            if (!CheckCollisionWithStatic(staticSpawnList, fullNewPosition))
            {
                position = fullNewPosition; // Move enemy if no collision
            }
            else
            {
                Vector2 newPositionX = new Vector2(position.X + direction.X * speed, position.Y); // Try horizontal movement 
                if (!CheckCollisionWithStatic(staticSpawnList, newPositionX))
                    position.X = newPositionX.X;         // Update X if no horizontal collision

                Vector2 newPositionY = new Vector2(position.X, position.Y + direction.Y * speed); // Try vertical movement 
                if (!CheckCollisionWithStatic(staticSpawnList, newPositionY))
                    position.Y = newPositionY.Y; // Update Y if no vertical collision
            }

            rectangle.X = (int)position.X; // Update collision rectangle X position
            rectangle.Y = (int)position.Y; // Update collision rectangle Y position
        }

        private bool CheckCollisionWithStatic(List<StaticSpawn> staticObjects, Vector2 newPosition)
        {
            // Create a rectangle at the new position with the same size as the texture
            Rectangle futureCollisionLocation = new Rectangle((int)newPosition.X, (int)newPosition.Y, texture.Width, texture.Height);
            foreach (var obj in staticObjects)
            {
                if (futureCollisionLocation.Intersects(obj.GetBounds()))
                    return true;  // true if collided
            }
            return false; // No collision 
        }

        public void RandomSpawn()
        {
            int spawnX = random.Next(0, screenWidth); // Generate random X position in screen width
            int spawnY = random.Next(0, screenHeight);  // Generate random Y position in screen height
            position.X = spawnX;                        // set new X position
            position.Y = spawnY;                        // Set new Y positoin
        }
    }
}
