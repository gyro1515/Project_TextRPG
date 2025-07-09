using System;
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
           
        }
        public override void ShowScene()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.ResetColor();
        }
    }
}
