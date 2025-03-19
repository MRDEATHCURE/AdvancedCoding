using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TopDownShooter.Spawners;

namespace TopDownShooter.Enemies
{
    public class PolygonEnemy : BaseEnemy
    {
        public PolygonEnemy(Vector2 position, float speed, int health, Texture2D texture, List<StaticSpawn> staticSpawns, int monitorWidth, int monitorHeight)
            : base(position, speed, health, texture, staticSpawns, monitorWidth, monitorHeight)
        {
            RandomSpawn(); // Randomly assigns a spawn location.
        }
    }
}
