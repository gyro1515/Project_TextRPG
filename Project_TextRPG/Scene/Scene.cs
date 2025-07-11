using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal abstract class Scene
    {
        protected List<string> options;
        protected int optionsLen = 0;
        protected int padding = 16;
        protected int exPadding = 50;
        protected int statPadding = 48;
        // 일시 정지 시간
        protected int sleepTime = 800;
        // 선택용 숫자
        protected int optionNum = 0;
        // 한번에 출력하기 위한 스트링 버퍼
        protected StringBuilder sb;
        public int OptionsLen
            { get { return optionsLen; } }
        public Scene()
        {
            options = new List<string>();
            sb = new StringBuilder();
        }

        public abstract void ShowScene();

        protected abstract void SceneControl();

        public virtual void SetupScene()
        {
            //optionNum = 0; 어떤 씬은 초기화 안해줘도 됨
        }
    }
}
