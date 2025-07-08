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
            Gold = 5000;
            PlusAtk = 0;
            PlusDef = 0;
            PlusHP = 0;

            inventory = new List<Item>();
            equipments = new Dictionary<Item.ItemType, Item>();

            // Dictionary로 구현해보기
            scenesDTY = new Dictionary<SceneState, Scene>();
            scenesDTY.Add(SceneState.StartScene, new StartScene());
            scenesDTY.Add(SceneState.StateScene, new StatScene());
            scenesDTY.Add(SceneState.InventoryScene, new InventoryScene());
            scenesDTY.Add(SceneState.ShopScene, new ShopScene());
            scenesDTY.Add(SceneState.SellScene, new SellScene());
            scenesDTY.Add(SceneState.Rest, new RestScene());
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
            StartScene, StateScene,InventoryScene, ShopScene, SellScene, Dungeon, Rest
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
        // 출력 후 대기 시간
        int sleepTime = 800;

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

        // UI 선택표시용
        int selNum = 0;
        

        
        public void StartGame()
        {
            // 모든 씬 생성하고 상황에 따라 씬 옮기기
            while (true)
            {
                // 버블 버퍼링을 구현하고 싶었으나, 포기...
                //ScreenManager.Instance.Clear();
                scenesDTY[sceneState].ShowScene(selNum);
                //ScreenManager.Instance.Flip();
                Console.Clear();
            }

            // 상황에 따라 씬 생성, 기존 씬 제거하는 방식으로 바꾸기??
            // 현재 약 11MB 사용
        }

        // 아래 씬관련을 모두 다 씬으로?
        // 키 입력은 플레이어에 구현하는 게 맞지 않나?
        // 씬에 구현하는 게 더 편하고, 덜 신경쓸 거 같긴 함
        public void StartScene()
        {
            ShowCtl();
            switch (InputKey)
            {
                case InputKey.Up:
                    if (selNum != 0) selNum--;
                    break;
                case InputKey.Down:
                    if (selNum != scenesDTY[SceneState.StartScene].OptionsLen - 1) selNum++;
                    break;
                case InputKey.Z:
                    switch (selNum)
                    {
                        case 0: // 스텟 보기
                            sceneState = SceneState.StateScene;
                            break;
                        case 1: // 인벤토리
                            sceneState = SceneState.InventoryScene;
                            // 인벤토리 출력 세팅
                            ((InventoryScene)scenesDTY[SceneState.InventoryScene]).SetInventoryString();
                            break;
                        case 2: // 상점
                            sceneState = SceneState.ShopScene;
                            break;
                        case 3: // 던전
                            Console.WriteLine("던전 입장!!! 미구현");
                            Thread.Sleep(sleepTime);
                            break;
                        case 4: // 휴식하기
                            sceneState = SceneState.Rest;
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
        public void StatScene()
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
        public void InventoryScene()
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

        public void ShopScene()
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
        public void SellScene()
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
        public void RestScene()
        {
            ShowCtl();
            switch (InputKey)
            {
                case InputKey.Up:
                    if (selNum != 0) selNum--;
                    break;
                case InputKey.Down:
                    if (selNum != scenesDTY[SceneState.Rest].OptionsLen - 1) selNum++;
                    break;
                case InputKey.Z:
                    // 선택
                    switch (selNum)
                    {
                        case 0: // 휴식
                            ((RestScene)scenesDTY[SceneState.Rest]).RestAndRecover();
                            
                            break;
                        case 1: // 나가기
                            sceneState = SceneState.StartScene;
                            break;
                        default:
                            break;
                    }
                    // 선택 다시 초기화
                    selNum = 0;
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
