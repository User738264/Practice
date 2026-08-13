using Fighters.Models.Races;

namespace Fighters.Models.Fighters
{
    public class Mercenary : FighterBase
    {
        public const int BonusDamage = 8;
        public const int BonusHealth = 5;

        protected override int ClassDamage => BonusDamage;

        protected override int ClassHealth => BonusHealth;

        public Mercenary( string name, IRace race ) : base( name, race )
        {
        }
    }
}
