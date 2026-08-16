using Fighters.Models.Armors;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public class Fighter : IFighter
    {
        private readonly IRace _race;
        private readonly IWeapon _weapon;
        private readonly IArmor _armor;
        private readonly int _classDamage;
        private readonly int _classHealth;

        private int _currentHealth;

        public string Name { get; }

        public Fighter( string name, IRace race, IFighterClass fighterClass, IWeapon weapon, IArmor armor )
        {
            Name = name;
            _race = race;
            _classDamage = fighterClass.BonusDamage;
            _classHealth = fighterClass.BonusHealth;
            _weapon = weapon;
            _armor = armor;
            _currentHealth = GetMaxHealth();
        }

        public int GetCurrentHealth() => _currentHealth;
        public int GetMaxHealth() => _race.Health + _classHealth;
        public int CalculateDamage() => _weapon.Damage + _race.Damage + _classDamage;
        public int CalculateArmor() => _armor.Armor + _race.Armor;
        public int CalculateSpeed() => Math.Max( _race.Speed - _armor.SpeedPenalty, 0 );
        public double GetCritChance() => _weapon.CritChance;
        public bool IsAlive() => _currentHealth > 0;

        public void TakeDamage( int damage )
        {
            int newHealth = _currentHealth - damage;
            if ( newHealth < 0 )
            {
                newHealth = 0;
            }

            _currentHealth = newHealth;
        }

        public override string ToString() =>
            $"{Name} — здоровье {GetMaxHealth()}, урон {CalculateDamage()}, броня {CalculateArmor()}, скорость {CalculateSpeed()}, крит {GetCritChance()} %";
    }
}
