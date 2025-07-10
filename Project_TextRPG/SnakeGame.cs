using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Project_TextRPG.SnakeGame;

namespace Project_TextRPG
{
    // 던전 수동 클리어용 미니 게임(스네이크)
    // 수동 클리어는 개발할 생각이 없었으나
    // 튜터님께서 텍스트 알피지 다 했으면
    // 강의 과제도 해보라고 하셔서 구현하게 되었다.

    // 스네이크 게임을 while로 초당 60프레임을 구현하고,
    // 또 동시에 게임이 끊기지 않게 멀티쓰레드로 입력값을 받아온다면
    // 보다 게임 엔진 시스템에 어울리는 과제가 되지 않을까 싶다.
    internal class SnakeGame
    {
        public enum MapState
        {
            Wall, None, Food, Player
        }
        public enum GameState
        {
            GameStart, GameOver
        }
        public enum GameStartState
        {
            Ready, GO
        }

        public enum GameOverState
        {
            Playing, Clear, Fail
        }

        public SnakeGame()
        {
            map = new List<List<MapState>>();
            dirList = new List<(int y, int x)>();
            // 상 하 좌 우 순
            dirList.Add((-1, 0));
            dirList.Add((1, 0));
            dirList.Add((0, -1));
            dirList.Add((0, 1));
            player = new LinkedList<(int y, int x)>();
            

            rand = new Random();
            sb = new StringBuilder(40 * 40);

            inputThread = new Thread(MulThreadInput);
            inputThread.IsBackground = true;
            inputThread.Start();
            canInput = true;

            gameState = GameState.GameStart;
            gameStartSte = GameStartState.Ready;
            gameOverSte = GameOverState.Playing;
        }
        // 출력용
        StringBuilder sb;
        // 맵 사이즈
        int x = 10;
        int y = 10;
        int delta = 10;
        int tmpTime = 0;
        int speed = 200;
        int dir = 2;
        // 게임 용 맵
        List<List<MapState>> map;
        // 플레이어 지렁이 위치
        LinkedList<(int y, int x)> player;
        // 방향용 리스트
        List<(int y, int x)> dirList;
        // 먹이 생성용 랜덤
        Random rand;
        // 멀티 쓰레드로 키입력 받기
        Thread inputThread;
        // 트루일 때만 입력받기
        volatile static bool canInput = true;
        int target = 10;
        int score = 0;

