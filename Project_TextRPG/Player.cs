using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project_TextRPG
{
    internal class Player
    {
        private static Player? instance;
        private Player()
        {
            Lv = 1;
            Name = "Unknown";
            Atk = 10;
            Def = 5;
            MaxHP = 100;
            //CurHP = MaxHP;
            CurHP = 40;
            Gold = 15000;
            PlusAtk = 0;
            PlusDef = 0;
            PlusHP = 0;

            inventory = new List<Item>();
            equipments = new Dictionary<Item.ItemType, Item>();

            // Dictionary로 구현해보기 -> 씬 매니저로 이동
            /*scenesDTY = new Dictionary<SceneState, Scene>();
            scenesDTY.Add(SceneState.StartScene, new StartScene());
            scenesDTY.Add(SceneState.StateScene, new StatScene());
            scenesDTY.Add(SceneState.InventoryScene, new InventoryScene());
            scenesDTY.Add(SceneState.ShopScene, new ShopScene());
            scenesDTY.Add(SceneState.SellScene, new SellScene());
            scenesDTY.Add(SceneState.Rest, new RestScene());*/
        }

        public static Player Instance
        {
            get
            {
                if (instance == null) instance = new Player();
                return instance;
            }
        }

        //private List<Scene> scenes;
        
        // 인벤토리
        public List<Item> inventory;
        // 장비창, 한 종류의 아이템만 장착하기
        public Dictionary<Item.ItemType, Item> equipments;
        // 출력 후 대기 시간 -> 현재 사용 x
        //int sleepTime = 800;

        // 플레이어 능력치
        public int Lv { get; set; }
        public string Name { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int MaxHP { get; set; }
        public int CurHP { get; set; }
        public int Gold { get; set; }
        // 장비 장착시 추가되는 능령치
        public int PlusAtk { get; set; }
        public int PlusDef { get; set; }
        public int PlusHP { get; set; }
        
        public void SetAbilityByEquipment()
        {
            // 능력치 초기화
            PlusHP = 0;
            PlusAtk = 0;
            PlusDef = 0;

            foreach (var item in equipments)
            {
                // 착용 능령치에 따라 능력치 증가
                switch (item.Value.Stat)
                {
                    case Item.ItemStat.Hp:
                        PlusHP += item.Value.StatPoint;
                        break;
                    case Item.ItemStat.Atk:
                        PlusAtk += item.Value.StatPoint;
                        break;
                    case Item.ItemStat.Def:
                        PlusDef += item.Value.StatPoint;
                        break;
                    default:
                        break;
                }
            }
            // 플레이어의 현재 체력은 최대 체력에 의해 결정되게끔
            CurHP = CurHP > MaxHP + PlusHP ? MaxHP + PlusHP : CurHP;
        }
    }
}
