using Battleship.Engine;
using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using OpenTK;
using OpenTK.Input;

namespace BattleshipClient.Game.RegularObjects
{
    class CameraController : RegularObject
    {
        public float PanDelta { get; set; } = 0.15f;
        public Vector3 TargetPosition { get; set; }
        public Vector3 CurrentPosition { get; private set; }
        public Camera Camera { get; }
        public CameraController(GameContainer container) : base(container)
        {
            TargetPosition = Vector3.Zero;
            Camera = new Camera(40, 0.1f, 100f);
            Camera.Transform.Rotate(55, 45, 0);
        }

        public override void Update(float delta)
        {
            PanHandler();
            SetPosition();
        }
        private void PanHandler()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Right))
            {
                Vector2 positionOnPlane = Utility.GetMousePositionOnXZPlane(Container);
                TargetPosition = new Vector3(positionOnPlane.X, 0, positionOnPlane.Y);
            }
        }
        private void SetPosition()
        {
            Vector3 path = (TargetPosition - CurrentPosition);
            CurrentPosition += path * PanDelta;
            Camera.Transform.localPosition = new Vector3(CurrentPosition.X - 10, 20, CurrentPosition.Z + 10);
        }
    }
}
