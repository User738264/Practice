namespace Fighters.Models.Armors
{
    public class LeatherArmor : IArmor
    {
        public string Name => "Кожаная броня";

        public int Armor => 2;

        public int SpeedPenalty => 3;
    }
}
