namespace Fighters.Models.Fighters
{
    public interface IFighterClass
    {
        public string Name { get; }
        public int BonusDamage { get; }
        public int BonusHealth { get; }
    }
}
