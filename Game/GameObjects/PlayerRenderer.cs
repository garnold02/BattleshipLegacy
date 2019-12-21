using BattleshipClient.Game.Structure;
using System.Collections.Generic;

namespace BattleshipClient.Game.GameObjects
{
    class PlayerRenderer : GameObject
    {
        public Player Player { get; }

        private readonly List<ShipRenderer> shipRenderers;

        public PlayerRenderer(GameContainer container, Player player) : base(container)
        {
            Player = player;
            shipRenderers = new List<ShipRenderer>();
        }
        public override void OnAdded()
        {

        }
        public override void OnRemoved()
        {

        }
        public override void Render()
        {
            foreach (ShipRenderer shipRenderer in shipRenderers)
            {
                shipRenderer.Render();
            }
        }
        public override void Update()
        {

        }

        public void CreateShipRenderer(Ship ship)
        {
            ShipRenderer shipRenderer = new ShipRenderer(Container.Board);
            shipRenderer.SetProperties(ship);

            shipRenderers.Add(shipRenderer);
        }
    }
}
