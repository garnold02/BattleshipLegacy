using BattleshipClient.Engine;
using OpenTK;

namespace BattleshipClient.Game.RegularObjects
{
    class CursorController : RegularObject
    {
        public Vector2 Position { get; private set; }
        public Vector2 ClaimPosition => new Vector2((int)(Position.X / Container.Board.PieceLength), (int)(Position.Y / Container.Board.PieceLength));
        public int ShipLength { get; set; } = 2;
        public bool IsShipVertical { get; set; }

        public CursorController(GameContainer container) : base(container)
        {
            Position = Vector2.Zero;
        }

        public override void Update(float delta)
        {
            if(!Container.TurnManager.IsMenuEnabled)
            {
                SetPositionOnBoard();
            }
        }
        private void SetPositionOnBoard()
        {
            Vector2 positionOnPlane = Utility.GetMousePositionOnXZPlane(Container);
            Position = new Vector2((int)(positionOnPlane.X + 30), (int)(positionOnPlane.Y + 30));
        }
    }
}
