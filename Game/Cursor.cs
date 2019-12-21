using BattleshipClient.Engine;
using OpenTK;

namespace BattleshipClient.Game
{
    class Cursor
    {
        public GameContainer Container;
        public Vector2 Position { get; private set; }
        public Vector2 ClaimPosition => new Vector2((int)(Position.X / Container.Board.PieceLength), (int)(Position.Y / Container.Board.PieceLength));
        public int ShipLength { get; set; } = 2;
        public bool IsShipVertical { get; set; }

        public Cursor(GameContainer container)
        {
            Container = container;
            Position = Vector2.Zero;
        }
        public void Update()
        {
            SetPositionOnBoard();
        }
        private void SetPositionOnBoard()
        {
            Vector2 clipSpaceMousePosition = new Vector2(Container.MousePosition.X / Container.Width - 0.5f, (Container.Height - Container.MousePosition.Y) / Container.Height - 0.5f) * 2;
            Vector3 ray = Utility.ClipToWorldRay(Container.MainCamera, clipSpaceMousePosition);
            Vector3 positionOnPlane = Utility.Raycast(Container.MainCamera.Transform.Position, ray);
            Position = new Vector2((int)(positionOnPlane.X + 30), (int)(positionOnPlane.Z + 30));
        }
    }
}
