namespace Fighters.Models.Fighters
{
    public interface IFighter
    {
        string Name { get; }

        public int GetCurrentHealth();
        public int GetMaxHealth();
        public int CalculateDamage();
        public int CalculateArmor();
        public int CalculateSpeed();
        public double GetCritChance();
        public bool IsAlive();

        public void TakeDamage( int damage );
    }
}
