using System.Threading;
using BattleshipClient.Game;

namespace BattleshipClient.Engine
{
    class Root
    {
        public static Thread gameThread;
        public static GameContainer GameContainer { get; private set; }
        public static void Start(string playerName, string hostname, string port)
        {
            gameThread = new Thread(GameThreadMethod)
            {
                Name = "gameLoop"
            };
            gameThread.Start(string.Format("{0};{1};{2}", playerName, hostname, port));
        }

        private static void GameThreadMethod(object data)
        {
            string[] dataTokens = data.ToString().Split(';');

            GameContainer = new GameContainer();
            GameContainer.Start(dataTokens[0],dataTokens[1],int.Parse(dataTokens[2]));
        }
    }
}
