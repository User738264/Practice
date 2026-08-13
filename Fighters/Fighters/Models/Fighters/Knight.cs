using Fighters.Models.Races;

namespace Fighters.Models.Fighters
{
    public class Knight : FighterBase
    {
        public const int BonusDamage = 5;
        public const int BonusHealth = 20;

        protected override int ClassDamage => BonusDamage;

        protected override int ClassHealth => BonusHealth;

        public Knight( string name, IRace race ) : base( name, race )
        {
        }
    }
}
