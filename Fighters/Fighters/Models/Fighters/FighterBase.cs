using Fighters.Models.Armors;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public abstract class FighterBase : IFighter
    {
        protected abstract int ClassDamage { get; }
        protected abstract int ClassHealth { get; }

        private readonly IRace _race;
        private IArmor _armor = new NoArmor();
        private IWeapon _weapon = new Firsts();

        private int _currentHealth;

        public string Name { get; }

        protected FighterBase( string name, IRace race )
        {
            Name = name;
            _race = race;

            _currentHealth = GetMaxHealth();
        }

        public int GetCurrentHealth() => _currentHealth;

        public int GetMaxHealth() => _race.Health + ClassHealth;

        public int CalculateDamage() => _weapon.Damage + _race.Damage + ClassDamage;

        public int CalculateArmor() => _armor.Armor + _race.Armor;

        public int CalculateSpeed() => Math.Max( _race.Speed - _armor.SpeedPenalty, 0 );

        public double GetCritChance() => _weapon.CritChance;

        public void SetArmor( IArmor armor )
        {
            _armor = armor;
        }

        public void SetWeapon( IWeapon weapon )
        {
            _weapon = weapon;
        }

        public void TakeDamage( int damage )
        {
            int newHealth = _currentHealth - damage;
            if ( newHealth < 0 )
            {
                newHealth = 0;
            }

            _currentHealth = newHealth;
        }
    }
}
