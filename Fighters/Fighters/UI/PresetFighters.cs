using Fighters.Models.Armors;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.UI
{
    public static class PresetFighters
    {
        public static readonly (string Name, Func<IFighter> Create)[] All =
        {
            ("Громила", () => Equip(new Berserker("Громила", new Orc()), new Axe(), new NoArmor())),
            ("Страж Ворот", () => Equip(new Guardian("Страж Ворот", new Dwarf()), new Sword(), new PlateArmor())),
            ("Теневой Клинок", () => Equip(new Mercenary("Теневой Клинок", new Elf()), new Dagger(), new SimpleClothes())),
            ("Ополченец", () => Equip(new Knight("Ополченец", new Human()), new Firsts(), new LeatherArmor())),
            ("Кровавый Клинок", () => Equip(new Mercenary("Кровавый Клинок", new Orc()), new Sword(), new ChainMail())),
            ("Хранитель Рощи", () => Equip(new Guardian("Хранитель Рощи", new Elf()), new Firsts(), new SimpleClothes())),
        };

        private static IFighter Equip( IFighter fighter, IWeapon weapon, IArmor armor )
        {
            fighter.SetWeapon( weapon );
            fighter.SetArmor( armor );
            return fighter;
        }
    }
}
