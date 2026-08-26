using Fighters;
using Fighters.Models.Fighters;
using Fighters.Randomization;
using Fighters.UI;
using Moq;
using Xunit;

namespace Fighters.Tests
{
    public class GameManagerTests
    {
        [Fact]
        public void Constructor_RandomProviderIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>( () => new GameManager( null! ) );
        }

        [Fact]
        public void Play_FightersIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            GameManager gameManager = CreateGameManager();

            // Assert
            Assert.Throws<ArgumentNullException>( () => gameManager.Play( null! ) );
        }

        [Theory]
        [InlineData( 0 )]
        [InlineData( 1 )]
        public void Play_FewerThanTwoFighters_ThrowsArgumentException( int fighterCount )
        {
            // Arrange
            GameManager gameManager = CreateGameManager();
            List<IFighter> fighters = Enumerable.Range( 0, fighterCount )
                .Select( i => CreateFighterMock( $"Боец{i}" ).Object )
                .ToList();

            // Assert
            Assert.Throws<ArgumentException>( () => gameManager.Play( fighters ) );
        }

        [Fact]
        public void Play_AllFightersAlreadyDead_ReturnsRandomFighterFromOriginalList()
        {
            // Arrange
            Mock<IFighter> deadFighterA = CreateFighterMock( "Погибший А", health: 0 );
            Mock<IFighter> deadFighterB = CreateFighterMock( "Погибший Б", health: 0 );
            var fighters = new List<IFighter> { deadFighterA.Object, deadFighterB.Object };

            var randomProviderMock = new Mock<IRandomProvider>();
            randomProviderMock.Setup( r => r.Next( fighters.Count ) ).Returns( 0 );

            GameManager gameManager = CreateGameManager( randomProviderMock );

            // Act
            IFighter winner = gameManager.Play( fighters );

            // Assert
            Assert.Same( deadFighterA.Object, winner );
        }

        [Fact]
        public void Play_FasterFighterAttacksFirst_KillsWeakerFighterAndWins()
        {
            // Arrange
            Mock<IFighter> fast = CreateFighterMock( "Быстрый", speed: 10, damage: 50, armor: 0, critChance: 0, health: 100 );
            Mock<IFighter> slow = CreateFighterMock( "Медленный", speed: 5, damage: 1, armor: 0, critChance: 0, health: 10 );
            var fighters = new List<IFighter> { fast.Object, slow.Object };

            GameManager gameManager = CreateGameManager();

            // Act
            IFighter winner = gameManager.Play( fighters );

            // Assert
            // Damage = 50 * (1 - 0.20) = 40; without armor or a critical hit => 40
            Assert.Same( fast.Object, winner );
            slow.Verify( f => f.TakeDamage( 40 ), Times.Once );
            fast.Verify( f => f.CalculateDamage(), Times.Once );
            slow.Verify( f => f.CalculateDamage(), Times.Never );
        }

        [Fact]
        public void Play_TurnOrderDependsOnSpeedNotListOrder_FasterFighterAttacksFirst()
        {
            // Arrange
            Mock<IFighter> slow = CreateFighterMock( "Медленный", speed: 5, damage: 1, armor: 0, critChance: 0, health: 10 );
            Mock<IFighter> fast = CreateFighterMock( "Быстрый", speed: 10, damage: 50, armor: 0, critChance: 0, health: 100 );

            // The slow fighter is first in the list but should attack second
            var fighters = new List<IFighter> { slow.Object, fast.Object };

            GameManager gameManager = CreateGameManager();

            // Act
            IFighter winner = gameManager.Play( fighters );

            // Assert
            Assert.Same( fast.Object, winner );
            slow.Verify( f => f.CalculateDamage(), Times.Never );
        }

        [Fact]
        public void Play_AttackerAlwaysCrits_AppliesDoubleDamageMultiplier()
        {
            // Arrange
            Mock<IFighter> attacker = CreateFighterMock( "Критующий", speed: 10, damage: 10, armor: 0, critChance: 1.0, health: 100 );
            Mock<IFighter> defender = CreateFighterMock( "Жертва", speed: 5, damage: 1, armor: 0, critChance: 0, health: 10 );
            var fighters = new List<IFighter> { attacker.Object, defender.Object };

            GameManager gameManager = CreateGameManager();

            // Act
            IFighter winner = gameManager.Play( fighters );

            // Assert
            // Base damage 10 * (1 - 0.20) = 8; critical hit x2 = 16; without armor => 16
            defender.Verify( f => f.TakeDamage( 16 ), Times.Once );
            Assert.Same( attacker.Object, winner );
        }

        [Fact]
        public void Play_MaxDamageVarianceRoll_IncreasesDamageByMaximumBonus()
        {
            // Arrange
            Mock<IFighter> attacker = CreateFighterMock( "Атакующий", speed: 10, damage: 10, armor: 0, critChance: 0, health: 100 );
            Mock<IFighter> defender = CreateFighterMock( "Защитник", speed: 5, damage: 1, armor: 0, critChance: 0, health: 5 );
            var fighters = new List<IFighter> { attacker.Object, defender.Object };

            var randomProviderMock = new Mock<IRandomProvider>();
            randomProviderMock.SetupSequence( r => r.NextDouble() )
                .Returns( 1.0 )   // Maximum damage variance
                .Returns( 0.0 );  // No critical hit

            GameManager gameManager = CreateGameManager( randomProviderMock );

            // Act
            gameManager.Play( fighters );

            // Assert
            // Base damage 10 * (1 + 0.10) = 11; without armor => 11
            defender.Verify( f => f.TakeDamage( 11 ), Times.Once );
        }

        [Fact]
        public void Play_DamageLowerThanDefenderArmor_ClampsToMinimumDamagePerHit()
        {
            // Arrange
            Mock<IFighter> attacker = CreateFighterMock( "Слабый атакующий", speed: 10, damage: 1, armor: 0, critChance: 0, health: 100 );
            Mock<IFighter> defender = CreateFighterMock( "Хорошо бронированный", speed: 5, damage: 1, armor: 50, critChance: 0, health: 1 );
            var fighters = new List<IFighter> { attacker.Object, defender.Object };

            GameManager gameManager = CreateGameManager();

            // Act
            gameManager.Play( fighters );

            // Assert
            // 1 * 0.8 = 0.8 -> rounds to 1; subtracting 50 armor produces a negative value,
            // but final damage cannot be less than 1
            defender.Verify( f => f.TakeDamage( 1 ), Times.Once );
        }

        [Fact]
        public void Play_MultiplePossibleTargets_SelectsTargetByRandomProviderIndex()
        {
            // Arrange
            var attackedOrder = new List<string>();

            Mock<IFighter> attacker = CreateFighterMock(
                "Атакующий", speed: 10, damage: 100, armor: 0, critChance: 0, health: 1000 );
            Mock<IFighter> targetA = CreateFighterMock(
                "ЦельА", speed: 1, damage: 1, armor: 0, critChance: 0, health: 5,
                onTakeDamage: () => attackedOrder.Add( "ЦельА" ) );
            Mock<IFighter> targetB = CreateFighterMock(
                "ЦельБ", speed: 2, damage: 1, armor: 0, critChance: 0, health: 5,
                onTakeDamage: () => attackedOrder.Add( "ЦельБ" ) );

            var fighters = new List<IFighter> { attacker.Object, targetA.Object, targetB.Object };

            var randomProviderMock = new Mock<IRandomProvider>();
            randomProviderMock.Setup( r => r.Next( 2 ) ).Returns( 1 );

            GameManager gameManager = CreateGameManager( randomProviderMock );

            // Act
            gameManager.Play( fighters );

            // Assert
            // possibleTargets for the attacker is built in the order [TargetA, TargetB]
            // (the order of the original list); Next(2) == 1 means TargetB (index 1) is selected
            Assert.Equal( "ЦельБ", attackedOrder[ 0 ] );
            targetB.Verify( f => f.TakeDamage( It.IsAny<int>() ), Times.Once );
        }

        [Fact]
        public void Play_BattleExceedsMaxRounds_ReturnsRandomSurvivorAmongAlive()
        {
            // Arrange
            // Both fighters are durable enough to survive 1,000 rounds without either one dying
            Mock<IFighter> fighterA = CreateFighterMock( "Вечный А", speed: 10, damage: 10, armor: 0, critChance: 0, health: 1000000 );
            Mock<IFighter> fighterB = CreateFighterMock( "Вечный Б", speed: 5, damage: 10, armor: 0, critChance: 0, health: 1000000 );
            var fighters = new List<IFighter> { fighterA.Object, fighterB.Object };

            GameManager gameManager = CreateGameManager();

            // Act
            IFighter winner = gameManager.Play( fighters );

            // Assert
            // Both fighters survived after exceeding the round limit, so the winner is selected randomly
            // from the survivors; the default Next(2) result of 0 selects the first survivor
            Assert.Same( fighterA.Object, winner );
        }

        private static Mock<IFighter> CreateFighterMock(
            string name,
            int speed = 10,
            int damage = 10,
            int armor = 0,
            double critChance = 0,
            int health = 100,
            Action? onTakeDamage = null )
        {
            int currentHealth = health;

            var fighter = new Mock<IFighter>();
            fighter.Setup( f => f.Name ).Returns( name );
            fighter.Setup( f => f.CalculateSpeed() ).Returns( speed );
            fighter.Setup( f => f.CalculateDamage() ).Returns( damage );
            fighter.Setup( f => f.CalculateArmor() ).Returns( armor );
            fighter.Setup( f => f.GetCritChance() ).Returns( critChance );
            fighter.Setup( f => f.IsAlive() ).Returns( () => currentHealth > 0 );
            fighter.Setup( f => f.GetCurrentHealth() ).Returns( () => currentHealth );
            fighter.Setup( f => f.TakeDamage( It.IsAny<int>() ) )
                .Callback<int>( dealtDamage =>
                {
                    currentHealth = Math.Max( currentHealth - dealtDamage, 0 );
                    onTakeDamage?.Invoke();
                } );

            return fighter;
        }

        private static GameManager CreateGameManager() => CreateGameManager( new Mock<IRandomProvider>() );

        private static GameManager CreateGameManager( Mock<IRandomProvider> randomProviderMock ) =>
            new( randomProviderMock.Object, new Mock<IConsoleIO>().Object );
    }
}
