using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    enum InputKey
    {
        Up, Down, Left, Right, Z, X
    }

    internal class Control
    {
        InputKey inputkey;

        public InputKey GetKey()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            // 방향키 확인
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    Console.WriteLine("↑ 위쪽 방향키 입력됨");
                    inputkey = InputKey.Up;
                    break;
                case ConsoleKey.DownArrow:
                    Console.WriteLine("↓ 아래쪽 방향키 입력됨");
                    inputkey = InputKey.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    Console.WriteLine("← 왼쪽 방향키 입력됨");
                    inputkey = InputKey.Left;
                    break;
                case ConsoleKey.RightArrow:
                    Console.WriteLine("→ 오른쪽 방향키 입력됨");
                    inputkey = InputKey.Right;
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine("종료합니다.");
                    break;
                case ConsoleKey.Z:
                    Console.WriteLine("z");
                    inputkey = InputKey.Z;
                    break;
                case ConsoleKey.X:
                    Console.WriteLine("x");
                    inputkey = InputKey.X;
                    break;
                default:
                    Console.WriteLine($"다른 키 입력됨: {keyInfo.Key}");
                    break;
            }
            return inputkey;
        }
    }
}
