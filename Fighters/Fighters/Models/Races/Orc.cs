namespace Fighters.Models.Races
{
    public class Orc : IRace
    {
        public string Name => "Орк";
        public int Damage => 3;
        public int Health => 130;
        public int Armor => 1;
        public int Speed => 4;
    }
}
