using BattleshipClient.Engine.Net;
using BattleshipClient.Game.GameObjects;
using OpenTK;
using System;
using System.Collections.Generic;

namespace BattleshipClient.Game.Structure
{
    class Board
    {
        public GameContainer Container { get; }
        public BoardRenderer Renderer { get; set; }
        public int FullSideLength { get; }
        public int SideLength { get; }
        public int PieceLength { get; }

        public BoardPiece[,] Pieces { get; }
        public List<Player> Players { get; }
        public Player LocalPlayer { get; private set; }
        public List<StrategyAction> Actions { get; }

        private readonly string localPlayerName;

        public Board(GameContainer gameContainer, int sideLength, int pieceSideLength, string localPlayerName)
        {
            Container = gameContainer;
            Pieces = new BoardPiece[sideLength, sideLength];
            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    Pieces[i, j] = new BoardPiece(this, pieceSideLength, i, j);
                }
            }

            Players = new List<Player>();
            Actions = new List<StrategyAction>();
            SideLength = sideLength;
            FullSideLength = sideLength * pieceSideLength;
            PieceLength = pieceSideLength;

            this.localPlayerName = localPlayerName;
        }
        public Player GetPlayerByName(string name)
        {
            return Players.Find(p => p.Name == name);
        }
        public Player GetPlayerByID(byte id)
        {
            return Players.Find(p => p.ID == id);
        }
        public void FillPlayerList(Chunk[] chunks)
        {
            for (int i = 0; i < chunks.Length / 2; i++)
            {
                int pos = i * 2;
                byte id = (chunks[pos] as ByteChunk).Data;
                string name = (chunks[pos + 1] as StringChunk).Data;

                Player player = new Player(this, name, id);
                Players.Add(player);
                PlayerRenderer playerRenderer = new PlayerRenderer(Container, player);
                player.Renderer = playerRenderer;
                Container.ObjManager.Add(playerRenderer);

                if (name == localPlayerName)
                {
                    LocalPlayer = player;
                }
            }
        }
        public void CreateMissiles()
        {
            foreach (StrategyAction action in Actions)
            {
                Player owner = action.Owner;
                float startX = action.Owner.BoardClaim.PositionX;
                float startY = action.Owner.BoardClaim.PositionY;
                float destX = action.DestinationX;
                float destY = action.DestinationY;
                bool isHit = action.IsHit;

                Missile missile = new Missile(Container, owner, new Vector2(startX, startY), new Vector2(destX, destY), isHit);
                Container.ObjManager.Add(missile);
                Container.TurnManager.activeMissiles.Add(missile);
            }
        }
    }
}
