namespace BattleshipClient.Game.Structure
{
    class BoardPiece
    {
        public Board Board { get; }
        public int PositionX { get; }
        public int PositionY { get; }
        public Cell[,] Cells { get; }

        public BoardPiece(Board board, int sideLength, int x, int y)
        {
            Board = board;
            PositionX = x;
            PositionY = y;
            Cells = new Cell[sideLength, sideLength];
            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    Cells[i, j] = new Cell(Board);
                }
            }
        }
    }
}
