using Fighters.Extensions;
using Fighters.Models.Fighters;

namespace Fighters
{
    public class GameManager
    {
        private const int MaxRounds = 1000;

        private const double MinDamageVariance = -0.20;
        private const double MaxDamageVariance = 0.10;

        private const double CriticalDamageMultiplier = 2.0;

        private const int MinDamagePerHit = 1;

        private readonly Random Random = new();

        public IFighter Play( IReadOnlyList<IFighter> fighters )
        {
            List<IFighter> turnOrder = fighters.OrderByDescending( fighter => fighter.CalculateSpeed() ).ToList();
            PrintTurnOrder( turnOrder );

            int round = 1;

            while ( CountAlive( fighters ) > 1 && round <= MaxRounds )
            {
                Console.WriteLine( $"Раунд {round}" );

                foreach ( IFighter attacker in turnOrder )
                {
                    if ( !attacker.IsAlive() )
                    {
                        continue;
                    }

                    List<IFighter> possibleTargets = fighters.Where( f => f != attacker && f.IsAlive() ).ToList();
                    if ( possibleTargets.Count == 0 )
                    {
                        break;
                    }

                    IFighter target = possibleTargets[ Random.Next( possibleTargets.Count ) ];

                    int damage = CalculateDamageDealt( attacker, target, out bool isCritical );
                    target.TakeDamage( damage );

                    PrintAttack( attacker, target, damage, isCritical );
                }

                round++;
            }

            return AnnounceResult( fighters );
        }

        private int CountAlive( IReadOnlyList<IFighter> fighters )
        {
            int alive = 0;

            foreach ( IFighter fighter in fighters )
            {
                if ( fighter.IsAlive() )
                {
                    alive++;
                }
            }

            return alive;
        }

        private void PrintTurnOrder( List<IFighter> turnOrder )
        {
            Console.WriteLine( "Порядок ходов (по инициативе):" );

            foreach ( IFighter fighter in turnOrder )
            {
                Console.WriteLine( $"  {fighter.Name} (скорость {fighter.CalculateSpeed()})" );
            }
        }

        private void PrintAttack( IFighter attacker, IFighter target, int damage, bool isCritical )
        {
            string critNote = isCritical ? " (критический удар!)" : string.Empty;
            Console.WriteLine( $"{attacker.Name} атакует {target.Name} и наносит {damage} урона{critNote}" );

            if ( !target.IsAlive() )
            {
                Console.WriteLine( $"{target.Name} погибает" );
            }
        }

        private int CalculateDamageDealt( IFighter attacker, IFighter defender, out bool isCritical )
        {
            double variance = MinDamageVariance + ( Random.NextDouble() * ( MaxDamageVariance - MinDamageVariance ) );
            double damage = attacker.CalculateDamage() * ( 1 + variance );

            isCritical = Random.NextDouble() < attacker.GetCritChance();
            if ( isCritical )
            {
                damage *= CriticalDamageMultiplier;
            }

            int finalDamage = ( int )Math.Round( damage ) - defender.CalculateArmor();
            return Math.Max( finalDamage, MinDamagePerHit );
        }

        private IFighter AnnounceResult( IReadOnlyList<IFighter> fighters )
        {
            List<IFighter> survivors = fighters.Where( f => f.IsAlive() ).ToList();

            if ( survivors.Count == 1 )
            {
                Console.WriteLine( $"{survivors[ 0 ].Name} остается единственным на арене и побеждает!" );
                return survivors[ 0 ];
            }

            if ( survivors.Count == 0 )
            {
                Console.WriteLine( "Все бойцы пали одновременно — победитель определяется случайно" );
                return fighters[ Random.Next( fighters.Count ) ];
            }

            Console.WriteLine( "Битва слишком затянулась — победитель определяется случайно" );
            IFighter winner = survivors[ Random.Next( survivors.Count ) ];
            Console.WriteLine( $"По воле случая побеждает {winner.Name}!" );
            return winner;
        }
    }
}
