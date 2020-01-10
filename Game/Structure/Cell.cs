namespace BattleshipClient.Game.Structure
{
    class Cell
    {
        public bool IsHit { get; set; }
        public bool HasShip => Ship != null;
        public Board Board { get; }
        public Ship Ship { get; set; }

        public Cell(Board board)
        {
            Board = board;
        }
    }
}
