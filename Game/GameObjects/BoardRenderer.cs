using BattleshipClient.Engine.Rendering;

namespace BattleshipClient.Game.GameObjects
{
    class BoardRenderer : GameObject
    {
        private MeshRenderer oceanPlaneRenderer;
        public override void OnAdded()
        {
            oceanPlaneRenderer = new MeshRenderer(Assets.Get<Mesh>("plane"), Assets.Get<Shader>("f_lit"))
            {
                Transform = Transform
            };
            Transform.localPosition = new OpenTK.Vector3(0,-0.5f,-2);
        }
        public override void OnRemoved()
        {

        }
        public override void Update()
        {

        }
        public override void Render()
        {
            oceanPlaneRenderer.Render();
        }
    }
}
