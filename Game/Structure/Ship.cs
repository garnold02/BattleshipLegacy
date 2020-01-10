using BattleshipClient.Engine.Rendering;

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

        public ShipRenderer Renderer { get; set; }

        public Ship(Player owner, int positionX, int positionY, int length, bool isVertical)
        {
            Owner = owner;
            Board = owner.Board;
            PositionX = positionX;
            PositionY = positionY;
            IsVertical = isVertical;

            Cells = new Cell[length];
            int rx = positionX % Board.PieceLength;
            int ry = positionY % Board.PieceLength;
            for (int i = 0; i < length; i++)
            {
                Cells[i] = Owner.BoardClaim.Cells[rx + (IsVertical ? 0 : i), ry + (IsVertical ? i : 0)];
                Cells[i].Ship = this;
            }
        }
    }
}
