using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project_TextRPG.SceneManager;

namespace Project_TextRPG
{
    internal class InventoryScene : Scene
    {
        public InventoryScene()
        {
            // 아이템 목록 가져와서 추가하기
            // 인벤토리 씬에 들어올 때만 설정하기
            //SetInventoryString(); 
        }
        public override void ShowScene()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("인벤토리");
            Console.ResetColor();
            Console.WriteLine("보유 중인 아이템을 장착할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            // 내가 가진 아이템 목록 출력, 장착한 표시도 해야함

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
            Console.WriteLine("이동: 방향키, 장착:z, 돌아가기: x");

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
                    // 장착하기 기능 추가, i는 플레이어가 보유한 최대 아이템 개수까지
                    for (int i = 0; i < Player.Instance.inventory.Count + 1; i++) // 인벤토리 아이템 개수 + 나가기 버튼까지
                    {
                        if (i == optionNum) // 해당 선택지를 고른 경우
                        {
                            // 선택지가 나가기일 시
                            if (optionNum == OptionsLen - 1)
                            {
                                SceneManager.Instance.SetSceneState = SceneManager.SceneState.StartScene;
                            }
                            else // 아이템 선택시
                            {
                                EquippedEquipment(optionNum);
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
            base.SetupScene(); // 부모것도 호출
            SetInventoryString();
            optionNum = 0;
        }
        public void SetInventoryString()
        {
            options.Clear();
            // 플레이어 변수명 짧게 변경
            Player tP = Player.Instance;
            for (int i = 0; i < tP.inventory.Count; i++)
            {
                string itemStat = "";
                string equipString = "   ";
                // 공간 정렬?을 위한 패딩, 딱 맞게 출력되도록
                int npad = padding - ControlManager.Instance.GetDisplayWidth(tP.inventory[i].Name);
                int epad = exPadding - ControlManager.Instance.GetDisplayWidth(tP.inventory[i].Explanation);
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
                options.Add("- " + equipString + tP.inventory[i].Name + new string(' ', npad) + "|  " + itemStat + tP.inventory[i].StatPoint + "\t|  " + tP.inventory[i].Explanation + new string(' ', epad) + "|");
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
                    Thread.Sleep(sleepTime);
                    // 해당 부위 삭제
                    tP.equipments.Remove(tP.inventory[idx].Type);
                }
                else
                {
                    // 교체
                    Console.WriteLine(tP.equipments[tP.inventory[idx].Type].Name + "을 " + tP.inventory[idx].Name + "으로 교체");
                    Thread.Sleep(sleepTime);
                    tP.equipments[tP.inventory[idx].Type].IsEquip = false;
                    tP.equipments[tP.inventory[idx].Type] = tP.inventory[idx];
                }
            }
            else // 해당 부위가 비었다면
            {
                // 착용
                Console.WriteLine(tP.inventory[idx].Name + " 장착");
                Thread.Sleep(sleepTime);
                tP.equipments.Add(tP.inventory[idx].Type, tP.inventory[idx]);
            }
            

            // 비효울 적이지만... 
            // 시간이 남으면 마지막에 출력때만 설정하게 하기
            SetInventoryString();

            // 플레이어 장비 능력치도 재설정
            tP.SetAbilityByEquipment();
        }

    }
}
