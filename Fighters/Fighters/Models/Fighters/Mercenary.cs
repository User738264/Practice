namespace Fighters.Models.Fighters
{
    public class Mercenary : IFighterClass
    {
        public string Name => "Наемник";
        public int BonusDamage => 8;
        public int BonusHealth => 5;
    }
}
