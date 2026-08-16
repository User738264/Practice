namespace Fighters.Models.Races
{
    public class Elf : IRace
    {
        public string Name => "Эльф";
        public int Damage => 1;
        public int Health => 80;
        public int Armor => 0;
        public int Speed => 18;
    }
}
