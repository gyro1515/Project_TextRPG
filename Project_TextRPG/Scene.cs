using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal class Scene
    {
        public virtual void ShowScene()
        {
            Console.WriteLine("Empty");
        }
    }

    class StartScene : Scene
    {
        public override void ShowScene()
        {

        }
    }
}
