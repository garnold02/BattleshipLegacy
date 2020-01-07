using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics;

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
        private readonly ParticleSystem particleSystem;
        private float time;

        public Missile(GameContainer container, Vector2 origin, Vector2 destination, bool isColliding) : base(container)
        {
            Origin = origin;
            Destination = destination;
            IsColliding = isColliding;

            Transform.localPosition = new Vector3(Origin.X, 0, Origin.Y);
            Transform.localRotation = Utility.LookAt(Transform.localPosition, new Vector3(Origin.X, 1, Origin.Y));
            meshRenderer = new MeshRenderer(Assets.Get<Mesh>("small_missile"), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Transform = Transform,
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>("smallMissile")
                }
            };

            particleSystem = new ParticleSystem(Container)
            {
                Frequency = 40,
                Transform = new Transform()
                {
                    Parent = Transform,
                    localPosition = new Vector3(0, 0, 0.8f),
                },
                ParticleProperties = new ParticleProperties()
                {
                    TextureName = "smoke",
                    Lifetime = 4f,
                    ColorBlendSeparator = 0.1f,
                    StartColor = new Color4(107, 198, 255, 255),
                    MiddleColor = new Color4(255, 231, 163, 255),
                    EndColor = new Color4(1f, 1f, 1f, 0),
                    ForceProbability = new Vector3(0.3f, 0.3f, 0.3f),
                    StartScale = 0.1f,
                    EndScale = 1f,
                }
            };
            Container.ObjManager.Add(particleSystem);
        }
        public override void OnAdded()
        {
        }
        public override void OnRemoved()
        {
            //Container.ObjManager.Remove(particleSystem);
        }
        public override void Update(float delta)
        {
            time += delta * 0.25f;
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
            if (Transform.localPosition.Y < -2)
            {
                Container.ObjManager.Remove(this);
                Container.ObjManager.Remove(particleSystem);
            }
        }
    }
}
