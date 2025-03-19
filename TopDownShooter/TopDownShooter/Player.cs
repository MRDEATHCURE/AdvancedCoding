using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TopDownShooter.Enemies;
using TopDownShooter.Enums;
using TopDownShooter.Managers;
using TopDownShooter.Spawners;

namespace TopDownShooter
{
    public class Player : Character, IShootable
    {
        private float shootCooldown = 0.5f;
        private float shootTimer = 0f;
        private Texture2D bulletTexture;
        private List<BaseEnemy> enemyBaseList;
        private EnemySpawner enemySpawner;
        private int screenWidth;
        private int screenHeight;
        private InputManager inputManager;
        private int extraBulletCount = 0; // Counter for extra bullets

        // Constructor
        public Player(Vector2 position, Texture2D playerTexture, EnemySpawner enemySpawner, Texture2D bulletTexture, int screenWidth, int screenHeight)
            : base(position, 3f, 100, playerTexture)
        {
            this.bulletTexture = bulletTexture;
            this.enemySpawner = enemySpawner;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            inputManager = new InputManager();
        }

        public void Update(float deltaTime, List<Bullet> bullets, List<BaseEnemy> enemies, List<StaticSpawn> staticObjects)
        {
            shootTimer += deltaTime;

            // Get movement vector from InputManager
            Vector2 movement = inputManager.GetMovement();

            // Calculate the desired position
            Vector2 desiredPosition = position + (movement * Speed);

            // Clamp the desired position to keep the player within screen bounds
            desiredPosition.X = MathHelper.Clamp(desiredPosition.X, 0, screenWidth - Texture.Width);
            desiredPosition.Y = MathHelper.Clamp(desiredPosition.Y, 0, screenHeight - Texture.Height);

            // If moving diagonally doesnt work because of collision try sliding along each axis
            if (!CheckCollisionWithStatic(staticObjects, desiredPosition))
            {
                position = desiredPosition;
            }
            else
            {
                // Try moving horizontally only
                Vector2 horizontalPosition = new Vector2(position.X + (movement.X * Speed), position.Y);
                horizontalPosition.X = MathHelper.Clamp(horizontalPosition.X, 0, screenWidth - Texture.Width);
                // Try moving vertically only
                Vector2 verticalPosition = new Vector2(position.X, position.Y + (movement.Y * Speed));
                verticalPosition.Y = MathHelper.Clamp(verticalPosition.Y, 0, screenHeight - Texture.Height);

                bool canMoveHorizontally = !CheckCollisionWithStatic(staticObjects, horizontalPosition);
                bool canMoveVertically = !CheckCollisionWithStatic(staticObjects, verticalPosition);

                // Prioritize the axis that is not blocked
                if (canMoveHorizontally && !canMoveVertically)
                {
                    position = horizontalPosition;
                }
                else if (!canMoveHorizontally && canMoveVertically)
                {
                    position = verticalPosition;
                }
                else if (canMoveHorizontally && canMoveVertically)
                {
                    // If possible, update both (diagonal slide)
                    position = new Vector2(horizontalPosition.X, verticalPosition.Y);
                }
                // if not possible,  player doesnt move
            }

            // Update the collision rectangle after moving
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            // Shooting logic remains the same
            if (shootTimer >= shootCooldown)
            {
                Shoot(bullets, enemies);
                shootTimer = 0f;
            }
        }

        public void Shoot(List<Bullet> bullets, List<BaseEnemy> enemies)
        {
            if (enemySpawner.AllEnemiesList.Count == 0)
                return;

            BaseEnemy closestEnemy = enemySpawner.AllEnemiesList
                .OrderBy(e => Vector2.Distance(position, e.Position))
                .FirstOrDefault();
            if (closestEnemy == null)
                return;

            Vector2 middlePoint = new Vector2(rectangle.X + Texture.Width / 2, rectangle.Y + Texture.Height / 2);
            Vector2 baseDirection = closestEnemy.Rectangle.Center.ToVector2() - middlePoint;
            baseDirection.Normalize();

            // Fire the main bullet without sine movement
            Bullet mainBullet = new Bullet(middlePoint, baseDirection, bulletTexture);
            mainBullet.SineMovement = false; // Ensure main bullet goes straight
            bullets.Add(mainBullet);

            // Limit extra bullet buff to maximum of 2 extra bullets
            int extraBulletsToFire = Math.Min(extraBulletCount, 2);

            if (extraBulletsToFire > 0)
            {
                // Define the offset angle in radians
                float offsetAngle = MathHelper.ToRadians(30); // 30 offset

                if (extraBulletsToFire == 1)
                {
                    // Fire one extra bullet with a fixed offset
                    Vector2 extraDirection = RotateVector(baseDirection, offsetAngle);
                    Bullet extraBullet = new Bullet(middlePoint, extraDirection, bulletTexture)
                    {
                        SineMovement = true // Extra bullet moves in a sine manner
                    };
                    bullets.Add(extraBullet);
                }
                else if (extraBulletsToFire == 2)
                {
                    // Fire two extra bullets: one by -30 and one by +30
                    Vector2 leftDirection = RotateVector(baseDirection, -offsetAngle);
                    Vector2 rightDirection = RotateVector(baseDirection, offsetAngle);

                    Bullet leftBullet = new Bullet(middlePoint, leftDirection, bulletTexture)
                    {
                        SineMovement = true
                    };
                    Bullet rightBullet = new Bullet(middlePoint, rightDirection, bulletTexture)
                    {
                        SineMovement = true
                    };

                    bullets.Add(leftBullet);
                    bullets.Add(rightBullet);
                }
            }
        }

        // rotate a vector by a given angle in radians
        private Vector2 RotateVector(Vector2 v, float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Vector2(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
        }










        private bool CheckCollisionWithStatic(List<StaticSpawn> staticObjects, Vector2 newPosition)
        {
            foreach (var obj in staticObjects)
            {
                Rectangle newBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, Texture.Width, Texture.Height);
                if (newBounds.Intersects(obj.GetBounds()))
                {
                    return true; // Collision detected.
                }
            }
            return false;
        }

        public override void TakeDamage(int amountOfDamage)
        {
            base.TakeDamage(amountOfDamage);
        }

        // applies a buff to the player
        public void ApplyBuff(BuffType buff)
        {
            switch (buff)
            {
                case BuffType.ExtraBullet:
                    extraBulletCount++;
                    System.Diagnostics.Debug.WriteLine("Extra Bullet Buff applied!");
                    break;
                case BuffType.IncreaseSpeed:
                    this.speed += 1f;
                    System.Diagnostics.Debug.WriteLine("Speed Increase Buff applied!");
                    break;
                default:
                    break;
            }
        }
    }
}
