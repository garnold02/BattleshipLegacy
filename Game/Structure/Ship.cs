namespace BattleshipClient.Game.Structure
{
    class Ship
    {
        public Board Board { get; }
        public int Length => Cells.Length;
        public bool IsVertical { get; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public Player Owner { get; }
        public Cell[] Cells { get; }

        public Ship(Player owner, int positionX, int positionY, int length, bool isVertical)
        {
            Owner = owner;
            Board = owner.Board;
            PositionX = positionX;
            PositionY = positionY;
            IsVertical = isVertical;

            Cells = new Cell[length];
            for (int i = 0; i < length; i++)
            {
                Cells[i] = Owner.BoardClaim.Cells[IsVertical ? 0 : i, IsVertical ? i : 0];
            }
        }
    }
}
