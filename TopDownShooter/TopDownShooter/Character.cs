using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooter
{
    public abstract class Character : IDamageable
    {
        protected Vector2 position;                    // Characters position
        protected Rectangle rectangle;                 // Collision rectangle for character
        protected float speed;                         // Movement speed
        protected Texture2D texture;                   // Texture for  drawing character
        protected int maxHealth;                       // Maximum health
        protected int currentHealth;                   // Current health

        // Properties to show internal fields
        public Vector2 Position { get { return position; } }            // show position
        public Rectangle Rectangle { get { return rectangle; } }        // show collision rectangle
        public float Speed { get { return speed; } }                    // show speed
        public Texture2D Texture { get { return texture; } }            // show texture
        public int MaxHealth { get { return maxHealth; } }              // show maximum health
        public int CurrentHealth { get { return currentHealth; } }      // show current health

        // Constructor
        public Character(Vector2 position, float speed, int health, Texture2D texture)
        {
            this.position = position;
            this.speed = speed;
            this.texture = texture;
            maxHealth = health;
            currentHealth = health; 
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); // Create collision rectangle based texture size
        }

        // Draws the character using texture and collision rectangle
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, rectangle, Color.White); // Draw the texture inside collision rectangle
        }

        protected Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height); // Returns a rectangle based on position and texture size
        }

        // Checks collision with another character by comparing their bounds
        protected bool CheckCollision(Character other)
        {
            return GetBounds().Intersects(other.GetBounds()); // Returns true if bounding rectangles intersect
        }

        // damage to the character
        public virtual void TakeDamage(int amountOfDamage)
        {
            currentHealth -= amountOfDamage;           // Lowers current health by the specified amount
        }

        public virtual bool IsDead()
        {
            if (currentHealth > 0)
                return false;                          // Still alive if health is above zero
            else
                return true;                           // Dead if health is zero or less
        }
    }
}
