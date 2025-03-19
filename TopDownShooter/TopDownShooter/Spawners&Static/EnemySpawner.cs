using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TopDownShooter.Enemies;

namespace TopDownShooter.Spawners
{
    public class EnemySpawner
    {
        private List<BaseEnemy> baseEnemiesTypes;    // active enemy instances
        private List<StaticSpawn> staticSpawns;      // static objects for collision in spawned enemies
        private float _enemySpawnTimer = 0f;         // Timer to control enemy spawn intervals
        private float _enemySpawnInterval = 0.5f;    // Spawn interval in seconds
        private Texture2D enemyTriangleTexture;      // Texture for TriangleEnemy
        private Texture2D enemyCircleTexture;        // Texture for CircleEnemy
        private Texture2D enemyPolygonTexture;       // Texture for PolygonEnemy
        private int screenWidth;                     // Screen width for spawning calculations
        private int screenHeight;                    // Screen height for spawning calculations
        private Random random = new Random();        // Random instance for selecting enemy types

        // Property to show the active enemy list
        public List<BaseEnemy> AllEnemiesList { get { return baseEnemiesTypes; } } // Shows the list of active enemies

        // Constructor
        public EnemySpawner(List<BaseEnemy> baseEnemies, List<StaticSpawn> staticspawns,
                            Texture2D enemytriangle, Texture2D enemycircle, Texture2D enemypolygon,
                            int monitorWidth, int monitorHeight)
        {
            baseEnemiesTypes = baseEnemies;
            enemyTriangleTexture = enemytriangle;
            enemyCircleTexture = enemycircle;
            enemyPolygonTexture = enemypolygon;
            staticSpawns = staticspawns;
            screenWidth = monitorWidth;
            screenHeight = monitorHeight;

            // Create a TriangleEnemy and add it to the active enemy list
            TriangleEnemy enemyTriangle = new TriangleEnemy(Vector2.Zero, Constants.TriangleEnemySpeed, Constants.TriangleEnemyHP, enemytriangle, staticspawns, monitorWidth, monitorHeight); // Create TriangleEnemy using constants
            baseEnemiesTypes.Add(enemyTriangle); // Add TriangleEnemy to the list

            // Create a PolygonEnemy and add it to the active enemy list
            PolygonEnemy polygonEnemy = new PolygonEnemy(Vector2.Zero, Constants.PolygonEnemySpeed, Constants.PolygonEnemyHP, enemypolygon, staticspawns, monitorWidth, monitorHeight); // Create PolygonEnemy using constants
            baseEnemiesTypes.Add(polygonEnemy); // Add PolygonEnemy to the list

            // Create a CircleEnemy and add it to the active enemy list
            CircleEnemy circleEnemy = new CircleEnemy(Vector2.Zero, Constants.CircleEnemySpeed, Constants.CircleEnemyHP, enemycircle, staticspawns, monitorWidth, monitorHeight); // Create CircleEnemy using constants
            baseEnemiesTypes.Add(circleEnemy); // Add CircleEnemy to the list
        }

        // increases the spawn timer and spawns a new enemy when the interval is reached
        public void Update(float deltaTime)
        {
            _enemySpawnTimer += deltaTime;           // Increase spawn timer by deltaTime
            if (_enemySpawnTimer >= _enemySpawnInterval) // Check if timer passes spawn interval
            {
                SpawnEnemy();                        // Spawn a new enemy
                _enemySpawnTimer = 0f;               // Reset spawn timer
            }
        }

        // Randomly selects an enemy type and creates a new instance, adds it to the active enemy list
        private void SpawnEnemy()
        {
            float randomValue = (float)random.NextDouble();  // Generate random value between 0 and 1
            BaseEnemy newEnemy = null;                       // Initialize newEnemy variable

            if (randomValue < 0.4f)
            {
                // If random value is less than 0.4, create a TriangleEnemy
                newEnemy = new TriangleEnemy(Vector2.Zero, Constants.TriangleEnemySpeed, Constants.TriangleEnemyHP, enemyTriangleTexture, staticSpawns, screenWidth, screenHeight);
            }
            else if (randomValue < 0.7f) // 0.4 to 0.7
            {
                // If random value is between 0.4 and 0.7, create a PolygonEnemy
                newEnemy = new PolygonEnemy(Vector2.Zero, Constants.PolygonEnemySpeed, Constants.PolygonEnemyHP, enemyPolygonTexture, staticSpawns, screenWidth, screenHeight);
            }
            else // remaining - 0.7 to 1.0
            {
                // Create a CircleEnemy
                newEnemy = new CircleEnemy(Vector2.Zero, Constants.CircleEnemySpeed, Constants.CircleEnemyHP, enemyCircleTexture, staticSpawns, screenWidth, screenHeight);
            }

            if (newEnemy != null)
            {
                baseEnemiesTypes.Add(newEnemy); // Add the newly created enemy to the active list
            }
        }
    }
}
