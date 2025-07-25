﻿using System;
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
            controlSelectoptions = new List<string>();
            dungeons = new List<Dungeon>();
            selectedDungeon = null;
            CreateDungeon();

        }
        enum DugeonState
        {
            DifficultySelect, ControlSelect, Clear, Fail, ManualPlaying
        }
        // 던전 스테이트
        DugeonState dgSt = DugeonState.DifficultySelect;
        // ControlSelect에서의 선택지
        List<string> controlSelectoptions;
        // 던전 관리
        List<Dungeon> dungeons;
        // 선택된 던전
        Dungeon? selectedDungeon;
        // 던전 이름 패딩용
        int dNamePad = 12;
        // ControlSelect용 넘버
        int cS_optionsNum = 0;
        //ControlSelect 선택지 길이
        int cS_optionsLen = 0;
        
        
        public override void SetupScene()
        {
            //base.SetupScene();
            optionNum = 0;
        }

        public override void ShowScene()
        {
            switch (dgSt)
            {
                case DugeonState.DifficultySelect:
                    ShowDS();
                    break;
                case DugeonState.ControlSelect:
                    ShowCS();
                    break;
                case DugeonState.ManualPlaying:
                    ShowMP();
                    break;
                case DugeonState.Clear:
                    ShowClear();
                    break;
                case DugeonState.Fail:
                    ShowFail();
                    break;
                default:
                    break;
            }
            
            //SceneControl(); 개별 씬으로 이동
        }
        void ShowDS()
        {
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
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 선택:z, 돌아가기: x");
            DgSelectCtl();
        }
        void ShowCS()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("던전 클리어 방식을 선택해주세요.");
            Console.WriteLine("플레이어의 방어력이 던전 권장 방어력보다 낮다면 60% 확률로 던전을 클리어할 수 있습니다.");
            Console.ResetColor();
            Console.WriteLine();
            for (int i = 0; i < cS_optionsLen; i++)
            {
                if (i == cS_optionsNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + controlSelectoptions[i]);
            }
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 선택:z, 돌아가기: x");
            CSCtl();
        }
        void ShowMP()
        {
            if (selectedDungeon == null || selectedDungeon.snake == null) return;
            
            selectedDungeon.snake.ShowGame();
            if (selectedDungeon.snake.gameOverSte == SnakeGame.GameOverState.Fail) // 클리어 실패
            {
                dgSt = DugeonState.Fail;
                selectedDungeon.EndSnake(); // 스네이크 해제, 메모리 누수 심함....
                ControlManager.Instance.ClearInputBuffer(); // 장면 넘어 갈 떄 기존에 입력된 키값들 없애기
            }
            else if (selectedDungeon.snake.gameOverSte == SnakeGame.GameOverState.Clear)// 성공
            {
                dgSt = DugeonState.Clear;
                selectedDungeon.EndSnake(); // 스네이크 해제, 메모리 누수 심함....
                ControlManager.Instance.ClearInputBuffer(); // 장면 넘어 갈 떄 기존에 입력된 키값들 없애기

            }

        }
        void ShowClear()
        {
            int beforeLv = Player.Instance.Lv;
            float beforeAtk = Player.Instance.Atk;
            float beforeDef = Player.Instance.Def;
            int beforeGold = Player.Instance.Gold;
            int beforeHp = Player.Instance.CurHP;
            if (selectedDungeon != null) selectedDungeon.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("던전 클리어");
            Console.ResetColor();
            Console.WriteLine("축하합니다!!");
            if(selectedDungeon != null) Console.WriteLine($"{selectedDungeon.Name}을 클리어 하였습니다.");
            Console.WriteLine();
            Console.WriteLine("[탐험 결과]");
            if (beforeLv != Player.Instance.Lv) // 레벨업을 했다면
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("레벨 업!!");
                Console.WriteLine($"Lv {beforeLv} -> {Player.Instance.Lv}");
                Console.WriteLine($"공격력 {beforeAtk} -> {Player.Instance.Atk}");
                Console.WriteLine($"방어력 {beforeDef} -> {Player.Instance.Def}");
                Console.WriteLine("-----------------------------");
            }
            Console.WriteLine($"체력 {beforeHp} -> {Player.Instance.CurHP}");
            Console.WriteLine($"Gold {beforeGold} G -> {Player.Instance.Gold} G");
            Console.WriteLine();
            //Console.WriteLine("▶ 0. 나가기");
            Thread.Sleep(1000);
            Console.WriteLine();
            Console.WriteLine("계속하려면 아무 키나 눌러주세요.");
            ClearCtl();
            Console.WriteLine("던전 입장으로 돌아갑니다.");
            Thread.Sleep(1000);
        }
        void ShowFail()
        {
            int beforeHp = Player.Instance.CurHP;
            Player.Instance.CurHP /= 2; // 체력 절반

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("던전 클리어 실패");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {beforeHp} -> {Player.Instance.CurHP}");
            Console.WriteLine();
            Thread.Sleep(1000);
            Console.WriteLine("계속하려면 아무 키나 눌러주세요.");
            FailCtl();
            Console.WriteLine("던전 입장으로 돌아갑니다.");
            Thread.Sleep(1000);
        }
        protected override void SceneControl()
        {
            switch (dgSt)
            {
                case DugeonState.DifficultySelect:
                    DgSelectCtl();
                    break;
                case DugeonState.ControlSelect:
                    CSCtl();
                    break;
                case DugeonState.Clear:
                    ClearCtl();
                    break;
                case DugeonState.Fail:
                    FailCtl();
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
                        case 1: // 일반
                        case 2: // 어려운
                            if (Player.Instance.CurHP <= 0) // 체력 0이라면 빠꾸
                            {
                                Console.WriteLine("플레이어의 체력이 0입니다.");
                                Thread.Sleep(300);
                                Console.WriteLine("시작화면으로 돌아갑니다.");
                                Thread.Sleep(sleepTime);
                                SceneManager.Instance.SetSceneState = SceneManager.SceneState.StartScene;
                            }
                            else
                            {
                                Console.WriteLine($"{dungeons[optionNum].Name} 던전을 선택하셨습니다.");
                                Thread.Sleep(sleepTime);
                                dgSt = DugeonState.ControlSelect;
                                selectedDungeon = dungeons[optionNum];
                            }
                            cS_optionsNum = 0;
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
        void CSCtl()
        {
            switch (ControlManager.Instance.GetKey())
            {
                case InputKey.Up:
                    if (cS_optionsNum != 0) cS_optionsNum--;
                    break;
                case InputKey.Down:
                    if (cS_optionsNum != cS_optionsLen - 1) cS_optionsNum++;
                    break;
                case InputKey.Z:
                    switch (cS_optionsNum)
                    {
                        case 0: // 자동
                            Console.WriteLine("자동 클리어를 선택하셨습니다.");
                            Thread.Sleep(sleepTime);
                            Console.WriteLine("던전 클리어중...");
                            Thread.Sleep(sleepTime);
                            if (CanClear()) // 클리어 가능 계산
                            {
                                dgSt = DugeonState.Clear;
                                Console.WriteLine("던전 클리어 성공!");
                                Thread.Sleep(sleepTime);
                            }
                            else // 클리어 실패
                            {
                                dgSt = DugeonState.Fail;
                                Console.WriteLine("던전 클리어 실패!");
                                Thread.Sleep(sleepTime);
                            }
                            break;
                        case 1: // 수동
                            Console.WriteLine("수동 클리어를 선택하셨습니다.");
                            Thread.Sleep(sleepTime);
                            Console.WriteLine("던전에 입장합니다.");
                            Thread.Sleep(sleepTime);
                            if(selectedDungeon != null) selectedDungeon.MakeSnake();
                            dgSt = DugeonState.ManualPlaying;
                            break;
                        case 2: // 나가기
                            dgSt = DugeonState.DifficultySelect;
                            break;
                        default:
                            break;
                    }
                    break;
                case InputKey.X:
                    dgSt = DugeonState.DifficultySelect;
                    break;
                default:
                    break;
            }
        }
        bool CanClear()
        {
            // 권장 방어력보다 크거나 같으면 클리어
            if (selectedDungeon != null && Player.Instance.TotalDef >= selectedDungeon.Def) return true;

            // 아니라면 60프로로 클리어
            int tmp = new Random().Next(0, 10);
            if(tmp > 5) return true;

            // 나머지 40프로 클리어 실패
            return false;
        }
        void ClearCtl()
        {
            switch (ControlManager.Instance.GetKey())
            {
                default:
                    dgSt = DugeonState.DifficultySelect;
                break;
            }
            // 클리어 출력후, 다시 난이도 선택으로 바꾸기
            //dgSt = DugeonState.DifficultySelect;
        }
        void FailCtl()
        {
            switch (ControlManager.Instance.GetKey())
            {
                default:
                    dgSt = DugeonState.DifficultySelect;
                    break;
            }
        }
        void CreateDungeon()
        {
            // 던전 생성
            dungeons.Add(new Dungeon());
            dungeons[0].Name = "쉬운 던전";
            dungeons[0].Def = 5;
            dungeons[0].Gold = 1000;
            dungeons[0].Exp = 5;
            dungeons[0].snakeMapSize = (30, 30);
            dungeons[0].snakeSpeed = 200;
            dungeons.Add(new Dungeon());
            dungeons[1].Name = "일반 던전";
            dungeons[1].Def = 11;
            dungeons[1].Gold = 1700;
            dungeons[1].Exp = 7;
            dungeons[1].snakeMapSize = (20, 20);
            dungeons[1].snakeSpeed = 150;
            dungeons.Add(new Dungeon());
            dungeons[2].Name = "어려운 던전";
            dungeons[2].Def = 17;
            dungeons[2].Gold = 2500;
            dungeons[2].Exp = 10;
            dungeons[2].snakeMapSize = (10, 10);
            dungeons[2].snakeSpeed = 100;

            // 난이도 선택지
            for (int i = 0; i < dungeons.Count; i++)
            {
                int npad = dNamePad - ControlManager.Instance.GetDisplayWidth(dungeons[i].Name);
                options.Add($"{i + 1}. {dungeons[i].Name}" + new string(' ', npad) + $"| 방어력 {dungeons[i].Def} 이상 권장");
            }
            options.Add("0. 나가기");
            optionsLen = options.Count();
            // 자동 / 수동 선택지
            controlSelectoptions.Add("1. 자동으로 클리어하기");
            controlSelectoptions.Add("2. 수동으로 클리어하기");
            controlSelectoptions.Add("0. 돌아가기");
            cS_optionsLen = controlSelectoptions.Count();
        }
    }
}
