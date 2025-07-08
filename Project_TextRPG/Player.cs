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
            HP = 100;
            Gold = 100000;
            PlusAtk = 0;
            PlusDef = 0;
            PlusHP = 0;

            inventory = new List<Item>();
            equipments = new Dictionary<Item.ItemType, Item>();

            //scenes = new List<Scene>();
            //scenes.Add(new StartScene());
            //scenes.Add(new StateScene());
            //scenes.Add(new InventoryScene());
            //scenes.Add(new EquipmentScene());
            //scenes.Add(new ShopScene());
            //scenes.Add(new BuyScene());

            // Dictionary로 구현해보기
            scenesDTY = new Dictionary<SceneState, Scene>();
            scenesDTY.Add(SceneState.StartScene, new StartScene());
            scenesDTY.Add(SceneState.StateScene, new StateScene());
            scenesDTY.Add(SceneState.InventoryScene, new InventoryScene());
            //scenesDTY.Add(SceneState.EquipmentScene, new EquipmentScene());
            scenesDTY.Add(SceneState.ShopScene, new ShopScene());
            scenesDTY.Add(SceneState.SellScene, new SellScene());
            //scenesDTY.Add(SceneState.BuyScene, new BuyScene());
        }

        public static Player Instance
        {
            get
            {
                if (instance == null) instance = new Player();
                return instance;
            }
        }

        enum SceneState
        {
            StartScene, StateScene,InventoryScene, ShopScene, SellScene
        }

        private SceneState sceneState = SceneState.StartScene;
        private InputKey InputKey;
        //private List<Scene> scenes;
        // 씬 저장용
        private Dictionary<SceneState, Scene> scenesDTY;
        // 인벤토리
        public List<Item> inventory;
        // 장비창, 한 종류의 아이템만 장착하기
        public Dictionary<Item.ItemType, Item> equipments;


        // 플레이어 능력치
        public int Lv { get; set; }
        public string Name { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int HP { get; set; }
        public int Gold { get; set; }
        // 장비 장착시 추가되는 능령치
        public int PlusAtk { get; set; }
        public int PlusDef { get; set; }
        public int PlusHP { get; set; }

        // UI 선택표시용
        int selNum = 0;
        
        public void StartGame()
        {
            while (true)
            {
                switch (sceneState)
                {
                    case SceneState.StartScene:
                        scenesDTY[SceneState.StartScene].ShowScene(selNum);
                        StartScene();
                        break;
                    case SceneState.StateScene:
                        scenesDTY[SceneState.StateScene].ShowScene(selNum);
                        StateScene();
                        break;
                    case SceneState.InventoryScene:
                        scenesDTY[SceneState.InventoryScene].ShowScene(selNum);
                        InventoryScene();
                        break;
                    case SceneState.ShopScene:
                        scenesDTY[SceneState.ShopScene].ShowScene(selNum);
                        ShopScene();
                        break;
                    case SceneState.SellScene:
                        scenesDTY[SceneState.SellScene].ShowScene(selNum);
                        SellScene();
                        break;
                    default:
                        break;
                }
                Console.Clear();
            }
        }

        void StartScene()
        {
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
                            // 인벤토리 출력 세팅
                            ((InventoryScene)scenesDTY[SceneState.InventoryScene]).SetInventoryString();
                            break;
                        case 2:
                            sceneState = SceneState.ShopScene;
                            break;
                        default:
                            break;
                    }
                    // 선택 다시 초기화
                    selNum = 0;

                    break;
                default:
                    break;
            }
        }
        void StateScene()
        {
            ShowCtl();
            switch (InputKey)
            {
                case InputKey.Z:
                case InputKey.X:
                    sceneState = SceneState.StartScene;
                    // 선택 다시 초기화
                    selNum = 0;
                    break;
                default:
                    break;
            }
        }
        void InventoryScene()
        {
            ShowCtl();
            switch (InputKey)
            {
                case InputKey.Up:
                    if (selNum != 0) selNum--;
                    break;
                case InputKey.Down:
                    if (selNum != scenesDTY[SceneState.InventoryScene].OptionsLen - 1) selNum++;
                    break;
                case InputKey.Z:
                    // 장착하기 기능 추가, i는 플레이어가 보유한 최대 아이템 개수까지
                    for (int i = 0; i < inventory.Count + 1; i++) // 인벤토리 아이템 개수 + 나가기 버튼까지
                    {
                        if(i == selNum) // 해당 선택지를 고른 경우
                        {
                            // 선택지가 나가기일 시
                            if(selNum == scenesDTY[SceneState.InventoryScene].OptionsLen - 1)
                            {
                                sceneState = SceneState.StartScene;
                                // 선택 다시 초기화
                                selNum = 0;
                            }
                            else // 아이템 선택시
                            {
                                // 아이템 장착
                                // 명시적 형변환
                                //((InventoryScene)scenesDTY[SceneState.InventoryScene]).EquippedEquipment(selNum);
                                // 형변환 1
                                if(scenesDTY[SceneState.InventoryScene] is InventoryScene invenScn)
                                {
                                    invenScn.EquippedEquipment(selNum);
                                }
                                // 형변환 2
                                /*InventoryScene? invenS = scenesDTY[SceneState.InventoryScene] as InventoryScene;
                                if (invenS != null)
                                {
                                    invenS.EquippedEquipment(selNum);
                                }*/

                            }
                        }
                    }

                    break;
                case InputKey.X:
                    sceneState = SceneState.StartScene;
                    // 선택 다시 초기화
                    selNum = 0;
                    break;
                default:
                    break;
            }
        }
        
        void ShopScene()
        {
            ShowCtl();
            switch (InputKey)
            {
                case InputKey.Up:
                    if (selNum != 0) selNum--;
                    break;
                case InputKey.Down:
                    if (selNum != scenesDTY[SceneState.ShopScene].OptionsLen - 1) selNum++;
                    break;
                case InputKey.Z:
                    // 구매하기 기능 추가
                    for (int i = 0; i < scenesDTY[SceneState.ShopScene].OptionsLen; i++)
                    {
                        if (i == selNum) // 해당 선택지를 고른 경우
                        {
                            // 선택지가 나가기일 시
                            if (selNum == scenesDTY[SceneState.ShopScene].OptionsLen - 1)
                            {
                                sceneState = SceneState.StartScene;
                                // 선택 다시 초기화
                                selNum = 0;
                            }
                            else if(selNum == scenesDTY[SceneState.ShopScene].OptionsLen - 2) // 아이템 판매 선택
                            {
                                sceneState = SceneState.SellScene;
                                if(scenesDTY[SceneState.SellScene] is SellScene sellS)
                                {
                                    sellS.SetSellList();
                                }
                                selNum = 0;
                            }
                            else // 아이템 선택시
                            {
                                // 아이템 구매, 다운 캐스팅
                                ((ShopScene)scenesDTY[SceneState.ShopScene]).BuyItem(selNum);
                            }
                        }
                    }
                    break;
                case InputKey.X:
                    sceneState = SceneState.StartScene;
                    // 선택 다시 초기화
                    selNum = 0;
                    break;
                default:
                    break;
            }
        }
        void SellScene()
        {
            ShowCtl();
            switch (InputKey)
            {
                case InputKey.Up:
                    if (selNum != 0) selNum--;
                    break;
                case InputKey.Down:
                    if (selNum != scenesDTY[SceneState.SellScene].OptionsLen - 1) selNum++;
                    break;
                case InputKey.Z:
                    // 구매하기 기능 추가
                    for (int i = 0; i < scenesDTY[SceneState.SellScene].OptionsLen; i++)
                    {
                        if (i == selNum) // 해당 선택지를 고른 경우
                        {
                            // 선택지가 돌아가기일 시, 다시 상점 구매로
                            if (selNum == scenesDTY[SceneState.SellScene].OptionsLen - 1)
                            {
                                sceneState = SceneState.ShopScene;
                                // 선택 다시 초기화
                                selNum = 0;
                                // 판매해서 아이템 구매 가능이 바뀔 수도 있으니
                                ((ShopScene)scenesDTY[SceneState.ShopScene]).SetItemString();
                            }
                            else // 아이템 선택시 판매
                            {
                                // 아이템 판, 다운 캐스팅
                                ((SellScene)scenesDTY[SceneState.SellScene]).SellItem(selNum);
                            }
                        }
                    }
                    break;
                case InputKey.X:
                    sceneState = SceneState.ShopScene;
                    // 선택 다시 초기화
                    selNum = 0;
                    break;
                default:
                    break;
            }
        }

        void ShowCtl()
        {
            /*Console.WriteLine();
            Console.WriteLine("이동: 방향키, 선택: z, 취소: x");*/
            InputKey = ControlManager.Instance.GetKey();
        }
        
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
        }
    }
}
