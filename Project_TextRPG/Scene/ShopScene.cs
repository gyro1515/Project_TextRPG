using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project_TextRPG.SceneManager;

namespace Project_TextRPG
{
    internal class ShopScene : Scene
    {
        public ShopScene()
        {
            items = new List<Item>();
            CreateItem();
            AddOptions();
            // 플레이어 아이템과 비교해여, 구매한 템 체크
            LoadPlayerItem();
        }

        // 아이템은 상점에 있어야?
        List<Item> items;
        public override void ShowScene()
        {
            sb.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 구매 수 있습니다.");
            Console.ResetColor();
            sb.Append("\n");
            sb.Append("[보유 골드]\n");
            // 플레이어 골드 가져오기, 123대신 넣기
            sb.Append(Player.Instance.Gold + " G\n");
            sb.Append("\n");
            // 아이템 목록
            sb.Append("[아이템 목록]\n");
            // 선택 출력
            for (int i = 0; i < optionsLen; i++)
            {
                // 마지막 두 줄 띄우기
                if (i == optionsLen - 2) sb.Append("\n");
                if (i == optionNum) sb.Append("▶");
                else sb.Append("　");

                sb.Append(" " + options[i] + "\n");
            }

            sb.Append("\n");
            sb.Append("이동: 방향키, 구매:z, 돌아가기: x\n");
            Console.Write(sb.ToString());
            SceneControl();
        }
        protected override void SceneControl()
        {
            switch (ControlManager.Instance.GetKey())
            {
                case InputKey.Up:
                    if (optionNum != 0) optionNum--;
                    break;
                case InputKey.Down:
                    if (optionNum != OptionsLen - 1) optionNum++;
                    break;
                case InputKey.Z:
                    // 구매하기 기능 추가
                    for (int i = 0; i < OptionsLen; i++)
                    {
                        if (i == optionNum) // 해당 선택지를 고른 경우
                        {
                            // 선택지가 나가기일 시
                            if (optionNum == OptionsLen - 1)
                            {
                                SceneManager.Instance.SetSceneState = SceneManager.SceneState.StartScene;
                            }
                            else if (optionNum == OptionsLen - 2) // 아이템 판매 선택
                            {
                                SceneManager.Instance.SetSceneState = SceneManager.SceneState.SellScene;

                                /*if (scenesDTY[SceneState.SellScene] is SellScene sellS)
                                {
                                    sellS.SetSellList();
                                }*/
                            }
                            else // 아이템 선택시
                            {
                                // 아이템 구매
                                BuyItem(optionNum);
                            }
                        }
                    }
                    break;
                case InputKey.X:
                    SceneManager.Instance.SetSceneState = SceneManager.SceneState.StartScene;
                    break;
                default:
                    break;
            }
        }
        public override void SetupScene()
        {
            //base.SetupScene();
            SetItemString();
            optionNum = 0;

        }
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
            items.Add(new Item());
            items[6].Name = "창조주의 벨트";
            items[6].Type = Item.ItemType.Armor;
            items[6].Stat = Item.ItemStat.Hp;
            items[6].StatPoint = 10000;
            items[6].Explanation = "창조주가 사용했다는 전설의 벨트입니다.";
            items[6].Gold = 10000;
        }

        void AddOptions()
        {
            // 아이템 목록 가져와서 추가하기
            for (int i = 0; i < items.Count; i++)
            {
                // 함수로 만들어 아이템 목록 가져오기
                options.Add(GetItemString(items[i]));
            }
            options.Add("1. 아이템 판매하기");
            options.Add("0. 나가기");
            optionsLen = options.Count;
        }
        public void SetItemString()
        {
            for (int i = 0; i < items.Count; i++)
            {
                options[i] = GetItemString(items[i]);
            }
        }
        string GetItemString(Item item)
        {
            string itemStat = "";
            string itemGold = "";
            // 공간 정렬?을 위한 패딩, 딱 맞게 출력되도록
            int npad = padding - ControlManager.Instance.GetDisplayWidth(item.Name);
            int epad = exPadding - ControlManager.Instance.GetDisplayWidth(item.Explanation);
            switch (item.Stat)
            {
                case Item.ItemStat.Hp:
                    itemStat = "체력";
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
            if (item.IsBuy)
            {
                itemGold = "구매 완료";
            }
            else
            {
                itemGold = item.Gold.ToString() + " G";
            }
            itemStat += " +";
            return "- " + item.Name + new string(' ', npad) + "|  " + itemStat + item.StatPoint + "\t|  " + item.Explanation + new string(' ', epad) + "|  " + itemGold;
           
        }
        public void BuyItem(int idx)
        {
            // 나가기는 신경 안써도 됨
            if (items[idx].IsBuy) // 이미 구매했다면
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                Thread.Sleep(sleepTime);
            }
            else
            {
                // 구매 가능하다면
                if (Player.Instance.Gold - items[idx].Gold >= 0)
                {
                    Console.WriteLine(items[idx].Name + " 구매 완료");
                    Thread.Sleep(sleepTime);
                    // 금액 차감
                    Player.Instance.Gold -= items[idx].Gold;
                    // 플레이어 인벤토리에 넣기
                    Player.Instance.Inventory.Add(items[idx]);
                    // 구매 완료로 표기
                    items[idx].IsBuy = true;
                    // 출력 고치기
                    SetItemString();
                }
                else
                {
                    Console.WriteLine("Gold 가 부족합니다.");
                    Thread.Sleep(sleepTime);
                }
            }
        }
        void LoadPlayerItem()
        {
            foreach(var item in Player.Instance.Inventory)
            {
                for (int i = 0; i < optionsLen - 2; i++)
                {
                    // 플레이어가 해당 아이템을 가지고 있다면
                    if (item.Name == items[i].Name)
                    {
                        //items[i] = null;
                        // 플레이어 아이템으로 세팅
                        // 기존 items[i]에 있던 객체는
                        // 더이상 그 무엇도 참조하지 않으므로
                        // GC에 들어갈것
                        items[i] = item;
                    }
                }
            }
        }
        
    }
}
