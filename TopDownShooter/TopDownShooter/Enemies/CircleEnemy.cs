using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TopDownShooter.Spawners;

namespace TopDownShooter.Enemies
{
    public class CircleEnemy : BaseEnemy
    {
        public CircleEnemy(Vector2 position, float speed, int health, Texture2D texture, List<StaticSpawn> staticSpawns, int monitorWidth, int monitorHeight)
            : base(position, speed, health, texture, staticSpawns, monitorWidth, monitorHeight)
        {
            RandomSpawn(); // Randoms  a spawn location
        }
    }
}
