using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.Structure;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace BattleshipClient.Game.GameObjects
{
    class PlayerRenderer : GameObject
    {
        public Player Player { get; }

        public readonly List<MeshRenderer> attackRenderers;
        private readonly ParticleSystem[,] smokeParticles;

        public PlayerRenderer(GameContainer container, Player player) : base(container)
        {
            Player = player;
            attackRenderers = new List<MeshRenderer>();
            smokeParticles = new ParticleSystem[Container.Board.PieceLength, Container.Board.PieceLength];
        }
        public override void OnAdded()
        {

        }
        public override void OnRemoved()
        {

        }
        public override void Render()
        {
            RenderShips();
            RenderAttackIndicators();
        }
        public override void Update(float delta)
        {
        }
        public void CreateShipRenderer(Ship ship)
        {
            ShipRenderer shipRenderer = new ShipRenderer(Container.Board);
            ship.Renderer = shipRenderer;
            shipRenderer.SetProperties(ship);
        }
        public void CreateActionRenderer(StrategyAction attack)
        {
            string meshName = "";
            string texName = "";
            switch (attack.Type)
            {
                case ActionType.Regular:
                    meshName = "small_missile";
                    texName = "smallMissile";
                    break;
                case ActionType.Big:
                    meshName = "icbm";
                    texName = "missileTex";
                    break;
                case ActionType.Repair:
                    meshName = "wrench";
                    texName = "wrenchTex";
                    break;
            }

            MeshRenderer actionRenderer = new MeshRenderer(Assets.Get<Mesh>(meshName), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>(texName),
                },
                Transform = new Transform()
                {
                    localRotation = Quaternion.FromEulerAngles(-MathHelper.PiOver2, 0, 0),
                }
            };
            actionRenderer.Transform.localPosition = new Vector3(attack.DestinationX - Container.Board.FullSideLength / 2 + 0.5f, 2, attack.DestinationY - Container.Board.FullSideLength / 2 + 0.5f);
            attackRenderers.Add(actionRenderer);
        }
        private void RenderShips()
        {
            foreach (Ship ship in Player.Ships)
            {
                ship.Renderer.Render();
            }
        }
        private void RenderAttackIndicators()
        {
            foreach (MeshRenderer attackRenderer in attackRenderers)
            {
                attackRenderer.Render();
            }
        }
    }
}
