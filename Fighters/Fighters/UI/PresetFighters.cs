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
            ("Громила", () => new Fighter( "Громила", new Orc(), new Berserker(), new Axe(), new NoArmor() )),
            ("Страж Ворот", () => new Fighter( "Страж Ворот", new Dwarf(), new Guardian(), new Sword(), new PlateArmor() )),
            ("Теневой Клинок", () => new Fighter( "Теневой Клинок", new Elf(), new Mercenary(), new Dagger(), new SimpleClothes() )),
            ("Ополченец", () => new Fighter( "Ополченец", new Human(), new Knight(), new Fists(), new LeatherArmor() )),
            ("Кровавый Клинок", () => new Fighter( "Кровавый Клинок", new Orc(), new Mercenary(), new Sword(), new ChainMail() )),
            ("Хранитель Рощи", () => new Fighter( "Хранитель Рощи", new Elf(), new Guardian(), new Fists(), new SimpleClothes() )),
        };
    }
}
