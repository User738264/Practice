namespace Fighters.Randomization
{
    public class RandomProvider : IRandomProvider
    {
        private readonly Random _random = new();

        public int Next( int maxValueExclusive ) => _random.Next( maxValueExclusive );
        public double NextDouble() => _random.NextDouble();
    }
}
