namespace Fighters.Models.Races
{
    public class Human : IRace
    {
        public string Name => "Человек";
        public int Damage => 1;
        public int Health => 100;
        public int Armor => 0;
        public int Speed => 10;
    }
}
