using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal class StartScene : Scene
    {
        public StartScene()
        {
            options.Add("1. 상태 보기");
            options.Add("2. 인벤토리");
            options.Add("3. 상점");
            options.Add("4. 던전입장");
            options.Add("5. 휴식하기");
            options.Add("6. 저장하기");
            optionsLen = options.Count;
        }
        public override void ShowScene()
        {
            sb.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("원진 마을에 오신 여러분 환영합니다.");
            Console.ResetColor();
            sb.Append("\n");

            // 선택 표시
            for (int i = 0; i < optionsLen; i++)
            {
                if (i == optionNum) sb.Append("▶");
                else sb.Append("　");

                sb.Append(" " + options[i] + "\n");
            }
            sb.Append("\n");
            sb.Append("이동: 방향키, 선택: z, 취소: x\n");
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
                    switch (optionNum)
                    {
                        case 0: // 스텟 보기
                            SceneManager.Instance.SetSceneState = SceneManager.SceneState.StatScene;
                            break;
                        case 1: // 인벤토리
                            SceneManager.Instance.SetSceneState = SceneManager.SceneState.InventoryScene;

                            // 인벤토리 출력 세팅 -> 자동으로 세팅되게 변경
                            //((InventoryScene)SceneManager.Instance.Scenes[SceneManager.SceneState.InventoryScene]).SetInventoryString();
                            //SceneManager.Instance.ScenesDict[SceneManager.SceneState.InventoryScene].SetupScene();
                            break;
                        case 2: // 상점
                            SceneManager.Instance.SetSceneState = SceneManager.SceneState.ShopScene;
                            break;
                        case 3: // 던전
                            SceneManager.Instance.SetSceneState = SceneManager.SceneState.Dungeon;
                            break;
                        case 4: // 휴식하기
                            SceneManager.Instance.SetSceneState = SceneManager.SceneState.Rest;
                            break;
                        case 5: // 저장하기
                            SaveManager.Save(Player.Instance);
                            break;
                        case 6: // 불러오기
                            
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        /*public override void SetupScene()
        {
            base.SetupScene(); // 부모것도 호출
        }*/
    }
}