        // 게임 상태
        public GameState gameState { get; set; }
        public GameStartState gameStartSte { get; set; }
        public GameOverState gameOverSte { get; set; }
        public void ShowGame()
        {
            switch (gameState)
            {
                case GameState.GameStart:
                    GameStart();
                    break;
                case GameState.GameOver:
                    GameOver();
                    Thread.Sleep(2000);// 일정 시간 후 자동으로 건너뛰기
                    break;
                default:
                    break;
            }

        }
        void GameStart()
        {
            sb.Clear();

            tmpTime += delta;

            switch (gameStartSte)
            {
                case GameStartState.Ready:
                    sb.Append("Ready\n");
                    if (tmpTime >= 1000) // 1초 뒤 시작
                    {
                        tmpTime = 0;
                        gameStartSte = GameStartState.GO;
                    }
                    break;
                case GameStartState.GO:
                    //sb.Append($"현재 점수: {score},  목표 점수: {target}\n");
                    sb.Append($"현재 점수: {score},  목표 점수: {target}, dir: {dir}, canInput: {canInput}\n");
                    if (tmpTime >= speed)
                    {
                        PlayerMove();
                        tmpTime = 0;
                    }
                    break;
                default:
                    break;
            }

            // 맵 출력 세팅 전에, 맵에 플레이어와 먹이 세팅해야 함
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    switch (map[i][j])
                    {
                        case MapState.Wall:
                            sb.Append("▩");
                            break;
                        case MapState.None:
                            sb.Append("　");
                            break;
                        case MapState.Food:
                            sb.Append("★");
                            break;
                        case MapState.Player:
                            sb.Append("■");
                            break;
                        default:
                            break;
                    }
                }
                sb.Append("\n");
            }
            Console.Write(sb.ToString());
            //string sin = Console.ReadLine();
            Thread.Sleep(delta);
            if (gameState == GameState.GameOver)
            {
                GameStartLastFrame();
                Thread.Sleep(2000);
            }

        }
        void GameStartLastFrame()
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 110; j++)
                {
                    buffer.Append(" ");
                }
                buffer.Append("\n");
            }
            Console.SetCursorPosition(0, 0);

            Console.Write(sb.ToString());
            Console.SetCursorPosition(0, 0);
            // 도달 점수 출력하기 위한...
            sb.Clear();

            tmpTime += delta;

            switch (gameStartSte)
            {
                case GameStartState.Ready:
                    sb.Append("Ready\n");
                    if (tmpTime >= 1000) // 1초 뒤 시작
                    {
                        tmpTime = 0;
                        gameStartSte = GameStartState.GO;
                    }
                    break;
                case GameStartState.GO:
                    //sb.Append($"현재 점수: {score},  목표 점수: {target}\n");
                    sb.Append($"현재 점수: {score},  목표 점수: {target}, dir: {dir}, canInput: {canInput}\n");
                    if (tmpTime >= speed)
                    {
                        PlayerMove();
                        tmpTime = 0;
                    }
                    break;
                default:
                    break;
            }

            // 맵 출력 세팅 전에, 맵에 플레이어와 먹이 세팅해야 함
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    switch (map[i][j])
                    {
                        case MapState.Wall:
                            sb.Append("▩");
                            break;
                        case MapState.None:
                            sb.Append("　");
                            break;
                        case MapState.Food:
                            sb.Append("★");
                            break;
                        case MapState.Player:
                            sb.Append("■");
                            break;
                        default:
                            break;
                    }
                }
                sb.Append("\n");
            }
            Console.Write(sb.ToString());
        }
        void GameOver()
        {
            // 게임 오버 출력후 넘어가기
            //Console.SetCursorPosition(0, 0);
            sb.Clear();
            sb.Append("Game Over");
            Console.Write(sb.ToString());
            if (score >= target)
            {
                gameOverSte = GameOverState.Clear;
            }
            else gameOverSte = GameOverState.Fail;
        }
        void PlayerMove()
        {
            if (player.First == null) return; // 그럴리는 없겠지만, 경고때문에 

            // 멀티스레드로 중간에 dir값 변경하더라도 오류가 없도록
            int tmpDir = dir;
            (int ny, int nx) = (player.First.Value.y + dirList[tmpDir].y, player.First.Value.x + dirList[tmpDir].x);
            switch (map[ny][nx]) // 다음 이동 지점 파악하여
            {
                case MapState.Wall: // 벽이면 죽음
                case MapState.Player: // 자기 자신도 죽음
                    gameState = GameState.GameOver;
                    // 죽음
                    break;
                case MapState.None: // 빈 공간이면 이동
                    // 꼬리부터 위치 갱신
                    var node = player.Last;
                    if (node == null) return;

                    // 꼬리는 맵 빈공간으로 변경
                    map[node.Value.y][node.Value.x] = MapState.None;

                    while (true)
                    {
                        if (node.Previous == null)
                        {
                            // 노드가 맨 앞이라면
                            node.Value = (ny, nx);
                            map[node.Value.y][node.Value.x] = MapState.Player;
                            break;
                        }
                        node.Value = node.Previous.Value;
                        node = node.Previous;
                    }
                    break;
                case MapState.Food: // 음식이면 앞으로 자라나고 먹이 세팅
                    player.AddFirst((ny, nx));
                    map[ny][nx] = MapState.Player;
                    score++;
                    // 10개 먹으면 클리어
                    if (score >= target)
                    {
                        gameState = GameState.GameOver;
                    }
                    SetFood();
                    break;
                default:
                    break;
            }
            canInput = true;
        }
        void SetFood()
        {
            // 음식 세팅 안되면 루프, 세팅 되면 브레이크
            while (true)
            {
                int ny = rand.Next(1, y - 1);
                int nx = rand.Next(1, x - 1);
                if (map[ny][nx] == MapState.None)
                {
                    map[ny][nx] = MapState.Food;
                    break;
                }
            }
        }
        public void SetGame(int _y, int _x, int _speed)
        {
            y = _y;
            x = _x;
            speed = _speed;
            // 전체 맵 공간
            for (int i = 0; i < y; i++)
            {
                map.Add(new List<MapState>());
                for (int j = 0; j < x; j++)
                {
                    if (i == 0 || i == y - 1 || j == 0 || j == x - 1) // 벽 만들기
                    {
                        map[i].Add(MapState.Wall);
                    }
                    else
                    {
                        map[i].Add(MapState.None);
                    }
                }
            }
            // 플레이어 위치 세팅
            player.AddLast((y / 2, x / 2));
            player.AddLast((y / 2, x / 2 + 1));
            player.AddLast((y / 2, x / 2 + 2));
            foreach (var playerPos in player)
            {
                map[playerPos.y][playerPos.x] = MapState.Player;
            }
            // 먹이 세팅
            SetFood();
            //map[12][15] = MapState.Food;

        }
        void MulThreadInput()
        {
            while (gameOverSte == GameOverState.Playing)
            {
                if (canInput == false) continue;
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if(dir != 1) dir = 0;
                        break;
                    case ConsoleKey.DownArrow:
                        if (dir != 0) dir = 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (dir != 3) dir = 2;
                        break;
                    case ConsoleKey.RightArrow:
                        if (dir != 2) dir = 3;
                        break;
                    default:
                        break;
                }
                canInput = false;
            }
        }
    }
}
