using Fighters.Models.Fighters;
using Fighters.Randomization;
using Fighters.UI;

namespace Fighters
{
    public class GameManager
    {
        private const int MinFightersToStartBattle = 2;

        private const int MaxRounds = 1000;

        private const double MinDamageVariance = -0.20;
        private const double MaxDamageVariance = 0.10;

        private const double CriticalDamageMultiplier = 2.0;

        private const int MinDamagePerHit = 1;

        private readonly IRandomProvider _random;
        private readonly IConsoleIO _io;

        public GameManager() : this( new RandomProvider(), new ConsoleIO() )
        {
        }

        public GameManager( IRandomProvider randomProvider ) : this( randomProvider, new ConsoleIO() )
        {
        }

        public GameManager( IRandomProvider randomProvider, IConsoleIO io )
        {
            _random = randomProvider ?? throw new ArgumentNullException( nameof( randomProvider ) );
            _io = io ?? throw new ArgumentNullException( nameof( io ) );
        }

        public IFighter Play( IReadOnlyList<IFighter> fighters )
        {
            ArgumentNullException.ThrowIfNull( fighters );

            if ( fighters.Count < MinFightersToStartBattle )
            {
                throw new ArgumentException(
                    $"Для проведения битвы нужно как минимум {MinFightersToStartBattle} бойца(ов)", nameof( fighters ) );
            }

            List<IFighter> turnOrder = fighters.OrderByDescending( fighter => fighter.CalculateSpeed() ).ToList();
            PrintTurnOrder( turnOrder );

            int round = 1;

            while ( CountAlive( fighters ) > 1 && round <= MaxRounds )
            {
                _io.WriteLine( $"Раунд {round}" );

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

                    IFighter target = possibleTargets[ _random.Next( possibleTargets.Count ) ];

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
            _io.WriteLine( "Порядок ходов (по инициативе):" );

            foreach ( IFighter fighter in turnOrder )
            {
                _io.WriteLine( $"  {fighter.Name} (скорость {fighter.CalculateSpeed()})" );
            }
        }

        private void PrintAttack( IFighter attacker, IFighter target, int damage, bool isCritical )
        {
            string critNote = isCritical ? " (критический удар!)" : string.Empty;
            _io.WriteLine( $"{attacker.Name} атакует {target.Name} и наносит {damage} урона{critNote}" );

            if ( !target.IsAlive() )
            {
                _io.WriteLine( $"{target.Name} погибает" );
            }
        }

        private int CalculateDamageDealt( IFighter attacker, IFighter defender, out bool isCritical )
        {
            double variance = MinDamageVariance + ( _random.NextDouble() * ( MaxDamageVariance - MinDamageVariance ) );
            double damage = attacker.CalculateDamage() * ( 1 + variance );

            isCritical = _random.NextDouble() < attacker.GetCritChance();
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
                _io.WriteLine( $"{survivors[ 0 ].Name} остается единственным на арене и побеждает!" );
                return survivors[ 0 ];
            }

            if ( survivors.Count == 0 )
            {
                _io.WriteLine( "Все бойцы пали одновременно — победитель определяется случайно" );
                return fighters[ _random.Next( fighters.Count ) ];
            }

            _io.WriteLine( "Битва слишком затянулась — победитель определяется случайно" );
            IFighter winner = survivors[ _random.Next( survivors.Count ) ];
            _io.WriteLine( $"По воле случая побеждает {winner.Name}!" );
            return winner;
        }
    }
}
