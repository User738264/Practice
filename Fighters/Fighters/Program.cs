using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters
{
    public class Program
    {
        private const int MinFightersPerBattle = 2;
        private const int MaxFightersPerBattle = 6;

        public static void Main( string[] args )
        {
            var fighterBuilder = new FighterBuilder();
            var gameManager = new GameManager();
            var arena = new List<IFighter>();

            PrintHelp();

            bool isRunning = true;

            while ( isRunning )
            {
                string? rawCommand = Console.ReadLine();

                if ( rawCommand is null )
                {
                    break;
                }

                string command = rawCommand.Trim().ToLowerInvariant();

                try
                {
                    switch ( command )
                    {
                        case "add-fighter":
                            AddFighter( fighterBuilder, arena );
                            break;

                        case "add-preset":
                            AddPreset( fighterBuilder, arena );
                            break;

                        case "play":
                            Play( gameManager, arena );
                            break;

                        case "reset":
                            Reset( arena );
                            break;

                        case "arena":
                            PrintArena( arena );
                            break;

                        case "exit":
                            isRunning = false;
                            break;

                        default:
                            Console.WriteLine( "Неизвестная команда, попробуйте снова" );
                            break;
                    }
                }
                catch ( EndOfInputException )
                {
                    isRunning = false;
                }
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine( "Введите команду" );
            Console.WriteLine( "add-fighter - Настроить нового бойца и добавить на арену" );
            Console.WriteLine( "add-preset - Добавить на арену готового заготовленного персонажа" );
            Console.WriteLine( $"play - Начать битву (нужно от {MinFightersPerBattle} до {MaxFightersPerBattle} бойцов)" );
            Console.WriteLine( "reset - Очистить арену, не начиная битву" );
            Console.WriteLine( "arena - Показать текущий состав арены" );
            Console.WriteLine( "exit - Выйти из игры" );
        }

        private static void AddFighter( FighterBuilder fighterBuilder, List<IFighter> arena )
        {
            if ( !TryReserveSlot( arena ) )
            {
                return;
            }

            IFighter fighter = fighterBuilder.Build( GetTakenNames( arena ) );
            arena.Add( fighter );
        }

        private static void AddPreset( FighterBuilder fighterBuilder, List<IFighter> arena )
        {
            if ( !TryReserveSlot( arena ) )
            {
                return;
            }

            IFighter fighter = fighterBuilder.BuildFromPreset( GetTakenNames( arena ) );
            arena.Add( fighter );
        }

        private static List<string> GetTakenNames( List<IFighter> arena )
        {
            return arena.Select( fighter => fighter.Name ).ToList();
        }

        private static bool TryReserveSlot( List<IFighter> arena )
        {
            if ( arena.Count >= MaxFightersPerBattle )
            {
                Console.WriteLine( $"На арене уже максимум бойцов ({MaxFightersPerBattle}), введите play, чтобы начать битву" );
                return false;
            }

            return true;
        }

        private static void Play( GameManager gameManager, List<IFighter> arena )
        {
            if ( arena.Count < MinFightersPerBattle )
            {
                Console.WriteLine( $"Для начала битвы нужно как минимум {MinFightersPerBattle} бойца(ов): add-fighter или add-preset" );
                return;
            }

            gameManager.Play( arena );

            arena.Clear();
        }

        private static void Reset( List<IFighter> arena )
        {
            if ( arena.Count == 0 )
            {
                Console.WriteLine( "Арена уже пуста" );
                return;
            }

            arena.Clear();
            Console.WriteLine( "Арена очищена" );
        }

        private static void PrintArena( List<IFighter> arena )
        {
            if ( arena.Count == 0 )
            {
                Console.WriteLine( "Арена пуста" );
                return;
            }

            Console.WriteLine( $"На арене {arena.Count} из {MaxFightersPerBattle}:" );

            for ( int i = 0; i < arena.Count; i++ )
            {
                Console.WriteLine( $"{i + 1}. {arena[ i ]}" );
            }
        }
    }
}

