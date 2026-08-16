namespace Fighters.Models.Armors
{
    public class SimpleClothes : IArmor
    {
        public string Name => "Простая одежда";
        public int Armor => 1;
        public int SpeedPenalty => 1;
    }
}
