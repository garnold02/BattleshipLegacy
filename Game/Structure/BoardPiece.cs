namespace BattleshipClient.Game.Structure
{
    class BoardPiece
    {
        public Cell[,] Cells { get; }

        public BoardPiece(int sideLength)
        {
            Cells = new Cell[sideLength, sideLength];
            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    Cells[i, j] = new Cell();
                }
            }
        }
    }
}
