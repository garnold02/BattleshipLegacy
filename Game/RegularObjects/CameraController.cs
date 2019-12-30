using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using OpenTK;
using OpenTK.Input;

namespace BattleshipClient.Game.RegularObjects
{
    class CameraController : RegularObject
    {
        public float PanDelta { get; set; } = 0.15f;
        public float ZoomDelta { get; set; } = 0.1f;
        public Vector3 TargetPosition { get; set; }
        public Vector3 CurrentPosition { get; private set; }
        public float TargetZoom { get; set; }
        public float CurrentZoom { get; private set; }
        public Camera Camera { get; }

        private readonly Vector3 zoomPosition;
        public CameraController(GameContainer container) : base(container)
        {
            TargetPosition = Vector3.Zero;
            TargetZoom = 12;
            CurrentZoom = 12;
            Camera = new Camera(40, 0.1f, 200f);
            Camera.Transform.Rotate(55, 45, 0);

            zoomPosition = new Vector3(-1, 2, 1);
        }

        public override void Update(float delta)
        {
            PanHandler();
            SetPosition();
            SetZoom();
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
            Camera.Transform.localPosition = new Vector3(CurrentPosition.X, 0, CurrentPosition.Z) + zoomPosition * CurrentZoom;
        }
        private void SetZoom()
        {
            float difference = TargetZoom - CurrentZoom;
            CurrentZoom += difference * ZoomDelta;
        }
    }
}
