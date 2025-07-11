using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project_TextRPG.SceneManager;

namespace Project_TextRPG
{
    internal class SellScene : Scene
    {
        public SellScene()
        {
            //options = new List<string>();
        }
        public override void ShowScene()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("상점");
            Console.WriteLine("필요없는 아이템을 팔 수 있습니다.");
            Console.ResetColor();
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
                if (i == optionNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 판매:z, 돌아가기: x");

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
                            // 선택지가 돌아가기일 시, 다시 상점 구매로
                            if (optionNum == OptionsLen - 1)
                            {
                                SceneManager.Instance.SetSceneState = SceneManager.SceneState.ShopScene;
                            }
                            else // 아이템 선택시 판매
                            {
                                SellItem(optionNum);
                            }
                        }
                    }
                    break;
                case InputKey.X:
                    SceneManager.Instance.SetSceneState = SceneManager.SceneState.ShopScene;
                    break;
                default:
                    break;
            }
        }
        public override void SetupScene()
        {
            base.SetupScene();
            SetSellList();
            optionNum = 0;

        }
        public void SetSellList()
        {
            options.Clear();
            // 변수 명 간소화
            List<Item> inven = Player.Instance.Inventory;

            for (int i = 0; i < inven.Count; i++)
            {
                string itemStat = "";
                int npad = padding - ControlManager.Instance.GetDisplayWidth(inven[i].Name);
                int epad = exPadding - ControlManager.Instance.GetDisplayWidth(inven[i].Explanation);
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
                options.Add("- " + inven[i].Name + new string(' ', npad) + "|  " + itemStat + inven[i].StatPoint + "\t|  " + inven[i].Explanation + new string(' ', epad) + "|  " + (int)(inven[i].Gold * 0.85f) + " G");

            }
            options.Add("0. 돌아가기");
            optionsLen = options.Count;
        }
        public void SellItem(int idx)
        {
            // 나가기는 신경 안써도 됨
            List<Item> inven = Player.Instance.Inventory;

            // 장착 중이라면 판매 불가
            if (inven[idx].IsEquip)
            {
                Console.WriteLine("장착 중인 아이템은 판매할 수 없습니다.");
                Thread.Sleep(sleepTime);
            }
            else // 장착 중이 아니라면 판매
            {
                Console.WriteLine(inven[idx].Name + " 판매 완료");
                Thread.Sleep(sleepTime);
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
