using System.Collections.Generic;

namespace BattleshipClient.Game.Structure
{
    class Board
    {
        public int FullSideLength { get; }
        public int SideLength { get; }

        public BoardPiece[,] Pieces { get; }
        public List<Player> Players { get; }

        public Board(int sideLength, int pieceSideLength)
        {
            Pieces = new BoardPiece[sideLength, sideLength];
            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    Pieces[i, j] = new BoardPiece(pieceSideLength);
                }
            }

            Players = new List<Player>();
            SideLength = sideLength;
            FullSideLength = sideLength * pieceSideLength;
        }
    }
}
