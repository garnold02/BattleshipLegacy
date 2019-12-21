using OpenTK;
using OpenTK.Graphics;
using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.Structure;
using BattleshipClient.Engine;

namespace BattleshipClient.Game.GameObjects
{
    class BoardRenderer : GameObject
    {
        private MeshRenderer oceanPlaneRenderer;
        private MeshRenderer smallGridRenderer;
        private MeshRenderer largeGridRenderer;

        public BoardRenderer(GameContainer container) : base(container)
        {
            Depth = 2;
        }
        public override void OnAdded()
        {
            Mesh planeModel = Assets.Get<Mesh>("plane");
            oceanPlaneRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(0.4f, 0.7f, 0.9f, 0.4f)
                }
            };
            oceanPlaneRenderer.Transform = new Transform()
            {
                localScale = Vector3.One * Container.Board.FullSideLength
            };
            smallGridRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(1f, 1f, 1f, 0.02f),
                    Texture = Assets.Get<Texture>("grid"),
                    Tiling = new Vector2(Container.Board.FullSideLength, Container.Board.FullSideLength)
                }
            };
            smallGridRenderer.Transform = new Transform()
            {
                localPosition = new Vector3(0, 0.01f, 0),
                localScale = Vector3.One * Container.Board.FullSideLength
            };
            largeGridRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(1f, 1f, 1f, 0.02f),
                    Texture = Assets.Get<Texture>("largeGrid"),
                    Tiling = new Vector2(Container.Board.SideLength, Container.Board.SideLength)
                }
            };
            largeGridRenderer.Transform = new Transform()
            {
                localPosition = new Vector3(0, 0.02f, 0),
                localScale = Vector3.One * Container.Board.FullSideLength
            };
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
            smallGridRenderer.Render();
            largeGridRenderer.Render();
        }
    }
}
