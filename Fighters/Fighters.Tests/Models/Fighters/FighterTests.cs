using Fighters.Models.Armors;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Moq;
using Xunit;

namespace Fighters.Tests.Models.Fighters
{
    public class FighterTests
    {
        private const string DefaultName = "Тестовый боец";

        [Fact]
        public void Constructor_ValidArguments_CurrentHealthEqualsMaxHealth()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceHealth: 100, classHealth: 20 );

            // Assert
            Assert.Equal( fighter.GetMaxHealth(), fighter.GetCurrentHealth() );
        }

        [Theory]
        [InlineData( ( string? )null )]
        [InlineData( "" )]
        [InlineData( "   " )]
        public void Constructor_NameIsNullOrWhiteSpace_ThrowsArgumentException( string? invalidName )
        {
            // Assert
            Assert.Throws<ArgumentException>( () =>
                new Fighter( invalidName!, CreateRace(), CreateFighterClass(), CreateWeapon(), CreateArmor() ) );
        }

        [Fact]
        public void Constructor_RaceIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>( () =>
                new Fighter( DefaultName, null!, CreateFighterClass(), CreateWeapon(), CreateArmor() ) );
        }

        [Fact]
        public void Constructor_FighterClassIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>( () =>
                new Fighter( DefaultName, CreateRace(), null!, CreateWeapon(), CreateArmor() ) );
        }

        [Fact]
        public void Constructor_WeaponIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>( () =>
                new Fighter( DefaultName, CreateRace(), CreateFighterClass(), null!, CreateArmor() ) );
        }

        [Fact]
        public void Constructor_ArmorIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>( () =>
                new Fighter( DefaultName, CreateRace(), CreateFighterClass(), CreateWeapon(), null! ) );
        }

        [Fact]
        public void GetMaxHealth_ValidStats_ReturnsRaceHealthPlusClassBonusHealth()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceHealth: 100, classHealth: 20 );

            // Act
            int result = fighter.GetMaxHealth();

            // Assert
            Assert.Equal( 120, result );
        }

        [Fact]
        public void CalculateDamage_ValidStats_ReturnsWeaponPlusRacePlusClassDamage()
        {
            // Arrange
            Fighter fighter = CreateFighter( weaponDamage: 5, raceDamage: 2, classDamage: 8 );

            // Act
            int result = fighter.CalculateDamage();

            // Assert
            Assert.Equal( 15, result );
        }

        [Fact]
        public void CalculateArmor_ValidStats_ReturnsArmorPlusRaceArmor()
        {
            // Arrange
            Fighter fighter = CreateFighter( armorValue: 3, raceArmor: 2 );

            // Act
            int result = fighter.CalculateArmor();

            // Assert
            Assert.Equal( 5, result );
        }

        [Fact]
        public void CalculateSpeed_PenaltyLowerThanRaceSpeed_ReturnsDifference()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceSpeed: 10, speedPenalty: 6 );

            // Act
            int result = fighter.CalculateSpeed();

            // Assert
            Assert.Equal( 4, result );
        }

        [Fact]
        public void CalculateSpeed_PenaltyExceedsRaceSpeed_ReturnsZero()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceSpeed: 5, speedPenalty: 10 );

            // Act
            int result = fighter.CalculateSpeed();

            // Assert
            Assert.Equal( 0, result );
        }

        [Fact]
        public void CalculateSpeed_PenaltyEqualsRaceSpeed_ReturnsZero()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceSpeed: 10, speedPenalty: 10 );

            // Act
            int result = fighter.CalculateSpeed();

            // Assert
            Assert.Equal( 0, result );
        }

        [Fact]
        public void GetCritChance_ValidWeapon_ReturnsWeaponCritChance()
        {
            // Arrange
            Fighter fighter = CreateFighter( critChance: 0.25 );

            // Act
            double result = fighter.GetCritChance();

            // Assert
            Assert.Equal( 0.25, result );
        }

        [Fact]
        public void IsAlive_HealthAboveZero_ReturnsTrue()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceHealth: 50 );

            // Act
            bool result = fighter.IsAlive();

            // Assert
            Assert.True( result );
        }

        [Fact]
        public void IsAlive_HealthDroppedToZero_ReturnsFalse()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceHealth: 30 );

            // Act
            fighter.TakeDamage( 30 );

            bool result = fighter.IsAlive();

            // Assert
            Assert.False( result );
        }

        [Fact]
        public void TakeDamage_PositiveDamage_ReducesCurrentHealthByExactAmount()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceHealth: 100 );

            // Act
            fighter.TakeDamage( 35 );

            int result = fighter.GetCurrentHealth();

            // Assert
            Assert.Equal( 65, result );
        }

        [Fact]
        public void TakeDamage_DamageExceedsCurrentHealth_ClampsHealthToZero()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceHealth: 20 );

            // Act
            fighter.TakeDamage( 999 );

            int result = fighter.GetCurrentHealth();

            // Assert
            Assert.Equal( 0, result );
        }

        [Fact]
        public void TakeDamage_MultipleHits_AccumulatesDamage()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceHealth: 100 );

            // Act
            fighter.TakeDamage( 20 );
            fighter.TakeDamage( 15 );
            fighter.TakeDamage( 10 );

            int result = fighter.GetCurrentHealth();

            // Assert
            Assert.Equal( 55, result );
        }

        [Fact]
        public void TakeDamage_ZeroDamage_DoesNotChangeHealth()
        {
            // Arrange
            Fighter fighter = CreateFighter( raceHealth: 100 );

            // Act
            fighter.TakeDamage( 0 );

            int result = fighter.GetCurrentHealth();

            // Assert
            Assert.Equal( 100, result );
        }

        [Fact]
        public void TakeDamage_NegativeDamage_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            Fighter fighter = CreateFighter();

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>( () => fighter.TakeDamage( -1 ) );
        }

        [Fact]
        public void ToString_ValidFighter_ContainsNameAndStats()
        {
            // Arrange
            Fighter fighter = CreateFighter( name: "Громила", raceHealth: 100, weaponDamage: 5, raceDamage: 1, armorValue: 2 );

            // Act
            string result = fighter.ToString();

            // Assert
            Assert.Contains( "Громила", result );
            Assert.Contains( fighter.GetMaxHealth().ToString(), result );
            Assert.Contains( fighter.CalculateDamage().ToString(), result );
        }

        private static Fighter CreateFighter(
            string name = DefaultName,
            int raceDamage = 1,
            int raceHealth = 100,
            int raceArmor = 0,
            int raceSpeed = 10,
            int classDamage = 0,
            int classHealth = 0,
            int weaponDamage = 0,
            double critChance = 0,
            int armorValue = 0,
            int speedPenalty = 0 )
        {
            var race = new Mock<IRace>();
            race.Setup( r => r.Damage ).Returns( raceDamage );
            race.Setup( r => r.Health ).Returns( raceHealth );
            race.Setup( r => r.Armor ).Returns( raceArmor );
            race.Setup( r => r.Speed ).Returns( raceSpeed );

            var fighterClass = new Mock<IFighterClass>();
            fighterClass.Setup( c => c.BonusDamage ).Returns( classDamage );
            fighterClass.Setup( c => c.BonusHealth ).Returns( classHealth );

            var weapon = new Mock<IWeapon>();
            weapon.Setup( w => w.Damage ).Returns( weaponDamage );
            weapon.Setup( w => w.CritChance ).Returns( critChance );

            var armor = new Mock<IArmor>();
            armor.Setup( a => a.Armor ).Returns( armorValue );
            armor.Setup( a => a.SpeedPenalty ).Returns( speedPenalty );

            return new Fighter( name, race.Object, fighterClass.Object, weapon.Object, armor.Object );
        }

        private static IRace CreateRace() =>
            Mock.Of<IRace>( r => r.Damage == 1 && r.Health == 100 && r.Armor == 0 && r.Speed == 10 );

        private static IFighterClass CreateFighterClass() =>
            Mock.Of<IFighterClass>( c => c.BonusDamage == 0 && c.BonusHealth == 0 );

        private static IWeapon CreateWeapon() =>
            Mock.Of<IWeapon>( w => w.Damage == 1 && w.CritChance == 0.05 );

        private static IArmor CreateArmor() =>
            Mock.Of<IArmor>( a => a.Armor == 0 && a.SpeedPenalty == 0 );
    }
}
