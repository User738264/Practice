using Fighters.Models.Races;

namespace Fighters.Models.Fighters
{
    public class Berserker : FighterBase
    {
        public const int BonusDamage = 12;
        public const int BonusHealth = 0;

        protected override int ClassDamage => BonusDamage;

        protected override int ClassHealth => BonusHealth;

        public Berserker( string name, IRace race ) : base( name, race )
        {
        }
    }
}
