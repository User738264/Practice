using Fighters.Models.Armors;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.UI
{
    public class FighterBuilder
    {
        private static readonly IRace[] Races =
        {
            new Human(),
            new Orc(),
            new Elf(),
            new Dwarf(),
        };

        private static readonly IFighterClass[] FighterClasses =
        {
            new Knight(),
            new Mercenary(),
            new Guardian(),
            new Berserker(),
        };

        private static readonly IWeapon[] Weapons =
        {
            new Fists(),
            new Dagger(),
            new Sword(),
            new Axe(),
        };

        private static readonly IArmor[] Armors =
        {
            new NoArmor(),
            new SimpleClothes(),
            new LeatherArmor(),
            new ChainMail(),
            new PlateArmor(),
        };

        private readonly IConsoleIO _io;

        public FighterBuilder() : this( new ConsoleIO() )
        {
        }

        public FighterBuilder( IConsoleIO io )
        {
            _io = io ?? throw new ArgumentNullException( nameof( io ) );
        }

        public IFighter Build( IReadOnlyCollection<string> takenNames )
        {
            _io.WriteLine( "Введите имя персонажа" );
            string name = ReadUniqueName( takenNames );

            IRace race = ChooseRace();
            IFighterClass fighterClass = ChooseFighterClass();
            IWeapon weapon = ChooseWeapon();
            IArmor armor = ChooseArmor();

            IFighter fighter = new Fighter( name, race, fighterClass, weapon, armor );
            PrintSummary( fighter );

            return fighter;
        }

        public IFighter BuildFromPreset( IReadOnlyCollection<string> takenNames )
        {
            (string Name, Func<IFighter> Create)[] available = PresetFighters.All
                .Where( preset => !takenNames.Contains( preset.Name ) )
                .ToArray();

            if ( available.Length == 0 )
            {
                available = PresetFighters.All;
            }

            _io.WriteLine( "Выберите заготовленного персонажа из списка ниже" );

            IFighter[] previews = available.Select( preset => preset.Create() ).ToArray();

            for ( int i = 0; i < previews.Length; i++ )
            {
                _io.WriteLine( $"{i} - {previews[ i ]}" );
            }

            int choice = ReadChoice( 0, previews.Length - 1 );
            IFighter fighter = previews[ choice ];

            PrintSummary( fighter );

            return fighter;
        }

        private IRace ChooseRace()
        {
            _io.WriteLine( "Выберите расу из списка ниже" );

            for ( int i = 0; i < Races.Length; i++ )
            {
                IRace race = Races[ i ];
                _io.WriteLine(
                    $"{i} - {race.Name} (урон +{race.Damage}, здоровье +{race.Health}, броня +{race.Armor}, скорость +{race.Speed})" );
            }

            int choice = ReadChoice( 0, Races.Length - 1 );
            return Races[ choice ];
        }

        private IFighterClass ChooseFighterClass()
        {
            _io.WriteLine( "Выберите класс персонажа из списка ниже" );

            for ( int i = 0; i < FighterClasses.Length; i++ )
            {
                IFighterClass fighterClass = FighterClasses[ i ];
                _io.WriteLine(
                    $"{i} - {fighterClass.Name} (доп. урон +{fighterClass.BonusDamage}, доп. здоровье +{fighterClass.BonusHealth})" );
            }

            int choice = ReadChoice( 0, FighterClasses.Length - 1 );

            if ( choice < 0 || choice >= FighterClasses.Length )
            {
                throw new ArgumentOutOfRangeException( nameof( choice ), choice, "Некорректный выбор класса персонажа" );
            }

            return FighterClasses[ choice ];
        }

        private IWeapon ChooseWeapon()
        {
            _io.WriteLine( "Выберите оружие из списка ниже" );

            for ( int i = 0; i < Weapons.Length; i++ )
            {
                IWeapon weapon = Weapons[ i ];
                _io.WriteLine( $"{i} - {weapon.Name} (урон +{weapon.Damage}, шанс крита {weapon.CritChance} %)" );
            }

            int choice = ReadChoice( 0, Weapons.Length - 1 );
            return Weapons[ choice ];
        }

        private IArmor ChooseArmor()
        {
            _io.WriteLine( "Выберите броню из списка ниже" );

            for ( int i = 0; i < Armors.Length; i++ )
            {
                IArmor armor = Armors[ i ];
                _io.WriteLine( $"{i} - {armor.Name} (броня +{armor.Armor}, штраф к скорости -{armor.SpeedPenalty})" );
            }

            int choice = ReadChoice( 0, Armors.Length - 1 );
            return Armors[ choice ];
        }

        private void PrintSummary( IFighter fighter )
        {
            _io.WriteLine( "Боец добавлен! Итоговые характеристики:" );
            _io.WriteLine( $"Имя: {fighter.Name}" );
            _io.WriteLine( $"Здоровье: {fighter.GetMaxHealth()}" );
            _io.WriteLine( $"Урон: {fighter.CalculateDamage()}" );
            _io.WriteLine( $"Броня: {fighter.CalculateArmor()}" );
            _io.WriteLine( $"Скорость: {fighter.CalculateSpeed()}" );
            _io.WriteLine( $"Шанс критического удара: {fighter.GetCritChance()} %" );
        }

        private string ReadUniqueName( IReadOnlyCollection<string> takenNames )
        {
            string name = ReadNonEmptyLine();

            while ( takenNames.Contains( name ) )
            {
                _io.WriteLine( "Боец с таким именем уже на арене, введите другое имя" );
                name = ReadNonEmptyLine();
            }

            return name;
        }

        private string ReadNonEmptyLine()
        {
            string input = ReadLineOrThrow();

            while ( string.IsNullOrWhiteSpace( input ) )
            {
                _io.WriteLine( "Имя не может быть пустым, попробуйте снова" );
                input = ReadLineOrThrow();
            }

            return input;
        }

        private int ReadChoice( int min, int max )
        {
            while ( true )
            {
                string input = ReadLineOrThrow();

                if ( int.TryParse( input, out int choice ) && choice >= min && choice <= max )
                {
                    return choice;
                }

                _io.WriteLine( $"Некорректный ввод, введите число от {min} до {max}" );
            }
        }

        private string ReadLineOrThrow()
        {
            return _io.ReadLine() ?? throw new EndOfInputException();
        }
    }
}
