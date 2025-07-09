using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal class DungeonScene : Scene
    {
        public DungeonScene()
        {
            CreateDungeon();
        }
        enum DugeonState
        {
            Select, Clear
        }
        DugeonState dgSt = DugeonState.Select;
        // 패딩용
        public override void SetupScene()
        {
            base.SetupScene();
            optionNum = 0;
        }

        public override void ShowScene()
        {
            switch (dgSt)
            {
                case DugeonState.Select:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("던전입장\r\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                    Console.ResetColor();
                    Console.WriteLine();
                    for (int i = 0; i < optionsLen; i++)
                    {
                        if (i == optionNum) Console.Write("▶");
                        else Console.Write("　");

                        Console.WriteLine(" " + options[i]);
                    }
                    break;
                case DugeonState.Clear:
                    break;
                default:
                    break;
            }
            SceneControl();
        }

        protected override void SceneControl()
        {
            switch (dgSt)
            {
                case DugeonState.Select:
                    DgSelectCtl();
                    break;
                case DugeonState.Clear:
                    break;
                default:
                    break;
            }
            
        }

        void DgSelectCtl()
        {
            switch (ControlManager.Instance.GetKey())
            {
                case InputKey.Up:
                    if (optionNum != 0) optionNum--;
                    break;
                case InputKey.Down:
                    if (optionNum != optionsLen - 1) optionNum++;
                    break;
                case InputKey.Z:
                    switch (optionNum)
                    {
                        case 0: // 쉬운 던전 선택
                            
                            break;
                        case 1: // 일반
                            
                            break;
                        case 2: // 보통
                            break;
                        case 3: // 나가기
                            SceneManager.Instance.SetSceneState = SceneManager.SceneState.StartScene;
                            break;
                        default:
                            break;
                    }
                    break;
                case InputKey.X:
                    SceneManager.Instance.SetSceneState = SceneManager.SceneState.StartScene;
                    break;
                default:
                    break;
            }
        }
        void CreateDungeon()
        {
            options.Add("1. 쉬운 던전　 | 방어력 5 이상 권장");
            options.Add("2. 일반 던전　 | 방어력 11 이상 권장");
            options.Add("3. 어려운 던전 | 방어력 17 이상 권장");
            options.Add("0. 나가기");
            optionsLen = options.Count();
        }
    }
}
