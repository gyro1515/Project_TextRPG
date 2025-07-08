using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal class Scene
    {
        protected List<string> options;
        protected int optionsLen = 0;
        protected int padding = 16;
        protected int exPadding = 50;
        protected int statPadding = 48;
        // 일시 정지 시간
        protected int sleepTime = 800;
        public int OptionsLen
            { get { return optionsLen; } }
        public Scene()
        {
            options = new List<string>();
        }
        public int SelNum{ get; set; }

        public virtual void ShowScene(int selNum)
        {
            Console.WriteLine("Empty");
        }

    }
}
