using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project_TextRPG.SceneManager;

namespace Project_TextRPG
{
    internal class StatScene : Scene
    {
        public StatScene()
        {
            options.Add("0. 나가기");
            optionsLen = options.Count;
        }
        public override void ShowScene()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.ResetColor();
            Console.WriteLine();

            // if와 switch, 삼항 연산자 중 어떤 걸?
            string addAtk;
            switch (Player.Instance.PlusAtk)
            {
                case 0:
                    addAtk = "";
                    break;
                default:
                    addAtk = " (+" + Player.Instance.PlusAtk + ")";
                    break;
            }
            string addDef = "";
            if (Player.Instance.PlusDef > 0) addDef = " (+" + Player.Instance.PlusDef + ")";
            string addHp = Player.Instance.PlusHP > 0 ? addHp = " (+" + Player.Instance.PlusHP + ")" : "";


            // 플레이어 가져와서 플레이어 능력치 보여줘야 함
            Console.WriteLine("Lv. " + Player.Instance.Lv);
            Console.WriteLine("이름: " + Player.Instance.Name);
            Console.WriteLine("공격력: " + (Player.Instance.Atk + Player.Instance.PlusAtk) + addAtk);
            Console.WriteLine("방어력: " + (Player.Instance.Def + Player.Instance.PlusDef) + addDef);
            Console.WriteLine("체력: " + Player.Instance.CurHP + " / " + (Player.Instance.MaxHP + Player.Instance.PlusHP) + addHp);
            Console.WriteLine("Gold: " + Player.Instance.Gold + " G");
            Console.WriteLine();
            // 나가기 표시
            for (int i = 0; i < optionsLen; i++)
            {
                if (i == optionNum) Console.Write("▶");
                else Console.Write("　");

                Console.WriteLine(" " + options[i]);
            }
            Console.WriteLine();
            Console.WriteLine("이동: 방향키, 선택:z, 돌아가기: x");

            SceneControl();
        }
        protected override void SceneControl()
        {
            switch (ControlManager.Instance.GetKey())
            {
                case InputKey.Z:
                case InputKey.X:
                    SceneManager.Instance.SetSceneState = SceneManager.SceneState.StartScene;
                    break;
                default:
                    break;
            }
        }
        /*public override void SetupScene()
        {
            base.SetupScene(); // 부모것도 호출
        }*/
    }
}
