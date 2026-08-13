using Fighters.Models.Fighters;

namespace Fighters.Extensions
{
    public static class IFighterExtensions
    {
        public static bool IsAlive( this IFighter fighter ) => fighter.GetCurrentHealth() > 0;
    }
}