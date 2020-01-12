using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Game.GameObjects
{
    class ShipRendererObject : GameObject
    {
        public Ship Ship { get; }
        private readonly ShipRenderer renderer;
        public ShipRendererObject(GameContainer container, Ship ship) : base(container)
        {
            Ship = ship;
            renderer = new ShipRenderer(Container.Board);
            renderer.SetProperties(Ship);
        }
        public override void OnAdded()
        {
        }
        public override void OnRemoved()
        {
        }
        public override void Update(float delta)
        {
        }
        public override void Render()
        {
            renderer.Render();
        }
    }
}
