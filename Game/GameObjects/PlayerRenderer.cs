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

        public PlayerRenderer(GameContainer container, Player player) : base(container)
        {
            Player = player;
            attackRenderers = new List<MeshRenderer>();
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
            RenderAttacks();
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
        public void CreateAttackRenderer(Attack attack)
        {
            MeshRenderer attackRenderer = new MeshRenderer(Assets.Get<Mesh>("plane"), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Texture = Assets.Get<Texture>("attackIndicator"),
                    Color = new Color4(1f, 1f, 1f, 0.5f)
                }
            };
            attackRenderer.Transform.localPosition = new Vector3(attack.DestinationX - Container.Board.FullSideLength / 2 + 0.5f, 0.03f, attack.DestinationY - Container.Board.FullSideLength / 2 + 0.5f);
            attackRenderers.Add(attackRenderer);
        }
        public void Refresh()
        {
            foreach (Ship ship in Player.Ships)
            {
                ship.Renderer.SetProperties(ship);
            }
        }
        private void RenderShips()
        {
            foreach (Ship ship in Player.Ships)
            {
                ship.Renderer.Render();
            }
        }
        private void RenderAttacks()
        {
            foreach (MeshRenderer attackRenderer in attackRenderers)
            {
                attackRenderer.Render();
            }
        }
    }
}
