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
        enum SceneState
        {
            StartScene, StateScene,InventoryScene, EquipmentScene, ShopScene, BuyScene, SellScene
        }

        private SceneState sceneState = SceneState.StartScene;
        private InputKey InputKey;
        public string Name { get; set; }
        private Control ctl;
        // UI 선택표시용
        int selNum = 0;
        
        public Player()
        {
            Name = "Unknown";
            ctl = new Control();
        }

        public void StartGame()
        {

            while (true)
            {
                switch (sceneState)
                {
                    case SceneState.StartScene:
                        StartScene();
                        break;
                    case SceneState.StateScene:
                        StateScene();
                        break;
                    case SceneState.InventoryScene:
                        InventoryScene();
                        break;
                    case SceneState.EquipmentScene:
                        EquipmentScene();
                        break;
                    case SceneState.ShopScene:
                        ShopScene();
                        break;
                    case SceneState.BuyScene:
                        BuyScene();
                        break;
                    case SceneState.SellScene:
                        break;
                    default:
                        break;
                }
                Console.Clear();

            }
        }

        void StartScene()
        {
            Console.WriteLine("원진 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            for (int i = 0; i < 3; i++)
            {
                if (i == selNum) Console.WriteLine("▶");
                else Console.WriteLine("　");

                
            }
            ShowCtl();
            switch (InputKey)
            {
                case InputKey.Up:
                    if (selNum != 0) selNum--;
                    break;
                case InputKey.Down:
                    if (selNum != 2) selNum++;
                    break;
                case InputKey.Z:
                    switch (selNum)
                    {
                        case 0:
                            sceneState = SceneState.StateScene;
                            break;
                        case 1:
                            sceneState = SceneState.InventoryScene;
                            break;
                        case 2:
                            sceneState = SceneState.ShopScene;
                            break;
                        default:
                            break;
                    }
                    break;
                    default:
                    break;
            }
            Console.Write(selNum);
            Thread.Sleep(1000);
        }
        void StateScene()
        {
            Console.WriteLine("상태 보기");
            ShowCtl();

        }
        void InventoryScene()
        {
            Console.WriteLine("인벤토리");
            ShowCtl();

        }
        void EquipmentScene()
        {
            Console.WriteLine("인벤토리 - 장착 관리");
            ShowCtl();

        }
        void ShopScene()
        {
            Console.WriteLine("상점");
            ShowCtl();

        }
        void BuyScene()
        {
            Console.WriteLine("상점 - 아이템 구매");
            ShowCtl();
        }

        void ShowCtl()
        {
            Console.WriteLine("이동: 방향키, 선택: z, 취소: x");
            InputKey = ctl.GetKey();
        }
        
    }
}
