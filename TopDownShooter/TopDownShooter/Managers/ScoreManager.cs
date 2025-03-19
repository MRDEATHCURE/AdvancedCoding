namespace TopDownShooter.Managers
{
    public class ScoreManager
    {
        public int Score { get; private set; } = 0; // Public read-only property for score

        public void AddScore(int points) // Method to add points to the score
        {
            Score += points; // Increment score by given points
        }

        public void ResetScore() 
        {
            Score = 0; 
        }
    }
}
