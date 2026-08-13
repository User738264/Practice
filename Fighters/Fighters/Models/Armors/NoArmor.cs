namespace Fighters.Models.Armors
{
    public class NoArmor : IArmor
    {
        public string Name => "Без одежды";

        public int Armor => 0;

        public int SpeedPenalty => 0;
    }
}
