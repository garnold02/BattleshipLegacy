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
        public List<Attack> Attacks { get; }

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
            Attacks = new List<Attack>();
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
            foreach (Attack attack in Attacks)
            {
                foreach (Player player in attack.Owners)
                {
                    float startX = player.BoardClaim.PositionX * PieceLength + PieceLength / 2 - FullSideLength / 2;
                    float startY = player.BoardClaim.PositionY * PieceLength + PieceLength / 2 - FullSideLength / 2;
                    float destX = attack.DestinationX - FullSideLength / 2 + 0.5f;
                    float destY = attack.DestinationY - FullSideLength / 2 + 0.5f;
                    bool isColliding = attack.Owners.Count > 1;

                    Missile missile = new Missile(Container, new Vector2(startX, startY), new Vector2(destX, destY), isColliding);
                    Container.ObjManager.Add(missile);
                }
            }
        }
    }
}
