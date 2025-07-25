﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project_TextRPG
{
    internal class Player
    {
        private static Player? instance;
        [JsonConstructor]
        private Player()
        {
            Lv = 1;
            Name = "Unknown";
            Atk = 10;
            Def = 5;
            MaxHP = 100;
            Gold = 15000;
            PlusAtk = 0;
            PlusDef = 0;
            PlusHP = 0;
            TotalAtk = Atk + PlusAtk;
            TotalDef = Def + PlusDef;
            TotalMaxHp = MaxHP + PlusHP;
            // 현재 체력 세팅
            //CurHP = TotalMaxHp;
            CurHP = 40;

            // 경험치 통 = lv * 5f
            MaxExp = (Lv * 5);
            CurExp = 0;

            /*inventory = new List<Item>();
            equipments = new Dictionary<Item.ItemType, Item>();*/
            Inventory = new List<Item>();
            Equipments = new Dictionary<Item.ItemType, Item>();

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
                // 인스턴스가 할당되어 있다면 인스턴스 리턴
                if (instance != null) return instance;

                // 인스턴스 할당 x
                // 세이브 데이터가 있다면 인스턴에에 로드 데이터 할당하고 리턴
                Player? player = SaveManager.Load();
                if (player != null)
                {
                    instance = player;
                    instance.SetLoad();
                    return instance = player;
                }

                // 인스턴스 할당 x, 세이브 데이터x
                // 인스턴스에 새로운 객체 할당하고 리턴
                return instance = new Player();

            }
        }

        // json 직렬화를 위한
        public List<Item> Inventory { get; set; }
        public Dictionary<Item.ItemType, Item> Equipments { get; set; }

        // 인벤토리
        /*public List<Item> inventory;
        // 장비창, 한 종류의 아이템만 장착하기
        public Dictionary<Item.ItemType, Item> equipments;*/

        // 출력 후 대기 시간 -> 현재 사용 x
        //int sleepTime = 800;

        // 플레이어 능력치
        public int Lv { get; set; }
        public string Name { get; set; }
        public float Atk { get; set; }
        public float Def { get; set; }
        public int MaxHP { get; set; }
        public int CurHP { get; set; }
        public int Gold { get; set; }
        public int MaxExp { get; set; }
        public int CurExp { get; set; }
        // 장비 장착시 추가되는 능령치
        public float PlusAtk { get; set; }
        public float PlusDef { get; set; }
        public int PlusHP { get; set; }
        public float TotalAtk { get; set; }
        public float TotalDef { get; set; }
        public int TotalMaxHp { get; set; }
        
        public void SetAbilityByEquipment()
        {
            // 능력치 초기화
            PlusHP = 0;
            PlusAtk = 0;
            PlusDef = 0;

            //foreach (var item in equipments)
            foreach (var item in Equipments)
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
            TotalAtk = Atk + PlusAtk;
            TotalDef = Def + PlusDef;
            TotalMaxHp = MaxHP + PlusHP;
            // 플레이어의 현재 체력은 최대 체력에 의해 결정되게끔
            CurHP = CurHP > TotalMaxHp ? TotalMaxHp : CurHP;
        }
        public void SetLvUp()
        {
            // 랩업하고 능령치 재세팅
            Lv++;
            MaxExp = (Lv * 5);
            Atk += 0.5f;
            Def += 1.0f;
            SetAbilityByEquipment();
        }

        void SetLoad()
        {
            // 인벤토리에서 작창 표시된 거 장착하기.
            // 현재는 로드된 Equipments와 Inventory의 객체가 서로 다름,
            // 기존에는 같은 객체라서 객체 주소값 다시 넘겨 줘야함
            Equipments.Clear(); // 참조 해제하여 기존 아이템들 가비지 컬렉션으로
            // 인벤토리 아이템들 체크하여
            foreach (var item in Inventory)
            {
                // 장착되어 있다면
                if(item.IsEquip)
                {
                    Equipments.Add(item.Type, item); // 장착
                }
            }
            // 안해도 되지만, 능력치도 재설정
            SetAbilityByEquipment();
        }
    }
    
}
