﻿using System.Threading;
using BattleshipClient.Game;

namespace BattleshipClient.Engine
{
    class Root
    {
        public static Thread gameThread;
        public static GameContainer GameContainer { get; private set; }
        public static void Start(string playerName)
        {
            gameThread = new Thread(GameThreadMethod)
            {
                Name = "gameLoop"
            };
            gameThread.Start(playerName);
        }

        private static void GameThreadMethod(object playerName)
        {
            GameContainer = new GameContainer();
            GameContainer.Start(playerName.ToString());
        }
    }
}
