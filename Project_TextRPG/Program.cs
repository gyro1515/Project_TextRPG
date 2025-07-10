namespace Project_TextRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ScreenManager.Instance.Initialize();
            Player.Instance.Name = "LEE";
            //SceneManager.Instance.SetScene = SceneManager.SceneState.StartScene; // 초기값은 이미 스타트 씬
            Console.SetWindowSize(120, 36);
            SceneManager.Instance.ShowScene(); // 루프
        }
    }
}
