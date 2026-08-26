namespace Fighters.Models.Fighters
{
    public class Guardian : IFighterClass
    {
        public string Name => "Страж";
        public int BonusDamage => 2;
        public int BonusHealth => 40;
    }
}
