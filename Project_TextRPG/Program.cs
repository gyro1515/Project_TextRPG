namespace Project_TextRPG
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            //ScreenManager.Instance.Initialize();
            Player.Instance.Name = "LEE";
            Player.Instance.StartGame();
        }
    }
}
