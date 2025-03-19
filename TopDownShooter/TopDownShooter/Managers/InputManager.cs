using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooter.Managers
{
    public class InputManager
    {
        // Gets the normalized movement vector based on keyboard input.
        public Vector2 GetMovement()
        {
            KeyboardState state = Keyboard.GetState(); //current keyboard state
            Vector2 movement = Vector2.Zero; // Initialize movement vector

            if (state.IsKeyDown(Keys.W))
                movement.Y -= 1;
            if (state.IsKeyDown(Keys.S))
                movement.Y += 1;
            if (state.IsKeyDown(Keys.A))
                movement.X -= 1;
            if (state.IsKeyDown(Keys.D))
                movement.X += 1;

            if (movement != Vector2.Zero)
                movement.Normalize();

            return movement;
        }
    }
}
