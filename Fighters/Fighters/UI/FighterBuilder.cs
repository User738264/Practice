using Fighters.Models.Armors;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.UI
{
    public class FighterBuilder
    {
        public IFighter Build( IReadOnlyCollection<string> takenNames )
        {
            Console.WriteLine( "Введите имя персонажа" );
            string name = ReadUniqueName( takenNames );

            IRace race = ChooseRace();
            IFighter fighter = ChooseFighterClass( name, race );

            IWeapon weapon = ChooseWeapon();
            fighter.SetWeapon( weapon );

            IArmor armor = ChooseArmor();
            fighter.SetArmor( armor );

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
                IFighter preview = previews[ i ];
                Console.WriteLine(
                    $"{i} - {preview.Name} (урон {preview.CalculateDamage()}, здоровье {preview.GetMaxHealth()}, броня {preview.CalculateArmor()}, скорость {preview.CalculateSpeed()}, крит {preview.GetCritChance()} %)" );
            }

            int choice = ReadChoice( 0, previews.Length - 1 );
            IFighter fighter = previews[ choice ];

            PrintSummary( fighter );

            return fighter;
        }

        private IRace ChooseRace()
        {
            IRace[] races =
            {
                new Human(),
                new Orc(),
                new Elf(),
                new Dwarf(),
            };

            Console.WriteLine( "Выберите расу из списка ниже" );

            for ( int i = 0; i < races.Length; i++ )
            {
                IRace race = races[ i ];
                Console.WriteLine(
                    $"{i} - {race.Name} (урон +{race.Damage}, здоровье +{race.Health}, броня +{race.Armor}, скорость +{race.Speed})" );
            }

            int choice = ReadChoice( 0, races.Length - 1 );
            return races[ choice ];
        }

        private IFighter ChooseFighterClass( string name, IRace race )
        {
            Console.WriteLine( "Выберите класс персонажа из списка ниже" );
            Console.WriteLine( $"0 - Рыцарь (доп. урон +{Knight.BonusDamage}, доп. здоровье +{Knight.BonusHealth})" );
            Console.WriteLine( $"1 - Наемник (доп. урон +{Mercenary.BonusDamage}, доп. здоровье +{Mercenary.BonusHealth})" );
            Console.WriteLine( $"2 - Страж (доп. урон +{Guardian.BonusDamage}, доп. здоровье +{Guardian.BonusHealth})" );
            Console.WriteLine( $"3 - Берсерк (доп. урон +{Berserker.BonusDamage}, доп. здоровье +{Berserker.BonusHealth})" );

            int choice = ReadChoice( 0, 3 );

            return choice switch
            {
                0 => new Knight( name, race ),
                1 => new Mercenary( name, race ),
                2 => new Guardian( name, race ),
                3 => new Berserker( name, race ),
                _ => new Knight( name, race ),
            };
        }

        private IWeapon ChooseWeapon()
        {
            IWeapon[] weapons =
            {
                new Firsts(),
                new Dagger(),
                new Sword(),
                new Axe(),
            };

            Console.WriteLine( "Выберите оружие из списка ниже" );

            for ( int i = 0; i < weapons.Length; i++ )
            {
                IWeapon weapon = weapons[ i ];
                Console.WriteLine( $"{i} - {weapon.Name} (урон +{weapon.Damage}, шанс крита {weapon.CritChance} %)" );
            }

            int choice = ReadChoice( 0, weapons.Length - 1 );
            return weapons[ choice ];
        }

        private IArmor ChooseArmor()
        {
            IArmor[] armors =
            {
                new NoArmor(),
                new SimpleClothes(),
                new LeatherArmor(),
                new ChainMail(),
                new PlateArmor(),
            };

            Console.WriteLine( "Выберите броню из списка ниже" );

            for ( int i = 0; i < armors.Length; i++ )
            {
                IArmor armor = armors[ i ];
                Console.WriteLine( $"{i} - {armor.Name} (броня +{armor.Armor}, штраф к скорости -{armor.SpeedPenalty})" );
            }

            int choice = ReadChoice( 0, armors.Length - 1 );
            return armors[ choice ];
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
