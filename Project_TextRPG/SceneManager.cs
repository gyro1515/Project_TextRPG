using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 씬 매니저로 하나의 씬만 로드되도록 하기? 굳이? 메모리 해제 연습?
namespace Project_TextRPG
{
    internal class SceneManager
    {
        private static SceneManager? instance;
        private SceneManager() 
        {
            // 시작은 스타트 씬으로
            curScene = new StartScene();
        }

        public static SceneManager Instance
        {
            get
            {
                if (instance == null) instance = new SceneManager();

                return instance;
            }
        }

        public Scene curScene;
    }
}
