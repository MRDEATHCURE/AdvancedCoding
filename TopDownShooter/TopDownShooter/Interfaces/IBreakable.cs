using TopDownShooter.Enums;

namespace TopDownShooter.Interfaces
{
    namespace TopDownShooter
    {
        public interface IBreakable
        {
            void TakeDamage(int damage); // damage to the object
            bool IsDestroyed { get; } // a value whether the object is destroyed
            BuffType GenerateRandomBuff(); // Generates a random buff once the object is destroyed
        }
    }
}
