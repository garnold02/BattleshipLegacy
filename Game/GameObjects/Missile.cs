using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.Structure;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace BattleshipClient.Game.GameObjects
{
    class Missile : GameObject
    {
        #region Static
        private static Vector3 GetPointInTime(Vector2 origin, Vector2 destination, float t, float h)
        {
            float distance = (destination - origin).Length;
            Vector2 flatPosition = Vector2.Lerp(origin, destination, t * (float)Math.Sqrt(distance) / distance);
            return new Vector3(flatPosition.X, GetParabola(t * (float)Math.Sqrt(distance), distance, h), flatPosition.Y);
        }
        private static float GetParabola(float t, float d, float h)
        {
            return 4 * h * (-(t * t) + d * t) / (d * d);
        }
        #endregion

        public Player Owner { get; }
        public Vector2 Origin { get; }
        public Vector2 Destination { get; }
        private float Height => 100 / tileDistance * heightModifier;

        private Vector2 renderOrigin;
        private Vector2 renderDestination;

        private readonly MeshRenderer meshRenderer;
        private readonly ParticleSystem particleSystem;
        private readonly float heightModifier;
        private readonly float tileDistance;

        private float time;

        public Missile(GameContainer container, Player owner, Vector2 origin, Vector2 destination) : base(container)
        {
            Owner = owner;
            Origin = origin;
            Destination = destination;

            float wsX = Owner.BoardClaim.PositionX * Container.Board.PieceLength + Container.Board.PieceLength / 2 - Container.Board.FullSideLength / 2;
            float wsY = Owner.BoardClaim.PositionY * Container.Board.PieceLength + Container.Board.PieceLength / 2 - Container.Board.FullSideLength / 2;
            float wdX = Destination.X - Container.Board.FullSideLength / 2 + 0.5f;
            float wdY = Destination.Y - Container.Board.FullSideLength / 2 + 0.5f;
            renderOrigin = new Vector2(wsX, wsY);
            renderDestination = new Vector2(wdX, wdY);

            tileDistance = (Destination - Origin).Length;
            heightModifier = 1 + Utility.RandomNormal() * 0.8f;

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
            Container.TurnManager.activeMissiles.Remove(this);
            //Container.ObjManager.Remove(particleSystem);
        }
        public override void Update(float delta)
        {
            time += delta * 2;
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
            Transform.localPosition = GetPointInTime(renderOrigin, renderDestination, time, Height);
        }
        private void SetRotation()
        {
            Vector3 futurePoint = GetPointInTime(renderOrigin, renderDestination, time + 0.1f, Height);
            Transform.localRotation = Utility.LookAt(Transform.localPosition, futurePoint);
        }
        private void HandleDestruction()
        {
            if (Transform.localPosition.Y < -1)
            {
                Container.ObjManager.Remove(this);
                Container.ObjManager.Remove(particleSystem);

                int px = (int)Destination.X / Container.Board.PieceLength;
                int py = (int)Destination.Y / Container.Board.PieceLength;
                int rx = (int)Destination.X % Container.Board.PieceLength;
                int ry = (int)Destination.Y % Container.Board.PieceLength;

                BoardPiece piece = Container.Board.Pieces[px, py];
                Cell cell = piece.Cells[rx, ry];
                cell.IsHit = true;
                if (cell.HasShip)
                {
                    Explosion explosion = new Explosion(Container, new Vector3(renderDestination.X, 0.5f, renderDestination.Y));
                    Container.ObjManager.Add(explosion);

                    piece.Owner.Renderer.Refresh();
                }
            }
        }
    }
}
