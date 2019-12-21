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
        public int PieceLength { get; }

        public BoardPiece[,] Pieces { get; }
        public List<Player> Players { get; }
        public Player LocalPlayer { get; private set; }

        private readonly string localPlayerName;

        public Board(GameContainer gameContainer, int sideLength, int pieceSideLength, string localPlayerName)
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
            PieceLength = pieceSideLength;

            this.localPlayerName = localPlayerName;
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
                Players.Add(player);
                if (playerName == localPlayerName)
                {
                    LocalPlayer = player;
                }

                PlayerRenderer playerRenderer = new PlayerRenderer(Container, player);
                player.Renderer = playerRenderer;
                Container.ObjManager.Add(playerRenderer);
            }
        }
    }
}
