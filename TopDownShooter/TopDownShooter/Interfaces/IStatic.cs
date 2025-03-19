using Microsoft.Xna.Framework;                         
using Microsoft.Xna.Framework.Graphics;              

namespace TopDownShooter
{
    public interface IStatic
    {
        void Draw(SpriteBatch spriteBatch); // Draws the object using the given spritebatch
        Rectangle GetBounds();  // shows object collision bounds
    }
}
