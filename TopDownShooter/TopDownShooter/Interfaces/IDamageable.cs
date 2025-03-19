namespace TopDownShooter
{
    public interface IDamageable  
    {
        void TakeDamage(int amount); // Method to apply damage to object
        bool IsDead(); // Method that returns true if object is dead
    }
}