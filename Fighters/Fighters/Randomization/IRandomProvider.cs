namespace Fighters.Randomization
{
    public interface IRandomProvider
    {
        public int Next( int maxValueExclusive );
        public double NextDouble();
    }
}
