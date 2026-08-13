using Fighters.Models.Armors;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public interface IFighter
    {
        string Name { get; }

        public int GetCurrentHealth();
        public int GetMaxHealth();
        public int CalculateDamage();
        public int CalculateArmor();
        public int CalculateSpeed();
        public double GetCritChance();

        public void SetArmor( IArmor armor );
        public void SetWeapon( IWeapon weapon );

        public void TakeDamage( int damage );
    }
}
