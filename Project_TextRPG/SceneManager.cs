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
            scenes.Add(SceneState.Dungeon, new DungeonScene());
        }

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SceneManager();
                }

                return instance;
            }
        }
        public enum SceneState
        {
            StartScene, StatScene, InventoryScene, ShopScene, SellScene, Dungeon, Rest,
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
                // 기존 씬 메모리 할당 해제할 거면 여기서 하기
                // 기존 씬 참조 받아오기(주소 값)
                //Scene? tmp = scenes[sceneState];
                // 현재 구현 기준 Dictionary에서 해당 키값 제거해야 함
                //scenes.Remove(sceneState);
                // 기존 씬 리소스 정리하고
                //tmp.Dispose();
                // 참조도 끊어 GC가 메모리 해제하게 하기
                //tmp = null;
                // 이런 방식으로 해제할거면 Scene? curScene을 선언해서 계속 할당과 해체해주기
                // curScene.Dispose();
                // curScene = null;
                // switch로 value값에 따라 다음 씬 할당하기
                // curScene = new 다음 씬();
                // curScene.SetupScene();
                


                // 씬 스테이트 세팅하면 씬 세팅 자동 초기화 해보기
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
