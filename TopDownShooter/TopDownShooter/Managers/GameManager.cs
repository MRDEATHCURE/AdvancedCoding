using TopDownShooter.Enums;

namespace TopDownShooter.Managers
{
    public class GameManager
    {
        public GameState CurrentState { get; private set; } = GameState.Playing;  // Current game state starts as Playing

        public void SetGameOver()              // set the game state to GameOver
        {
            CurrentState = GameState.GameOver;  
        }

        public void ResetGame()                // reset the game state to Playing
        {
            CurrentState = GameState.Playing;   // Change state back to Playing
        }
    }
}
