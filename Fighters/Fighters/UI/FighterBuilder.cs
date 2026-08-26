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

        public IFighter Build( IReadOnlyCollection<string> takenNames )
        {
            Console.WriteLine( "Введите имя персонажа" );
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

            Console.WriteLine( "Выберите заготовленного персонажа из списка ниже" );

            IFighter[] previews = available.Select( preset => preset.Create() ).ToArray();

            for ( int i = 0; i < previews.Length; i++ )
            {
                Console.WriteLine( $"{i} - {previews[ i ]}" );
            }

            int choice = ReadChoice( 0, previews.Length - 1 );
            IFighter fighter = previews[ choice ];

            PrintSummary( fighter );

            return fighter;
        }

        private IRace ChooseRace()
        {
            Console.WriteLine( "Выберите расу из списка ниже" );

            for ( int i = 0; i < Races.Length; i++ )
            {
                IRace race = Races[ i ];
                Console.WriteLine(
                    $"{i} - {race.Name} (урон +{race.Damage}, здоровье +{race.Health}, броня +{race.Armor}, скорость +{race.Speed})" );
            }

            int choice = ReadChoice( 0, Races.Length - 1 );
            return Races[ choice ];
        }

        private IFighterClass ChooseFighterClass()
        {
            Console.WriteLine( "Выберите класс персонажа из списка ниже" );

            for ( int i = 0; i < FighterClasses.Length; i++ )
            {
                IFighterClass fighterClass = FighterClasses[ i ];
                Console.WriteLine(
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
            Console.WriteLine( "Выберите оружие из списка ниже" );

            for ( int i = 0; i < Weapons.Length; i++ )
            {
                IWeapon weapon = Weapons[ i ];
                Console.WriteLine( $"{i} - {weapon.Name} (урон +{weapon.Damage}, шанс крита {weapon.CritChance} %)" );
            }

            int choice = ReadChoice( 0, Weapons.Length - 1 );
            return Weapons[ choice ];
        }

        private IArmor ChooseArmor()
        {
            Console.WriteLine( "Выберите броню из списка ниже" );

            for ( int i = 0; i < Armors.Length; i++ )
            {
                IArmor armor = Armors[ i ];
                Console.WriteLine( $"{i} - {armor.Name} (броня +{armor.Armor}, штраф к скорости -{armor.SpeedPenalty})" );
            }

            int choice = ReadChoice( 0, Armors.Length - 1 );
            return Armors[ choice ];
        }

        private void PrintSummary( IFighter fighter )
        {
            Console.WriteLine( "Боец добавлен! Итоговые характеристики:" );
            Console.WriteLine( $"Имя: {fighter.Name}" );
            Console.WriteLine( $"Здоровье: {fighter.GetMaxHealth()}" );
            Console.WriteLine( $"Урон: {fighter.CalculateDamage()}" );
            Console.WriteLine( $"Броня: {fighter.CalculateArmor()}" );
            Console.WriteLine( $"Скорость: {fighter.CalculateSpeed()}" );
            Console.WriteLine( $"Шанс критического удара: {fighter.GetCritChance()} %" );
        }

        private string ReadUniqueName( IReadOnlyCollection<string> takenNames )
        {
            string name = ReadNonEmptyLine();

            while ( takenNames.Contains( name ) )
            {
                Console.WriteLine( "Боец с таким именем уже на арене, введите другое имя" );
                name = ReadNonEmptyLine();
            }

            return name;
        }

        private string ReadNonEmptyLine()
        {
            string input = ReadLineOrThrow();

            while ( string.IsNullOrWhiteSpace( input ) )
            {
                Console.WriteLine( "Имя не может быть пустым, попробуйте снова" );
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

                Console.WriteLine( $"Некорректный ввод, введите число от {min} до {max}" );
            }
        }

        private string ReadLineOrThrow()
        {
            return Console.ReadLine() ?? throw new EndOfInputException();
        }
    }
}
