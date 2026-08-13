namespace Fighters.Models.Weapons
{
    public class Dagger : IWeapon
    {
        public string Name => "Кинжал";

        public int Damage => 3;

        public double CritChance => 0.25;
    }
}
