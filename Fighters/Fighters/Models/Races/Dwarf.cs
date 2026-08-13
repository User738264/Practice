namespace Fighters.Models.Races
{
    public class Dwarf : IRace
    {
        public string Name => "Гном";

        public int Damage => 2;

        public int Health => 110;

        public int Armor => 2;

        public int Speed => 6;
    }
}
