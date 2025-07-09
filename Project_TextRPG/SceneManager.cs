using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project_TextRPG.ScreenManager;


// 씬 매니저로 하나의 씬만 로드되도록 하기? 굳이? 메모리 해제 연습?
namespace Project_TextRPG
{
    internal class SceneManager
    {
        private static SceneManager? instance;
        private SceneManager() 
        {
            // 시작은 스타트 씬으로
            // Dictionary로 구현해보기
            scenes = new Dictionary<SceneState, Scene>();
            scenes.Add(SceneState.StartScene, new StartScene());
            scenes.Add(SceneState.StatScene, new StatScene());
            scenes.Add(SceneState.InventoryScene, new InventoryScene());
            scenes.Add(SceneState.ShopScene, new ShopScene());
            scenes.Add(SceneState.SellScene, new SellScene());
            scenes.Add(SceneState.Rest, new RestScene());
        }

        public static SceneManager Instance
        {
            get
            {
                if (instance == null) instance = new SceneManager();

                return instance;
            }
        }
        public enum SceneState
        {
            StartScene, StatScene, InventoryScene, ShopScene, SellScene, Dungeon, Rest
        }

        private SceneState sceneState = SceneState.StartScene;
        // 씬 저장용
        private Dictionary<SceneState, Scene> scenes;

        public SceneState SetSceneState
        {
            /*get
            {  return sceneState; }*/
            set
            {
                // 씬 스테이트 세팅하면 자동 초기화 해보기
                sceneState = value;
                scenes[sceneState].SetupScene();
            }
        }
        public Dictionary<SceneState, Scene> ScenesDict
        {
            get { return scenes; }
        }
        public Scene GetCurScene
        {
            get
            {
                return scenes[sceneState];
            }
        }
        public void ShowScene()
        {
            while (true)
            {
                scenes[sceneState].ShowScene();
                Console.Clear();
            }
        }
    }
}
