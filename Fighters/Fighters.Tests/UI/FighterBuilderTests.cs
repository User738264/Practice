using Fighters.Models.Fighters;
using Fighters.UI;
using Moq;
using Xunit;

namespace Fighters.Tests.UI
{
    public class FighterBuilderTests
    {
        private static readonly string[] AllPresetNames =
        {
            "Громила", "Страж Ворот", "Теневой Клинок", "Ополченец", "Кровавый Клинок", "Хранитель Рощи",
        };

        [Fact]
        public void Constructor_IoIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>( () => new FighterBuilder( null! ) );
        }

        [Fact]
        public void Build_ValidSelections_CreatesFighterWithChosenAttributes()
        {
            // Arrange
            (FighterBuilder builder, _) = CreateBuilder( "Артур", "0", "0", "0", "0" );

            // Act
            IFighter fighter = builder.Build( Array.Empty<string>() );

            // Assert
            // Human + Knight + Fists + No armor
            Assert.Equal( "Артур", fighter.Name );
            Assert.Equal( 120, fighter.GetMaxHealth() );
            Assert.Equal( 7, fighter.CalculateDamage() );
            Assert.Equal( 0, fighter.CalculateArmor() );
            Assert.Equal( 10, fighter.CalculateSpeed() );
            Assert.Equal( 0.05, fighter.GetCritChance() );
        }

        [Fact]
        public void Build_EmptyOrWhitespaceNameInput_PromptsAgainUntilValidNameProvided()
        {
            // Arrange
            (FighterBuilder builder, Mock<IConsoleIO> io) =
                CreateBuilder( "", "   ", "Валидное Имя", "0", "0", "0", "0" );

            // Act
            IFighter fighter = builder.Build( Array.Empty<string>() );

            // Assert
            Assert.Equal( "Валидное Имя", fighter.Name );
            io.Verify( x => x.ReadLine(), Times.Exactly( 7 ) );
        }

        [Fact]
        public void Build_NameAlreadyTaken_PromptsAgainAndUsesUniqueName()
        {
            // Arrange
            (FighterBuilder builder, _) = CreateBuilder( "Григорий", "Александр", "0", "0", "0", "0" );

            // Act
            IFighter fighter = builder.Build( new[] { "Григорий" } );

            // Assert
            Assert.Equal( "Александр", fighter.Name );
        }

        [Fact]
        public void Build_InvalidThenOutOfRangeRaceChoice_PromptsAgainAndUsesValidChoice()
        {
            // Arrange
            (FighterBuilder builder, _) = CreateBuilder( "Тест", "abc", "99", "1", "0", "0", "0" );

            // Act
            IFighter fighter = builder.Build( Array.Empty<string>() );

            // Assert
            // Orc (index 1) + Knight + Fists + No armor
            Assert.Equal( 150, fighter.GetMaxHealth() );
            Assert.Equal( 9, fighter.CalculateDamage() );
            Assert.Equal( 1, fighter.CalculateArmor() );
            Assert.Equal( 4, fighter.CalculateSpeed() );
        }

        [Fact]
        public void Build_NoMoreInputDuringPrompt_ThrowsEndOfInputException()
        {
            // Arrange
            var io = new Mock<IConsoleIO>();
            io.Setup( x => x.ReadLine() ).Returns( ( string? )null );
            var builder = new FighterBuilder( io.Object );

            // Assert
            Assert.Throws<EndOfInputException>( () => builder.Build( Array.Empty<string>() ) );
        }

        [Fact]
        public void BuildFromPreset_ValidChoice_ReturnsMatchingPresetFighter()
        {
            // Arrange
            (FighterBuilder builder, _) = CreateBuilder( "0" );

            // Act
            IFighter fighter = builder.BuildFromPreset( Array.Empty<string>() );

            // Assert
            // Brute: Orc + Berserker + Axe + No armor
            Assert.Equal( "Громила", fighter.Name );
            Assert.Equal( 130, fighter.GetMaxHealth() );
            Assert.Equal( 22, fighter.CalculateDamage() );
            Assert.Equal( 1, fighter.CalculateArmor() );
            Assert.Equal( 4, fighter.CalculateSpeed() );
            Assert.Equal( 0.05, fighter.GetCritChance() );
        }

        [Fact]
        public void BuildFromPreset_NameAlreadyTaken_ExcludesItFromAvailableChoices()
        {
            // Arrange
            (FighterBuilder builder, _) = CreateBuilder( "0" );

            // Act
            IFighter fighter = builder.BuildFromPreset( new[] { "Громила" } );

            // Assert
            // Brute is excluded from the list, so index 0 points to the next preset in order
            Assert.Equal( "Страж Ворот", fighter.Name );
        }

        [Fact]
        public void BuildFromPreset_AllPresetNamesTaken_FallsBackToFullPresetList()
        {
            // Arrange
            (FighterBuilder builder, _) = CreateBuilder( "5" );

            // Act
            IFighter fighter = builder.BuildFromPreset( AllPresetNames );

            // Assert
            // When all presets are taken, the list resets to the full list, so index 5 is valid again
            Assert.Equal( "Хранитель Рощи", fighter.Name );
        }

        private static (FighterBuilder Builder, Mock<IConsoleIO> Io) CreateBuilder( params string?[] inputLines )
        {
            var io = new Mock<IConsoleIO>();

            var sequence = io.SetupSequence( x => x.ReadLine() );
            foreach ( string? line in inputLines )
            {
                sequence = sequence.Returns( line );
            }

            return (new FighterBuilder( io.Object ), io);
        }
    }
}
