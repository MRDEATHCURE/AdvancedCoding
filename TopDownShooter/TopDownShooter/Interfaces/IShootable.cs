using System.Collections.Generic;
using TopDownShooter.Enemies;

namespace TopDownShooter
{
    public interface IShootable
    {
        void Shoot(List<Bullet> bullets, List<BaseEnemy> enemies); // Method  for shooting lists of bullets and enemies
    }

}