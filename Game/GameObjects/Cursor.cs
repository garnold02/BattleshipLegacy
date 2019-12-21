using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.Structure;

namespace BattleshipClient.Game.GameObjects
{
    class Cursor : GameObject
    {
        private MeshRenderer claimRenderer;
        private ShipRenderer shipRenderer;
        private MeshRenderer attackRenderer;

        public Cursor(Board board) : base(board)
        {

        }
        public override void OnAdded()
        {
            claimRenderer = new MeshRenderer(Assets.Get<Mesh>("plane"), Assets.Get<Shader>("f_lit"));
            shipRenderer = new ShipRenderer(Board);
        }
        public override void OnRemoved()
        {

        }
        public override void Update()
        {

        }
        public override void Render()
        {

        }
    }
}
