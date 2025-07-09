using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    enum InputKey
    {
        Up, Down, Left, Right, Z, X, None
    }

    internal class ControlManager
    {
        private static ControlManager? instance;
        private ControlManager() { }

        public static ControlManager Instance
        {
            get
            {
                if (instance == null) instance = new ControlManager();

                return instance;
            }
        }

        InputKey inputkey;

        public InputKey GetKey()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // 방향키 확인
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    //Console.WriteLine("↑ 위쪽 방향키 입력됨");
                    inputkey = InputKey.Up;
                    break;
                case ConsoleKey.DownArrow:
                    //Console.WriteLine("↓ 아래쪽 방향키 입력됨");
                    inputkey = InputKey.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    //Console.WriteLine("← 왼쪽 방향키 입력됨");
                    inputkey = InputKey.Left;
                    break;
                case ConsoleKey.RightArrow:
                    //Console.WriteLine("→ 오른쪽 방향키 입력됨");
                    inputkey = InputKey.Right;
                    break;
                case ConsoleKey.Escape:
                    //Console.WriteLine("종료합니다.");
                    break;
                case ConsoleKey.Z:
                    //Console.WriteLine("z");
                    inputkey = InputKey.Z;
                    break;
                case ConsoleKey.X:
                    //Console.WriteLine("x");
                    inputkey = InputKey.X;
                    break;
                default:
                    //Console.WriteLine($"다른 키 입력됨: {keyInfo.Key}");
                    inputkey = InputKey.None;

                    break;
            }
            return inputkey;
        }

        // 콘솔 출력 너비 맞추기용
        public int GetDisplayWidth(string s)
        {
            int width = 0;
            foreach (char c in s)
            {
                if (IsKorean(c))
                    width += 2;
                else
                    width += 1;
            }
            return width;
        }
        public bool IsKorean(char c)
        {
            // 한글 완성형 범위: U+AC00 ~ U+D7A3
            return c >= 0xAC00 && c <= 0xD7A3;
        }
    }
}
