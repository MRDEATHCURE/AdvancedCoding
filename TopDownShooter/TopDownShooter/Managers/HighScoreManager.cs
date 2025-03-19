using System;
using System.IO;

namespace TopDownShooter.Managers 
{
    public class HighScoreManager
    {
        private string filePath = "highscores.txt"; // Path to the high score file

        public int LoadHighScore() // Loads the high score from file  
        {
            try
            {
                if (File.Exists(filePath)) // Check if the file exists  
                {
                    string line = File.ReadAllText(filePath); // Read file   
                    if (int.TryParse(line, out int highScore)) // Try parsing the string to int  
                    {
                        return highScore; // Return the high score if parsing succeeful  
                    }
                }
            }
            catch (Exception exception)  // Catch exceptions during file operations  
            {
                Console.WriteLine("Error loading high score: " + exception.Message);  
            }
            return 0; // Return 0 if file doesnt exist or parsing fails  
        }

        public void SaveHighScore(int newScore)     // Saves the high score if the new score is higher  
        {
            int currentHighScore = LoadHighScore(); // Load current high score  
            if (newScore > currentHighScore)        // Only save if new score is higher  
            {
                try
                {
                    File.WriteAllText(filePath, newScore.ToString()); // Write new score to file  
                }
                catch (Exception exception) // Catch any exceptions during file write  
                {
                    Console.WriteLine("Error saving high score: " + exception.Message); 
                }
            }
        }
    }
}
