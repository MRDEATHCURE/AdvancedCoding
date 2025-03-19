using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooter.Spawners
{
    public class StaticSpawn : IStatic
    {
        public Vector2 staticPosition { get; private set; }   // Position of static object
        private Texture2D staticTexture;                      // Texture to draw the static object 

        // Constructor
        public StaticSpawn(Vector2 position, Texture2D staticTexture)
        {
            staticPosition = position;
            this.staticTexture = staticTexture;
        }

        // renders the static object
        public void Draw(SpriteBatch spriteBatch)
        {
            if (staticTexture != null)
            {
                spriteBatch.Draw(staticTexture, staticPosition, Color.White); // Draw texture at position
            }
        }

        //  Returns a rectangle representing the static objects bounds - basically collision size
        public Rectangle GetBounds()
        {
            return new Rectangle((int)staticPosition.X, (int)staticPosition.Y, staticTexture.Width, staticTexture.Height);
        }
    }
}
