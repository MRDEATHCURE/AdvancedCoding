using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TopDownShooter.Enemies;
using TopDownShooter.Enums;
using TopDownShooter.Managers;
using TopDownShooter.Particles;
using TopDownShooter.Spawners;

namespace TopDownShooter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch; 
        private Player _player;
        private EnemySpawner _enemySpawner;
        private SpriteFont gameFont;
        private ParticleSystem particleSystem;
        private CollisionManager _collisionManager;
        private ScoreManager scoreManager;                        // Shared ScoreManager instance
        private GameManager gameManager;                          // Manages game state
        private UIManager _uiManager;
        private List<BaseEnemy> baseEnemyList;                    // Active enemy list
        private List<Bullet> bulletsList;
        private List<StaticSpawn> _staticObjects;
        private List<BreakableStatic> breakableStatics;
        private Texture2D playerTexture;
        private Texture2D enemyTriangleTexture;
        private Texture2D enemyCircleTexture;
        private Texture2D enemyPolgygonTexture;
        private Texture2D staticSpawTexutres;
        private Texture2D breakableTexture;
        private Texture2D bulletTexture;
        private Texture2D particleTexture;
        private Texture2D backgroundTexture;

       

        private HighScoreManager highScoreManager;
        private int[] highScores;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            highScoreManager = new HighScoreManager();
            gameManager = new GameManager();
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        // Generates static objects at random positions
        private void GenerateStaticObjects(int count)
        {
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                int spawnX = random.Next(0, _graphics.PreferredBackBufferWidth - 50);
                int spawnY = random.Next(0, _graphics.PreferredBackBufferHeight - 50);
                _staticObjects.Add(new StaticSpawn(new Vector2(spawnX, spawnY), staticSpawTexutres));
            }
        }

        // Generates breakable static objects at random positions
        private void GenerateBreakableStatics(int count)
        {
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                int spawnX = random.Next(0, _graphics.PreferredBackBufferWidth - breakableTexture.Width);
                int spawnY = random.Next(0, _graphics.PreferredBackBufferHeight - breakableTexture.Height);
                breakableStatics.Add(new BreakableStatic(new Vector2(spawnX, spawnY), breakableTexture, 10)); // Add new breakable object with 10 HP
            }
        }

        protected override void LoadContent()
        {
            // Load assets
            breakableTexture = Content.Load<Texture2D>("BreakableStatic");
            staticSpawTexutres = Content.Load<Texture2D>("StaticBlock");
            gameFont = Content.Load<SpriteFont>("GameFont");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTexture = Content.Load<Texture2D>("Player");
            enemyTriangleTexture = Content.Load<Texture2D>("EnemyTriangle");
            enemyCircleTexture = Content.Load<Texture2D>("EnemyCircle");
            enemyPolgygonTexture = Content.Load<Texture2D>("EnemyPolygon");
            bulletTexture = Content.Load<Texture2D>("Bullet");
            backgroundTexture = Content.Load<Texture2D>("Background");
            particleTexture = new Texture2D(GraphicsDevice, 1, 1);           // Create texture for particles
            particleTexture.SetData(new[] { Color.White });                  // Set particle texture data to white
            particleSystem = new ParticleSystem(particleTexture);            // Initialize particle system with texture

            int monitorWidth = _graphics.PreferredBackBufferWidth;             // Screen width
            int monitorHeight = _graphics.PreferredBackBufferHeight;           // Screen height
            baseEnemyList = new List<BaseEnemy>();                             // Initialize enemy list
            bulletsList = new List<Bullet>();                                  // Initialize bullet list
            _staticObjects = new List<StaticSpawn>();                          // Initialize static objects list
            breakableStatics = new List<BreakableStatic>();                    // Initialize breakable objects list

            // Initialize enemy spawner with enemy and static objects lists
            _enemySpawner = new EnemySpawner(baseEnemyList, _staticObjects, enemyTriangleTexture, enemyCircleTexture, enemyPolgygonTexture, monitorWidth, monitorHeight);

            // Create the player at center of the screen
            _player = new Player(new Vector2(960, 540), playerTexture, _enemySpawner, bulletTexture, monitorWidth, monitorHeight);

            scoreManager = new ScoreManager();                                 // Initialize score manager
            _uiManager = new UIManager(GraphicsDevice, gameFont);              // Initialize UI manager

            _collisionManager = new CollisionManager(_player, baseEnemyList, bulletsList, scoreManager, particleSystem); // Initialize collision manager with particle system

            GenerateStaticObjects(300);
            GenerateBreakableStatics(20);

            // Load high score from file and initialize the highScores array
            int loadedScore = highScoreManager.LoadHighScore();              
            highScores = new int[1];                                          
            highScores[0] = loadedScore;                                      
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;  // Calculate deltaTime

            particleSystem.Update(deltaTime);                                 // Update particle system

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            if (gameManager.CurrentState == GameState.Playing)
            {
                _player.Update(deltaTime, bulletsList, baseEnemyList, _staticObjects); // Update player movement and actions
                _enemySpawner.Update(deltaTime);                              // Update enemy spawner
                _collisionManager.Update(deltaTime);                          // Update collisions

                foreach (var enemy in baseEnemyList)                           // Update each enemy's movement
                    enemy.Update(_player.Position);

                // Update bullets and remove expired ones in one loop
                for (int i = bulletsList.Count - 1; i >= 0; i--)
                {
                    bulletsList[i].Update(deltaTime);                          // Update bullet position and lifetime
                    if (bulletsList[i].IsExpired)                              // Check if bullet has expired
                        bulletsList.RemoveAt(i);                               // Remove expired bullet
                }

                // Process collisions for breakable static objects
                for (int i = breakableStatics.Count - 1; i >= 0; i--)
                {
                    BreakableStatic breakStatic = breakableStatics[i];                  // Get breakable static object
                    for (int j = 0; j < bulletsList.Count; j++)                         // Loop through bullets
                    {
                        if (bulletsList[j].Rectangle.Intersects(breakStatic.GetBounds())) // Check collision with bullet
                        {
                            breakStatic.TakeDamage(20);
                            if (breakStatic.IsDestroyed)
                            {
                                BuffType buff = breakStatic.GenerateRandomBuff();
                                _player.ApplyBuff(buff);
                                breakableStatics.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }

                // Check if player is dead to update game state
                if (_player.IsDead())
                {
                    gameManager.SetGameOver();
                    highScores[0] = scoreManager.Score;
                    highScoreManager.SaveHighScore(scoreManager.Score);
                }
            }
            else if (gameManager.CurrentState == GameState.GameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the sprite batch with a wrapping sampler state (for background tiling)
            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            // Create destination rectangle covering entire screen for background
            Rectangle destRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            // Create source rectangle for tiling the background texture
            Rectangle sourceRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _spriteBatch.Draw(backgroundTexture, destRect, sourceRect, Color.White); // Draw the tiled background

            foreach (var staticObj in _staticObjects)
                staticObj.Draw(_spriteBatch);
            foreach (var breakStatic in breakableStatics)
                breakStatic.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
            foreach (var bullet in bulletsList)
                bullet.Draw(_spriteBatch);
            foreach (var enemy in baseEnemyList)
                enemy.Draw(_spriteBatch);

            particleSystem.Draw(_spriteBatch);

            _uiManager.Draw(_spriteBatch, _player, scoreManager.Score, highScores);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
