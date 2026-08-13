using Fighters.Models.Races;

namespace Fighters.Models.Fighters
{
    public class Guardian : FighterBase
    {
        public const int BonusDamage = 2;
        public const int BonusHealth = 40;

        protected override int ClassDamage => BonusDamage;

        protected override int ClassHealth => BonusHealth;

        public Guardian( string name, IRace race ) : base( name, race )
        {
        }
    }
}
