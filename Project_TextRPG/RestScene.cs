using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal class RestScene : Scene
    {
        int gold = 500;
        public RestScene()
        {
            options.Add("1. 휴식하기");
            options.Add("0. 나가기");
            optionsLen = options.Count;
        }
        public override void ShowScene(int selNum)
        {
            Console.WriteLine("휴식하기");
            Console.WriteLine(gold + " G 를 내면 체력을 회복할 수 있습니다. (보유 골드: " + Player.Instance.Gold + " G)");
            Console.WriteLine();
            for (int i = 0; i < optionsLen; i++)
            {
                if (i == selNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 선택: z, 취소: x");

            Player.Instance.RestScene();
        }

        public void RestAndRecover()
        {
            Player p = Player.Instance;
            if (p.Gold - gold >= 0) // 구매가능
            {
                p.CurHP = Player.Instance.MaxHP;
                p.Gold -= gold;
                Console.WriteLine("휴식을 완료했습니다.");
                Thread.Sleep(sleepTime);
            }
            else
            {
                Console.WriteLine("금액이 부족합니다.");
                Thread.Sleep(sleepTime);
            }
        }
    }
}
