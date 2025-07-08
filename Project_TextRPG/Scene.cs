using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal class Scene
    {
        protected List<string> options;
        protected int optionsLen = 0;
        public int OptionsLen
            { get { return optionsLen; } }
        public Scene()
        {
            options = new List<string>();
        }
        public int SelNum{ get; set; }

        public virtual void ShowScene(int selNum)
        {
            Console.WriteLine("Empty");
        }
    }

    class StartScene : Scene
    {
        
        public StartScene()
        {
            options.Add("1. 상태 보기");
            options.Add("2. 인벤토리");
            options.Add("3. 상점");
            optionsLen = options.Count;
        }
        public override void ShowScene(int selNum)
        {
            Console.WriteLine("원진 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();

            // 선택 표시
            for (int i = 0; i < optionsLen; i++)
            {
                if (i == selNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 선택: z, 취소: x");
        }
    }
    class StateScene : Scene
    {
        public StateScene()
        {
            options.Add("0. 나가기");
            optionsLen = options.Count;
        }
        public override void ShowScene(int selNum)
        {
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();

            string addAtk;
            string addDef = "";
            string addHp = "";
            // if와 switch 중 어떤 걸?
            switch (Player.Instance.PlusAtk)
            {
                case 0:
                    addAtk = "";
                    break;
                default:
                    addAtk = " (+" + Player.Instance.PlusAtk + ")";
                    break;
            }

            // 플레이어 가져와서 플레이어 능력치 보여줘야 함
            Console.WriteLine("Lv. " + Player.Instance.Lv);
            Console.WriteLine("이름: " + Player.Instance.Name);
            //if(Player.Instance.PlusAtk > 0) addAtk = " (+" + Player.Instance.PlusAtk + ")";
            Console.WriteLine("공격력: " + Player.Instance.Atk + addAtk);
            if(Player.Instance.PlusDef > 0) addDef = " (+" + Player.Instance.PlusDef + ")";
            Console.WriteLine("방어력: " + Player.Instance.Def + addDef);
            if(Player.Instance.PlusHP > 0) addHp = " (+" + Player.Instance.PlusHP + ")";
            Console.WriteLine("체력: " + Player.Instance.HP + addHp);
            Console.WriteLine("Gold: " + Player.Instance.Gold + " G");
            Console.WriteLine();
            // 나가기 표시
            for (int i = 0; i < optionsLen; i++)
            {
                if (i == selNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 선택:z, 돌아가기: x");
        }
    }
    class InventoryScene : Scene
    {
        
        public InventoryScene()
        {
            // 아이템 목록 가져와서 추가하기
            // 인벤토리 씬에 들어올 때만 설정하기
            //SetInventoryString(); 
        }
        public override void ShowScene(int selNum)
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 장착할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            // 내가 가진 아이템 목록 출력, 장착한 표시도 해야함

            // 선택 출력
            for (int i = 0; i < optionsLen; i++)
            {
                // 마지막 한 줄 띄우기
                if (i == optionsLen - 1) Console.WriteLine();
                if (i == selNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }

            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 장착:z, 돌아가기: x");
        }
        public void SetInventoryString()
        {
            options.Clear();
            // 플레이어 변수명 짧게 변경
            Player tP = Player.Instance;
            for (int i = 0; i < tP.inventory.Count; i++)
            {
                string itemStat = "";
                string equipString = "";
                switch (tP.inventory[i].Stat)
                {
                    case Item.ItemStat.Hp:
                        itemStat = "체력　";
                        break;
                    case Item.ItemStat.Atk:
                        itemStat = "공격력";
                        break;
                    case Item.ItemStat.Def:
                        itemStat = "방어력";
                        break;
                    default:
                        break;
                }
                if (tP.inventory[i].IsEquip) equipString = "[E]";
                
                itemStat += " +";
                options.Add("- " + equipString + tP.inventory[i].Name + "  |  " + itemStat + tP.inventory[i].StatPoint + "\t|  " + tP.inventory[i].Explanation + "  |");
            }
            options.Add("0. 나가기");
            optionsLen = options.Count;
        }
        // 장착용 함수
        public void EquippedEquipment(int idx)
        {
            Player tP = Player.Instance;
            tP.inventory[idx].IsEquip = true;
            // 해당 부위를 이미 장착하고 있다면
            if (tP.equipments.ContainsKey(tP.inventory[idx].Type))
            {
                // 장착한 부위를 다시 클릭했다면, 장착 해제
                if (tP.equipments[tP.inventory[idx].Type] == tP.inventory[idx])
                {
                    tP.equipments[tP.inventory[idx].Type].IsEquip = false;
                    Console.WriteLine(tP.equipments[tP.inventory[idx].Type].Name + " 장착 해제");
                    Thread.Sleep(500);
                    // 해당 부위 삭제
                    tP.equipments.Remove(tP.inventory[idx].Type);
                }
                else
                {
                    // 교체
                    Console.WriteLine(tP.equipments[tP.inventory[idx].Type].Name + "을 " + tP.inventory[idx] + "으로 교체");
                    Thread.Sleep(500);
                    tP.equipments[tP.inventory[idx].Type].IsEquip = false;
                    tP.equipments[tP.inventory[idx].Type] = tP.inventory[idx];
                }
            }
            else // 해당 부위가 비었다면
            {
                // 착용
                Console.WriteLine(tP.inventory[idx].Name + " 장착");
                Thread.Sleep(500);
                tP.equipments.Add(tP.inventory[idx].Type, tP.inventory[idx]);
            }

            // 비효울 적이지만... 
            // 시간이 남으면 마지막에 출력때만 설정하게 하기
            SetInventoryString();

            // 플레이어 장비 능력치도 재설정
            tP.SetAbilityByEquipment();
        }
    }
    internal class ShopScene : Scene
    {
        public ShopScene()
        {
            items = new List<Item>();
            CreateItem();
            AddOptions();
        }

        // 아이템은 상점에 있어야?
        List<Item> items;


        void CreateItem()
        {
            items.Add(new Item());
            //items[0].Name = "수련자 갑옷　　";
            items[0].Name = "수련자 갑옷";
            items[0].Type = Item.ItemType.Armor;
            items[0].Stat = Item.ItemStat.Def;
            items[0].StatPoint = 5;
            items[0].Explanation = "수련에 도움을 주는 갑옷입니다.";
            items[0].Gold = 1000;
            items.Add(new Item());
            //items[1].Name = "무쇠갑옷　　　 ";
            items[1].Name = "무쇠갑옷";
            items[1].Type = Item.ItemType.Armor;
            items[1].Stat = Item.ItemStat.Def;
            items[1].StatPoint = 9;
            items[1].Explanation = "무쇠로 만들어져 튼튼한 갑옷입니다.";
            items[1].Gold = 2200;
            items.Add(new Item());
            items[2].Name = "스파르타의 갑옷";
            items[2].Type = Item.ItemType.Armor;
            items[2].Stat = Item.ItemStat.Def;
            items[2].StatPoint = 15;
            items[2].Explanation = "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.";
            items[2].Gold = 3500;
            items.Add(new Item());
            //items[3].Name = "낡은 검　　　　";
            items[3].Name = "낡은 검";
            items[3].Type = Item.ItemType.Weapon;
            items[3].Stat = Item.ItemStat.Atk;
            items[3].StatPoint = 6;
            items[3].Explanation = "쉽게 볼 수 있는 낡은 검 입니다.";
            items[3].Gold = 600;
            items.Add(new Item());
            //items[4].Name = "청동 도끼　　　";
            items[4].Name = "청동 도끼";
            items[4].Type = Item.ItemType.Weapon;
            items[4].Stat = Item.ItemStat.Atk;
            items[4].StatPoint = 5;
            items[4].Explanation = "어디선가 사용됐던거 같은 도끼입니다.";
            items[4].Gold = 1500;
            items.Add(new Item());
            //items[5].Name = "스파르타의 창　";
            items[5].Name = "스파르타의 창";
            items[5].Type = Item.ItemType.Weapon;
            items[5].Stat = Item.ItemStat.Atk;
            items[5].StatPoint = 7;
            items[5].Explanation = "스파르타의 전사들이 사용했다는 전설의 창입니다.";
            items[5].Gold = 2000;
        }

        void AddOptions()
        {
            // 아이템 목록 가져와서 추가하기
            for (int i = 0; i < items.Count; i++)
            {
                string itemStat = "";
                string itemGold = "";
                switch (items[i].Stat)
                {
                    case Item.ItemStat.Hp:
                        itemStat = "체력　";
                        break;
                    case Item.ItemStat.Atk:
                        itemStat = "공격력";
                        break;
                    case Item.ItemStat.Def:
                        itemStat = "방어력";
                        break;
                    default:
                        break;
                }
                if (items[i].IsBuy)
                {
                    itemGold = "구매 완료";
                }
                else
                {
                    itemGold = items[i].Gold.ToString() + " G";
                }
                itemStat += " +";
                options.Add("- " + items[i].Name + "\t|  " + itemStat + items[i].StatPoint + "\t|  " + items[i].Explanation + "\t|  " + itemGold);

            }
            options.Add("1. 아이템 판매하기");
            options.Add("0. 나가기");
            optionsLen = options.Count;
        }
        public void SetItemString()
        {
            for (int i = 0; i < items.Count; i++)
            {
                string itemStat = "";
                string itemGold = "";
                switch (items[i].Stat)
                {
                    case Item.ItemStat.Hp:
                        itemStat = "체력　";
                        break;
                    case Item.ItemStat.Atk:
                        itemStat = "공격력";
                        break;
                    case Item.ItemStat.Def:
                        itemStat = "방어력";
                        break;
                    default:
                        break;
                }
                if (items[i].IsBuy)
                {
                    itemGold = "구매 완료";
                }
                else
                {
                    itemGold = items[i].Gold.ToString() + " G";
                }
                itemStat += " +";
                options[i] = ("- " + items[i].Name + "  |  " + itemStat + items[i].StatPoint + "\t|  " + items[i].Explanation + "\t|  " + itemGold);

            }
        }
        public void BuyItem(int idx)
        {
            // 나가기는 신경 안써도 됨
            // 구매 가능하다면
            if (Player.Instance.Gold - items[idx].Gold >= 0 && !items[idx].IsBuy) 
            {
                Console.WriteLine(items[idx].Name + " 구매 완료");
                Thread.Sleep(500);
                // 금액 차감
                Player.Instance.Gold -= items[idx].Gold;
                // 플레이어 인벤토리에 넣기
                Player.Instance.inventory.Add(items[idx]);
                // 구매 완료로 표기
                items[idx].IsBuy = true;
                // 출력 고치기
                SetItemString();
            }
            else
            {
                Console.WriteLine("금액이 부족합니다.");
                Thread.Sleep(500);
            }
        }
        public override void ShowScene(int selNum)
        {
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 구매 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            // 플레이어 골드 가져오기, 123대신 넣기
            Console.WriteLine(Player.Instance.Gold + " G");
            Console.WriteLine();
            // 아이템 목록
            Console.WriteLine("[아이템 목록]");

            // 선택 출력
            for (int i = 0; i < optionsLen; i++)
            {
                // 마지막 두 줄 띄우기
                if (i == optionsLen - 2) Console.WriteLine();
                if (i == selNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }

            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 구매:z, 돌아가기: x");
        }
    }
    class SellScene : Scene
    {
        public SellScene()
        {
            options = new List<string>();
        }
        public override void ShowScene(int selNum)
        {
            Console.WriteLine("상점");
            Console.WriteLine("필요없는 아이템을 팔 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            // 플레이어 골드 가져오기
            Console.WriteLine(Player.Instance.Gold + " G");
            Console.WriteLine();
            // 아이템 목록
            Console.WriteLine("[인벤토리 아이템 목록]");
            // 선택 출력
            for (int i = 0; i < optionsLen; i++)
            {
                // 마지막 한 줄 띄우기
                if (i == optionsLen - 1) Console.WriteLine();
                if (i == selNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 판매:z, 돌아가기: x");
        }

        public void SetSellList()
        {
            options.Clear();
            // 변수 명 간소화
            List<Item> inven = Player.Instance.inventory;
            for (int i = 0;i < inven.Count;i++)
            {
                string itemStat = "";
                switch (inven[i].Stat)
                {
                    case Item.ItemStat.Hp:
                        itemStat = "체력　";
                        break;
                    case Item.ItemStat.Atk:
                        itemStat = "공격력";
                        break;
                    case Item.ItemStat.Def:
                        itemStat = "방어력";
                        break;
                    default:
                        break;
                }
               
                itemStat += " +";
                options.Add("- " + inven[i].Name + "  |  " + itemStat + inven[i].StatPoint + "\t|  " + inven[i].Explanation + "\t|  " + (int)(inven[i].Gold * 0.85f) + " G");

            }
            options.Add("0. 돌아가기");
            optionsLen = options.Count;
        }
        public void SellItem(int idx)
        {
            // 나가기는 신경 안써도 됨
            List<Item> inven = Player.Instance.inventory;

            // 장착 중이라면 판매 불가
            if (inven[idx].IsEquip)
            {
                Console.WriteLine("장착 중인 아이템은 판매할 수 없습니다.");
                Thread.Sleep(500);
            }
            else // 장착 중이 아니라면 판매
            {
                Console.WriteLine(inven[idx].Name + " 판매 완료");
                Thread.Sleep(500);
                // 금액 증가
                Player.Instance.Gold += (int)(inven[idx].Gold * 0.85f);
                // 구매 가능으로 표기
                inven[idx].IsBuy = false;
                // 플레이어 인벤토리에서 제거
                inven.Remove(inven[idx]);
                // 판매 리스트 재설정
                SetSellList();
            }
        }

    }
}
