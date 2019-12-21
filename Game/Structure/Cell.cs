namespace BattleshipClient.Game.Structure
{
    class Cell
    {
        public bool HasShip => Ship != null;
        public Board Board { get; }
        Ship Ship { get; set; }
        bool IsHit { get; set; }

        public Cell(Board board)
        {
            Board = board;
        }
    }
}
