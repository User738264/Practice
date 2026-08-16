namespace Fighters.Models.Weapons
{
    public class Axe : IWeapon
    {
        public string Name => "Топор";
        public int Damage => 7;
        public double CritChance => 0.05;
    }
}
