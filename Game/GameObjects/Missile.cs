using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using OpenTK;

namespace BattleshipClient.Game.GameObjects
{
    class Missile : GameObject
    {
        #region Static
        private static Vector3 GetPointInTime(Vector2 origin, Vector2 destination, float t)
        {
            float distance = (destination - origin).Length;
            Vector2 flatPosition = Vector2.Lerp(origin, destination, t);
            return new Vector3(flatPosition.X, GetParabola(t * distance, distance, 6), flatPosition.Y);
        }
        private static float GetParabola(float t, float d, float h)
        {
            return 4 * h * (-(t * t) + d * t) / (d * d);
        }
        #endregion

        public Vector2 Origin { get; }
        public Vector2 Destination { get; }
        public bool IsColliding { get; }

        private readonly MeshRenderer meshRenderer;
        private float time;

        public Missile(GameContainer container, Vector2 origin, Vector2 destination, bool isColliding) : base(container)
        {
            Origin = origin;
            Destination = destination;
            IsColliding = isColliding;

            Transform.localPosition = new Vector3(Origin.X, 0, Origin.Y);
            Transform.localRotation = Utility.LookAt(Transform.localPosition, new Vector3(Origin.X, 1, Origin.Y));
            meshRenderer = new MeshRenderer(Assets.Get<Mesh>("missile"), Assets.Get<Shader>("f_lit"))
            {
                Transform = Transform
            };
        }
        public override void OnAdded()
        {

        }
        public override void OnRemoved()
        {

        }
        public override void Update(float delta)
        {
            time += delta * 0.5f;
            SetPosition();
            SetRotation();
            HandleDestruction();
        }
        public override void Render()
        {
            meshRenderer.Render();
        }

        private void SetPosition()
        {
            Transform.localPosition = GetPointInTime(Origin, Destination, time);
        }
        private void SetRotation()
        {
            Vector3 futurePoint = GetPointInTime(Origin, Destination, time + 0.1f);
            Transform.localRotation = Utility.LookAt(Transform.localPosition, futurePoint);
        }
        private void HandleDestruction()
        {
            if (Transform.localPosition.Y < -16)
            {
                Container.ObjManager.Remove(this);
            }
        }
    }
}
