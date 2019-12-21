using BattleshipClient.Game.GameObjects;
using System;
using System.Collections.Generic;

namespace BattleshipClient.Game.Structure
{
    class Board
    {
        public GameContainer Container { get; }
        public int FullSideLength { get; }
        public int SideLength { get; }

        public BoardPiece[,] Pieces { get; }
        public List<Player> Players { get; }

        public Board(GameContainer gameContainer, int sideLength, int pieceSideLength)
        {
            Container = gameContainer;
            Pieces = new BoardPiece[sideLength, sideLength];
            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    Pieces[i, j] = new BoardPiece(this, pieceSideLength);
                }
            }

            Players = new List<Player>();
            SideLength = sideLength;
            FullSideLength = sideLength * pieceSideLength;
        }
        public Player GetPlayer(string name)
        {
            return Players.Find(p => p.Name == name);
        }
        public void FillPlayerList(string[] playerNames)
        {
            foreach (string playerName in playerNames)
            {
                Player player = new Player(this, playerName);
                PlayerRenderer playerRenderer = new PlayerRenderer(this, player);
                player.Renderer = playerRenderer;

                Container.ObjManager.Add(playerRenderer);
            }
        }
    }
}
