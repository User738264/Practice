namespace Fighters.Models.Weapons
{
    public class Sword : IWeapon
    {
        public string Name => "Меч";

        public int Damage => 5;

        public double CritChance => 0.10;
    }
}
