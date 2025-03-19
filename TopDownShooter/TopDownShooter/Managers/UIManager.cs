using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooter.Managers
{
    public class UIManager
    {
        private SpriteFont font;                // Font used drawing text
        private Texture2D healthBarTexture;     // 1x1 texture for the health bar fill
        private Texture2D healthBarBackground;  // 1x1 texture for the health bar background
        private GraphicsDevice _graphicsDevice; // Graphics for creating textures

        public UIManager(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            _graphicsDevice = graphicsDevice;                    
            healthBarTexture = new Texture2D(_graphicsDevice, 1, 1); // Create a 1x1 texture health bar fill
            healthBarTexture.SetData(new[] { Color.Red });         // Set fill color red
            healthBarBackground = new Texture2D(_graphicsDevice, 1, 1); // Create a 1x1 texture health bar background
            healthBarBackground.SetData(new[] { Color.Gray });     // Set background color gray
            this.font = font;                                      // Store the font
        }

        public void Draw(SpriteBatch spriteBatch, Player player, int score, int[] highScores = null)
        {
            spriteBatch.DrawString(font, $"Score: {score}", new Vector2(10, 10), Color.AntiqueWhite); // Draw score location
            spriteBatch.Draw(healthBarBackground, new Rectangle(5, 35, 210, 30), Color.Gray);         // Draw health bar background location with size 210x30
            float healthPercentage = (float)player.CurrentHealth / player.MaxHealth;                  // Calculate players health percentage
            spriteBatch.Draw(healthBarTexture, new Rectangle(10, 40, (int)(200 * healthPercentage), 20), Color.Red); // Draw health bar fill to players health

            if (highScores != null) // If there is an array provided
            {
                int yOffset = 70;                                                                          // Set vertical position for high score
                string label = "High Score:";                                                              
                spriteBatch.DrawString(font, label, new Vector2(10, yOffset), Color.Cyan);                 // Draw the text at 10, yOffset
                Vector2 labelSize = font.MeasureString(label);                                             // measure the text width
                string scoreText = highScores.Length > 0 ? highScores[0].ToString() : "0";                 // Determine score text ,default 0
                spriteBatch.DrawString(font, scoreText, new Vector2(15 + labelSize.X, yOffset), Color.Cyan); // Draw the high score to the right of the text
            }
        }
    }
}
