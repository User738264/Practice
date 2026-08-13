namespace Fighters.Models.Weapons
{
    public class Firsts : IWeapon
    {
        public string Name => "Без оружия";

        public int Damage => 1;

        public double CritChance => 0.05;
    }
}
