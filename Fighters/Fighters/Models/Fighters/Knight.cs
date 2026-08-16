namespace Fighters.Models.Fighters
{
    public class Knight : IFighterClass
    {
        public string Name => "Рыцарь";
        public int BonusDamage => 5;
        public int BonusHealth => 20;
    }
}
