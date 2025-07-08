using System;
using System.Collections.Generic;
using System.Linq;
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
            optionsLen = options.Count;
        }
        public override void ShowScene(int selNum)
        {
            /*ScreenManager.Instance.Print(0, 1, "원진 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            ScreenManager.Instance.Print(0, 2, "");
            // 선택 표시
            for (int i = 0; i < optionsLen; i++)
            {
                if (i == selNum)
                {
                    ScreenManager.Instance.Print(0, 3 + i, "▶ " + options[i]);
                }
                else
                {
                    ScreenManager.Instance.Print(0, 3 + i, "　 " + options[i]);
                }
            }
            ScreenManager.Instance.Print(0, 3 + optionsLen, "");
            ScreenManager.Instance.Print(0, 4 + optionsLen, "이동: 방향키, 선택: z, 취소: x");*/


            Console.WriteLine("원진 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();

            // 선택 표시
            for (int i = 0; i < optionsLen; i++)
            {
                if (i == selNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 선택: z, 취소: x");

            // 씬으로 옮기기?
            Player.Instance.StartScene();
        }
    }
}
