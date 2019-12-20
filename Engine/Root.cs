using System.Threading;
using BattleshipClient.Game;

namespace BattleshipClient.Engine
{
    class Root
    {
        public static Thread gameThread;
        public static GameContainer GameContainer { get; private set; }
        public static void Start()
        {
            gameThread = new Thread(GameThreadMethod)
            {
                Name = "gameLoop"
            };
            gameThread.Start();
        }

        private static void GameThreadMethod()
        {
            GameContainer = new GameContainer();
            GameContainer.Start();
        }
    }
}
