using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    //internal class Item
    public class Item
    {
        public Item()
        {
            Name = "UnKnown";
            Type = ItemType.Weapon;
            Stat = ItemStat.Hp;
            StatPoint = 0;
            Explanation = "깡통";
            Gold = 0;
            IsEquip = false;
            IsBuy = false;
        }

        public enum ItemStat
        {
            Hp, Atk, Def
        }
        public enum ItemType
        {
            Weapon, Armor
        }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public ItemStat Stat { get; set; }
        public int StatPoint { get; set; }
        public string Explanation { get; set; }
        public int Gold { get; set; }
        public bool IsEquip { get; set; }
        public bool IsBuy { get; set; }
    }
}
