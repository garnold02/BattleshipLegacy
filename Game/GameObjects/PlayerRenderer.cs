using BattleshipClient.Game.Structure;
using System.Collections.Generic;

namespace BattleshipClient.Game.GameObjects
{
    class PlayerRenderer : GameObject
    {
        public Player Player { get; }

        private List<ShipRenderer> shipRenderers;

        public PlayerRenderer(Board board, Player player) : base(board)
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
            ShipRenderer shipRenderer = new ShipRenderer(Board);
            shipRenderer.SetProperties(ship);
        }
    }
}
